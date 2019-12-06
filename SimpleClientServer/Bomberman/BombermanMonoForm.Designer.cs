namespace Bomberman
{
    partial class BombermanMonoForm
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
            this.bombermanMonoControl = new Bomberman.BombermanMonoControl();
            this.SuspendLayout();
            // 
            // bombermanMonoControl
            // 
            this.bombermanMonoControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bombermanMonoControl.Location = new System.Drawing.Point(0, 0);
            this.bombermanMonoControl.MouseHoverUpdatesOnly = false;
            this.bombermanMonoControl.Name = "bombermanMonoControl";
            this.bombermanMonoControl.Size = new System.Drawing.Size(800, 450);
            this.bombermanMonoControl.TabIndex = 0;
            this.bombermanMonoControl.Text = "bombermanMonoControl";
            // 
            // BombermanMonoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.bombermanMonoControl);
            this.Name = "BombermanMonoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BombermanMonoForm";
            this.ResumeLayout(false);

        }

        #endregion

        private BombermanMonoControl bombermanMonoControl;
    }
}