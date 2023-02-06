using System.Net.Sockets;

string server = "ADD_YOUR_SERVER";

void enviarTCP(string message = "11:02")
{
    Int32 port = 6722;

    using TcpClient client = new TcpClient(server, port);

    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

    NetworkStream stream = client.GetStream();

    stream.Write(data, 0, data.Length);

    data = new Byte[256];

    String responseData = String.Empty;

    Int32 bytes = stream.Read(data, 0, data.Length);
    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

    Console.WriteLine("Received: {0}", responseData);
}


while (true)
{    
    try
    {
        Console.WriteLine($"\nMessage to send to {server}...");

        var message = Console.ReadLine();

        enviarTCP(message);
        
    }
    catch (ArgumentNullException e)
    {
        Console.WriteLine("ArgumentNullException: {0}", e);
    }
    catch (SocketException e)
    {
        Console.WriteLine("SocketException: {0}", e);
    }

    Thread.Sleep(1000);
    
}

