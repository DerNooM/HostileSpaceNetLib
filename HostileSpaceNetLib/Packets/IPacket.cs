using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostileSpaceNetLib.Packets
{
    public interface IPacket
    {
        PacketID ID { get; }
    }
}
