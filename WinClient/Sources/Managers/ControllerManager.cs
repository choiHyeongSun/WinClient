using WinClient.Sources.Controllers;
using WinClient.Sources.Interfaces;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Managers
{
    internal static class ControllerManager
    {
        private static Dictionary<EMESSAGE_TYPE, List<IRecvMessage>> recvMessages = new();
        private static object recvMessagesLock = new object();

        public static void CreateControllers()
        {
            ResultController.Create();
            EntryServerController.Create();
            SendRoomController.Create();
            EntryRoomMemberController.Create();
            ExitRoomMemberController.Create();
            SendMessageController.Create();
        }

        public static void DestroyControllers()
        {
            ResultController.Destroy();
            EntryServerController.Destroy();
            SendRoomController.Destroy();
            EntryRoomMemberController.Destroy();
            ExitRoomMemberController.Destroy();
            SendMessageController.Destroy();
        }

        public static void ExecuteRecvMessage(EMESSAGE_TYPE message_type, PacketHeader header, byte[] packet)
        {

            List<IRecvMessage> snapshot = null; 
            lock (recvMessagesLock)
            {
                if (recvMessages.TryGetValue(message_type, out List<IRecvMessage>? originalList))
                {
                    snapshot = originalList.ToList();
                }
            }

            if (snapshot != null)
            {
                foreach (var msg in snapshot)
                {
                    msg.RecvMessage(header, packet);
                }
            }
        }

        public static void RegisterRecvMessage(EMESSAGE_TYPE type ,IRecvMessage msg)
        {
            lock (recvMessagesLock)
            {
                recvMessages.TryAdd(type, new List<IRecvMessage>());
                recvMessages[type].Add(msg);
            }
        }

        public static void UnregisterRecvMessage(EMESSAGE_TYPE type, IRecvMessage msg)
        {
            lock (recvMessagesLock)
            {
                if (recvMessages.TryGetValue(type, out List<IRecvMessage>? msgs))
                {
                    if (msgs.Contains(msg))
                        msgs.Remove(msg);
                }
            }
            
        }

        public static void Clear()
        {
            lock (recvMessagesLock)
            {
                recvMessages.Clear();
            }
        }
    }
}
