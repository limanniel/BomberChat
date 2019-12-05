using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using Packets;
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
                        case PacketType.CHATMESSAGE:
                            foreach (Client fClient in _clients)
                            {
                                client.SendPacketTCP(packet, fClient);
                            }
                            break;

                        case PacketType.NICKNAME:
                            client._nickname = (packet as NicknamePacket)._nickname;
                            UpdateClientsNicknameList();
                            break;

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
            int noOfIncomingBytes;
            byte[] buffer = new byte[256];
            if ((noOfIncomingBytes = _udpSocket.Receive(buffer)) != 0)
            {
                client._udpSocket.Receive(buffer);
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
