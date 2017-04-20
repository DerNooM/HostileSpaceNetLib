using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.System;

namespace HostileSpaceNetLib.Packets
{
    public class SetDestination
    {
        PacketBase packet;

        Vector2f destination = new Vector2f(0, 0);

        public SetDestination(Vector2f Destination)
        {
            packet = new PacketBase(PacketID.SetDestination);

            packet.Writer.Write(Destination.X);
            packet.Writer.Write(Destination.Y);
        }

        public SetDestination(PacketBase Packet)
        {
            packet = Packet;

            destination.X = packet.Reader.ReadSingle();
            destination.Y = packet.Reader.ReadSingle();
        }


        public Vector2f Destination
        {
            get { return destination; }
        }

        public PacketBase Packet
        {
            get { return packet; }
        }


    }
}
