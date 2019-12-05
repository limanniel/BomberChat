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
        Hangman _hangmanGame;
        bool _isHangmanActive;

        public Server(string ipAddress, int port)
        {
            _clients = new List<Client>();
            _clientsNicknames = new List<string>();
            _iPAddress = IPAddress.Parse(ipAddress);

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
                    // Chat Meesage Packet Detected
                    case PacketType.CHATMESSAGE:
                        ChatMessagePacket chatPacket = packet as ChatMessagePacket;
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
                                    client.SendPacketTCP(new ChatMessagePacket("[SERVER] " + client._nickname + " started hangman game!"), fClient);
                                    client.SendPacketTCP(new ChatMessagePacket("[SERVER] \n" + _hangmanGame.GetObscuredWord() + "\n"), fClient);
                                }
                            }
                            else
                            {
                                client.SendPacketTCP(new ChatMessagePacket("[SERVER] Hangman game is already in progress!"), client);
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
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER] User: " + client._nickname + " has guessed the word, being: " + _hangmanGame.GetHangmanWord()), fClient);
                                        }
                                        _isHangmanActive = false;
                                    }
                                    // Out of tries
                                    else if (hangmanUpdateResult == 1)
                                    {
                                        foreach (Client fClient in _clients)
                                        {
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER]  \n" + _hangmanGame.GetHangmanASCIIPicture()), fClient);
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER] Out of guesses!, maybe next time you will have more luck :)"), fClient);
                                        }
                                        _isHangmanActive = false;
                                    }
                                    // Send update of the game to the clients if hangman game is active
                                    if (_isHangmanActive)
                                    {
                                        foreach (Client fClient in _clients)
                                        {
                                            client.SendPacketTCP(new ChatMessagePacket("\n[SERVER]  \n" + _hangmanGame.GetHangmanASCIIPicture()), fClient);
                                            client.SendPacketTCP(new ChatMessagePacket("\n" + _hangmanGame.GetObscuredWord() + "\n"), fClient);
                                        }
                                    }
                                }
                            }
                            break;
                        }
                        break;
                        
                    // Nickname packet detected
                    case PacketType.NICKNAME:
                        client._nickname = (packet as NicknamePacket)._nickname;
                        UpdateClientsNicknameList();
                        break;
                        
                    // UDP packet detected
                    case PacketType.LOGIN:
                        HandleLoginPacket(client, (LoginPacket)packet);
                        break;

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
            client.UDPConnect(loginpacket._endPoint);
            client.SendPacketTCP(new LoginPacket(client._udpSocket.LocalEndPoint), client);
            Thread u = new Thread(new ParameterizedThreadStart(UDPClientMethod));
            u.Start(client);
        }

        private void UDPClientMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            Packet packet;

            while (true)
            {
                client.UDPSend(new NicknamesList(_clientsNicknames));

                //packet = client.UDPRead(client);
                //switch (packet.getPacketType())
                //{
                //    default:
                //        break;
                //}

                Thread.Sleep(1000);
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
