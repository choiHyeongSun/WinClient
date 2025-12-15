using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Utilities;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class PasswordInput : Form
    {
        private int roomID = 0;
        private string roomTitle;
        private BlockSocketWrapper socketWrapper;
        public PasswordInput(BlockSocketWrapper socketWrapper, int roomID, string roomTitle)
        {
            this.roomID = roomID;
            this.socketWrapper = socketWrapper;
            this.roomTitle = roomTitle;
            InitializeComponent();
        }

        private void PasswordInput_Load(object sender, EventArgs e)
        {

        }

        private void btn_compare_click(object sender, EventArgs e)
        {
            EntryRoomPacket? entryRoom = PacketManager.GeneratePacket<EntryRoomPacket>();
            if (entryRoom == null) return;
            int size = Marshal.SizeOf<EntryRoomPacket>();
            entryRoom.packetType = IPAddress.HostToNetworkOrder(entryRoom.packetType);
            entryRoom.packetLen = IPAddress.HostToNetworkOrder(size);
            entryRoom.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)NetworkManager.GetLocalID());
            entryRoom.RoomId = (uint)IPAddress.HostToNetworkOrder(roomID);
            ConvertUtility.ByteConvertByString(ref entryRoom.Password, tb_password.Text);
            byte[] buffer = PacketManager.StructToByte(entryRoom, size);
            FormManager.RegisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM, EntryRoomResult);
            socketWrapper.SendMessage(buffer);
        }

        private void EntryRoomResult(ResultPacket resultPacket)
        {
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM, EntryRoomResult);
            if (resultPacket.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                int roomID = int.Parse(Encoding.UTF8.GetString(resultPacket.ResultMsg));
                Invoke(() =>
                {
                    ChattingRoomOpen(roomID, roomTitle);
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
        private void ChattingRoomOpen(int id, String title)
        {
            ChattingRoom chattingRoom = new ChattingRoom(socketWrapper, id);
            chattingRoom.Text = title;
            chattingRoom.Show();
        }

        private void btn_cancel_click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
