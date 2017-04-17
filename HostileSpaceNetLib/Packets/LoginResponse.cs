using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostileSpaceNetLib.Packets
{
    public class LoginResponse
    {
        public enum Responses { Unknown, Accepted, UsernameNotFound, InvalidPassword }

        PacketBase packet;
    
        Responses response = Responses.Unknown;


        public LoginResponse(Responses Response)
        {
            packet = new PacketBase(PacketID.LoginResponse);

            packet.Writer.Write((Int32)Response);
        }

        public LoginResponse(PacketBase Packet)
        {
            packet = Packet;

            response = (Responses)packet.Reader.ReadInt32();
        }


        public Responses Response
        {
            get { return response; }
        }

        public PacketBase Packet
        {
            get { return packet; }
        }


    }
}
