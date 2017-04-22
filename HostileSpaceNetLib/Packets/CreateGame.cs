using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostileSpaceNetLib.Packets
{
    [Serializable]
    public class CreateGame : Packet
    {
        public enum GameType
        {
            SinglePlayer,
            CoopA,
            CoopB,
            VSA,
            VSB
        }

        public CreateGame()
            : base(PacketID.CreateGame)
        {

        }


    }
}
