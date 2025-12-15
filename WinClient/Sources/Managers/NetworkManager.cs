
namespace WinClient.Sources.Managers
{
    internal static class NetworkManager
    {
        private static uint localUserID = 0;
        private static object userIDLock = new object();

        private static bool isLogin = false;
        private static object loginLock = new object();

        private static string nickname;
        private static object nicknameLock = new object();

        public static void Init()
        {
            SetLocalUserID(0);
            SetLocalLogin(false);
            SetNickname("");
        }
        public static void SetLocalUserID(uint id)
        {
            lock (userIDLock)
            {
                localUserID = id;
            }
        }

        public static void SetLocalLogin(bool login)
        {
            lock (loginLock)
            {
                isLogin = login;
            }
        }
        public static void SetNickname(string newNickname)
        {
            lock (nicknameLock)
            {
                nickname = newNickname;
            }
        }
        public static string GetNickname() => nickname;
        public static uint GetLocalID() => localUserID;
        public static bool IsLogin() => isLogin;
    }
}
