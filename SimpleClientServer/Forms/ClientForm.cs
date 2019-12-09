using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleServer
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string nickname, string message);
        delegate void UpdateNicknamesListDelegate(ref List<string> nicknamesList);
        UpdateChatWindowDelegate updateChatWindowDelegate;
        UpdateNicknamesListDelegate updateNicknamesDelegate;
        SimpleClient client;

        public ClientForm(SimpleClient client)
        {
            this.client = client;
            InitializeComponent();
            updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
            updateNicknamesDelegate = new UpdateNicknamesListDelegate(UpdateNicknamesList);
            chatSendBox.Select();
        }

        public void UpdateChatWindow(string nickname, string message)
        {
            if (messageDisplayBox.InvokeRequired)
            {
                Invoke(updateChatWindowDelegate, nickname, message);
            }
            else
            {
                messageDisplayBox.Text += nickname;
                if (nickname.Contains("[DIRECT MESSAGE]"))
                {
                    messageDisplayBox.Text += " whispers: ";
                }
                else if (nickname.Contains("[SERVER]"))
                {
                    messageDisplayBox.Text += " ";
                }
                else
                {
                    messageDisplayBox.Text += " says: ";
                }
                
                messageDisplayBox.Text += message;
                messageDisplayBox.SelectionStart = messageDisplayBox.Text.Length;
                messageDisplayBox.ScrollToCaret();
                messageDisplayBox.Text += "\n";
            }
        }

        public void UpdateNicknamesList(ref List<string> nicknamesList)
        {
            if (messageDisplayBox.InvokeRequired)
            {
                Invoke(updateNicknamesDelegate, nicknamesList);
            }
            else
            {
                // Clear Previous Nicknames
                NicknamesList.Items.Clear();

                // Populate List with new nicknames
                foreach (string nickname in nicknamesList)
                {
                    NicknamesList.Items.Add(nickname);
                }

            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            client.Run();
        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            client.Stop();
        }

        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            // Don't allow empty messages to be sent over as well as ones starting with whitespaces
            if (chatSendBox.Text.Length != 0 && !chatSendBox.Text.StartsWith(" "))
            {
                client.SendMessage(chatSendBox.Text);
                chatSendBox.Clear();
            }
            chatSendBox.Focus();
        }

        private void NicknamesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                // Double clicked own nick -> pop-up window to change it
                if (client._nickname == NicknamesList.SelectedItem.ToString())
                {
                    client._nicknameForm.ShowDialog();
                    client.SendPacketTCP(new Packets.NicknamePacket(client._nickname));
                }
                // Double clicked else -> fill chat with prefix to send DM
                else
                {
                    chatSendBox.Clear();
                    chatSendBox.Text += "@";
                    chatSendBox.Text += NicknamesList.SelectedItem.ToString();
                    chatSendBox.Text += "  ";
                    chatSendBox.Select(chatSendBox.Text.Length - 1, 0);
                    chatSendBox.ScrollToCaret();
                    chatSendBox.Focus();
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        //
        ////  GAME RELATED
        //

        public void UpdateCharacterPosition(int id, float x, float y, int direction)
        {
            if (client._playerId != id)
            {
                int index = bombermanMonoControl1._characterList.FindIndex(cl => cl._id == id);
                if (index != -1)
                {
                    if (bombermanMonoControl1._characterList[index]._position.X != x || bombermanMonoControl1._characterList[index]._position.Y != y)
                    {
                        bombermanMonoControl1._characterList[index]._position.X = x;
                        bombermanMonoControl1._characterList[index]._position.Y = y;
                        bombermanMonoControl1._characterList[index].UpdateAnimation(bombermanMonoControl1.Editor.GameTime, direction);
                    }
                }
            }
        }

        public void AssignCharacter(int id)
        {
            int index = CharacterIDToIndex(id);
            if (index != -1)
            {
                bombermanMonoControl1._characterList[index]._possessed = true;
            }
        }

        public void CreateCharacter(int id, int r, int g, int b)
        {
            bombermanMonoControl1.CreateCharacter(id, r, g, b);
        }

        public void RemoveCharacter(int id)
        {
            int index = CharacterIDToIndex(id);
            if (index != -1)
            {
                bombermanMonoControl1.RemoveCharacet(index);
            }
        }

        private void bombermanMonoControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                int index = bombermanMonoControl1._characterList.FindIndex(cl => cl._id == client._playerId);
                if (index != -1)
                {
                    if (bombermanMonoControl1._characterList[index]._canSpawnBomb)
                    {
                        bombermanMonoControl1.SpawnBomb(bombermanMonoControl1._characterList[index]._position, client._playerId);
                        bombermanMonoControl1._characterList[index]._canSpawnBomb = false;
                        client.SendPacketTCP(new Packets.SpawnBombPacket(bombermanMonoControl1._characterList[index]._position.X, bombermanMonoControl1._characterList[index]._position.Y, client._playerId));
                    }
                }
            }

            if (e.KeyCode == Keys.E)
            {
                client.SendPacketTCP(new Packets.RemoveCharacterPacket(client._playerId));
            }
        }

        public void SpawnBomb(float posX, float posY, int playerID)
        {
            bombermanMonoControl1.SpawnBomb(posX, posY, playerID);
        }

        public int CharacterIDToIndex(int id)
        {
            return bombermanMonoControl1._characterList.FindIndex(cl => cl._id == id);
        }
    }
}
