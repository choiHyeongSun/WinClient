using System;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Utilities;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class CreateRoomForm : Form
    {
        private BlockSocketWrapper wrapper;
        private Action<int> openRoom;
        public CreateRoomForm(BlockSocketWrapper wrapper, Action<int> openRoom)
        {
            InitializeComponent();
            this.wrapper = wrapper;
            this.openRoom = openRoom;
        }

        private void btn_Complate_Click(object sender, EventArgs e)
        {
            if (tb_roomName.Text == "") return;
            CreateRoomPacket? packet = PacketManager.GeneratePacket<CreateRoomPacket>();
            if (packet == null) return;
            int size = Marshal.SizeOf(packet);

            packet.packetType = IPAddress.HostToNetworkOrder(packet.packetType);
            packet.packetLen = IPAddress.HostToNetworkOrder(size);
            packet.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)packet.userLocalId);

            ConvertUtility.ByteConvertByString(ref packet.RoomName, tb_roomName.Text);
            if (tb_password.Text != "")
            {
                ConvertUtility.ByteConvertByString(ref packet.Password, tb_password.Text);
            }

            byte[] buffer = PacketManager.StructToByte(packet, size);
            FormManager.RegisterResultMessage(EMESSAGE_TYPE.CREATE_ROOM, CreateRoomResult);
            wrapper.SendMessage(buffer);
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateRoomResult(ResultPacket msg)
        {
            if (msg.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                Invoke(() =>
                {
                    int roomID = int.Parse(Encoding.UTF8.GetString(msg.ResultMsg));
                    EntryRoomPacket? entryRoomPacket = PacketManager.GeneratePacket<EntryRoomPacket>();
                    if (entryRoomPacket == null) return;
                    int size = Marshal.SizeOf<EntryRoomPacket>();
                    entryRoomPacket.packetType = IPAddress.HostToNetworkOrder(entryRoomPacket.packetType);
                    entryRoomPacket.packetLen = IPAddress.HostToNetworkOrder(size);
                    entryRoomPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)NetworkManager.GetLocalID());
                    entryRoomPacket.RoomId = (uint)IPAddress.HostToNetworkOrder(roomID);
                    ConvertUtility.ByteConvertByString(ref entryRoomPacket.Password, tb_password.Text);

                    byte[] buffer = PacketManager.StructToByte(entryRoomPacket, size);
                    FormManager.RegisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM, EntryRoomResult);
                    wrapper.SendMessage(buffer);
                });
            }
            if (msg.ResultType == (int)EMESSAGE_RESULT.FAILED)
            {
                string str = Encoding.UTF8.GetString(msg.ResultMsg);
                MessageBox.Show(str, "실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.CREATE_ROOM, CreateRoomResult);
        }



        private void EntryRoomResult(ResultPacket resultPacket)
        {
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM, EntryRoomResult);
            if (resultPacket.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                int roomID = int.Parse(Encoding.UTF8.GetString(resultPacket.ResultMsg));
                Invoke(() =>
                {
                    openRoom?.Invoke(roomID);
                    DialogResult = DialogResult.OK;
                    Close();
                });
            }
            else if (resultPacket.ResultType == (int)EMESSAGE_RESULT.FAILED)
            {
                Invoke(() =>
                {
                    String msg = Encoding.UTF8.GetString(resultPacket.ResultMsg);
                    MessageBox.Show(msg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }
    }
}
