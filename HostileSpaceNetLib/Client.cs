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

        UInt32 totalBytesUp = 0;
        UInt32 totalBytesDown = 0;
        UInt16 bytesSecUp = 0;
        UInt16 bytesSecDown = 0;
        readonly Object syncRoot = new Object();


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

                    lock (syncRoot)
                    {
                        totalBytesUp += (UInt32)stream.Length;
                        bytesSecUp += (UInt16)stream.Length;
                    }
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

                lock (syncRoot)
                {
                    totalBytesDown += (UInt32)bytesRead;
                    bytesSecDown += (UInt16)bytesRead;
                }

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

        public IPacket Packet
        {
            get { return packet; }
        }

        public UInt32 TotalBytesUp
        {
            get { return totalBytesUp; }
        }

        public UInt32 TotalBytesDown
        {
            get { return totalBytesDown; }
        }

        public UInt16 BytesSecUp
        {
            get { return bytesSecUp; }
            set { bytesSecUp = value; }
        }

        public UInt16 BytesSecDown
        {
            get { return bytesSecDown; }
            set { bytesSecDown = value; }
        }


    }
}
