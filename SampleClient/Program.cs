using System;
using System.Net;
using System.Net.Sockets;
using PassThruLoggerControl;
using System.IO;
using Microsoft.Win32;

public class FullBinaryWriter : BinaryWriter
{
    public FullBinaryWriter(Stream stream) : base(stream) { }
    public new void Write7BitEncodedInt(int i)
    {
        base.Write7BitEncodedInt(i);
    }
}

public class AsynchronousClient
{
    // The port number for the remote device.
    private const int port = 2534;

    private static void StartClient()
    {
        // Connect to a remote device.
        try
        {
            // Establish the remote endpoint for the socket.
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            // Create a TCP/IP socket.
            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Connect to the remote endpoint.
            client.Connect(remoteEP);
            Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

            NetworkStream stream = new NetworkStream(client);
            FullBinaryWriter writer = new FullBinaryWriter(stream);

            writer.Write((UInt16)0x0000); //WireProtocolVersion
            writer.Write((UInt16)0x0404); //J2534ProtocolVersion

            writer.Write((byte)msgtype.reportParam);
            writer.Write((byte)param.client);
            writer.Write(System.AppDomain.CurrentDomain.FriendlyName);

            string drivername = "";
            using (var reg32 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32))
            {
                using (var loggerRegEntry = reg32.CreateSubKey(@"Software\Passthru Logger"))
                {
                    var defaultkeypath = loggerRegEntry.GetValue("DefaultDriverKey");
                    if (defaultkeypath == null || loggerRegEntry.GetValueKind("DefaultDriverKey") != RegistryValueKind.String)
                    {
                        drivername = "UNKNOWN";
                    }
                    else
                    {
                        drivername = (string)defaultkeypath;
                    }
                }
            }

            writer.Write((byte)msgtype.reportParam);
            writer.Write((byte)param.driver);
            writer.Write(drivername);
            writer.Write((int)0);

            while (true)
            {
                Console.WriteLine("Sending data from loop...");
                SendJ2534(writer, J2534_0404func.PassThruOpen);
                if (Console.ReadLine() == null) break;
            }

            // Release the socket.
            client.Shutdown(SocketShutdown.Both);
            client.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private static void SendJ2534(BinaryWriter writer, J2534_0404func func)
    {
        writer.Write((byte)msgtype.J2534Msg);
        writer.Write((byte)func);

        writer.Flush();
    }

    public static int Main(String[] args)
    {
        StartClient();
        //Console.ReadKey();
        return 0;
    }
}
