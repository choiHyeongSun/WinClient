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
    internal class SendMessageController : SingleTon<SendMessageController>, IRecvMessage, ISingleTon_Init, ISingleTon_Remove
    {
        public void RecvMessage(PacketHeader header, byte[] packet)
        {
            FormManager.ExecuteArrayMessage((EMESSAGE_TYPE)header.packetType, packet);
        }

        public void Initialization()
        {
            ControllerManager.RegisterRecvMessage(EMESSAGE_TYPE.SEND_MESSAGE, this);
        }

        public void Remove()
        {
            ControllerManager.UnregisterRecvMessage(EMESSAGE_TYPE.SEND_MESSAGE, this);
        }
    }
}
