using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System;
using MessagePack;

public class ClientPacketManager : MonoSingleton<ClientPacketManager>
{


    private Socket serverSocket;
    private IPEndPoint serverEndPoint;
    private string userID = string.Empty;

    private Thread recvThread;

    private Queue<TKPacketChat> chatQueue;

    private bool isConnected = false;

    public string UserID
    {
        get { return userID; }
    }

    public bool IsConnected
    {
        get { return isConnected; }
        set
        {
            if(value == true)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);
                serverSocket.Connect(serverEndPoint);

                //UserID 부여
                byte[] userIdBytes = new byte[36];
                int bytesRead = serverSocket.Receive(userIdBytes);
                userID = Encoding.UTF8.GetString(userIdBytes, 0, bytesRead);

                recvThread = new Thread(new ThreadStart(RecvPacket));
                recvThread.IsBackground = true;
                recvThread.Start();

                UIManager.Chat?.OnOffChat(true);
            }
            else //
            {
                if (recvThread != null)
                {
                    recvThread.Abort();
                }

                if (serverSocket != null)
                {
                    serverSocket.Shutdown(SocketShutdown.Both);
                    serverSocket.Close();
                }

                userID = string.Empty;
                UIManager.Chat?.OnOffChat(false);
            }

            isConnected = value;
        }
    }

    
    protected override void Awake()
    {
        base.Awake();
        chatQueue = new Queue<TKPacketChat>();

    }

    private void Update()
    {
        if(IsConnected == false)
        {
            return;
        }

        if (chatQueue.Count > 0)
        {
            var packet = chatQueue.Dequeue();
            UIManager.Chat.UpdateChatLog(packet);
        }
    }

    //@TODO 이거 이제 메세지 팩으로 처리
    private void RecvPacket()
    {
        while (true)
        {
            byte[] lengthBuffer = new byte[2];

            int RecvLength = serverSocket.Receive(lengthBuffer, 2, SocketFlags.None);
            ushort length = BitConverter.ToUInt16(lengthBuffer, 0);
            length = (ushort)IPAddress.NetworkToHostOrder((short)length);

            byte[] recvBuffer = new byte[length];
            RecvLength = serverSocket.Receive(recvBuffer, length, SocketFlags.None);

            var packet = MessagePackSerializer.Deserialize<TKPacketChat>(recvBuffer);

            chatQueue.Enqueue(packet);
            Thread.Sleep(10);
        }
    }

    //TODO : 이거 이제 메세지 팩으로 처리
    public bool SendPacket(TKPacketChat packet)
    {
        byte[] tkpacket = MessagePackSerializer.Serialize(packet);
        ushort length = (ushort)tkpacket.Length;

        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)length));

        serverSocket.Send(header);
        int sendLength = serverSocket.Send(tkpacket, 0, length, SocketFlags.None);

        return sendLength > 0;
    }

    private void OnApplicationQuit()
    {
        if (recvThread != null)
        {
            recvThread.Abort();
        }

        if (serverSocket != null)
        {
            serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
        }

        userID = string.Empty;

    }
}
