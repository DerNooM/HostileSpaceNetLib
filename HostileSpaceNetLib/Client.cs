using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Net.Sockets;
using HostileSpaceNetLib.Packets;


namespace HostileSpaceNetLib
{
    public class Client
    {
        Socket socket;

        Byte[] buffer = new Byte[1024];
        IPacket packet;

        Boolean loggedIn = false;


        public Client()
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public Client(Socket Socket)
        {
            socket = Socket;

            BeginReceive();
        }


        public void BeginSend(IPacket Packet)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, Packet);

                    socket.BeginSend(stream.GetBuffer(), 0, (Int32)stream.Length, SocketFlags.None, EndSend, null);
                }
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
                    packet = null;

                    using (MemoryStream stream = new MemoryStream(buffer))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();

                        try
                        {
                            packet = (IPacket)formatter.Deserialize(stream);
                        }
                        catch(Exception E)
                        {
                            Console.WriteLine(E.Message);
                            Console.WriteLine("deserialization problem");
                        }
                    }

                    if(packet != null)
                        PacketReceieved?.Invoke(this, null);
                }
                else
                {
                    Disconnect();
                }

                BeginReceive();
            }
            catch// (Exception E)
            {
                // noom: dont need an errormessage here
                //Console.WriteLine(E.Message);
                //Console.WriteLine("endreceieve error");

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


        public event EventHandler PacketReceieved;
        public event EventHandler Disconnected;

        public Boolean Connected
        {
            get { return socket.Connected; }
        }

        public Boolean LoggedIn
        {
            get { return loggedIn; }
            set { loggedIn = value; }
        }

        public IPacket Packet
        {
            get { return packet; }
        }


    }
}
