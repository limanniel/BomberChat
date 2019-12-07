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
using Bomberman;
using Microsoft.Xna.Framework;

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
        Vector2 _localCharacterPosition;
        ClientForm _messageForm;
        List<string> _nicknamesList;
        public string _nickname { get; private set; }
        public SetNicknameForm _nicknameForm { get; private set; }

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
            _messageForm = new ClientForm(this);
            _nicknameForm = new SetNicknameForm(this);
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
            SendPacketTCP(new LoginPacket(_udpClient.Client.LocalEndPoint));
            // Send initial nickname of the client
            SendPacketTCP(new NicknamePacket(_nickname));
        }

        void ProcessServerResponseTCP()
        {
            Packet packet;

            while ((packet = RetrievePacketTCP()) != null)
            {
                switch (packet.getPacketType())
                {
                    case PacketType.CHATMESSAGE:
                        ChatMessagePacket chPck = (ChatMessagePacket)packet;
                        _messageForm.UpdateChatWindow(chPck._message);
                        break;

                    case PacketType.NICKNAME:
                        break;

                    case PacketType.LOGIN:
                        LoginPacket lgnPacket = (LoginPacket)packet;
                        _udpClient.Connect((IPEndPoint)lgnPacket._endPoint);
                        _udpReaderThread = new Thread(new ThreadStart(ProcessServerResponseReadUDP));
                        _udpReaderThread.Start();
                        _udpWriterThread = new Thread(new ThreadStart(ProcessServerResponseWriteUDP));
                        _udpWriterThread.Start();
                        Console.WriteLine("UDP CONNECTED!");
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
                            NicknamesList nicknameListPacket = packet as NicknamesList;
                            if (!_nicknamesList.SequenceEqual(nicknameListPacket._nicknamesList))
                            {
                                _nicknamesList = nicknameListPacket._nicknamesList;
                                _messageForm.UpdateNicknamesList(ref _nicknamesList);
                            }
                            break;

                        case PacketType.CHARACTERPOSITION:
                            CharacterPositionPacket characterPositionPacket = (CharacterPositionPacket)packet;
                            Console.WriteLine("Character position packet!: " + characterPositionPacket._x + " " + characterPositionPacket._y);
                            _messageForm.UpdateCharacterPosition(characterPositionPacket._x, characterPositionPacket._y, characterPositionPacket._direction);
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
                // Keep Sending Character Position
                if (_messageForm.bombermanMonoControl1._character._isMoving)
                {
                    SendPacketUDP(new CharacterPositionPacket(_messageForm.bombermanMonoControl1._character._position.X, _messageForm.bombermanMonoControl1._character._position.Y, _messageForm.bombermanMonoControl1._character._direction));
                }
            }
        }

        public void SendMessage(string message)
        {
            //string prefixedMessage = _nickname + " says: " + message;
            Packet packet = new ChatMessagePacket(message);
            SendPacketTCP(packet);

            Console.WriteLine("SEND!");
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
