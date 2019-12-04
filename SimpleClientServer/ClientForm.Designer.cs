namespace SimpleClientServer
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
            this.SuspendLayout();
            // 
            // messageDisplayBox
            // 
            this.messageDisplayBox.Location = new System.Drawing.Point(12, 12);
            this.messageDisplayBox.Name = "messageDisplayBox";
            this.messageDisplayBox.ReadOnly = true;
            this.messageDisplayBox.Size = new System.Drawing.Size(776, 353);
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
            // ClientForm
            // 
            this.AcceptButton = this.sendMessageButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.sendMessageButton);
            this.Controls.Add(this.chatSendBox);
            this.Controls.Add(this.messageDisplayBox);
            this.Name = "ClientForm";
            this.Text = "Chat Window";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox messageDisplayBox;
        private System.Windows.Forms.RichTextBox chatSendBox;
        private System.Windows.Forms.Button sendMessageButton;
    }
}