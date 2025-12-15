using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Interfaces;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Managers
{
    internal static class FormManager
    {
        #region Result

        private static Dictionary<EMESSAGE_TYPE, List<Action<ResultPacket>>> resultCallback = new();
        private static object resultCallbackLock = new object();
        public static void ExecuteResultMessage(EMESSAGE_TYPE message_type, ResultPacket resultPacket)
        {
            List<Action<ResultPacket>>? snapshot = null;
            lock (resultCallbackLock)
            {
                if (resultCallback.TryGetValue(message_type, out List<Action<ResultPacket>>? originalList))
                {
                    snapshot = originalList.ToList();
                }
            }

            if (snapshot != null)
            {
                foreach (var msg in snapshot)
                {
                    msg?.Invoke(resultPacket);
                }
            }

        }

        public static void RegisterResultMessage(EMESSAGE_TYPE type, Action<ResultPacket> msg)
        {
            lock (resultCallbackLock)
            {
                resultCallback.TryAdd(type, new List<Action<ResultPacket>>());
                resultCallback[type].Add(msg);
            }
        }

        public static void UnregisterResultMessage(EMESSAGE_TYPE type, Action<ResultPacket> msg)
        {
            lock (resultCallbackLock)
            {
                if (resultCallback.TryGetValue(type, out List<Action<ResultPacket>>? msgs))
                {
                    if (msgs.Contains(msg))
                        msgs.Remove(msg);
                }
            }

        }

        #endregion
        #region Array

        private static Dictionary<EMESSAGE_TYPE, List<Action<byte[]>>> arrayCallback = new();
        private static object arrayCallbackLock = new object();
        public static void ExecuteArrayMessage(EMESSAGE_TYPE message_type, byte[] data)
        {
            List<Action<byte[]>>? snapshot = null;

            lock (arrayCallbackLock)
            {
                if (arrayCallback.TryGetValue(message_type, out List<Action<byte[]>>? originalList))
                {
                    snapshot = originalList.ToList();
                }
            }

            if (snapshot != null)
            {
                foreach (var msg in snapshot)
                {
                    msg?.Invoke(data);
                }
            }
        }

        public static void RegisterArrayMessage(EMESSAGE_TYPE type, Action<byte[]> msg)
        {
            lock (arrayCallbackLock)
            {
                arrayCallback.TryAdd(type, new List<Action<byte[]>>());
                arrayCallback[type].Add(msg);
            }
        }

        public static void UnregisterArrayMessage(EMESSAGE_TYPE type, Action<byte[]> msg)
        {
            lock (arrayCallbackLock)
            {
                if (arrayCallback.TryGetValue(type, out List<Action<byte[]>>? msgs))
                {
                    if (msgs.Contains(msg))
                        msgs.Remove(msg);
                }
            }
        }

        #endregion
    }
}
