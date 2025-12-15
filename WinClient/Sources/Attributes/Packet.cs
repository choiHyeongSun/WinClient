using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    internal class Packet : Attribute
    {
        public Packet(EMESSAGE_TYPE messageType)
        {
            this.messageType = messageType;
        }
        public EMESSAGE_TYPE messageType { get; private set; }
    }
}
