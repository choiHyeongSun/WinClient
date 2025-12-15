using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinClient.Sources.Managers;
using WinClient.Sources.Packets;

namespace WinClient.Sources.Wrapper
{
    public enum EPROTOCOL_TYPE
    {
        TCP,
        UDP
    }
    public class BlockSocketWrapper
    {
        private static readonly int port = 8080;
        private static readonly IPAddress serverAddress = IPAddress.Loopback;
        private Socket socket;
        private EPROTOCOL_TYPE protocolType;
        private IPEndPoint endpoint = new IPEndPoint(serverAddress, port);
        private bool isFinish = false;

        private readonly object socketLock = new object();

        private Thread recvThread;

        public BlockSocketWrapper(EPROTOCOL_TYPE protocolType)
        {
            switch (protocolType)
            {
                case EPROTOCOL_TYPE.TCP:
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    break;
                case EPROTOCOL_TYPE.UDP:
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Udp);
                    break;
            }

            this.protocolType = protocolType;
        }
        public bool Connect()
        {
            try
            {
                switch (protocolType)
                {
                    case EPROTOCOL_TYPE.TCP:
                        socket.Connect(endpoint);
                        recvThread = new Thread(ReceiveMessage);
                        recvThread.Start();
                        break;
                    case EPROTOCOL_TYPE.UDP:
                        EntryServer? entryServer = PacketManager.GeneratePacket<EntryServer>();
                        if (entryServer == null)
                        {
                            return false;
                        }

                        int size = Marshal.SizeOf(entryServer);
                        entryServer.packetType = IPAddress.HostToNetworkOrder(entryServer.packetType);
                        entryServer.packetLen = IPAddress.HostToNetworkOrder(size);
                        entryServer.userLocalId = 0;
                        byte[] buffer = PacketManager.StructToByte(entryServer, size);
                        SendMessage(buffer);
                        break;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void ReceiveMessage()
        {
            while (!isFinish)
            {
                int headerSize = Marshal.SizeOf<PacketHeader>();
                byte[] packetHeader = new byte[headerSize];
                PacketHeader header;
                byte[] packet;
                int sumLen;
                int revlen;
                try
                {
                    if (protocolType == EPROTOCOL_TYPE.TCP)
                    { 
                        int len = socket.Receive(packetHeader, SocketFlags.Peek);
                        if (len <= 0) break;

                        header = PacketManager.ByteToStruct<PacketHeader>(packetHeader, headerSize);
                        header.packetLen = IPAddress.NetworkToHostOrder(header.packetLen);
                        header.packetType = IPAddress.NetworkToHostOrder(header.packetType);
                        header.userLocalId = (uint)IPAddress.NetworkToHostOrder((int)header.userLocalId);


                        packet = new byte[header.packetLen];
                        sumLen = 0;
                        while (sumLen < header.packetLen)
                        {
                            revlen = 0;
                            lock (socketLock)
                            {
                                revlen = socket.Receive(packet, sumLen, header.packetLen, SocketFlags.None);
                            }

                            sumLen += revlen;
                        }

                        ControllerManager.ExecuteRecvMessage((EMESSAGE_TYPE)header.packetType, header, packet);
                    }
                    else if (protocolType == EPROTOCOL_TYPE.UDP)
                    {
                        EndPoint recvEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        socket.ReceiveFrom(packetHeader, SocketFlags.Peek, ref recvEndPoint);
                        header = PacketManager.ByteToStruct<PacketHeader>(packetHeader, headerSize);
                        header.packetLen = IPAddress.NetworkToHostOrder(header.packetLen);
                        header.packetType = IPAddress.NetworkToHostOrder(header.packetType);

                        packet = new byte[header.packetLen];
                        sumLen = 0;
                        while (sumLen < header.packetLen)
                        {
                            revlen = 0;
                            lock (socketLock)
                            {
                                revlen = socket.ReceiveFrom(packet, sumLen, header.packetLen, SocketFlags.None,
                                    ref recvEndPoint);
                            }

                            sumLen += revlen;
                        }

                        ControllerManager.ExecuteRecvMessage((EMESSAGE_TYPE)header.packetType, header, packet);
                    }


                }
                catch (SocketException se)
                {
                    if (se.ErrorCode == 10004 || se.ErrorCode == 10053 || se.ErrorCode == 10054)
                    {
                        // "정상적인 소켓 종료입니다."
                        return;
                    }

                    MessageBox.Show(se.Message , "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        public void SendMessage(byte[] msg)
        {
            int sendLen;
            int sumLen = 0;
            int totalLen = msg.Length;
            switch (protocolType)
            {
                case EPROTOCOL_TYPE.TCP:
                    while (sumLen < totalLen)
                    {
                        sendLen = 0;
                        lock (socketLock)
                        {
                            sendLen = socket.Send(msg, sumLen, totalLen, SocketFlags.None);
                        }
                        sumLen += sendLen;
                    }
                    break;
                case EPROTOCOL_TYPE.UDP:
                    while (sumLen < totalLen)
                    {
                        sendLen = 0;
                        lock (socketLock)
                        {
                            sendLen = socket.SendTo(msg, sumLen, totalLen, SocketFlags.None, endpoint);
                        }
                        sumLen += sendLen;
                    }
                    break;
            }
        }

        public void Disconnect()
        {
            isFinish = true;
            socket.Close();
        }
    }
}
