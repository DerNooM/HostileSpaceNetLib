using System;
using System.Net;
using System.Net.Sockets;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceNetLib
{
    class Client
    {
        Socket socket;

        Byte[] buffer = new Byte[1024];
        PacketBase packet;


        public Client()
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }


        public void BeginSend(PacketBase Packet)
        {
            try
            {
                socket.BeginSend(Packet.GetBuffer(), 0, (int)Packet.Length, SocketFlags.None, EndSend, null);
            }
            catch(Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("begin send error");
            }
        }

        void EndSend(IAsyncResult ar)
        {
            try
            {
                socket.EndSend(ar);
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("begin send error");
            }
        }

        public void BeginReceive()
        {
            try
            {
                if (socket.Connected)
                {
                    socket.BeginReceive(buffer, 0, 1024, SocketFlags.None, EndReceive, null);
                }
            }
            catch
            {
                Disconnect();
                Console.WriteLine("receive error");
            }
        }

        void EndReceive(IAsyncResult ar)
        {
            try
            {
                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    packet = new PacketBase(buffer, bytesRead);
                    PacketReceieved?.Invoke(this, null);
                }
                else
                {
                    Disconnect();
                }

                BeginReceive();
            }
            catch
            {
                Console.WriteLine("receive error1");
                Disconnect();
            }
        }

        public void Connect(IPAddress ServerIP)
        {
            socket.Connect(ServerIP, 1326);
        }

        public void Disconnect()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }


        public event EventHandler PacketReceieved;

        public PacketBase Packet
        {
            get { return packet; }
        }


    }
}
