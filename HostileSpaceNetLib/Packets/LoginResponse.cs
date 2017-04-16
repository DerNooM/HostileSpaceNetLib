using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostileSpaceNetLib.Packets
{
    class LoginResponse : PacketBase
    {
        public enum Responses { Unknown, Accepted, UsernameNotFound, InvalidPassword }

        Responses response = Responses.Unknown;


        public LoginResponse()
            : base(PacketID.LoginResponse)
        {

        }


        public override void ReadData()
        {
            response = (Responses)Reader.ReadInt32();
        }

        public override void WriteData()
        {
            Writer.Write((Int32)response);
        }


        public Responses Response
        {
            get { return response; }
            set { response = value; }
        }


    }
}
