using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Attributes;

namespace WinClient.Sources.Packets
{
    public enum EMESSAGE_TYPE
    {
        NONE = 0,
        ENTRY_SERVER,
        LOGIN_PACKET,
        JOIN_PACKET,
        SEND_MESSAGE,
        RESULT_MESSAGE,
        LOGOUT_PACKET,
        EXIT_CLIENT,
        CREATE_ROOM,
        SEND_ROOM,
        ENTRY_ROOM_COMPARE,
        ENTRY_ROOM,
        EXIT_ROOM,
        ENTRY_MEMBER,
        EXIT_MEMBER,
        REQUIRE_ROOM_INFO

    }

    public enum EMESSAGE_RESULT
    {
        SUCCESS = 0,
        FAILED = 1,
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PacketHeader
    {
        public int packetLen;
        public int packetType;
        public uint userLocalId = 0;
    }


    [Packet(EMESSAGE_TYPE.ENTRY_SERVER)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class EntryServer : PacketHeader
    {
    }

    [Packet(EMESSAGE_TYPE.JOIN_PACKET)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class JoinPacket : PacketHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] UserID = new byte[64];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Password = new byte[64];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Nickname = new byte[64];
    }

    [Packet(EMESSAGE_TYPE.SEND_MESSAGE)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SendMessagePacket : PacketHeader
    {
        public int RoomID;
        public int MessageCount = 0;
        public uint MsgLen = 0;
    }

    [Packet(EMESSAGE_TYPE.RESULT_MESSAGE)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ResultPacket : PacketHeader
    {
        public int PrevMessageType = (int)EMESSAGE_TYPE.NONE;
        public int ResultType = (int)EMESSAGE_RESULT.SUCCESS;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] ResultMsg = new byte[256];
    }

    [Packet(EMESSAGE_TYPE.LOGIN_PACKET)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LoginPacket : PacketHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] UserID = new byte[64];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Password = new byte[64];
    }

    [Packet(EMESSAGE_TYPE.LOGOUT_PACKET)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class LogoutPacket : PacketHeader
    {
    }
    [Packet(EMESSAGE_TYPE.EXIT_CLIENT)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ExitClientPacket : PacketHeader
    {
    }
    [Packet(EMESSAGE_TYPE.REQUIRE_ROOM_INFO)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class RequireRoomInfoPacket : PacketHeader
    {
        public int RoomID = 0;
    };
    [Packet(EMESSAGE_TYPE.CREATE_ROOM)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class CreateRoomPacket : PacketHeader
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] RoomName = new byte[256];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Password = new byte[256];
    };

    [Packet(EMESSAGE_TYPE.SEND_ROOM)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class SendRoomPacket : PacketHeader
    {
        public uint Count = 0;
        public uint ArrayBufferLen = 0;
    }

    [Packet(EMESSAGE_TYPE.ENTRY_ROOM_COMPARE)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EntryRoomComparePacket : PacketHeader
    {
        public uint RoomId = 0;
    }
    [Packet(EMESSAGE_TYPE.ENTRY_ROOM)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EntryRoomPacket : PacketHeader
    {
        public uint RoomId = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Password = new byte[256];
    }
    [Packet(EMESSAGE_TYPE.EXIT_ROOM)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ExitRoomPacket : PacketHeader
    {
        public uint RoomID;
    }
    [Packet(EMESSAGE_TYPE.ENTRY_MEMBER)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class EntryMemberPacket : PacketHeader
    {
        public int RoomID = 0;
        public int MemberCount = 0;
        public int RoomMemberLen = 0;
    };
    [Packet(EMESSAGE_TYPE.EXIT_MEMBER)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class ExitMemberPacket : PacketHeader
    {
        public int RoomID = 0;
        public int MemberCount = 0;
        public int RoomMemberLen = 0;
    };


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RoomInfoPacket
    {
        public uint RoomID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] RoomName = new byte [256];
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class RoomMember
    {
        public uint userID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] userName = new byte[256];
    };
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    class MessageHeader
    {
        public int SendUserID = 0;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] UserName = new byte [256];
        public int MsgLen = 0;
    };

}
