using System.Net;
using System.Net.Sockets;
using System.Text;

namespace JailbreakExtras;

public partial class JailbreakExtras
{
    public class SharedMemoryConsumer
    {
        public enum SharedMemoryDataTypes
        {
            None = 0,
            LatestWarden = 1,
        }

        public static void StartListenerData()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, 12345);
            listener.Start();

            while (true)
            {
                using (TcpClient client = listener.AcceptTcpClient())
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = new byte[10000];
                    int bytesRead = stream.Read(data, 0, data.Length);

                    var str = Encoding.UTF8.GetString(data, 0, bytesRead);
                    Console.WriteLine(str);
                    ProcessData(str);
                }
            }
        }

        private static void ProcessData(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return;
            }
            if (str.Contains('|') == false)
            {
                return;
            }
            var splitted = str.Split('|');
            if (splitted.Length <= 1)
            {
                return;
            }
            if (!Enum.TryParse<SharedMemoryDataTypes>(splitted[0], out var dataType))
            {
                dataType = SharedMemoryDataTypes.None;
            }
            switch (dataType)
            {
                case SharedMemoryDataTypes.LatestWarden:
                    if (string.IsNullOrWhiteSpace(splitted[1]) == false)
                    {
                        if (ulong.TryParse(splitted[1] ?? "", out var parsed))
                        {
                            LatestWCommandUser = parsed;
                        }
                    }
                    break;

                case SharedMemoryDataTypes.None:
                default:
                    break;
            }
        }
    }
}