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
        DIRECTMESSAGE,
        LOGIN,
        NICKNAMESLIST,
        JOINGAME,
        STARTGAMEBUTTON,
        STARTGAME,
        RESTARTGAME,
        CHARACTERPOSITION,
        ASSIGNCHARACTER,
        CREATECHARACTER,
        REMOVECHARACTER,
        SPAWNBOMB
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
        public string _nickname;
        public string _message;
        public ChatMessagePacket(string nickname, string message)
        {
            _type = PacketType.CHATMESSAGE;
            _nickname = nickname;
            _message = message;
        }
    }

    [Serializable]
    public class DirectMessagePacket : Packet
    {
        public string _receiver;
        public string _message;

        public DirectMessagePacket(string receiver, string message)
        {
            _type = PacketType.DIRECTMESSAGE;
            _receiver = receiver;
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
        public int _id;
        public EndPoint _endPoint;
        public LoginPacket(EndPoint endpoint, int id)
        {
            _type = PacketType.LOGIN;
            _endPoint = endpoint;
            _id = id;
        }
    }

    [Serializable]
    public class NicknamesListPacket : Packet
    {
        public List<string> _nicknamesList;

        public NicknamesListPacket(List<string> list)
        {
            _type = PacketType.NICKNAMESLIST;
            _nicknamesList = list;
        }
    }

    [Serializable]
    public class JoinGamePacket : Packet
    {
        public int _id;
        public string _nickname;
        public int _buttonID;
        public JoinGamePacket(int id, string nickname, int buttonID)
        {
            _type = PacketType.JOINGAME;
            _id = id;
            _nickname = nickname;
            _buttonID = buttonID;
        }
    }

    [Serializable]
    public class StartGameButtonPacket : Packet
    {
        public bool _canStartGame;
        public StartGameButtonPacket(bool startGameState)
        {
            _type = PacketType.STARTGAMEBUTTON;
            _canStartGame = startGameState;
        }
    }

    [Serializable]
    public class StartGamePacket : Packet
    {
        public bool _startGame;
        public StartGamePacket(bool state)
        {
            _type = PacketType.STARTGAME;
            _startGame = state;
        }
    }

    [Serializable]
    public class RestartGamePacket : Packet
    {
        public RestartGamePacket()
        {
            _type = PacketType.RESTARTGAME;
        }
    }

    [Serializable]
    public class CharacterPositionPacket : Packet
    {
        public int _id;
        public float _x, _y;
        public int _direction;
        public CharacterPositionPacket(int id, float x, float y, int direction)
        {
            _type = PacketType.CHARACTERPOSITION;
            _id = id;
            _x = x;
            _y = y;
            _direction = direction;
        }
    }

    [Serializable]
    public class AssignCharacterPacket : Packet
    {
        public int _playerID;
        public AssignCharacterPacket(int playerID)
        {
            _type = PacketType.ASSIGNCHARACTER;
            _playerID = playerID;
        }
    }

    [Serializable]
    public class CreateCharacterPacket : Packet
    {
        public int _id;
        public int _ColourR, _ColourG, _ColourB;
        public CreateCharacterPacket(int id, int r, int g, int b)
        {
            _type = PacketType.CREATECHARACTER;
            _id = id;
            _ColourR = r;
            _ColourG = g;
            _ColourB = b;
        }
    }

    [Serializable]
    public class RemoveCharacterPacket : Packet
    {
        public int _id;
        public RemoveCharacterPacket(int id)
        {
            _type = PacketType.REMOVECHARACTER;
            _id = id;
        }
    }

    [Serializable]
    public class SpawnBombPacket : Packet
    {
        public int _id;
        public float _posX, _posY;
        public SpawnBombPacket(float posX, float posY, int id)
        {
            _type = PacketType.SPAWNBOMB;
            _id = id;
            _posX = posX;
            _posY = posY;
        }
    }
}
