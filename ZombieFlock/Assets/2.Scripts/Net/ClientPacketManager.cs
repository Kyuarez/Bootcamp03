using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System;

public class ClientPacketManager : MonoSingleton<ClientPacketManager>
{
    private Socket serverSocket;
    private IPEndPoint serverEndPoint;

    private Thread recvThread;

    private Queue<string> chatQueue;

    private bool isConnected = false;

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

                recvThread = new Thread(new ThreadStart(RecvPacket));
                recvThread.IsBackground = true;
                recvThread.Start();
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
            }

            isConnected = value;
        }
    }

    
    protected override void Awake()
    {
        base.Awake();
        chatQueue = new Queue<string>();
    }

    private void Update()
    {
        if (chatQueue.Count > 0)
        {
            string message = chatQueue.Dequeue();
        }
    }

    //@TODO 이거 이제 메세지 팩으로 처리
    private void RecvPacket()
    {
        while (true)
        {
            //byte[] lengthBuffer = new byte[2];

            //int RecvLength = serverSocket.Receive(lengthBuffer, 2, SocketFlags.None);
            //ushort length = BitConverter.ToUInt16(lengthBuffer, 0);
            //length = (ushort)IPAddress.NetworkToHostOrder((short)length);
            //byte[] recvBuffer = new byte[4096];
            //RecvLength = serverSocket.Receive(recvBuffer, length, SocketFlags.None);

            //string jsonString = Encoding.UTF8.GetString(recvBuffer);
            //JObject clientData = JObject.Parse(jsonString);
            //string code = clientData.Value<String>("code");

            //if (code == "Chat")
            //{
            //    //TODO 이거 패킷 자체를 처리하는 걸로 하자
            //    string id = clientData.Value<String>("id");
            //    string message = clientData.Value<String>("message");
            //    string data = $"[{id}] : {message}";
            //    chatQueue.Enqueue(data);
            //}

            //Debug.Log(jsonString);
            //Thread.Sleep(10);
            //Parsing
        }
    }

    //TODO : 이거 이제 메세지 팩으로 처리
    private void SendPacket(string message)
    {
        byte[] messageBuffer = Encoding.UTF8.GetBytes(message);
        ushort length = (ushort)IPAddress.HostToNetworkOrder((short)messageBuffer.Length);

        byte[] headerBuffer = BitConverter.GetBytes(length);

        byte[] packetBuffer = new byte[headerBuffer.Length + messageBuffer.Length];
        Buffer.BlockCopy(headerBuffer, 0, packetBuffer, 0, headerBuffer.Length);
        Buffer.BlockCopy(messageBuffer, 0, packetBuffer, headerBuffer.Length, messageBuffer.Length);
        int SendLength = serverSocket.Send(packetBuffer, packetBuffer.Length, SocketFlags.None);

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
    }
}
