using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Packets;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Net;

namespace SimpleServer
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            SimpleClient myClient = new SimpleClient();
            if (myClient.Connect("127.0.0.1", 4444))
            {
                Console.WriteLine("CONNECTED!");
                myClient.Run();
            }
            else
            {
                Console.WriteLine("Couldn't Connect :c!");
            }
        }
    }

    public class SimpleClient
    {
        public int _playerId;
        public string _nickname { get; private set; }
        public SetNicknameForm _nicknameForm { get; private set; }

        ClientForm _messageForm;
        List<string> _nicknamesList;
        List<int> _localCharactersIds;

        // TCP
        TcpClient _tcpClient;
        Thread _tcpReaderThread;
        NetworkStream _tcpStream;
        BinaryWriter _tcpWriter;
        BinaryReader _tcpReader;
        // UDP
        UdpClient _udpClient;
        Thread _udpReaderThread;
        Thread _udpWriterThread;
        IPEndPoint _remoteIpEndPoint;

        public SimpleClient()
        {
            _nicknamesList = new List<string>();
            _localCharactersIds = new List<int>();
            _localCharactersIds.Add(-1); // Init list with default value so can be evaluated
            _messageForm = new ClientForm(this);
            _nicknameForm = new SetNicknameForm(this);
            _playerId = 0;
        }

        public bool Connect(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            _udpClient = new UdpClient();
            _tcpReaderThread = new Thread(new ThreadStart(ProcessServerResponseTCP));

            // Display Set Nickname window
            _nicknameForm.Owner = _messageForm;
            _nicknameForm.StartPosition = FormStartPosition.CenterParent;

            // Set Nickname Dialog
            _nicknameForm.ShowDialog();

            // Try to connect to a server with tcp and then establish handshake to udp
            try
            {
                _tcpClient.Connect(ipAddress, port);
                _tcpStream = _tcpClient.GetStream();
                _tcpWriter = new BinaryWriter(_tcpStream, Encoding.UTF8);
                _tcpReader = new BinaryReader(_tcpStream, Encoding.UTF8);
                _udpClient.Connect(ipAddress, port);
                _remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }

            // Display Chat window
            Application.Run(_messageForm);

            if (!_tcpReaderThread.IsAlive)
            {
                return false;
            }

            return true;
        }

        public void Run()
        {
            Console.WriteLine("STARTED");
            _tcpReaderThread.Start();
            // Log-in UDP
            SendPacketTCP(new LoginPacket(_udpClient.Client.LocalEndPoint, _playerId));
            // Send initial nickname of the client
            SendPacketTCP(new NicknamePacket(_nickname));
            // Send that you'd like an character
            Random random = new Random();
            SendPacketTCP(new CreateCharacterPacket(_playerId, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
        }

        void ProcessServerResponseTCP()
        {
            Packet packet;

            while ((packet = RetrievePacketTCP()) != null)
            {
                switch (packet.getPacketType())
                {
                    case PacketType.CHATMESSAGE:
                        ChatMessagePacket chatMessagePacket = (ChatMessagePacket)packet;
                        _messageForm.UpdateChatWindow(chatMessagePacket._nickname, chatMessagePacket._message);
                        break;

                    case PacketType.DIRECTMESSAGE:
                        DirectMessagePacket directMessagePacket = (DirectMessagePacket)packet;
                        _messageForm.UpdateChatWindow(directMessagePacket._receiver, directMessagePacket._message);
                        break;

                    case PacketType.NICKNAME:
                        break;

                    case PacketType.LOGIN:
                        LoginPacket loginPacket = (LoginPacket)packet;
                        _playerId = loginPacket._id;
                        _udpClient.Connect((IPEndPoint)loginPacket._endPoint);
                        _udpReaderThread = new Thread(new ThreadStart(ProcessServerResponseReadUDP));
                        _udpReaderThread.Start();
                        Console.WriteLine("UDP CONNECTED!");
                        break;

                    case PacketType.CREATECHARACTER:
                        CreateCharacterPacket createCharacterPacket = (CreateCharacterPacket)packet;

                        // create character if it isn't already locally created
                        for (var i = 0; i < _localCharactersIds.Count; i++)
                        {
                            if (!_localCharactersIds.Contains(createCharacterPacket._id))
                            {
                                _localCharactersIds.Add(createCharacterPacket._id);
                                _messageForm.CreateCharacter(createCharacterPacket._id, createCharacterPacket._ColourR, createCharacterPacket._ColourG, createCharacterPacket._ColourB);
                            }
                        }

                        // Create thread if it hasn't been created before // TO BE MOVED FROM HERE //
                        if (_udpWriterThread == null)
                        {
                            _udpWriterThread = new Thread(new ThreadStart(ProcessServerResponseWriteUDP));
                            _udpWriterThread.Start();

                        }
                        break;

                    case PacketType.REMOVECHARACTER:
                        RemoveCharacterPacket removeCharacterPacket = (RemoveCharacterPacket)packet;
                        int index = _localCharactersIds.FindIndex(lc => lc == removeCharacterPacket._id);
                        if (index != -1)
                        {
                            _localCharactersIds.RemoveAt(index);
                        }
                        _messageForm.RemoveCharacter(removeCharacterPacket._id);
                        break;
                    
                    // Possess own character
                    case PacketType.ASSIGNCHARACTER:
                        _messageForm.AssignCharacter(_playerId);
                        break;

                    default:
                        break;
                }
            }
        }

        void ProcessServerResponseReadUDP()
        {
            Packet packet;
            if (_udpClient.Client.Connected)
            {
                while((packet = ReadPacketUDP(ref _remoteIpEndPoint)) != null)
                {
                    // Process received udp packet
                    switch (packet.getPacketType())
                    {
                        case PacketType.NICKNAMESLIST:
                            // Update local nicklist only if it's different from server one
                            NicknamesListPacket nicknameListPacket = packet as NicknamesListPacket;
                            if (!_nicknamesList.SequenceEqual(nicknameListPacket._nicknamesList))
                            {
                                _nicknamesList = nicknameListPacket._nicknamesList;
                                _messageForm.UpdateNicknamesList(ref _nicknamesList);
                            }
                            break;

                        case PacketType.CHARACTERPOSITION:
                            CharacterPositionPacket characterPositionPacket = (CharacterPositionPacket)packet;
                            //Console.WriteLine("Character position packet!: " + characterPositionPacket._x + " " + characterPositionPacket._y);
                            _messageForm.UpdateCharacterPosition(characterPositionPacket._id, characterPositionPacket._x, characterPositionPacket._y, characterPositionPacket._direction);
                            break;

                        case PacketType.SPAWNBOMB:
                            SpawnBombPacket spawnBombPacket = (SpawnBombPacket)packet;
                            Console.WriteLine("CLIENT: RETRIEVED BOMB LOCATION");
                            _messageForm.SpawnBomb(spawnBombPacket._posX, spawnBombPacket._posY, spawnBombPacket._id);
                            break;

                        default:
                            break;
                    }
                }

            }
        }

        void ProcessServerResponseWriteUDP()
        {
            while (_udpClient.Client.Connected)
            {
                try
                {
                    if (_localCharactersIds.Contains(_playerId))
                    {
                        int index = _messageForm.bombermanMonoControl1._characterList.FindIndex(cl => cl._id == _playerId);
                        //Keep Sending Character Position
                        if (_messageForm.bombermanMonoControl1._characterList[index]._isMoving)
                        {
                            SendPacketUDP(new CharacterPositionPacket(_playerId, _messageForm.bombermanMonoControl1._characterList[index]._position.X, _messageForm.bombermanMonoControl1._characterList[index]._position.Y, _messageForm.bombermanMonoControl1._characterList[index]._direction));
                        }
                    }
                }
                catch
                {

                }
            }
        }

        public void SendMessage(string message)
        {
            //string prefixedMessage = _nickname + " says: " + message;
            Packet packet = new ChatMessagePacket(_nickname ,message);
            SendPacketTCP(packet);

            Console.WriteLine("MESSAGE SENT!");
        }

        public void SetNickname(string nickname)
        {
            _nickname = nickname;
            Console.WriteLine("Nickname set to: " + _nickname);
        }

        public void Stop()
        {
            _udpReaderThread.Abort();
            _tcpReaderThread.Abort();
            _tcpClient.Close();
        }

        public void SendPacketTCP(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            _tcpWriter.Write(buffer.Length);
            _tcpWriter.Write(buffer);
            _tcpWriter.Flush();
        }

        public Packet RetrievePacketTCP()
        {
            int noOfIncomingBytes;

            while ((noOfIncomingBytes = _tcpReader.ReadInt32()) != 0)
            {
                byte[] buffer = _tcpReader.ReadBytes(noOfIncomingBytes);
                MemoryStream ms = new MemoryStream(buffer);
                BinaryFormatter bf = new BinaryFormatter();
                Packet packet = (Packet)bf.Deserialize(ms);
                return packet;
            }
            return null;
        }

        public void SendPacketUDP(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            _udpClient.Send(buffer, buffer.Length);
        }

        public Packet ReadPacketUDP(ref IPEndPoint endpointref)
        {
            byte[] buffer = _udpClient.Receive(ref endpointref);
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter bf = new BinaryFormatter();
            Packet packet = (Packet)bf.Deserialize(ms);

            return packet;
        }
    }
}
