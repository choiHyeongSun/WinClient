using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class UserInfoUserControl : UserControl
    {
        public delegate void LogoutSuccess();

        private LogoutSuccess logoutSuccessCallback;
        private BlockSocketWrapper wrapper;

        public UserInfoUserControl(BlockSocketWrapper wrapper, LogoutSuccess logoutSuccess)
        {
            InitializeComponent();
            logoutSuccessCallback = logoutSuccess;
            this.wrapper = wrapper;
        }

        private void UserInfoUserControl_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            lb_nicknameField.Text = NetworkManager.GetNickname();
        }

        private void Logout_Buttn_Click(object sender, EventArgs e)
        {
            LogoutPacket? logoutPacket = PacketManager.GeneratePacket<LogoutPacket>();
            if (logoutPacket == null)
                return;

            btn_logout.Enabled = false;

            int size = Marshal.SizeOf(logoutPacket);
            logoutPacket.packetType = IPAddress.HostToNetworkOrder(logoutPacket.packetType);
            logoutPacket.packetLen = IPAddress.HostToNetworkOrder(size);
            logoutPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)logoutPacket.userLocalId);

            byte[] buffer = PacketManager.StructToByte(logoutPacket, size);
            FormManager.RegisterResultMessage(EMESSAGE_TYPE.LOGOUT_PACKET, LogoutResult);
            wrapper.SendMessage(buffer);
        }

        private void LogoutResult(ResultPacket resultPacket)
        {
            if (resultPacket.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                Invoke(() =>
                {
                    String str = Encoding.UTF8.GetString(resultPacket.ResultMsg);
                    MessageBox.Show(str, "성공", MessageBoxButtons.OK, MessageBoxIcon.None);
                    NetworkManager.Init();
                    logoutSuccessCallback();
                });
            }
            else if (resultPacket.ResultType == (int)EMESSAGE_RESULT.FAILED)
            {

            }
            Invoke(() =>
            {
                btn_logout.Enabled = true;
            });
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.LOGOUT_PACKET, LogoutResult);
        }
    }
}
