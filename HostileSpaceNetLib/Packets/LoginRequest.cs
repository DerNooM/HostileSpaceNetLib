using System;


namespace HostileSpaceNetLib.Packets
{
    [Serializable]
    public class LoginRequest : Packet
    {
        String username = "";
        Byte[] password = new Byte[32];


        public LoginRequest()
            : base(PacketID.LoginRequest)
        {
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
