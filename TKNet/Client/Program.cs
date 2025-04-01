using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using MessagePack;
using TKPacket;
    
namespace Client
{
    public class Program
    {
        static Socket clientSocket;
        static string clientGUID = string.Empty;
        public static void Main(string[] args)
        {
            Console.Title = "Client";
            ClientWithThread();
        }


        static void SendPacket(Socket toSocket, TKPacketChat packet)
        {
            byte[] tkpacket = MessagePackSerializer.Serialize(packet);
            ushort length = (ushort)tkpacket.Length;
            
            // 1. 헤더 생성
            byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)length));
            
            toSocket.Send(header);
            int sendLength = toSocket.Send(tkpacket, 0, length, SocketFlags.None);
        }

        static TKPacketChat RecvPacket(Socket toSocket)
        {
            byte[] headerBuffer = new byte[2];
            int recvLength = clientSocket.Receive(headerBuffer, 2, SocketFlags.None);

            if(recvLength > 0)
            {
                short packetLength = BitConverter.ToInt16(headerBuffer, 0);
                packetLength = IPAddress.NetworkToHostOrder(packetLength);

                byte[] recvBuffer = new byte[packetLength];
                int RecvLength = clientSocket.Receive(recvBuffer, packetLength, SocketFlags.None);
                return MessagePackSerializer.Deserialize<TKPacketChat>(recvBuffer);
            }

            return null;
        }

        public static void ChatInput()
        {
            while (true)
            {
                string InputChat;
                Console.Write("채팅 : ");
                InputChat = Console.ReadLine();

                TKPacketChat chatPacket = new TKPacketChat()
                {
                    Message = InputChat,
                    SendTime = DateTime.Now,
                    UserID = clientGUID,
                    NickName = "console"
                };
                SendPacket(clientSocket, chatPacket);
            }
        }

        public static void RecvThread()
        {
            while (true)
            {
                byte[] lengthBuffer = new byte[2];

                TKPacketChat recvPacket = RecvPacket(clientSocket);

                if(recvPacket != null)
                    Console.WriteLine($"{recvPacket.NickName} : {recvPacket.Message}");
            }
        }

        public static void ClientWithThread()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.22"), 4000);
            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);

            clientSocket.Connect(listenEndPoint);
            byte[] userIdBytes = new byte[36];
            int bytesRead = clientSocket.Receive(userIdBytes);
            clientGUID = Encoding.UTF8.GetString(userIdBytes, 0, bytesRead);
            Console.WriteLine($"Connected with UserID: {clientGUID}");


            Thread chatInputThread = new Thread(new ThreadStart(ChatInput));//TODO PACKET 변경
            Thread recvThread = new Thread(new ThreadStart(RecvThread)); //TODO PACKET 변경
            chatInputThread.IsBackground = true;
            recvThread.IsBackground = true;

            chatInputThread.Start();
            recvThread.Start();

            chatInputThread.Join();
            recvThread.Join();

            clientSocket.Close();
        }

        
    }
}
