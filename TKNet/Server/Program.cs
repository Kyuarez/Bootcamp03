using MessagePack;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TKPacket;

namespace Server
{
    public class Program
    {
        static Socket listenSocket;
        static List<Socket> clientSockets = new List<Socket>();
        //static List<Thread> threadManager = new List<Thread>();
        static object _lock = new object();
        
        public static void Main(string[] args)
        {
            Console.Title = "Server";
            OnServer();

        }

        static void SendPacket(Socket toSocket, TKPacketChat packet)
        {
            byte[] tkpacket = MessagePackSerializer.Serialize(packet);
            ushort length = (ushort)tkpacket.Length;

            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)length));

            toSocket.Send(header);
            int sendLength = toSocket.Send(tkpacket, 0, length, SocketFlags.None);
        }

        static void AcceptThread()
        {
            while (true)
            {
                Socket clientSocket = listenSocket.Accept();

                lock (_lock)
                {
                    clientSockets.Add(clientSocket);
                }

                Console.WriteLine($"Connect client : {clientSocket.RemoteEndPoint}");

                // 클라이언트에게 User ID 전송
                string guid = GenerateRandomUserId();
                byte[] userIdBytes = Encoding.UTF8.GetBytes(guid);
                clientSocket.Send(userIdBytes);

                Thread workThread = new Thread(new ParameterizedThreadStart(WorkThread));
                workThread.IsBackground = true;
                workThread.Start(clientSocket);
            }
        }

        static void WorkThread(Object clientObjectSocket)
        {
            Socket clientSocket = clientObjectSocket as Socket;

            while (true)
            {
                try
                {
                    byte[] headerBuffer = new byte[2];
                    int RecvLength = clientSocket.Receive(headerBuffer, 2, SocketFlags.None);
                    if (RecvLength > 0)
                    {
                        short packetLength = BitConverter.ToInt16(headerBuffer, 0);
                        packetLength = IPAddress.NetworkToHostOrder(packetLength);

                        byte[] dataBuffer = new byte[packetLength];
                        RecvLength = clientSocket.Receive(dataBuffer, packetLength, SocketFlags.None);
                        var packet = MessagePackSerializer.Deserialize<TKPacketChat>(dataBuffer);

                        try
                        {
                            Console.WriteLine();

                            lock (_lock)
                            {
                                foreach (Socket client in clientSockets)
                                {
                                    SendPacket(client, packet);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            TKPacketChat ePacket = new TKPacketChat()
                            {
                                Message = "Failed : " + clientSocket.RemoteEndPoint,
                                SendTime = DateTime.Now,
                                UserID = 1.ToString(),
                                NickName = "Server"
                            };
                            
                            SendPacket(clientSocket, ePacket);
                        }
                    }
                    else
                    {
                        TKPacketChat ePacket = new TKPacketChat()
                        {
                            Message = "Disconnect : " + clientSocket.RemoteEndPoint,
                            SendTime = DateTime.Now,
                            UserID = 1.ToString(),
                            NickName = "Server"
                        };

                        SendPacket(clientSocket, ePacket);

                        lock (_lock)
                        {
                            clientSockets.Remove(clientSocket);
                        }

                        clientSocket.Close();
                        return;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Disconnect : " + clientSocket.RemoteEndPoint);

                    TKPacketChat ePacket = new TKPacketChat()
                    {
                        Message = $"Disconnect : " + clientSocket.RemoteEndPoint,
                        SendTime = DateTime.Now,
                        UserID = 1.ToString(),
                        NickName = "Server"
                    };

                    SendPacket(clientSocket, ePacket);

                    lock (_lock)
                    {
                        clientSockets.Remove(clientSocket);
                    }

                    clientSocket.Close();
                    return;
                }
            }
        }

        public static void OnServer()
        {
            listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);

            listenSocket.Bind(listenEndPoint);

            listenSocket.Listen(10);

            Thread acceptThread = new Thread(new ThreadStart(AcceptThread));
            acceptThread.IsBackground = true;
            acceptThread.Start();

            acceptThread.Join();

            listenSocket.Close();
        }

        public static string GenerateRandomUserId()
        {
            return Guid.NewGuid().ToString(); // 예: "3F2504E0-4F89-11D3-9A0C-0305E82C3301"
        }
    }
}
