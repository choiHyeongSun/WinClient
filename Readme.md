### WinServer Client 

<br>

## 1. 프로젝트 소개 
C++ 기반의 Windows 서버와 연동되는 C# WinForms 기반 채팅 클라이언트입니다. 이종 언어(C# ↔ C++) 간의 데이터 통신을 위해 마샬링(Marshaling) 기술을 적용하여 메모리 구조를 일치시켰으며, 안정적인 통신 환경을 구축했습니다.

## 2. 실행 화면 
[![실행 영상](http://img.youtube.com/vi/H227_PjhVBg/0.jpg)](https://youtu.be/H227_PjhVBg)

## 3. 구현 방식 
서버로부터 패킷을 수신하면 PacketManager와 FormManager를 통해 사전에 등록된 핸들러를 호출하는 이벤트 기반 방식으로 동작합니다. 이를 통해 UI와 비즈니스 로직(Controller)의 의존성을 낮추었습니다.



<br>

<img width="482" height="532" alt="Image" src="https://github.com/user-attachments/assets/a0d2e61d-1a85-4d2b-b870-46d1b9bf0c36" />

C++ 서버의 구조체(Struct) 메모리 레이아웃과 C#의 클래스 메모리 레이아웃을 일치시키기 위해 StructLayout 속성을 사용했습니다.
- Pack = 1: 바이트 패딩(Padding)을 제거하여 1바이트 단위로 정렬, 데이터 불일치 방지.
- MarshalAs: 고정 크기 배열을 지정하여 C++의 배열과 호환성 확보.
```cs
    // Custom Attribute를 활용하여 패킷 Type 매핑
    [Packet(EMESSAGE_TYPE.JOIN_PACKET)]
    // C++ 구조체와 메모리 구조를 맞추기 위해 1바이트 정렬 (Marshaling)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class JoinPacket : PacketHeader
    {
        // C++의 char[64]와 매핑하기 위해 고정 크기 지정
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] UserID = new byte[64];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Password = new byte[64];
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] Nickname = new byte[64];
    }
```

TCP 통신의 특성상 패킷이 쪼개져서 도착하거나 뭉쳐서 도착할 수 있습니다. 이를 해결하기 위해 Header를 먼저 읽어 데이터 길이를 파악한 후, 해당 길이만큼 데이터를 모두 수신할 때까지 루프를 도는 방식을 구현했습니다.
```cs
  // 1. 헤더(Header) 수신 (Peek를 사용하여 소켓 버퍼 확인)
  int len = socket.Receive(packetHeader, SocketFlags.Peek);
  if (len <= 0) break;
  // 헤더 역직렬화 및 엔디안 변환 
  header = PacketManager.ByteToStruct<PacketHeader>(packetHeader, headerSize);
  header.packetLen = IPAddress.NetworkToHostOrder(header.packetLen);
  header.packetType = IPAddress.NetworkToHostOrder(header.packetType);
  header.userLocalId = (uint)IPAddress.NetworkToHostOrder((int)header.userLocalId);

  // 2. 전체 패킷 수신 루프 (패킷 쪼개짐 방지 로직)
  // 헤더에 명시된 전체 크기만큼 데이터가 모일 때까지 반복 수신
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
// 3. 컨트롤러로 메시지 전달 (Dispatcher)
ControllerManager.ExecuteRecvMessage((EMESSAGE_TYPE)header.packetType, header, packet);
```

수신된 패킷은 ControllerManager를 통해 해당 패킷을 처리할 Controller로 라우팅됩니다. 각 컨트롤러는 싱글톤으로 관리되며, 처리 결과를 UI(Form)에 전달하거나 다음 로직을 수행합니다.
```cs

    internal class ResultController : SingleTon<ResultController> ,IRecvMessage, ISingleTon_Init, ISingleTon_Remove
    {
        public void RecvMessage(PacketHeader header, byte[] packet)
        {
            // 바이트 배열을 구조체로 역직렬화
            ResultPacket result = PacketManager.ByteToStruct<ResultPacket>(packet, header.packetLen);
            // 엔디안 변환 및 데이터 가공
            result.PrevMessageType = IPAddress.NetworkToHostOrder(result.PrevMessageType);
            result.ResultType = IPAddress.NetworkToHostOrder(result.ResultType);
            result.userLocalId = (uint)IPAddress.NetworkToHostOrder((int)result.userLocalId);

            // UI 업데이트 요청 (FormManager 위임)
            FormManager.ExecuteResultMessage((EMESSAGE_TYPE)result.PrevMessageType, result);
        }

        public void Initialization()
        {
            ControllerManager.RegisterRecvMessage(EMESSAGE_TYPE.RESULT_MESSAGE, this);
        }

        public void Remove()
        {
            ControllerManager.UnregisterRecvMessage(EMESSAGE_TYPE.RESULT_MESSAGE, this);
        }
    }
```

