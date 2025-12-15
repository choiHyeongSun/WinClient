using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Interfaces
{
    public interface IRecvMessage
    {
        public abstract void RecvMessage(PacketHeader header, byte[] packet);
    }
}
