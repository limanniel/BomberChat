namespace SimpleServer
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.messageDisplayBox = new System.Windows.Forms.RichTextBox();
            this.chatSendBox = new System.Windows.Forms.RichTextBox();
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.NicknamesList = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ClearChatButton = new System.Windows.Forms.Button();
            this.HelpInfoButton = new System.Windows.Forms.Button();
            this.GameLobbyPanel = new System.Windows.Forms.Panel();
            this.Player4Label = new System.Windows.Forms.Label();
            this.Player3Label = new System.Windows.Forms.Label();
            this.Player2Label = new System.Windows.Forms.Label();
            this.Player1Label = new System.Windows.Forms.Label();
            this.JoinGameButton4 = new System.Windows.Forms.Button();
            this.JoinGameButton3 = new System.Windows.Forms.Button();
            this.JoinGameButton2 = new System.Windows.Forms.Button();
            this.StartGameButton = new System.Windows.Forms.Button();
            this.JoinGameButton1 = new System.Windows.Forms.Button();
            this.bombermanMonoControl1 = new Bomberman.BombermanMonoControl();
            this.GameLobbyPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messageDisplayBox
            // 
            this.messageDisplayBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.messageDisplayBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageDisplayBox.Location = new System.Drawing.Point(12, 12);
            this.messageDisplayBox.MaxLength = 255;
            this.messageDisplayBox.Name = "messageDisplayBox";
            this.messageDisplayBox.ReadOnly = true;
            this.messageDisplayBox.Size = new System.Drawing.Size(556, 490);
            this.messageDisplayBox.TabIndex = 0;
            this.messageDisplayBox.Text = "";
            // 
            // chatSendBox
            // 
            this.chatSendBox.BackColor = System.Drawing.Color.LightSteelBlue;
            this.chatSendBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chatSendBox.Location = new System.Drawing.Point(12, 550);
            this.chatSendBox.Name = "chatSendBox";
            this.chatSendBox.Size = new System.Drawing.Size(657, 37);
            this.chatSendBox.TabIndex = 1;
            this.chatSendBox.Text = "";
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.BackColor = System.Drawing.Color.AliceBlue;
            this.sendMessageButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendMessageButton.Location = new System.Drawing.Point(685, 550);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(103, 37);
            this.sendMessageButton.TabIndex = 2;
            this.sendMessageButton.Text = "Send";
            this.sendMessageButton.UseVisualStyleBackColor = false;
            this.sendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // NicknamesList
            // 
            this.NicknamesList.BackColor = System.Drawing.Color.LightSteelBlue;
            this.NicknamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknamesList.FormattingEnabled = true;
            this.NicknamesList.ItemHeight = 20;
            this.NicknamesList.Location = new System.Drawing.Point(587, 38);
            this.NicknamesList.Name = "NicknamesList";
            this.NicknamesList.Size = new System.Drawing.Size(201, 464);
            this.NicknamesList.TabIndex = 3;
            this.NicknamesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NicknamesList_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(583, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Active Users";
            // 
            // ClearChatButton
            // 
            this.ClearChatButton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClearChatButton.Location = new System.Drawing.Point(587, 508);
            this.ClearChatButton.Name = "ClearChatButton";
            this.ClearChatButton.Size = new System.Drawing.Size(93, 26);
            this.ClearChatButton.TabIndex = 8;
            this.ClearChatButton.Text = "Clear Chat";
            this.ClearChatButton.UseVisualStyleBackColor = false;
            this.ClearChatButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ClearChatButton_MouseClick);
            // 
            // HelpInfoButton
            // 
            this.HelpInfoButton.BackColor = System.Drawing.Color.LightSteelBlue;
            this.HelpInfoButton.Location = new System.Drawing.Point(695, 508);
            this.HelpInfoButton.Name = "HelpInfoButton";
            this.HelpInfoButton.Size = new System.Drawing.Size(93, 26);
            this.HelpInfoButton.TabIndex = 9;
            this.HelpInfoButton.Text = "Help";
            this.HelpInfoButton.UseVisualStyleBackColor = false;
            this.HelpInfoButton.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HelpInfoButton_MouseClick);
            // 
            // GameLobbyPanel
            // 
            this.GameLobbyPanel.BackColor = System.Drawing.Color.SkyBlue;
            this.GameLobbyPanel.Controls.Add(this.Player4Label);
            this.GameLobbyPanel.Controls.Add(this.Player3Label);
            this.GameLobbyPanel.Controls.Add(this.Player2Label);
            this.GameLobbyPanel.Controls.Add(this.Player1Label);
            this.GameLobbyPanel.Controls.Add(this.JoinGameButton4);
            this.GameLobbyPanel.Controls.Add(this.JoinGameButton3);
            this.GameLobbyPanel.Controls.Add(this.JoinGameButton2);
            this.GameLobbyPanel.Controls.Add(this.StartGameButton);
            this.GameLobbyPanel.Controls.Add(this.JoinGameButton1);
            this.GameLobbyPanel.Location = new System.Drawing.Point(805, 4);
            this.GameLobbyPanel.Name = "GameLobbyPanel";
            this.GameLobbyPanel.Size = new System.Drawing.Size(756, 606);
            this.GameLobbyPanel.TabIndex = 10;
            // 
            // Player4Label
            // 
            this.Player4Label.AutoSize = true;
            this.Player4Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player4Label.Location = new System.Drawing.Point(603, 477);
            this.Player4Label.Name = "Player4Label";
            this.Player4Label.Size = new System.Drawing.Size(77, 24);
            this.Player4Label.TabIndex = 1;
            this.Player4Label.Text = "Player 4";
            // 
            // Player3Label
            // 
            this.Player3Label.AutoSize = true;
            this.Player3Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player3Label.Location = new System.Drawing.Point(69, 477);
            this.Player3Label.Name = "Player3Label";
            this.Player3Label.Size = new System.Drawing.Size(77, 24);
            this.Player3Label.TabIndex = 1;
            this.Player3Label.Text = "Player 3";
            // 
            // Player2Label
            // 
            this.Player2Label.AutoSize = true;
            this.Player2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player2Label.Location = new System.Drawing.Point(603, 5);
            this.Player2Label.Name = "Player2Label";
            this.Player2Label.Size = new System.Drawing.Size(77, 24);
            this.Player2Label.TabIndex = 1;
            this.Player2Label.Text = "Player 2";
            // 
            // Player1Label
            // 
            this.Player1Label.AutoSize = true;
            this.Player1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Player1Label.Location = new System.Drawing.Point(69, 5);
            this.Player1Label.Name = "Player1Label";
            this.Player1Label.Size = new System.Drawing.Size(77, 24);
            this.Player1Label.TabIndex = 1;
            this.Player1Label.Text = "Player 1";
            // 
            // JoinGameButton4
            // 
            this.JoinGameButton4.BackColor = System.Drawing.Color.Khaki;
            this.JoinGameButton4.Location = new System.Drawing.Point(570, 504);
            this.JoinGameButton4.Name = "JoinGameButton4";
            this.JoinGameButton4.Size = new System.Drawing.Size(157, 64);
            this.JoinGameButton4.TabIndex = 0;
            this.JoinGameButton4.Text = "Join!";
            this.JoinGameButton4.UseVisualStyleBackColor = false;
            this.JoinGameButton4.Click += new System.EventHandler(this.JoinGameButton4_Click);
            // 
            // JoinGameButton3
            // 
            this.JoinGameButton3.BackColor = System.Drawing.Color.Khaki;
            this.JoinGameButton3.Location = new System.Drawing.Point(30, 504);
            this.JoinGameButton3.Name = "JoinGameButton3";
            this.JoinGameButton3.Size = new System.Drawing.Size(157, 64);
            this.JoinGameButton3.TabIndex = 0;
            this.JoinGameButton3.Text = "Join!";
            this.JoinGameButton3.UseVisualStyleBackColor = false;
            this.JoinGameButton3.Click += new System.EventHandler(this.JoinGameButton3_Click);
            // 
            // JoinGameButton2
            // 
            this.JoinGameButton2.BackColor = System.Drawing.Color.Khaki;
            this.JoinGameButton2.Location = new System.Drawing.Point(570, 34);
            this.JoinGameButton2.Name = "JoinGameButton2";
            this.JoinGameButton2.Size = new System.Drawing.Size(157, 64);
            this.JoinGameButton2.TabIndex = 0;
            this.JoinGameButton2.Text = "Join!";
            this.JoinGameButton2.UseVisualStyleBackColor = false;
            this.JoinGameButton2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.JoinGameButton2_MouseClick);
            // 
            // StartGameButton
            // 
            this.StartGameButton.BackColor = System.Drawing.Color.Goldenrod;
            this.StartGameButton.Enabled = false;
            this.StartGameButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartGameButton.Location = new System.Drawing.Point(272, 237);
            this.StartGameButton.Name = "StartGameButton";
            this.StartGameButton.Size = new System.Drawing.Size(214, 113);
            this.StartGameButton.TabIndex = 0;
            this.StartGameButton.Text = "START GAME";
            this.StartGameButton.UseVisualStyleBackColor = false;
            this.StartGameButton.Click += new System.EventHandler(this.StartGameButton_Click);
            // 
            // JoinGameButton1
            // 
            this.JoinGameButton1.BackColor = System.Drawing.Color.Khaki;
            this.JoinGameButton1.Location = new System.Drawing.Point(30, 34);
            this.JoinGameButton1.Name = "JoinGameButton1";
            this.JoinGameButton1.Size = new System.Drawing.Size(157, 64);
            this.JoinGameButton1.TabIndex = 0;
            this.JoinGameButton1.Text = "Join!";
            this.JoinGameButton1.UseVisualStyleBackColor = false;
            this.JoinGameButton1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.JoinGameButton1_MouseClick);
            // 
            // bombermanMonoControl1
            // 
            this.bombermanMonoControl1.Location = new System.Drawing.Point(805, 4);
            this.bombermanMonoControl1.MouseHoverUpdatesOnly = false;
            this.bombermanMonoControl1.Name = "bombermanMonoControl1";
            this.bombermanMonoControl1.Size = new System.Drawing.Size(756, 606);
            this.bombermanMonoControl1.TabIndex = 5;
            this.bombermanMonoControl1.Text = "bombermanMonoControl1";
            this.bombermanMonoControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bombermanMonoControl1_KeyDown);
            // 
            // ClientForm
            // 
            this.AcceptButton = this.sendMessageButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(1560, 608);
            this.Controls.Add(this.GameLobbyPanel);
            this.Controls.Add(this.HelpInfoButton);
            this.Controls.Add(this.ClearChatButton);
            this.Controls.Add(this.bombermanMonoControl1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NicknamesList);
            this.Controls.Add(this.sendMessageButton);
            this.Controls.Add(this.chatSendBox);
            this.Controls.Add(this.messageDisplayBox);
            this.Name = "ClientForm";
            this.Text = "Chat Window";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.GameLobbyPanel.ResumeLayout(false);
            this.GameLobbyPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox messageDisplayBox;
        private System.Windows.Forms.RichTextBox chatSendBox;
        private System.Windows.Forms.Button sendMessageButton;
        private System.Windows.Forms.ListBox NicknamesList;
        private System.Windows.Forms.Label label1;
        public Bomberman.BombermanMonoControl bombermanMonoControl1;
        private System.Windows.Forms.Button ClearChatButton;
        private System.Windows.Forms.Button HelpInfoButton;
        private System.Windows.Forms.Panel GameLobbyPanel;
        private System.Windows.Forms.Label Player4Label;
        private System.Windows.Forms.Label Player3Label;
        private System.Windows.Forms.Label Player2Label;
        private System.Windows.Forms.Label Player1Label;
        private System.Windows.Forms.Button StartGameButton;
        private System.Windows.Forms.Button JoinGameButton4;
        private System.Windows.Forms.Button JoinGameButton3;
        private System.Windows.Forms.Button JoinGameButton2;
        private System.Windows.Forms.Button JoinGameButton1;
    }
}