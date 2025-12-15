using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Attributes;
using WinClient.Sources.Interfaces;
using WinClient.Sources.Managers;
using WinClient.Sources.Other;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Controllers
{
    internal class ResultController : SingleTon<ResultController> ,IRecvMessage, ISingleTon_Init, ISingleTon_Remove
    {
        public void RecvMessage(PacketHeader header, byte[] packet)
        {
            ResultPacket result = PacketManager.ByteToStruct<ResultPacket>(packet, header.packetLen);
            result.PrevMessageType = IPAddress.NetworkToHostOrder(result.PrevMessageType);
            result.ResultType = IPAddress.NetworkToHostOrder(result.ResultType);
            result.userLocalId = (uint)IPAddress.NetworkToHostOrder((int)result.userLocalId);

            FormManager.ExecuteResultMessage((EMESSAGE_TYPE)result.PrevMessageType, result);
        }

        public void Initialization()
        {
            ControllerManager.RegisterRecvMessage(EMESSAGE_TYPE.RESULT_MESSAGE, this);
        }

        public void Remove()
        {
            ControllerManager.UnregisterRecvMessage(EMESSAGE_TYPE.RESULT_MESSAGE, this);
        }
    }
}
