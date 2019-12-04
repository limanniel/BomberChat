using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Packets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;

namespace SimpleClientServer
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
        string _nickname = "";
        ClientForm _messageForm;
        SetNicknameForm _nicknameForm;

        // TCP
        TcpClient _tcpClient;
        Thread _readerThread;
        NetworkStream _tcpStream;
        BinaryWriter _tcpWriter;
        BinaryReader _tcpReader;
        // UDP
        UdpClient _udpClient;
        Thread _udpServerProcess;

        public SimpleClient()
        {
            _messageForm = new ClientForm(this);
            _nicknameForm = new SetNicknameForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            _udpClient = new UdpClient();
            _readerThread = new Thread(new ThreadStart(ProcessServerResponse));

            try
            {
                _tcpClient.Connect(ipAddress, port);
                _tcpStream = _tcpClient.GetStream();
                _tcpWriter = new BinaryWriter(_tcpStream, Encoding.UTF8);
                _tcpReader = new BinaryReader(_tcpStream, Encoding.UTF8);
                _udpClient.Connect(ipAddress, port);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }

            // Display Set Nickname window
            _nicknameForm.Owner = _messageForm;
            _nicknameForm.StartPosition = FormStartPosition.CenterParent;
            do
            {
                _nicknameForm.ShowDialog();
            } while (_nickname == "");

            // Display Chat window
            Application.Run(_messageForm);

            if (!_readerThread.IsAlive)
            {
                return false;
            }

            return true;
        }

        public void Run()
        {
            Console.WriteLine("STARTED");
            _readerThread.Start();
            SendPacketTCP(new LoginPacket(_udpClient.Client.LocalEndPoint));
        }

        void ProcessServerResponse()
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
                        _udpServerProcess = new Thread(new ThreadStart(ProcessServerResponseUDP));
                        _udpServerProcess.Start();
                        Console.Write("UDP CONNECTED?");
                        break;

                    default:
                        break;
                }
            }
        }

        void ProcessServerResponseUDP()
        {
            Console.WriteLine("UDP KILLED MY DOG!");
        }

        public void SendMessage(string message)
        {
            string prefixedMessage = _nickname + " says: " + message;
            Packet packet = new ChatMessagePacket(prefixedMessage);
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
            _readerThread.Abort();
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

            _udpClient.Send(buffer, 256);
        }

        public Packet ReadPacketUDP(ref IPEndPoint endpointref)
        {
            _udpClient.Receive(ref endpointref);
            byte[] buffer = _udpClient.Receive(ref endpointref);
            MemoryStream ms = new MemoryStream(buffer);
            BinaryFormatter bf = new BinaryFormatter();
            Packet packet = (Packet)bf.Deserialize(ms);

            return packet;
        }
    }
}
