using System;
using System.Net;
using System.Collections.Generic;

namespace Packets
{
    public enum PacketType
    {
        EMPTY = 0,
        NICKNAME,
        CHATMESSAGE,
        LOGIN,
        NICKNAMESLIST,
        CHARACTERPOSITION
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
    public class NicknamePacket : Packet
    {
        public string _nickname = string.Empty;
        public NicknamePacket(string nickname)
        {
            _type = PacketType.NICKNAME;
            _nickname = nickname;
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

    [Serializable]
    public class NicknamesList : Packet
    {
        public List<string> _nicknamesList;

        public NicknamesList(List<string> list)
        {
            _type = PacketType.NICKNAMESLIST;
            _nicknamesList = list;
        }
    }

    [Serializable]
    public class CharacterPositionPacket : Packet
    {
        public float _x, _y;
        public int _direction;
        public CharacterPositionPacket(float x, float y, int direction)
        {
            _type = PacketType.CHARACTERPOSITION;
            _x = x;
            _y = y;
            _direction = direction;
        }
    }
}
