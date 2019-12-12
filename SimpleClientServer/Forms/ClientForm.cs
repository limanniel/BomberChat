using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SimpleServer
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string nickname, string message);
        delegate void UpdateNicknamesListDelegate(ref List<string> nicknamesList);
        delegate void UpdateGameJoinButtonDelegate(string nickname, int buttonID);
        delegate void UpdateGameStartButtonDelegate(bool state);
        delegate void StartGameDelegate();
        delegate void RestartGameDelegate();
        UpdateChatWindowDelegate updateChatWindowDelegate;
        UpdateNicknamesListDelegate updateNicknamesDelegate;
        UpdateGameJoinButtonDelegate updateGameJoinButtonDelegate;
        UpdateGameStartButtonDelegate updateGameStartButtonDelegate;
        StartGameDelegate startGameDelegate;
        RestartGameDelegate restartGameDelegate;
        SimpleClient client;
        bool _joinedGame;

        public ClientForm(SimpleClient client)
        {
            this.client = client;
            InitializeComponent();
            updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
            updateNicknamesDelegate = new UpdateNicknamesListDelegate(UpdateNicknamesList);
            updateGameJoinButtonDelegate = new UpdateGameJoinButtonDelegate(UpdateJoinGameButton);
            updateGameStartButtonDelegate = new UpdateGameStartButtonDelegate(EnableStartGameButton);
            startGameDelegate = new StartGameDelegate(StartGame);
            restartGameDelegate = new RestartGameDelegate(RestartGame);
            _joinedGame = false;
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

        private void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.SendPacketTCP(new Packets.ExitPacket());
            client._udpReadThreadExit = true;
            client._udpWriteThreadExit = true;
            client._tcpThreadExit = true;
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

        private void ClearChatButton_MouseClick(object sender, MouseEventArgs e)
        {
            messageDisplayBox.Clear();
        }

        private void HelpInfoButton_MouseClick(object sender, MouseEventArgs e)
        {
            messageDisplayBox.Text += "------------------------------------------------ [ HELP ] ------------------------------------------------\n";
            messageDisplayBox.Text += "\n[*] To change nickname, double click on own name within Nickname List\n";
            messageDisplayBox.Text += "\n[*] To direct message a person, either double click on their nick within Nickname list. Or prefix manually message with @nick [message]\n";
            messageDisplayBox.Text += "\n[*] Type '!hangman' to start a game of hangman server wide, type your responses by prefixing them with '!'. For example: '!c' or '!word' \n";
            messageDisplayBox.Text += "\n--------------------------------------------------------------------------------------------------------------";
        }

        private void JoinGameButton1_MouseClick(object sender, MouseEventArgs e)
        {
            _joinedGame = true;
            client.SendPacketTCP(new Packets.JoinGamePacket(client._playerId, client._nickname, 1));
        }

        private void JoinGameButton2_MouseClick(object sender, MouseEventArgs e)
        {
            _joinedGame = true;
            client.SendPacketTCP(new Packets.JoinGamePacket(client._playerId, client._nickname, 2));
        }

        private void JoinGameButton3_Click(object sender, EventArgs e)
        {
            _joinedGame = true;
            client.SendPacketTCP(new Packets.JoinGamePacket(client._playerId, client._nickname, 3));
        }

        private void JoinGameButton4_Click(object sender, EventArgs e)
        {
            _joinedGame = true;
            client.SendPacketTCP(new Packets.JoinGamePacket(client._playerId, client._nickname, 4));
        }

        public void UpdateJoinGameButton(string nickname, int buttonID)
        {
            if (GameLobbyPanel.InvokeRequired)
            {
                Invoke(updateGameJoinButtonDelegate, nickname, buttonID);
            }
            else
            {
                switch (buttonID)
                {
                    case 1:
                        JoinGameButton1.Text = nickname;
                        JoinGameButton1.Enabled = false;
                        break;
                    case 2:
                        JoinGameButton2.Text = nickname;
                        JoinGameButton2.Enabled = false;
                        break;
                    case 3:
                        JoinGameButton3.Text = nickname;
                        JoinGameButton3.Enabled = false;
                        break;
                    case 4:
                        JoinGameButton4.Text = nickname;
                        JoinGameButton4.Enabled = false;
                        break;

                    default:
                        break;
                }
                if (_joinedGame)
                {
                    JoinGameButton1.Enabled = false;
                    JoinGameButton2.Enabled = false;
                    JoinGameButton3.Enabled = false;
                    JoinGameButton4.Enabled = false;
                }
            }    
        }

        public void EnableStartGameButton(bool state)
        {
            if (GameLobbyPanel.InvokeRequired)
            {
                Invoke(updateGameStartButtonDelegate, state);
            }
            else
            {
                StartGameButton.Enabled = state;
            }
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            client.SendPacketTCP(new Packets.StartGamePacket(true));
        }

        public void StartGame()
        {
            if (GameLobbyPanel.InvokeRequired)
            {
                Invoke(startGameDelegate);
            }
            else
            {
                GameLobbyPanel.Enabled = false;
                GameLobbyPanel.Visible = false;
            }
        }

        public void RestartGame()
        {
            if (GameLobbyPanel.InvokeRequired)
            {
                Invoke(restartGameDelegate);
            }
            else
            {
                // Clear local game characters
                bombermanMonoControl1._characterList.Clear();
                _joinedGame = false;

                // Reset Buttons
                JoinGameButton1.Text = "Join!";
                JoinGameButton1.Enabled = true;
                JoinGameButton2.Text = "Join!";
                JoinGameButton2.Enabled = true;
                JoinGameButton3.Text = "Join!";
                JoinGameButton3.Enabled = true;
                JoinGameButton4.Text = "Join!";
                JoinGameButton4.Enabled = true;

                // Reset Start Game Button
                StartGameButton.Enabled = false;

                // Re-open lobby panel
                GameLobbyPanel.Enabled = true;
                GameLobbyPanel.Visible = true;
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
                bombermanMonoControl1.RemoveCharacter(index);
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
        }

        public void SpawnBomb(float posX, float posY, int playerID)
        {
            bombermanMonoControl1.SpawnBomb(posX, posY, playerID);
        }

        public int CharacterIDToIndex(int id)
        {
            return bombermanMonoControl1._characterList.FindIndex(cl => cl._id == id);
        }

        public bool CheckIfThereAreCharactersToDelete()
        {
            // List empty
            if (!bombermanMonoControl1._charactersToDelete.Any())
            {
                return false;
            }
            // Contains Characters waiting to be deleted
            else
            {
                return true;
            }
        }

        public void DeleteCharacters()
        {
            bombermanMonoControl1._charactersToDelete.ToList().ForEach(ctd =>
            {
                client.SendPacketTCP(new Packets.RemoveCharacterPacket(ctd));
                bombermanMonoControl1._charactersToDelete.Remove(ctd);
            });
        }
    }
}
