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
        public static void Main(string[] args)
        {
            Console.Title = "Client";
            ClientWithThread();
        }


        static void SendPacket(Socket toSocket, TKPacketChat packet)
        {
            byte[] tkpacket = MessagePackSerializer.Serialize(packet);
            ushort length = (ushort)tkpacket.Length;
            int sendLength = toSocket.Send(tkpacket, 0, length, SocketFlags.None);
        }

        static TKPacketChat RecvPacket(Socket toSocket)
        {
            byte[] recvBuffer = new byte[4096];
            int RecvLength = clientSocket.Receive(recvBuffer, recvBuffer.Length, SocketFlags.None);
            return MessagePackSerializer.Deserialize<TKPacketChat>(recvBuffer);
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
                Console.WriteLine($"{recvPacket.NickName} : {recvPacket.Message}");
            }
        }

        public static void ClientWithThread()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.22"), 4000);
            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);

            clientSocket.Connect(listenEndPoint);

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
