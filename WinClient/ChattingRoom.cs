using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using WinClient.Sources.Attributes;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;
using WinClient.Sources.Utilities;
using WinClient.Sources.Wrapper;

namespace WinClient
{
    public partial class ChattingRoom : Form
    {
        private BlockSocketWrapper wrapper;
        private int roomID;

        public ChattingRoom(BlockSocketWrapper wrapper, int roomID)
        {
            InitializeComponent();
            this.wrapper = wrapper;
            this.roomID = roomID;
            lv_members.Clear();
            lv_members.Columns.Add("이름", 189);
            lv_members.View = View.Details;
            lv_members.FullRowSelect = false;

            lv_chatting.Clear();
            lv_chatting.Columns.Add("이름", 100);
            lv_chatting.Columns.Add("메세지", 270);
            lv_chatting.View = View.Details;
            lv_chatting.FullRowSelect = false;

            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

        }

        private void ChattingRoom_Load(object sender, EventArgs e)
        {
            RequireRoomInfoPacket? entryRoom = PacketManager.GeneratePacket<RequireRoomInfoPacket>();
            if (entryRoom == null) return;
            int size = Marshal.SizeOf(entryRoom);

            entryRoom.packetType = IPAddress.HostToNetworkOrder(entryRoom.packetType);
            entryRoom.packetLen = IPAddress.HostToNetworkOrder(size);
            entryRoom.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)entryRoom.userLocalId);
            entryRoom.RoomID = IPAddress.HostToNetworkOrder(roomID);

            byte[] buffer = PacketManager.StructToByte(entryRoom, size);
            FormManager.RegisterArrayMessage(EMESSAGE_TYPE.ENTRY_MEMBER, OnSendEntryRoomMemberArray);
            FormManager.RegisterArrayMessage(EMESSAGE_TYPE.EXIT_MEMBER, OnSendExitRoomMemberArray);
            FormManager.RegisterArrayMessage(EMESSAGE_TYPE.SEND_MESSAGE, OnSendMessage);

            wrapper.SendMessage(buffer);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            FormManager.UnregisterArrayMessage(EMESSAGE_TYPE.ENTRY_MEMBER, OnSendEntryRoomMemberArray);
            FormManager.UnregisterArrayMessage(EMESSAGE_TYPE.EXIT_MEMBER, OnSendExitRoomMemberArray);
            FormManager.UnregisterArrayMessage(EMESSAGE_TYPE.SEND_MESSAGE, OnSendMessage);

            base.OnFormClosed(e);
            ExitRoomPacket? exitRoomPacket = PacketManager.GeneratePacket<ExitRoomPacket>();
            if (exitRoomPacket == null) return;
            int size = Marshal.SizeOf(exitRoomPacket);

            exitRoomPacket.packetType = IPAddress.HostToNetworkOrder(exitRoomPacket.packetType);
            exitRoomPacket.packetLen = IPAddress.HostToNetworkOrder(size);
            exitRoomPacket.userLocalId = (uint)IPAddress.HostToNetworkOrder((int)exitRoomPacket.userLocalId);
            exitRoomPacket.RoomID = (uint)IPAddress.HostToNetworkOrder(roomID);

            byte[] buffer = PacketManager.StructToByte(exitRoomPacket, size);
            wrapper.SendMessage(buffer);
        }

        private void SendMessageToServer()
        {
            if (tb_message_box.Text == "") return;

            SendMessagePacket? sendMessagePacket = PacketManager.GeneratePacket<SendMessagePacket>();
            if (sendMessagePacket == null) return;

            int userID = (int)NetworkManager.GetLocalID();
            byte[] buffer = Encoding.UTF8.GetBytes(tb_message_box.Text);
            int packetSize = Marshal.SizeOf<SendMessagePacket>();
            int msgLen = buffer.Length;
            int messageHeaderLen = Marshal.SizeOf<MessageHeader>();

            sendMessagePacket.MsgLen = (uint)IPAddress.HostToNetworkOrder(msgLen + packetSize);
            sendMessagePacket.packetLen = IPAddress.HostToNetworkOrder(packetSize + msgLen + messageHeaderLen);
            sendMessagePacket.MessageCount = IPAddress.HostToNetworkOrder(1);
            sendMessagePacket.RoomID = IPAddress.HostToNetworkOrder(roomID);
            sendMessagePacket.packetType = IPAddress.HostToNetworkOrder(sendMessagePacket.packetType);
            sendMessagePacket.userLocalId = (uint)IPAddress.HostToNetworkOrder(userID);


            MessageHeader header = new MessageHeader();
            header.SendUserID = IPAddress.HostToNetworkOrder(userID);
            header.MsgLen = IPAddress.HostToNetworkOrder(msgLen);
            ConvertUtility.ByteConvertByString(ref header.UserName, NetworkManager.GetNickname());

            byte[] headerBuffer = new byte[packetSize + msgLen + messageHeaderLen];

            byte[] packetHeader = PacketManager.StructToByte(sendMessagePacket, packetSize);
            byte[] messageHeader = PacketManager.StructToByte(header, messageHeaderLen);

            Array.Copy(packetHeader, 0, headerBuffer, 0, packetSize);
            Array.Copy(messageHeader, 0, headerBuffer, packetSize, messageHeaderLen);
            Array.Copy(buffer, 0, headerBuffer, packetSize + messageHeaderLen, msgLen);
            wrapper.SendMessage(headerBuffer);
            tb_message_box.Text = "";
        }
        private void btn_send_message_click(object sender, EventArgs e)
        {
            SendMessageToServer();
        }
        private void tb_message_box_keyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessageToServer();
            }
        }
        private void OnSendEntryRoomMemberArray(byte[] members)
        {
            int packetSize = Marshal.SizeOf<EntryMemberPacket>();
            int memberSize = Marshal.SizeOf<RoomMember>();

            EntryMemberPacket entryMemberPacket = PacketManager.ByteToStruct<EntryMemberPacket>(members, packetSize, 0);
            entryMemberPacket.RoomID = IPAddress.NetworkToHostOrder(entryMemberPacket.RoomID);
            entryMemberPacket.MemberCount = IPAddress.NetworkToHostOrder(entryMemberPacket.MemberCount);

            if (entryMemberPacket.RoomID != roomID)
            {
                return;
            }

            RoomMember[] memberList = new RoomMember[entryMemberPacket.MemberCount];
            int totalSize = packetSize;
            for (int i = 0; i < entryMemberPacket.MemberCount; i++)
            {
                memberList[i] = PacketManager.ByteToStruct<RoomMember>(members, memberSize, totalSize);
                totalSize += memberSize;
            }

            Invoke(() =>
            {
                for (int i = 0; i < entryMemberPacket.MemberCount; i++)
                {
                    ListViewItem item = new ListViewItem();
                    String name = Encoding.UTF8.GetString(memberList[i].userName) + "#" + memberList[i].userID;
                    item.Text = name;
                    lv_members.Items.Add(item);
                }
            });
        }

        private void OnSendExitRoomMemberArray(byte[] members)
        {
            int packetSize = Marshal.SizeOf<EntryMemberPacket>();
            int memberSize = Marshal.SizeOf<RoomMember>();

            ExitMemberPacket entryMemberPacket = PacketManager.ByteToStruct<ExitMemberPacket>(members, packetSize, 0);
            entryMemberPacket.RoomID = IPAddress.NetworkToHostOrder(entryMemberPacket.RoomID);
            entryMemberPacket.MemberCount = IPAddress.NetworkToHostOrder(entryMemberPacket.MemberCount);

            if (entryMemberPacket.RoomID != roomID)
            {
                return;
            }

            RoomMember[] memberList = new RoomMember[entryMemberPacket.MemberCount];
            int totalSize = packetSize;
            for (int i = 0; i < entryMemberPacket.MemberCount; i++)
            {
                memberList[i] = PacketManager.ByteToStruct<RoomMember>(members, memberSize, totalSize);
                totalSize += memberSize;
            }

            Invoke(() =>
            {
                ListViewItem? item = null;
                for (int i = 0; i < lv_members.Items.Count; i++)
                {
                    item = lv_members.Items[i];
                }

                if (item != null)
                {
                    lv_members.Items.Remove(item);
                }
            });
        }


        private void OnSendMessage(byte[] message)
        {
            int packetSize = Marshal.SizeOf<SendMessagePacket>();
            SendMessagePacket sendMessagePacket = PacketManager.ByteToStruct<SendMessagePacket>(message, packetSize, 0);
            sendMessagePacket.MsgLen = (uint)IPAddress.NetworkToHostOrder((int)sendMessagePacket.MsgLen);
            sendMessagePacket.MessageCount = IPAddress.NetworkToHostOrder(sendMessagePacket.MessageCount);
            sendMessagePacket.RoomID = IPAddress.NetworkToHostOrder(sendMessagePacket.RoomID);
            sendMessagePacket.packetType = IPAddress.NetworkToHostOrder(sendMessagePacket.packetType);
            sendMessagePacket.userLocalId = (uint)IPAddress.NetworkToHostOrder((int)sendMessagePacket.userLocalId);
            sendMessagePacket.packetLen = IPAddress.NetworkToHostOrder(sendMessagePacket.packetLen);

            if (sendMessagePacket.RoomID != roomID)
            {
                return;
            }


            int len = packetSize;
            int messageHeaderLen = Marshal.SizeOf<MessageHeader>();

            for (int i = 0; i < sendMessagePacket.MessageCount; i++)
            {
                MessageHeader header = PacketManager.ByteToStruct<MessageHeader>(message, messageHeaderLen, len);
                len += messageHeaderLen;
                int msgLen = IPAddress.NetworkToHostOrder(header.MsgLen);
                byte[] msgBuffer = new byte[msgLen];
                Array.Copy(message, len, msgBuffer, 0, msgLen);
                len += msgLen;

                String nickname = Encoding.UTF8.GetString(header.UserName);
                String msg = Encoding.UTF8.GetString(msgBuffer);

                Invoke(() =>
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = nickname;
                    item.SubItems.Add(msg);
                    lv_chatting.Items.Add(item);
                });
            }
        }


    }
}
