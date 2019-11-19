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
        IPAddress _iPAddress;
        List<Client> _clients;

        public Server(string ipAddress, int port)
        {
            _clients = new List<Client>();
            _iPAddress = IPAddress.Parse(ipAddress);
            try
            {
                _tcpListener = new TcpListener(_iPAddress, 4444);
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
                Socket socket = _tcpListener.AcceptSocket();

                if (socket.Connected)
                {
                    Console.WriteLine("Connection established!");
                    Client c = new Client(socket);
                    _clients.Add(c);
                    Thread t = new Thread(new ParameterizedThreadStart(ClientMethod));
                    t.Start(c);

                    Console.WriteLine("Clients: " + _clients.Count);
                }
            }
        }

        public void Stop()
        {
            _tcpListener.Stop();
        }

        private void ClientMethod(object clientObj)
        {
            Client client = (Client)clientObj;
            Packet packet;

            try
            {
                while ((packet = RetrievePacket(client)) != null)
                {
                    switch (packet.getPacketType())
                    {
                        case PacketType.CHATMESSAGE:
                            foreach (Client fClient in _clients)
                            {
                                SendPacket(packet, fClient);
                            }
                            break;

                        case PacketType.NICKNAME:
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
            }
        }

        public void SendPacket(Packet packet, Client client)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            client.Writer.Write(buffer.Length);
            client.Writer.Write(buffer);
            client.Writer.Flush();
        }
        public Packet RetrievePacket(Client client)
        {
            int noOfIncomingBytes;

            while ((noOfIncomingBytes = client.Reader.ReadInt32()) != 0)
            {
                byte[] buffer = client.Reader.ReadBytes(noOfIncomingBytes);
                MemoryStream ms = new MemoryStream(buffer);
                BinaryFormatter bf = new BinaryFormatter();
                Packet packet = (Packet)bf.Deserialize(ms);
                return packet;
            }
            return null;
        }
    }

    class Client
    {
        Socket _socket;
        NetworkStream _stream;

        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }

        public Client(Socket socket)
        {
            _socket = socket;
            _stream = new NetworkStream(socket);
            Reader = new BinaryReader(_stream, Encoding.UTF8);
            Writer = new BinaryWriter(_stream, Encoding.UTF8);
        }
        public void Close()
        {
            _socket.Close();
        }
    }
}
