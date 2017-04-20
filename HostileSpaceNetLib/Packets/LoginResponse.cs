using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostileSpaceNetLib.Packets
{
    [Serializable]
    public class LoginResponse : Packet
    {
        public enum Responses { Unknown,
            Accepted, InvalidPassword, AccountCreated }
    
        Responses response = Responses.Unknown;


        public LoginResponse()
            : base(PacketID.LoginResponse)
        {
        }


        public Responses Response
        {
            get { return response; }
            set { response = value; }
        }


    }
}
