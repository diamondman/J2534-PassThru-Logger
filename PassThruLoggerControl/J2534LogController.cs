using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace PassThruLoggerControl
{
    public partial class J2534LogController : Form
    {
        private List<J2534Driver> drivers = new List<J2534Driver>();
        private bool initDone = false;

        private Socket listener;
        BindingList<ConnectionInfo> connectionBindingList = new BindingList<ConnectionInfo>();

        Thread socketServerThread;
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        /////////////////////// Constructor/Destructor
        public J2534LogController()
        {
            InitializeComponent();

            socketServerThread = new Thread(new ThreadStart(socketServerFunction));

            connectionBindingSource.DataSource = connectionBindingList;

            using (var reg32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                using (var passthruList = reg32.OpenSubKey(@"Software\PassThruSupport.04.04\", false))
                {
                    foreach (var v in passthruList.GetSubKeyNames())
                    {
                        if (v.Contains("Passthru Logger")) continue;
                        RegistryKey passThruEntry = passthruList.OpenSubKey(v);
                        if (passThruEntry != null)
                        {
                            drivers.Add(new J2534Driver(v, passThruEntry));
                        }
                    }
                }
            }

            defaultdriver.DataSource = drivers;

            using (var reg32 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
            {
                using (var loggerRegEntry = reg32.CreateSubKey(@"Software\Passthru Logger"))
                {
                    var defaultkeypath = loggerRegEntry.GetValue("DefaultDriverKey");
                    if (defaultkeypath == null || loggerRegEntry.GetValueKind("DefaultDriverKey") != RegistryValueKind.String)
                    {
                        if (drivers.Count() > 0)
                            loggerRegEntry.SetValue("DefaultDriverKey", drivers[0].key, RegistryValueKind.String);
                    }
                    else
                    {
                        int index = 0;
                        bool entryfound = false;
                        foreach (var driver in drivers)
                        {
                            if (driver.key == (string)defaultkeypath)
                            {
                                defaultdriver.SelectedIndex = index;
                                entryfound = true;
                                break;
                            }

                            index++;
                        }

                        if (!entryfound && drivers.Count() > 0)
                        {
                            loggerRegEntry.SetValue("DefaultDriverKey", drivers[0].key, RegistryValueKind.String);
                        }
                    }
                }
            }

            initDone = true;

            // Establish the local endpoint for the socket.
            IPAddress ipAddress = Dns.GetHostEntry("localhost").AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 2534);

            // Create a TCP/IP socket.
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Windows.Forms.MessageBox.Show("Error opening socket: " + e.ToString());
                Application.Exit();
            }

            socketServerThread.Start();
        }

        /////////////////////// Server setup functions
        private void socketServerFunction()
        {
            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (System.ObjectDisposedException)
            {
                //ObjectDisposedException will be raised if we close the connection.
                //Good way to break the blocking socket functions.
                Console.WriteLine("Connection closed, shutting down server thread.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                System.Windows.Forms.MessageBox.Show("Error accepting connection: " + e.ToString());
                Application.Exit();
            }

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            try
            {
                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // Create the state object.
                var conninfo = new ConnectionInfo(this, handler);
                conninfo.start();

                Invoke(new MethodInvoker(delegate () { connectionBindingList.Add(conninfo); }));

                Console.WriteLine("Connection established...");
            }
            catch (System.ObjectDisposedException)
            {
                Console.WriteLine("Connectionclosed, unable to do stuff.");
            }
        }

        /////////////////////// Extra functions
        public void updateConnectionListEntry(ConnectionInfo conninfo)
        {
            //Invoke(new MethodInvoker(delegate () { loggerconnections.Rows[connectionBindingList.IndexOf(conninfo)].Cells[1].Style.BackColor = Color.Red; }));
            Invoke(new MethodInvoker(delegate () { connectionBindingList.ResetItem(connectionBindingList.IndexOf(conninfo)); }));
        }

        public void addLinesToLogPreview(ConnectionInfo conninfo, string[] lines)
        {
            Invoke(new MethodInvoker(delegate () {
                if (connectionBindingList[loggerconnections.CurrentCell.RowIndex] != conninfo) return;

                //if (logpreview.TopIndex == logpreview.Items.Count) return;

                int visibleItems = logpreview.ClientSize.Height / logpreview.ItemHeight;
                var tmp = Math.Max(logpreview.Items.Count - visibleItems, 0);
                bool doscroll = (logpreview.TopIndex == tmp);

                logpreview.Items.AddRange(lines);
                while (logpreview.Items.Count > conninfo.maxLogPreviewEntryCount)
                    logpreview.Items.RemoveAt(0);

                if(doscroll)
                    logpreview.TopIndex = Math.Max(logpreview.Items.Count - visibleItems + 1, 0);
            }));
        }

        /////////////////////// UI Events
        private void savelog_Click(object sender, EventArgs e)
        {
            logsavewindow.ShowDialog();
        }

        private void defaultdriver_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!initDone) return;
            using (var reg32 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
            {
                using (var loggerRegEntry = reg32.CreateSubKey(@"Software\Passthru Logger"))
                {
                    if (drivers.Count() > defaultdriver.SelectedIndex)
                        loggerRegEntry.SetValue("DefaultDriverKey", drivers[defaultdriver.SelectedIndex].key, RegistryValueKind.String);
                }
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (e.Cancel) return;
            connectionBindingList[loggerconnections.CurrentCell.RowIndex].saveLog(logsavewindow.FileName);
        }

        private void Form1_FormClosing(object sender, EventArgs e)
        {
            Console.WriteLine("CLOSING OBJECT");
            listener.Close();

            foreach(var conn in connectionBindingList)
            {
                conn.close();
            }
        }

        private void loggerconnections_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //Rows can not be removed, so there is no reason to disable this button after a row appears.
            savelog.Enabled = true;

            logpreview.Items.Clear();
            foreach (var row in connectionBindingList[e.RowIndex].logPreviewEntries)
                logpreview.Items.Add(row);
        }

        private void loggerconnections_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("CellLeave");
        }
    }
}
