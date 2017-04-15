﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HostileSpaceNetLib.Packets
{
    class PacketBase : MemoryStream
    {
        PacketID id = PacketID.Unknown;

        BinaryReader reader;
        BinaryWriter writer;


        public PacketBase(PacketID PacketID)
        {
            id = PacketID;

            writer = new BinaryWriter(this);
            writer.Write((Int32)PacketID);
        }

        public PacketBase(Byte[] Buffer, Int32 Lenght)
        {
            this.Write(Buffer, 0, Lenght);
            this.Seek(0, SeekOrigin.Begin);

            reader = new BinaryReader(this);
            id = (PacketID)reader.ReadInt32();
        }


        public virtual void ReadData()
        {
        }


        public virtual void WriteData()
        {
        }


        public PacketID ID
        {
            get { return id; }
        }

        public BinaryReader Reader
        {
            get { return reader; }
        }

        public BinaryWriter Writer
        {
            get { return writer; }
        }
    }
}
