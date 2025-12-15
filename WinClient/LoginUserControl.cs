using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Utilities;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class LoginUserControl : UserControl
    {
        public delegate void LoginSuccess_Delegate();

        private BlockSocketWrapper socketWrapper;
        private LoginSuccess_Delegate loginSuccessCallback;

        public LoginUserControl(BlockSocketWrapper socketWrapper, LoginSuccess_Delegate loginSuccessCallback)
        {
            InitializeComponent();
            this.socketWrapper = socketWrapper;
            this.loginSuccessCallback = loginSuccessCallback;
        }

        private void LoginPanel_Load(object sender, EventArgs e)
        {
            tb_password.PasswordChar = '*';
            tb_password.UseSystemPasswordChar = true;
        }

        private void Join_Buttn_Click(object sender, EventArgs e)
        {
            JoinForm joinForm = new JoinForm(socketWrapper);
            joinForm.ShowDialog();
        }

        private void Login_Buttn_Click(object sender, EventArgs e)
        {
            if (tb_userID.Text == "") return;
            if (tb_password.Text == "") return;

            tb_userID.Enabled = false;
            tb_password.Enabled = false;
            btn_join.Enabled = false;
            btn_login.Enabled = false;

            LoginPacket? loginPacket = PacketManager.GeneratePacket<LoginPacket>();
            if (loginPacket == null) return;
            int size = Marshal.SizeOf(loginPacket);

            loginPacket.packetType = IPAddress.HostToNetworkOrder(loginPacket.packetType);
            loginPacket.packetLen = IPAddress.HostToNetworkOrder(size);
            loginPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)loginPacket.userLocalId);

            ConvertUtility.ByteConvertByString(ref loginPacket.UserID, tb_userID.Text);
            ConvertUtility.ByteConvertByString(ref loginPacket.Password, tb_password.Text);

            byte[] buffer = PacketManager.StructToByte(loginPacket, size);
            FormManager.RegisterResultMessage(EMESSAGE_TYPE.LOGIN_PACKET, LoginResult);
            socketWrapper.SendMessage(buffer);
        }

        private void LoginResult(ResultPacket resultPacket)
        {
            if (resultPacket.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                Invoke(() =>
                {
                    NetworkManager.SetNickname(tb_userID.Text);
                    NetworkManager.SetLocalLogin(true);
                    loginSuccessCallback();
                });
            }
            else if (resultPacket.ResultType == (int)EMESSAGE_RESULT.FAILED)
            {
                String str = Encoding.UTF8.GetString(resultPacket.ResultMsg);
                MessageBox.Show(str, "실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Invoke(() =>
            {
                tb_userID.Enabled = true;
                tb_password.Enabled = true;
                btn_join.Enabled = true;
                btn_login.Enabled = true;
            });
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.LOGIN_PACKET, LoginResult);
        }


        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            tb_password.Text = "";
            tb_userID.Text = "";
        }
    }
}
