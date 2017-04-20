using System;
using System.IO;


namespace HostileSpaceNetLib.Packets
{
    [Serializable]
    public class Packet : IPacket
    {
        PacketID id = PacketID.Unknown;


        public Packet(PacketID PacketID)
        {
            id = PacketID;
        }


        public PacketID ID
        {
            get { return id; }
        }


    }
}
