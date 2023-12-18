//TO Jail.cs
//end of the line, before last }
public class SharedMemoryProducer
{
    public static void WriteData(string message)
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 12345))
        using (NetworkStream stream = client.GetStream())
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine(message);
        }
    }
}





//to set_warden method (search name with ctrl + t on visual studio)
SharedMemoryProducer.WriteData(player.SteamID.ToString());
