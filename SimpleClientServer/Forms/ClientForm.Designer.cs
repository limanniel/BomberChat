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
            this.bombermanMonoControl1 = new Bomberman.BombermanMonoControl();
            this.SuspendLayout();
            // 
            // messageDisplayBox
            // 
            this.messageDisplayBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageDisplayBox.Location = new System.Drawing.Point(12, 12);
            this.messageDisplayBox.MaxLength = 255;
            this.messageDisplayBox.Name = "messageDisplayBox";
            this.messageDisplayBox.ReadOnly = true;
            this.messageDisplayBox.Size = new System.Drawing.Size(556, 353);
            this.messageDisplayBox.TabIndex = 0;
            this.messageDisplayBox.Text = "";
            // 
            // chatSendBox
            // 
            this.chatSendBox.Location = new System.Drawing.Point(12, 388);
            this.chatSendBox.Name = "chatSendBox";
            this.chatSendBox.Size = new System.Drawing.Size(657, 37);
            this.chatSendBox.TabIndex = 1;
            this.chatSendBox.Text = "";
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.Location = new System.Drawing.Point(685, 388);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(103, 37);
            this.sendMessageButton.TabIndex = 2;
            this.sendMessageButton.Text = "Send";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // NicknamesList
            // 
            this.NicknamesList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknamesList.FormattingEnabled = true;
            this.NicknamesList.ItemHeight = 20;
            this.NicknamesList.Location = new System.Drawing.Point(578, 38);
            this.NicknamesList.Name = "NicknamesList";
            this.NicknamesList.Size = new System.Drawing.Size(210, 324);
            this.NicknamesList.TabIndex = 3;
            this.NicknamesList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NicknamesList_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(574, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Active Users";
            // 
            // bombermanMonoControl1
            // 
            this.bombermanMonoControl1.Location = new System.Drawing.Point(805, 12);
            this.bombermanMonoControl1.MouseHoverUpdatesOnly = false;
            this.bombermanMonoControl1.Name = "bombermanMonoControl1";
            this.bombermanMonoControl1.Size = new System.Drawing.Size(932, 719);
            this.bombermanMonoControl1.TabIndex = 5;
            this.bombermanMonoControl1.Text = "bombermanMonoControl1";
            this.bombermanMonoControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.bombermanMonoControl1_KeyDown);
            // 
            // ClientForm
            // 
            this.AcceptButton = this.sendMessageButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1749, 743);
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
    }
}