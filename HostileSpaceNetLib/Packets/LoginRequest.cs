using System;


namespace HostileSpaceNetLib.Packets
{
    public class LoginRequest
    {
        PacketBase packet;

        String username = "";
        Byte[] password = new Byte[32];


        public LoginRequest(String Username, Byte[] Password)
        {
            packet = new PacketBase(PacketID.LoginRequest);

            packet.Writer.Write(Username);
            packet.Writer.Write(Password);
        }

        public LoginRequest(PacketBase Packet)
        {
            packet = Packet;

            username = packet.Reader.ReadString();
            password = packet.Reader.ReadBytes(32);
        }


        public String Username
        {
            get { return username; }
        }

        public Byte[] Password
        {
            get { return password; }
        }

        public PacketBase Packet
        {
            get { return packet; }
        }


    }
}
