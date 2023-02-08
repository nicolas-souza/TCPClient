using System.Net.Sockets;

internal class Program
{   
    public struct Server
    {
        public string? ip;
        public List<int> ports;

        public Server()
        {
            ip = null;
            ports = new List<int>();
        }
    }
    public static void EnviarMensagem(string server, int port, string? message = "")
    {
        using TcpClient client = new TcpClient(server, port);

        byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

        NetworkStream stream = client.GetStream();

        stream.Write(data, 0, data.Length);

        data = new byte[256];

        string responseData = string.Empty;

        int bytes = stream.Read(data, 0, data.Length);
        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        Console.WriteLine("Received: {0}", responseData);
    }
    public static string Menu()
    {
        Console.WriteLine($"1. Cadastrar Servidor\r\n" +
                          $"2. Cadastrar Porta\r\n" +
                          $"3. Excluir Servidor e suas Portas\r\n" +
                          $"4. Enviar menssagens a um Servidor cadastrado");
        var opc_ = Console.ReadLine();

        var opc = string.IsNullOrEmpty(opc_) ? "0" : opc_;

        return opc;
    }

    public static void  ListarIPs(List<Server> listServers)
    {
        if (listServers.Count == 0)
        {
            Console.WriteLine("Nenhum servidor cadastrado");
            return;
        }

        Console.WriteLine("Servidores cadastrados");
        int i = 0;
        foreach (var item in listServers)
        {
            Console.WriteLine($"{i}. IP: {item.ip}");
            Console.WriteLine("\tPortas:");
            item.ports.ForEach(port => {
                Console.WriteLine($"\t       {port}");
            });
            i++;
        }
    }
    
    public static void MensagemSucesso(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(mensagem);
        Console.ForegroundColor = ConsoleColor.White;
        
    }

    public static void Continue()
    {
        Console.WriteLine("Aperte qualquer tecla para continuar....");
        Console.ReadLine();
    }

    private static void Main(string[] args)
    {
        Console.Title = "TCP Client";        

        Server server = new();
        List<Server> listServers = new List<Server>();
        string exit = String.Empty;

        while (true)
        {
            try
            {                
                var opc = Menu();

                switch (opc)
                {
                    case "1":
                        exit = String.Empty;
                        do { 
                            Console.Clear();
                            Console.WriteLine("1. Cadastrar Servido");
                            Console.WriteLine("Digite o IP do servidor.......");
                            var ip_ = Console.ReadLine();

                            if (String.IsNullOrEmpty(ip_))
                            {
                                Console.WriteLine("IP inválido");
                                break;
                            }
                            
                            if (listServers.Where(x => x.ip == ip_).Count() > 0)
                            {
                                Console.WriteLine("Ip já adicionado");
                                break;
                            }

                            server = new();
                            server.ip = ip_;

                            listServers.Add(server);

                            MensagemSucesso("IP adicionado com sucesso!");

                            Console.WriteLine("Deseja adicionar outro servidor? [S/N]");
                            exit = Console.ReadLine();
                            exit = String.IsNullOrEmpty(exit) ? "N" : exit;

                        } while (exit.ToUpper() == "S") ;

                        Continue();
                        break;

                    case "2":
                        exit = String.Empty;

                        Console.Clear();
                        Console.WriteLine("2. Cadastrar Porta");

                        ListarIPs(listServers);

                        Console.WriteLine("Selecione o servidor......");
                        var selectedIP = Console.ReadLine();

                        if (String.IsNullOrEmpty(selectedIP) || ( Convert.ToInt32(selectedIP) > listServers.Count ))
                        {
                            Console.WriteLine("IP inválido");
                            break;
                        }
                        do
                        {


                            Console.WriteLine($"Digite a PORTA para o IP  {listServers[Convert.ToInt32(selectedIP)].ip}");
                            var porta = Console.ReadLine();

                            if (String.IsNullOrEmpty(selectedIP))
                            {
                                Console.WriteLine("Porta inválida");
                                break;
                            }

                            if (listServers[Convert.ToInt32(selectedIP)].ports.Contains(Convert.ToInt32(porta)))
                            {
                                Console.WriteLine("Porta já adicionada");
                                break;
                            }

                            listServers[Convert.ToInt32(selectedIP)].ports.Add(Convert.ToInt32(porta));

                            MensagemSucesso("Porta adicionada com sucesso!");

                            Console.WriteLine("Deseja adicionar outra porta? [S/N]");

                            exit = Console.ReadLine();
                            exit = String.IsNullOrEmpty(exit) ? "N" : exit;

                        } while (exit.ToUpper() == "S");

                        Continue();

                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("3. Excluir Servidor e suas Portas");

                        ListarIPs(listServers);
                        Console.WriteLine("Selecione o servidor......");
                        var selectedIPToDelete = Console.ReadLine();
                        var removed = listServers[Convert.ToInt32(selectedIPToDelete)];

                        listServers.Remove(listServers[Convert.ToInt32(selectedIPToDelete)]);

                        MensagemSucesso($"IP {removed.ip} removido com sucesso");
                        Continue();

                        break;
                    case "4":
                        
                        Console.Clear();
                        Console.WriteLine("4. Enviar menssagens a um Servidor cadastrado");

                        
                        ListarIPs(listServers);
                        Console.WriteLine("Selecione o servidor......");
                        var selectedIPToSendMessage = Console.ReadLine();
                        var IPToSendMessage = listServers[Convert.ToInt32(selectedIPToSendMessage)];

                        Console.WriteLine($"Portas cadastradas para o IP {IPToSendMessage.ip}");

                        int i = 0;
                        foreach (var port in IPToSendMessage.ports)
                        {
                            Console.WriteLine($"{i}. {port}");
                            i++;
                        }
                        Console.WriteLine("Selecione a porta......");
                        var selectedPort = Console.ReadLine();

                        if (String.IsNullOrEmpty(selectedPort) || Convert.ToInt32(selectedPort) > --i)
                        {
                            Console.WriteLine("Opção inválida!");
                            Continue();
                            break;
                        }

                        do
                        {
                            Console.WriteLine("Mensagem a ser enviada....");
                            var mensagem = Console.ReadLine();

                            EnviarMensagem(IPToSendMessage.ip, IPToSendMessage.ports[Convert.ToInt32(selectedPort)], mensagem);

                            Console.WriteLine("Deseja enviar mais mensagens para essa porta? [S/N]");
                            exit = Console.ReadLine();
                            exit = String.IsNullOrEmpty(exit) ? "N" : exit;

                        } while (exit.ToUpper() == "S") ;

                        Continue();

                        break;

                    default:
                            Console.WriteLine("Opção inválida!!");                         
                        break;
                }

                Console.Clear();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }


        }
    }
}