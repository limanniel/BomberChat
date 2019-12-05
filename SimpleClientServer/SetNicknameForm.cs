using System;
using System.Windows.Forms;

namespace SimpleClientServer
{
    public partial class SetNicknameForm : Form
    {
        SimpleClient _client;

        public SetNicknameForm(SimpleClient client)
        {
            _client = client;
            InitializeComponent();
        }

        private void NicknameSubmitButton_Click(object sender, EventArgs e)
        {
            _client.SetNickname(NicknameTextBox.Text);
            NicknameTextBox.Clear();
            Close();
        }
    }
}
