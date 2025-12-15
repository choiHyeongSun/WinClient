using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Interfaces;
using WinClient.Sources.Managers;
using WinClient.Sources.Other;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Controllers
{
    internal class EntryRoomMemberController : SingleTon<EntryRoomMemberController>, IRecvMessage, ISingleTon_Init, ISingleTon_Remove
    {
        public void RecvMessage(PacketHeader header, byte[] packet)
        {
            FormManager.ExecuteArrayMessage((EMESSAGE_TYPE)header.packetType, packet);
        }

        public void Initialization()
        {
            ControllerManager.RegisterRecvMessage(EMESSAGE_TYPE.ENTRY_MEMBER, this);
        }

        public void Remove()
        {
            ControllerManager.UnregisterRecvMessage(EMESSAGE_TYPE.ENTRY_MEMBER, this);
        }
    }
}
