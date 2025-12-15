using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using WinClient.Sources.Attributes;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Managers
{
    internal static class PacketManager
    {
        public static T? GeneratePacket<T>() where T : PacketHeader, new()
        {
            T newPacket = new T();
            Type type = newPacket.GetType();
            Packet? packet = type.GetCustomAttribute<Packet>();

            if (packet == null) return null;
            newPacket.packetType = (int)packet.messageType;
            newPacket.userLocalId = NetworkManager.GetLocalID();
            return newPacket;
        }

        public static byte[] StructToByte<T>(T packet, int size)
        {
            byte[] buffer = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(packet, ptr, false);
            Marshal.Copy(ptr, buffer, 0, size);
            Marshal.FreeHGlobal(ptr);
            return buffer;
        }

        public static T ByteToStruct<T>(byte[] buffer, int size, int offset = 0) where T: new()
        {
            T newStruct = new T();
            IntPtr headerPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(buffer, offset, headerPtr, size);
            Marshal.PtrToStructure(headerPtr, newStruct);
            return newStruct;
        }
    }
}
