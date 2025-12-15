using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class MainForm : Form
    {
        private LoginUserControl loginUserControl;
        private UserInfoUserControl userInfoControl;

        private BlockSocketWrapper socketWrapper;

        public MainForm()
        {
            InitializeComponent();
        }

        public void MainForm_Load(Object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            lv_rooms.Clear();
            lv_rooms.FullRowSelect = true;

            lv_rooms.Columns.Add("번호", 50);
            lv_rooms.Columns.Add("제목", 189);

            socketWrapper = new BlockSocketWrapper(EPROTOCOL_TYPE.TCP);

            loginUserControl = new LoginUserControl(socketWrapper, LoginSuccess);
            loginUserControl.Location = new Point(127, 15);
            loginUserControl.Visible = true;

            userInfoControl = new UserInfoUserControl(socketWrapper, LogoutSuccess);
            userInfoControl.Location = new Point(127, 15);
            userInfoControl.Visible = false;

            Controls.Add(loginUserControl);
            Controls.Add(userInfoControl);

            btn_Create_ChattingRoom.Enabled = false;
            btn_entryRoom.Enabled = false;

            FormManager.RegisterArrayMessage(EMESSAGE_TYPE.SEND_ROOM, OnSendRoomArray);
            FormManager.RegisterResultMessage(EMESSAGE_TYPE.EXIT_ROOM, OnRoomRemove);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            FormManager.UnregisterArrayMessage(EMESSAGE_TYPE.SEND_ROOM, OnSendRoomArray);
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.EXIT_ROOM, OnRoomRemove);
        }

        public void MainForm_Shown(Object sender, EventArgs e)
        {
            if (!socketWrapper.Connect())
            {
                MessageBox.Show("ERROR 연결 실패 !! ", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        public void MainForm_Close(Object sender, EventArgs e)
        {
            ExitClientPacket? exitClientPacket = PacketManager.GeneratePacket<ExitClientPacket>();
            if (exitClientPacket != null)
            {
                int size = Marshal.SizeOf(exitClientPacket);
                exitClientPacket.packetType = IPAddress.HostToNetworkOrder(exitClientPacket.packetType);
                exitClientPacket.packetLen = IPAddress.HostToNetworkOrder(size);
                exitClientPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)NetworkManager.GetLocalID());
                byte[] buffer = PacketManager.StructToByte(exitClientPacket, size);
                socketWrapper.SendMessage(buffer);
            }
            socketWrapper.Disconnect();
        }

        private void btn_Create_ChattingRoom_Click(object sender, EventArgs e)
        {
            CreateRoomForm form = new CreateRoomForm(socketWrapper, ChattingRoomOpen);
            if (form.ShowDialog() == DialogResult.OK)
            {
                return;
            }
        }

        private void lv_rooms_DoubleClick(object sender, EventArgs e)
        {
            SendEntryRooms();
        }
        private void btn_entry_room(object sender, EventArgs e)
        {
            SendEntryRooms();
        }


        // 콜백
        //===============================================================================================================
        private void SendEntryRooms()
        {
            foreach (var item in lv_rooms.SelectedItems)
            {
                EntryRoomComparePacket? exitClientPacket = PacketManager.GeneratePacket<EntryRoomComparePacket>();
                if (exitClientPacket == null)
                    continue;

                ListViewItem listViewItem = item as ListViewItem;
                int size = Marshal.SizeOf<EntryRoomComparePacket>();
                int roomID = int.Parse(listViewItem.Text);
                exitClientPacket.packetType = IPAddress.HostToNetworkOrder(exitClientPacket.packetType);
                exitClientPacket.packetLen = IPAddress.HostToNetworkOrder(size);
                exitClientPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)NetworkManager.GetLocalID());
                exitClientPacket.RoomId = (uint)IPAddress.HostToNetworkOrder(roomID);
                byte[] buffer = PacketManager.StructToByte(exitClientPacket, size);
                FormManager.RegisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM_COMPARE, EntryRoomCompareResult);
                socketWrapper.SendMessage(buffer);
            }
        }

        private void EntryRoomCompareResult(ResultPacket resultPacket)
        {
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM_COMPARE, EntryRoomCompareResult);

            if (resultPacket.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                int roomID = int.Parse(Encoding.UTF8.GetString(resultPacket.ResultMsg));
                EntryRoomPacket? entryRoomPacket = PacketManager.GeneratePacket<EntryRoomPacket>();
                if (entryRoomPacket == null) return;
                int size = Marshal.SizeOf<EntryRoomPacket>();
                entryRoomPacket.packetType = IPAddress.HostToNetworkOrder(entryRoomPacket.packetType);
                entryRoomPacket.packetLen = IPAddress.HostToNetworkOrder(size);
                entryRoomPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)NetworkManager.GetLocalID());
                entryRoomPacket.RoomId = (uint)IPAddress.HostToNetworkOrder(roomID);

                byte[] buffer = PacketManager.StructToByte(entryRoomPacket, size);
                FormManager.RegisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM, EntryRoomResult);
                socketWrapper.SendMessage(buffer);
            }
            else if (resultPacket.ResultType == (int)EMESSAGE_RESULT.FAILED)
            {
                Invoke(() =>
                {
                    string str = Encoding.UTF8.GetString(resultPacket.ResultMsg);
                    if (int.TryParse(str, out int roomID))
                    {
                        ListViewItem listViewItem = null;
                        for (int i = 0; i < lv_rooms.Items.Count; i++)
                        {
                            if (lv_rooms.Items[i].Text.Equals(roomID.ToString()))
                            {
                                listViewItem = lv_rooms.Items[i];
                                break;
                            }
                        }
                        if (listViewItem != null)
                        {
                            PasswordInput paswordInput = new PasswordInput(socketWrapper, roomID, listViewItem.SubItems[1].Text);
                            paswordInput.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show(str);
                    }
                });
                
            }
        }

        private void EntryRoomResult(ResultPacket resultPacket)
        {
            FormManager.UnregisterResultMessage(EMESSAGE_TYPE.ENTRY_ROOM, EntryRoomResult);
            if (resultPacket.ResultType == (int)EMESSAGE_RESULT.SUCCESS)
            {
                int roomID = int.Parse(Encoding.UTF8.GetString(resultPacket.ResultMsg));
                Invoke(() =>
                {
                    ChattingRoomOpen(roomID);
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

        private void OnSendRoomArray(byte[] msg)
        {
            if (!NetworkManager.IsLogin()) return;
            int sendRoomSize = Marshal.SizeOf<SendRoomPacket>();
            int roomInfoSize = Marshal.SizeOf<RoomInfoPacket>();

            SendRoomPacket sendRoomPack = PacketManager.ByteToStruct<SendRoomPacket>(msg, sendRoomSize);
            sendRoomPack.Count = (uint)IPAddress.NetworkToHostOrder((int)sendRoomPack.Count);
            sendRoomPack.ArrayBufferLen = (uint)IPAddress.NetworkToHostOrder((int)sendRoomPack.ArrayBufferLen);

            SortedList<int, RoomInfoPacket> roomInfoPacks = new(Comparer<int>.Create((x, y) => y.CompareTo(x)));
            int totalLen = sendRoomSize;
            for (int i = 0; i < sendRoomPack.Count; i++)
            {
                RoomInfoPacket roomInfoPack = PacketManager.ByteToStruct<RoomInfoPacket>(msg, roomInfoSize, totalLen);
                roomInfoPacks.Add((int)roomInfoPack.RoomID, roomInfoPack);
                totalLen += roomInfoSize;
            }

            Invoke(() =>
            {
                foreach (var roomInfo in roomInfoPacks)
                {
                    ListViewItem item = new ListViewItem();
                    String title = Encoding.UTF8.GetString(roomInfo.Value.RoomName);
                    item.Text = roomInfo.Value.RoomID.ToString();
                    item.SubItems.Add(title);
                    lv_rooms.Items.Add(item);
                }
            });
        }

        private void OnRoomRemove(ResultPacket result)
        {
            if (!NetworkManager.IsLogin()) return;
            Invoke(() =>
            {
                string msg = Encoding.UTF8.GetString(result.ResultMsg);
                if (int.TryParse(msg, out int id))
                {
                    ListViewItem? listviewItem = null;
                    foreach (var item in lv_rooms.Items)
                    {
                        listviewItem = item as ListViewItem;
                        if (listviewItem == null) break;
                        if (listviewItem.Text.Equals(id.ToString()))
                        {
                            break;
                        }
                    }

                    if (listviewItem != null)
                    {
                        lv_rooms.Items.Remove(listviewItem);
                    }
                }
                
            });
        }

        //=============================================================================================================

        private void ChattingRoomOpen(int id)
        {
            ListViewItem listViewItem = null;
            for (int i = 0; i < lv_rooms.Items.Count; i++)
            {
                if (lv_rooms.Items[i].Text.Equals(id.ToString()))
                {
                    listViewItem = lv_rooms.Items[i];
                    break;
                }
            }

            String title = listViewItem.SubItems[1].Text;
            ChattingRoom chattingRoom = new ChattingRoom(socketWrapper, id);
            chattingRoom.Text = title;
            chattingRoom.Show();
        }

        private void LoginSuccess()
        {
            loginUserControl.Visible = false;
            userInfoControl.Visible = true;

            btn_Create_ChattingRoom.Enabled = true;
            btn_entryRoom.Enabled = true;
        }

        private void LogoutSuccess()
        {
            loginUserControl.Visible = true;
            userInfoControl.Visible = false;

            btn_Create_ChattingRoom.Enabled = false;
            btn_entryRoom.Enabled = false;
        }
    }
}
