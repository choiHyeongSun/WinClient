
using WinClient.Sources.Interfaces;
using WinClient.Sources.Managers;
using WinClient.Sources.Other;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Controllers
{
    internal class EntryServerController : SingleTon<EntryServerController>, IRecvMessage, ISingleTon_Init, ISingleTon_Remove
    {
        public void RecvMessage(PacketHeader header, byte[] packet)
        {
            NetworkManager.SetLocalUserID(header.userLocalId);
        }

        public void Initialization()
        {
            ControllerManager.RegisterRecvMessage(EMESSAGE_TYPE.ENTRY_SERVER, this);
        }

        public void Remove()
        {
            ControllerManager.UnregisterRecvMessage(EMESSAGE_TYPE.ENTRY_SERVER, this);
        }
    }
}
