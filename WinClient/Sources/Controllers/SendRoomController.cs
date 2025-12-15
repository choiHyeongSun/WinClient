using WinClient.Sources.Interfaces;
using WinClient.Sources.Managers;
using WinClient.Sources.Other;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Controllers
{
    internal class SendRoomController : SingleTon<SendRoomController>, IRecvMessage, ISingleTon_Init, ISingleTon_Remove
    {
        public void RecvMessage(PacketHeader header, byte[] packet)
        {
            FormManager.ExecuteArrayMessage((EMESSAGE_TYPE)header.packetType, packet);
        }

        public void Initialization()
        {
            ControllerManager.RegisterRecvMessage(EMESSAGE_TYPE.SEND_ROOM, this);
        }

        public void Remove()
        {
            ControllerManager.UnregisterRecvMessage(EMESSAGE_TYPE.SEND_ROOM, this);
        }
    }
}
