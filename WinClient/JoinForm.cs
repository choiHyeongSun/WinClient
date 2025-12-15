using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Utilities;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class JoinForm : Form
    {
        private BlockSocketWrapper socketWrapper;
        public JoinForm(BlockSocketWrapper socketWrapper)
        {
            InitializeComponent();
            this.socketWrapper = socketWrapper;

            tb_password.PasswordChar = '*';
            tb_password.UseSystemPasswordChar = true;

            tb_password.PlaceholderText = "Password";
            tb_userID.PlaceholderText = "UserID";
            tb_nickname.PlaceholderText = "userNickname";
        }

        void Cancel_Button_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        void Complate_Button_Click(object sender, EventArgs e)
        {
            if (tb_password.Text == "" || tb_userID.Text == "" || tb_nickname.Text == "")
            {
                MessageBox.Show("user id, password, nickname을 모두 작성 해주세요.", "경고",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            JoinPacket? joinPacket = PacketManager.GeneratePacket<JoinPacket>();

            if (joinPacket == null) return;

            int size = Marshal.SizeOf(joinPacket);
            joinPacket.packetType = IPAddress.HostToNetworkOrder(joinPacket.packetType);
            joinPacket.packetLen = IPAddress.HostToNetworkOrder(size);
            joinPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)joinPacket.userLocalId);

            ConvertUtility.ByteConvertByString(ref joinPacket.UserID, tb_userID.Text);
            ConvertUtility.ByteConvertByString(ref joinPacket.Nickname, tb_nickname.Text);
            ConvertUtility.ByteConvertByString(ref joinPacket.Password, tb_password.Text);


            byte[] buffer = PacketManager.StructToByte(joinPacket, size);
            FormManager.RegisterResultMessage(EMESSAGE_TYPE.JOIN_PACKET, RecvToJoinResult);
            socketWrapper.SendMessage(buffer);
        }

        void JoinForm_FormClosed(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void JoinForm_Load(object sender, EventArgs e)
        {

        }


        void RecvToJoinResult(ResultPacket msg)
        {
            if (msg.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                string str = Encoding.UTF8.GetString(msg.ResultMsg);
                MessageBox.Show(str, "성공", MessageBoxButtons.OK, MessageBoxIcon.None);
                Invoke(() =>
                {
                    DialogResult = DialogResult.OK;
                    this.Close();
                });
            }
            if (msg.ResultType == (int)EMESSAGE_RESULT.FAILED)
            {
                string str = Encoding.UTF8.GetString(msg.ResultMsg);
                MessageBox.Show(str, "실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.JOIN_PACKET, RecvToJoinResult);
        }
    }
}
