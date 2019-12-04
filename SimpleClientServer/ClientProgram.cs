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
        TcpClient _tcpClient;
        UdpClient _udpClient;
        NetworkStream _stream;
        BinaryWriter _writer;
        BinaryReader _reader;
        Thread _readerThread;
        Thread _udpServerProcess;
        ClientForm _messageForm;
        IPEndPoint _ipEndPoint;

        public SimpleClient()
        {
            _messageForm = new ClientForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            _udpClient = new UdpClient();
            _readerThread = new Thread(new ThreadStart(ProcessServerResponse));
            //_ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), 4444);

            try
            {
                _tcpClient.Connect(ipAddress, port);
                _udpClient.Connect(ipAddress, port);
                _stream = _tcpClient.GetStream();
                _writer = new BinaryWriter(_stream, Encoding.UTF8);
                _reader = new BinaryReader(_stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }

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
           // SendPacketTCP(new LoginPacket(_ipEndPoint));

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
            Packet packet = new ChatMessagePacket(message);
            SendPacketTCP(packet);

            Console.WriteLine("SEND!");
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

            _writer.Write(buffer.Length);
            _writer.Write(buffer);
            _writer.Flush();
        }

        public Packet RetrievePacketTCP()
        {
            int noOfIncomingBytes;

            while ((noOfIncomingBytes = _reader.ReadInt32()) != 0)
            {
                byte[] buffer = _reader.ReadBytes(noOfIncomingBytes);
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
