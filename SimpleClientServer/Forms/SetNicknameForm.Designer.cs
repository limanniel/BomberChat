namespace SimpleServer
{
    partial class SetNicknameForm
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
            this.NicknameTextBox = new System.Windows.Forms.TextBox();
            this.NicknameText = new System.Windows.Forms.Label();
            this.NicknameSubmitButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NicknameTextBox
            // 
            this.NicknameTextBox.Location = new System.Drawing.Point(32, 68);
            this.NicknameTextBox.MaxLength = 16;
            this.NicknameTextBox.Name = "NicknameTextBox";
            this.NicknameTextBox.Size = new System.Drawing.Size(281, 20);
            this.NicknameTextBox.TabIndex = 0;
            this.NicknameTextBox.WordWrap = false;
            // 
            // NicknameText
            // 
            this.NicknameText.AutoSize = true;
            this.NicknameText.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknameText.Location = new System.Drawing.Point(28, 25);
            this.NicknameText.Name = "NicknameText";
            this.NicknameText.Size = new System.Drawing.Size(285, 24);
            this.NicknameText.TabIndex = 1;
            this.NicknameText.Text = "Please enter your new nickname";
            // 
            // NicknameSubmitButton
            // 
            this.NicknameSubmitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NicknameSubmitButton.Location = new System.Drawing.Point(32, 110);
            this.NicknameSubmitButton.Name = "NicknameSubmitButton";
            this.NicknameSubmitButton.Size = new System.Drawing.Size(281, 36);
            this.NicknameSubmitButton.TabIndex = 2;
            this.NicknameSubmitButton.Text = "Submit";
            this.NicknameSubmitButton.UseVisualStyleBackColor = true;
            this.NicknameSubmitButton.Click += new System.EventHandler(this.NicknameSubmitButton_Click);
            // 
            // SetNicknameForm
            // 
            this.AcceptButton = this.NicknameSubmitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(345, 158);
            this.Controls.Add(this.NicknameSubmitButton);
            this.Controls.Add(this.NicknameText);
            this.Controls.Add(this.NicknameTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetNicknameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox NicknameTextBox;
        private System.Windows.Forms.Label NicknameText;
        private System.Windows.Forms.Button NicknameSubmitButton;
    }
}