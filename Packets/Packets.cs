using System;
using System.Net;

namespace Packets
{
    public enum PacketType
    {
        EMPTY = 0,
        NICKNAME,
        CHATMESSAGE,
        LOGIN
    }

    [Serializable]
    public class Packet
    {
        protected PacketType _type = PacketType.EMPTY;
        public PacketType getPacketType() { return _type; }
    }

    [Serializable]
    public class ChatMessagePacket : Packet
    {
        public string _message = string.Empty;
        public ChatMessagePacket(string message)
        {
            _type = PacketType.CHATMESSAGE;
            _message = message;
        }
    }

    [Serializable]
    public class NickNamePacket : Packet
    {
        public string _nickName = string.Empty;
        public NickNamePacket(string nickname)
        {
            _type = PacketType.NICKNAME;
            _nickName = nickname;
        }
    }

    [Serializable]
    public class LoginPacket : Packet
    {
        public EndPoint _endPoint;
        public LoginPacket(EndPoint endpoint)
        {
            _type = PacketType.LOGIN;
            _endPoint = endpoint;
        }
    }
}
