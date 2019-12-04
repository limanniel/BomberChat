using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleClientServer
{
    public partial class ClientForm : Form
    {
        delegate void UpdateChatWindowDelegate(string message);
        UpdateChatWindowDelegate updateChatWindowDelegate;
        SimpleClient client;

        public ClientForm(SimpleClient client)
        {
            this.client = client;
            InitializeComponent();
            updateChatWindowDelegate = new UpdateChatWindowDelegate(UpdateChatWindow);
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

        private void chatSendBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
