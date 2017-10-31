using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace PassThruLoggerControl
{
    public enum CONNSTATE { Setup, Connected, Disconnected, Error, Unsupported, MissingDriver, BadDriver };

    public class ConnectionInfo
    {
        private static int nextConnectionId = 0;

        private J2534LogController form;
        public Socket socket = null;
        NetworkStream stream;
        FullBinaryReader streamreader;
        Thread thread;
        private string tmpLogPath;

        public LinkedList<string> logPreviewEntries = new LinkedList<string>();

        private StreamWriter logWriter;
        private bool closed = false;

        J2534ProtocolInterpreter interpreter;
        public CONNSTATE state;
        string extraStatus = "";

        public int ID { get; set; }
        public string Status { get { return state.ToString() + extraStatus; } }
        public int EventCount { get; set; }
        public string Driver { get; set; }
        public string Client { get; set; }
        public string J2534Version
        {
            get
            {
                if (interpreter == null) return "UNK";
                return interpreter.J2534Version;
            }
        }

        private uint _maxLogPreviewEntryCount = 150;
        public uint maxLogPreviewEntryCount
        {
            get { return _maxLogPreviewEntryCount; }
            set
            {
                _maxLogPreviewEntryCount = value;
                while (logPreviewEntries.Count > _maxLogPreviewEntryCount)
                {
                    logPreviewEntries.RemoveFirst();
                }
            }
        }

        public ConnectionInfo(J2534LogController form1, Socket handler)
        {
            socket = handler;
            stream = new NetworkStream(socket);

            streamreader = new FullBinaryReader(stream, System.Text.Encoding.ASCII);
            form = form1;

            ID = Interlocked.Increment(ref nextConnectionId);
            state = CONNSTATE.Setup;
            Driver = "";
            Client = "";
            thread = new Thread(new ThreadStart(recvThreadFunc));

            tmpLogPath = Path.GetTempFileName();
            logWriter = new StreamWriter(tmpLogPath);
        }

        ~ConnectionInfo()
        {
            //tmpLogstream.Close();
            File.Delete(tmpLogPath);
        }

        private void recvThreadFunc()
        {
            bool is_mid_msg = false;
            try
            {
                UInt16 wireProtoVersion = streamreader.ReadUInt16();
                if (wireProtoVersion != 0x0000)
                {
                    state = CONNSTATE.Unsupported;
                    form.updateConnectionListEntry(this);
                    return;
                }
                UInt16 J2534ProtoVersion = streamreader.ReadUInt16();
                switch (J2534ProtoVersion)
                {
                    case 0x0404:
                        interpreter = new J2534ProtocolInterpreter_0404();
                        break;
                    default:
                        state = CONNSTATE.Unsupported;
                        form.updateConnectionListEntry(this);
                        return;
                }

                state = CONNSTATE.Connected;
                form.updateConnectionListEntry(this);

                //Setup complete, Loop forever processing messages.
                while (state == CONNSTATE.Connected)
                {
                    msgtype mtype = (msgtype)streamreader.ReadByte();
                    checkEnum(typeof(msgtype), mtype);
                    is_mid_msg = true;

                    switch (mtype)
                    {
                        case msgtype.reportParam:
                            StringBuilder logentry = new StringBuilder();
                            param p = (param)streamreader.ReadByte();
                            checkEnum(typeof(param), p);

                            switch (p)
                            {
                                case param.client:
                                    Client = streamreader.ReadString();
                                    form.updateConnectionListEntry(this);
                                    logentry.Append("Client: " + Client);
                                    //logWriter.WriteLine("Client: " + Client);
                                    break;
                                case param.driver:
                                    Driver = streamreader.ReadString();
                                    logentry.Append("Driver: " + Driver);
                                    //logWriter.WriteLine("Driver: " + Driver);
                                    int driverstatus = streamreader.ReadInt32();
                                    if (driverstatus != 0)
                                    {
                                        close();
                                        state = CONNSTATE.Error;
                                        if (driverstatus > 0)
                                        {
                                            state = CONNSTATE.BadDriver;
                                        }
                                        else if (driverstatus == -1)
                                        {
                                            state = CONNSTATE.MissingDriver;
                                        }
                                    }
                                    form.updateConnectionListEntry(this);
                                    break;
                            }

                            saveLogEntry(logentry.ToString());
                            break;
                        case msgtype.J2534Msg:
                            saveLogEntry(interpreter.interpret(streamreader));
                            EventCount++;
                            form.updateConnectionListEntry(this);
                            break;
                    }
                    is_mid_msg = false;
                }
            }
            catch (System.ObjectDisposedException)
            {
                Console.WriteLine("Connectionclosed, unable to do stuff.");
                state = CONNSTATE.Disconnected;
                form.updateConnectionListEntry(this);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                catchSocketException(e);
            }
            catch (System.IO.IOException e)
            {
                if (e.InnerException is System.Net.Sockets.SocketException)
                {
                    catchSocketException((System.Net.Sockets.SocketException)e.InnerException);
                }
                else if (!is_mid_msg)
                {
                    state = CONNSTATE.Disconnected;
                    form.updateConnectionListEntry(this);
                    socket.Close();
                }
                else
                    die();
            }
            catch (InvalidEnumException e)
            {
                Console.WriteLine(e.ToString());
                die();
            }
        }

        private void catchSocketException(System.Net.Sockets.SocketException e)
        {
            Console.WriteLine("Socket Error...");
            if (e.ErrorCode == /*WSAECONNRESET*/0x2746)
            {
                state = CONNSTATE.Disconnected;
                form.updateConnectionListEntry(this);
            }
            else
                die();
        }

        private void checkEnum(Type type, object thing)
        {
            if (!Enum.IsDefined(type, thing))
            {
                throw new InvalidEnumException(String.Format("Got invalid enum value for {0}: {1}", type.Name, thing));
            }
        }

        private void die(bool error = true)
        {
            if (closed) return;
            state = CONNSTATE.Error;
            form.updateConnectionListEntry(this);
            socket.Close();
            closed = true;
        }

        internal void start()
        {
            thread.Start();
        }

        internal void saveLog(string fileName)
        {
            logWriter.Flush();
            try
            {
                File.Delete(fileName);
            }
            catch (Exception) { }
            File.Copy(tmpLogPath, fileName);
        }

        internal void close()
        {
            closed = true;
            state = CONNSTATE.Disconnected;
            socket.Close();
        }

        public void flushLog()
        {
            logWriter.Flush();
        }

        public void saveLogEntry(string entry)
        {
            logWriter.WriteLine(entry);

            string[] lines = entry.Split('\n');
            form.addLinesToLogPreview(this, lines);
            foreach (string line in lines)
                logPreviewEntries.AddLast(entry);
            while (logPreviewEntries.Count > maxLogPreviewEntryCount)
                logPreviewEntries.RemoveFirst();
        }
    }
}
