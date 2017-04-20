using System;
using SFML.System;

namespace HostileSpaceNetLib.Packets
{
    public class UpdateShip
    {
        public class ShipData
        {
            public Byte ID { get; set; }

            public Vector2f Position { get; set; }
            public Vector2f Destination { get; set; }
            public Single Rotation { get; set; }

            public UInt16 Shield { get; set; }
            public UInt16 Armor { get; set; }

        }

        PacketBase packet;
        ShipData data = new ShipData();


        public UpdateShip(ShipData Data)
        {
            packet = new PacketBase(PacketID.UpdateShip);

            packet.Writer.Write(Data.ID);

            packet.Writer.Write(Data.Position.X);
            packet.Writer.Write(Data.Position.Y);

            packet.Writer.Write(Data.Destination.X);
            packet.Writer.Write(Data.Destination.Y);

            packet.Writer.Write(Data.Rotation);

            packet.Writer.Write(Data.Shield);
            packet.Writer.Write(Data.Armor);
        }

        public UpdateShip(PacketBase Packet)
        {
            packet = Packet;

            data.ID = packet.Reader.ReadByte();

            Vector2f tmp = new Vector2f(0, 0);
            tmp.X = packet.Reader.ReadSingle();
            tmp.Y = packet.Reader.ReadSingle();
            data.Position = tmp;

            tmp = new Vector2f(0, 0);
            tmp.X = packet.Reader.ReadSingle();
            tmp.Y = packet.Reader.ReadSingle();
            data.Destination = tmp;

            data.Rotation = packet.Reader.ReadSingle();

            data.Shield = packet.Reader.ReadUInt16();
            data.Armor = packet.Reader.ReadUInt16();
        }


        public ShipData Data
        {
            get { return data; }
        }

        public PacketBase Packet
        {
            get { return packet; }
        }


    }
}
