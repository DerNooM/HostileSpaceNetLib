using System;


namespace HostileSpaceNetLib.Packets
{
    class LoginRequest : PacketBase
    {
        String username = "";
        Byte[] password = new Byte[32];


        public LoginRequest()
            : base(PacketID.LoginRequest)
        {

        }


        public override void ReadData()
        {
            username = Reader.ReadString();
            password = Reader.ReadBytes(32);
        }

        public override void WriteData()
        {
            Writer.Write(username);
            Writer.Write(password);
        }


        public String Username
        {
            get { return username; }
            set { username = value; }
        }

        public Byte[] Password
        {
            get { return password; }
            set { password = value; }
        }


    }
}
