using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimpleServer
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string message);
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

        public void UpdateChatWindow(string message)
        {
            if (messageDisplayBox.InvokeRequired)
            {
                Invoke(updateChatWindowDelegate, message);
            }
            else
            {
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
            client.SendMessage(chatSendBox.Text);
            chatSendBox.Clear();
            chatSendBox.Focus();
        }

        private void NicknamesList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (client._nickname == NicknamesList.SelectedItem.ToString() && e.Clicks == 2)
                {
                    client._nicknameForm.ShowDialog();
                    client.SendPacketTCP(new Packets.NicknamePacket(client._nickname));
                }
            }
            catch (NullReferenceException)
            {
            }
        }

        public void UpdateCharacterPosition(int id, float x, float y, int direction)
        {
            if (client._playerId != id)
            {
                if (bombermanMonoControl1._characterList[id]._position.X != x || bombermanMonoControl1._characterList[id]._position.Y != y)
                {
                    bombermanMonoControl1._characterList[id]._position.X = x;
                    bombermanMonoControl1._characterList[id]._position.Y = y;
                    bombermanMonoControl1._characterList[id].UpdateAnimation(bombermanMonoControl1.Editor.GameTime, direction);
                }
            }
        }

        public void AssignCharacter(int id)
        {
            bombermanMonoControl1._characterList[id]._possessed = true;
        }

        public void CreateCharacter(int r, int g, int b)
        {
            bombermanMonoControl1.CreateCharacter(r, g, b);
        }
    }
}
