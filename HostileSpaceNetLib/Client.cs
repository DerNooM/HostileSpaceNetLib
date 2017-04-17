using System;
using System.Net;
using System.Net.Sockets;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceNetLib
{
    public class Client
    {
        Socket socket;

        Byte[] buffer = new Byte[1024];
        PacketBase packet;


        public Client()
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public Client(Socket Socket)
        {
            socket = Socket;

            BeginReceive();
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
                Console.WriteLine("beginsend error");

                Disconnect();
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
                Console.WriteLine("endsend error");

                Disconnect();
            }
        }

        void BeginReceive()
        {
            try
            {
                if (socket.Connected)
                {
                    socket.BeginReceive(buffer, 0, 1024, SocketFlags.None, EndReceive, null);
                }
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("beginreceieve error");

                Disconnect();
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
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("endreceieve error");

                Disconnect();
            }
        }

        public void Connect(IPAddress ServerIP)
        {
            try
            {
                socket.Connect(ServerIP, 1326);
                BeginReceive();
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("connect error");

                Disconnect();
            }
        }

        public void Disconnect()
        {
            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("disconnect error");
            }

            Disconnected?.Invoke(this, null);
        }

        public Boolean Connected
        {
            get { return socket.Connected; }
        }
        


        public event EventHandler PacketReceieved;
        public event EventHandler Disconnected;

        public PacketBase Packet
        {
            get { return packet; }
        }


    }
}
