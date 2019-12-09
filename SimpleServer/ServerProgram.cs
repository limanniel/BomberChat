using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Packets;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleServer
{
    class Character
    {
        // Character Position
        public int _id;
        public float _PosX { get; set; }
        public float _PosY { get; set; }
        public int _direction { get; set; }
        // Character Colour
        public int _ColourR, _ColourG, _ColourB;
        public Character(int id, int r, int g, int b)
        {
            _id = id;
            _PosX = 0.0f;
            _PosY = 0.0f;
            _direction = 0;
            _ColourR = r;
            _ColourG = g;
            _ColourB = b;
        }
    }

    class ActivePlayer
    {
        public int _id { get; set; }
        public string _nickname { get; set; }
        public int _buttonID { get; set; }
        public ActivePlayer(int id, string nickname, int buttonID)
        {
            _id = id;
            _nickname = nickname;
            _buttonID = buttonID;
        }
    }

    class ServerProgram
    {
        static void Main(string[] args)
        {
            Server myServer = new Server("127.0.0.1", 4444);
            myServer.Start();
            myServer.Stop();
        }
    }

    class Server
    {
        TcpListener _tcpListener;
        Socket _tcpSocket;
        IPAddress _iPAddress;
        List<Client> _clients;
        List<string> _clientsNicknames;
        List<ActivePlayer> _playersInGame;
        Hangman _hangmanGame;
        bool _isHangmanActive;
        bool _isGameActive;
        int _count;
        List<Character> _characters;

        public Server(string ipAddress, int port)
        {
            _clients = new List<Client>();
            _clientsNicknames = new List<string>();
            _iPAddress = IPAddress.Parse(ipAddress);
            _characters = new List<Character>();
            _playersInGame = new List<ActivePlayer>();
            _isHangmanActive = false;
            _isGameActive = false;
            _count = 0;

            try
            {
                _tcpListener = new TcpListener(_iPAddress, port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void Start()
        {
            _tcpListener.Start();
            while (true)
            {
                 _tcpSocket = _tcpListener.AcceptSocket();

                if (_tcpSocket.Connected)
                {
                    Console.WriteLine("Connection established!");
                    Client c = new Client(_tcpSocket);
                    _clients.Add(c);
                    Thread t = new Thread(new ParameterizedThreadStart(TCPClientMethod));
                    t.Start(c);

                    Console.WriteLine("Clients: " + _clients.Count);
                }
            }
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }

        private void TCPClientMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            Packet packet;

            try
            {
                while ((packet = client.RetrievePacketTCP(client)) != null)
                {
                    switch (packet.getPacketType())
                    {
                        #region Chat Message
                        // Chat Meesage Packet Detected
                        case PacketType.CHATMESSAGE:
                            ChatMessagePacket chatPacket = packet as ChatMessagePacket;
                        #region Direct Message
                            // Direct message
                            if (chatPacket._message.Length != 0)
                            {
                                if (chatPacket._message[0] == '@')
                                {
                                    string receiver = chatPacket._message.Substring(0, chatPacket._message.IndexOf(' ')); // Extract receipent
                                    string message = chatPacket._message.Substring(chatPacket._message.IndexOf(' ') + 1, (chatPacket._message.Length - receiver.Length) - 1); // Extract Message
                                    receiver = receiver.Remove(0, 1); // Remove @ prefix
                                    int indexes = _clients.FindIndex(cl => cl._nickname == receiver); // Check if receipent is on the server

                                    if (indexes != -1)
                                    {
                                        _clients[indexes].SendPacketTCP(new DirectMessagePacket("[DIRECT MESSAGE] " + client._nickname, message), _clients[indexes]);
                                    }
                                    else
                                    {
                                        client.SendPacketTCP(new ChatMessagePacket("[SERVER] ", "User of that id hasn't been found :c"), client);
                                    }
                                    break;
                                }
                            }
                        #endregion
                        #region Hangman Message
                        // Normal Message
                        switch (chatPacket._message)
                        {
                            // Hangman command activation
                            case "!hangman":
                                // Check if hangman game is active already
                                if (!_isHangmanActive)
                                {
                                    _isHangmanActive = true;
                                    _hangmanGame = new Hangman();
                                    foreach (Client fClient in _clients)
                                    {
                                        client.SendPacketTCP(new ChatMessagePacket("[SERVER] ", client._nickname + " started hangman game!"), fClient);
                                        client.SendPacketTCP(new ChatMessagePacket("[SERVER] \n", _hangmanGame.GetObscuredWord() + "\n"), fClient);
                                    }
                                }
                                else
                                {
                                    client.SendPacketTCP(new ChatMessagePacket("[SERVER] ", "Hangman game is already in progress!"), client);
                                }
                                break;

                            // No command detected, relay normally the message to all connected clients
                            default:
                                // Relay sent message to everyone that's connected
                                foreach (Client fClient in _clients)
                                {
                                    client.SendPacketTCP(packet, fClient);
                                }

                            // If hangman game is active
                            if (_isHangmanActive)
                            {
                                // Check if message contains "!" prefix to indicate that message is for the hangman game
                                if (chatPacket._message[0] == '!')
                                {
                                    int hangmanUpdateResult = _hangmanGame.Update(chatPacket._message);
                                    // Win
                                    if (hangmanUpdateResult == 2)
                                    {
                                        foreach (Client fClient in _clients)
                                        {
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER] ", "User: " + client._nickname + " has guessed the word, being: " + _hangmanGame.GetHangmanWord()), fClient);
                                        }
                                        _isHangmanActive = false;
                                    }
                                    // Out of tries
                                    else if (hangmanUpdateResult == 1)
                                    {
                                        foreach (Client fClient in _clients)
                                        {
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER]  \n", _hangmanGame.GetHangmanASCIIPicture()), fClient);
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER] ", "Out of guesses!, maybe next time you will have more luck :)"), fClient);
                                        }
                                        _isHangmanActive = false;
                                    }
                                    // Send update of the game to the clients if hangman game is active
                                    if (_isHangmanActive)
                                    {
                                        foreach (Client fClient in _clients)
                                        {
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER]  \n",  _hangmanGame.GetHangmanASCIIPicture()), fClient);
                                            client.SendPacketTCP(new ChatMessagePacket("\n", _hangmanGame.GetObscuredWord() + "\n"), fClient);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        break;
                        #endregion
                        #endregion

                        #region Nickname
                        // Nickname packet detected
                        case PacketType.NICKNAME:
                        client._nickname = (packet as NicknamePacket)._nickname;
                        UpdateClientsNicknameList();
                        break;
                        #endregion

                        #region Login
                        // UDP packet detected
                        case PacketType.LOGIN:
                            HandleLoginPacket(client, (LoginPacket)packet);
                            // If game is in-progress, synchronise newely connected player
                            if (_isGameActive)
                            {
                                // Send over players characters
                                foreach (ActivePlayer activePlayer in _playersInGame)
                                {
                                    int characterIndex = _characters.FindIndex(ch => ch._id == activePlayer._id);
                                    client.SendPacketTCP(new CreateCharacterPacket(_characters[characterIndex]._id, _characters[characterIndex]._ColourR, _characters[characterIndex]._ColourG, _characters[characterIndex]._ColourB), client);
                                }
                                // Get off lobby screen
                                client.SendPacketTCP(new StartGamePacket(true), client);
                            }
                            if (_playersInGame.Count > 0)
                            {
                                foreach (ActivePlayer activePlayer in _playersInGame)
                                {
                                    client.SendPacketTCP(new JoinGamePacket(activePlayer._id, activePlayer._nickname, activePlayer._buttonID), client);
                                }
                            }
                            break;
                        #endregion

                        #region Join Game
                        case PacketType.JOINGAME:
                                JoinGamePacket joinGamePacket = (JoinGamePacket)packet;
                                _playersInGame.Add(new ActivePlayer(joinGamePacket._id, joinGamePacket._nickname, joinGamePacket._buttonID));
                                foreach (Client fClient in _clients)
                                {
                                    client.SendPacketTCP(joinGamePacket, fClient);
                                }
                                // Create Character for the player that joined
                                Random random = new Random();
                                _characters.Add(new Character(joinGamePacket._id, random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));

                                // Check if enough players to start game
                                if (_playersInGame.Count >= 2)
                                {
                                    foreach (ActivePlayer activePlayer in _playersInGame)
                                    {
                                        int playerIndex = _playersInGame.FindIndex(pg => pg._id == activePlayer._id);
                                        client.SendPacketTCP(new StartGameButtonPacket(true), _clients[playerIndex]);
                                    }
                                }
                                break;
                        #endregion

                        #region Start Game
                        case PacketType.STARTGAME:
                            // Send character to everyone
                            foreach (Client fClient in _clients)
                            {
                                foreach (ActivePlayer activePlayer in _playersInGame)
                                {
                                    int characterIndex = _characters.FindIndex(ch => ch._id == activePlayer._id);
                                    client.SendPacketTCP(new CreateCharacterPacket(_characters[characterIndex]._id, _characters[characterIndex]._ColourR, _characters[characterIndex]._ColourG, _characters[characterIndex]._ColourB), fClient);
                                }
                            }

                            // Let participating players possess their characters
                            foreach (ActivePlayer activePlayer in _playersInGame)
                            {
                                int playerIndex = _playersInGame.FindIndex(pg => pg._id == activePlayer._id);
                                client.SendPacketTCP(new AssignCharacterPacket(activePlayer._id), _clients[playerIndex]);
                            }

                            // Start the game for everyone
                            foreach (Client fClient in _clients)
                            {
                                client.SendPacketTCP(new StartGamePacket(true), fClient);
                            }
                            _isGameActive = true;
                            break;
                        #endregion

                        #region Remove Character
                        case PacketType.REMOVECHARACTER:
                            RemoveCharacterPacket removeCharacterPacket = (RemoveCharacterPacket)packet;
                            int index = _characters.FindIndex(ch => ch._id == removeCharacterPacket._id);
                            if (index != -1)
                            {
                                _characters.RemoveAt(index);
                                _clients.ForEach(cl =>
                                {
                                    client.SendPacketTCP(removeCharacterPacket, cl);
                                });
                            }

                            // Check if just one play standing
                            if (_characters.Count == 1)
                            {
                                _isGameActive = false;
                                int winnerIndex = _clients.FindIndex(cl => cl._id == _characters[0]._id);
                                foreach (Client fClient in _clients)
                                {
                                    // Notify clients of the winner
                                    client.SendPacketTCP(new ChatMessagePacket("[SERVER] ", "Player: " + _clients[winnerIndex]._nickname + " won a game of bomberman!"), fClient);
                                    // Restart clients games
                                    client.SendPacketTCP(new RestartGamePacket(), fClient);
                                }

                                // Clear characters and active players
                                _characters.Clear();
                                _playersInGame.Clear();
                            }
                            break;
                        #endregion

                        #region SpawnBomb
                        case PacketType.SPAWNBOMB:
                            Console.WriteLine("BOMB SPAWNED");
                            _clients.ForEach(cl =>
                            {
                                if (cl != client)
                                {
                                    cl.SendPacketTCP(packet, cl);
                                }
                            });
                            break;
                        #endregion

                        default:
                            break;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                client.Close();
                _clients.Remove(client);
                Console.WriteLine("Client Disconnected!");
                Console.WriteLine("Clients: " + _clients.Count);
                UpdateClientsNicknameList();
            }
        }

        void HandleLoginPacket(Client client, LoginPacket loginpacket)
        {
            client._id = _count;
            client.UDPConnect(loginpacket._endPoint);
            client.SendPacketTCP(new LoginPacket(client._udpSocket.LocalEndPoint, _count), client);
            _count++;
            Thread w = new Thread(new ParameterizedThreadStart(UDPClientWriteMethod));
            Thread r = new Thread(new ParameterizedThreadStart(UDPClientReadMethod));
            w.Start(client);
            r.Start(client);
        }

        private void UDPClientWriteMethod(object clientObj)
        {
            Client client = (Client)clientObj;

            while (true)
            {
                // Keep pinging nickname list to clients
                client.UDPSend(new NicknamesListPacket(_clientsNicknames));

                // Send server's character positions to clients
                for (var i = 0; i < _characters.Count; i++)
                {
                    client.UDPSend(new CharacterPositionPacket(_characters[i]._id, _characters[i]._PosX, _characters[i]._PosY, _characters[i]._direction));
                }
                
                Thread.Sleep(10);
            }
        }

        private void UDPClientReadMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            Packet packet;
            while (true)
            {
                packet = client.UDPRead(client);
                switch (packet.getPacketType())
                {
                    case PacketType.CHARACTERPOSITION:
                        CharacterPositionPacket characterPositionPacket = (CharacterPositionPacket)packet;
                        int index = _characters.FindIndex(ch => ch._id == characterPositionPacket._id);
                        if (index != -1)
                        {
                            _clients.ForEach(cl => 
                            {
                                if (_characters[index]._PosX != characterPositionPacket._x || _characters[index]._PosY != characterPositionPacket._y)
                                {
                                    _characters[index]._PosX = characterPositionPacket._x;
                                    _characters[index]._PosY = characterPositionPacket._y;
                                    _characters[index]._direction = characterPositionPacket._direction;
                                }
                            });
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        void UpdateClientsNicknameList()
        {
            _clientsNicknames.Clear(); // Clear current list
            foreach (Client client in _clients)
            {
                _clientsNicknames.Add(client._nickname);
            }
        }
    }

    class Client
    {
        Socket _socket;
        NetworkStream _stream;
        public Socket _udpSocket;

        public string _nickname { get; set; }
        public int _id { get; set; }
        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }

        public Client(Socket socket)
        {
            _socket = socket;
            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _stream = new NetworkStream(socket);
            Reader = new BinaryReader(_stream, Encoding.UTF8);
            Writer = new BinaryWriter(_stream, Encoding.UTF8);
        }

        public void Close()
        {
            _socket.Close();
        }

        public void UDPConnect(EndPoint clientConnection)
        {
            _udpSocket.Connect(clientConnection);
        }

        public void UDPSend(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            _udpSocket.Send(buffer);
        }

        public Packet UDPRead(Client client)
        {
            byte[] buffer = new byte[1024];
            if (_udpSocket.Receive(buffer) != 0)
            {
                MemoryStream ms = new MemoryStream(buffer);
                BinaryFormatter bf = new BinaryFormatter();
                Packet packet = (Packet)bf.Deserialize(ms);
                return packet;
            }
            return null;    
        }

        public Packet RetrievePacketTCP(Client client)
        {
            int noOfIncomingBytes;

            while ((noOfIncomingBytes = client.Reader.ReadInt32()) != 0)
            {
                byte[] buffer = client.Reader.ReadBytes(noOfIncomingBytes);
                MemoryStream ms = new MemoryStream(buffer);
                BinaryFormatter bf = new BinaryFormatter();
                Packet packet = bf.Deserialize(ms) as Packet;
                return packet;
            }
            return null;
        }

        public void SendPacketTCP(Packet packet, Client client)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            client.Writer.Write(buffer.Length);
            client.Writer.Write(buffer);
            client.Writer.Flush();
        }
    }
}
