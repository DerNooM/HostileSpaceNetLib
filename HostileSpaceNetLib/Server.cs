using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using HostileSpaceNetLib.Packets;

namespace HostileSpaceNetLib
{
    public class Server
    {
        TcpListener listener;
        IPAddress address = IPAddress.Any;

        HashSet<Client> clients = new HashSet<Client>();
        readonly object syncRoot = new object();


        public Server()
        {

        }


        public void Start()
        {
            listener = new TcpListener(IPAddress.Any, 1326);
            listener.Start(100);

            BeginAcceptSocket();
        }

        public void Stop()
        {
            listener.Stop();
            listener = null;

            lock (syncRoot)
            {
                foreach (Client client in clients)
                { 
                    client.Disconnect();
                }
            }

            clients.Clear();
        }

        public void Disconnect(Client Client)
        {
            lock (syncRoot)
            {
                clients.Remove(Client);
            }
        }

        void BeginAcceptSocket()
        {
            if (listener == null)
                return;

            try
            {
                listener.BeginAcceptSocket(
                    new AsyncCallback(EndAcceptSocket),
                    listener);
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("beginacceptsocket error");
            }
        }

        void EndAcceptSocket(IAsyncResult ar)
        {
            if (listener == null)
                return;

            try
            {
                Socket socket = listener.EndAcceptSocket(ar);

                Client client = new Client(socket);               

                lock (syncRoot)
                {
                    clients.Add(client);
                }

                client.PacketReceieved += Client_PacketReceieved;
                client.Disconnected += Client_Disconnected;

                BeginAcceptSocket();
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
                Console.WriteLine("endacceptsocket error");
            }
        }

        private void Client_PacketReceieved(object sender, EventArgs e)
        {
            Client client = (Client)sender;

            if (client.Packet.ID == PacketID.Ping)
            {
                Packet pong = new Packet(PacketID.Ping);
                client.BeginSend(pong);
            }
            else if(client.Packet.ID == Packets.PacketID.Unknown)
            {
                Console.WriteLine("Unknown packet receieved. that should not happen :D");
            }
            else
            {
                if (Enum.IsDefined(typeof(PacketID), client.Packet.ID))
                {
                    PacketReceieved?.Invoke(client, null);
                }
                else
                {
                    Console.WriteLine("unhandled packet receieved. disconnecting client.");
                    Disconnect(client);
                }
            }
        }

        private void Client_Disconnected(object sender, EventArgs e)
        {          
            Disconnect((Client)sender);
            sender = null;
        }


        public event EventHandler PacketReceieved;

        public TcpListener Listener
        {
            get { return listener; }
        }


    }
}
