using System;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Packets;
using System.Runtime.Serialization.Formatters.Binary;

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
        NetworkStream _stream;
        BinaryWriter _writer;
        BinaryReader _reader;
        Thread _readerThread;
        ClientForm _messageForm;
        SetNicknameForm _nicknameForm;
        string _nickname;

        public SimpleClient()
        {
            _messageForm = new ClientForm(this);
            _nicknameForm = new SetNicknameForm(this);
        }

        public bool Connect(string ipAddress, int port)
        {
            _tcpClient = new TcpClient();
            _readerThread = new Thread(new ThreadStart(ProcessServerResponse));

            try
            {
                _tcpClient.Connect(ipAddress, port);
                _stream = _tcpClient.GetStream();
                _writer = new BinaryWriter(_stream, Encoding.UTF8);
                _reader = new BinaryReader(_stream, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                return false;
            }

            // Spawn nickname set dialog in center of chat window
            _nicknameForm.Owner = _messageForm;
            _nicknameForm.StartPosition = FormStartPosition.CenterParent;
            _nicknameForm.ShowDialog();
            // Spawn chat window
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
        }

        void ProcessServerResponse()
        {
            Packet packet;

            while ((packet = RetrievePacket()) != null)
            {
                switch (packet.getPacketType())
                {
                    case PacketType.CHATMESSAGE:
                        ChatMessagePacket chPck = (ChatMessagePacket)packet;
                        _messageForm.UpdateChatWindow(chPck._message);
                        break;
                    case PacketType.NICKNAME:
                        break;
                    default:
                        break;
                }
            }
        }

        public void SendMessage(string message)
        {
            Packet packet = new ChatMessagePacket(message);
            SendPacket(packet);

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

        public void SendPacket(Packet packet)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, packet);
            byte[] buffer = ms.GetBuffer();

            _writer.Write(buffer.Length);
            _writer.Write(buffer);
            _writer.Flush();
        }

        public Packet RetrievePacket()
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
    }
}
