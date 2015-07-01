namespace ResponseCallback
{
    partial class ResponseCallbackForm
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
            this.PulseTactor1Button = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DiscoverButton = new System.Windows.Forms.Button();
            this.ComPortLabel = new System.Windows.Forms.Label();
            this.ComPortComboBox = new System.Windows.Forms.ComboBox();
            this.ConsoleOutputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // PulseTactor1Button
            // 
            this.PulseTactor1Button.Enabled = false;
            this.PulseTactor1Button.Location = new System.Drawing.Point(15, 139);
            this.PulseTactor1Button.Name = "PulseTactor1Button";
            this.PulseTactor1Button.Size = new System.Drawing.Size(257, 44);
            this.PulseTactor1Button.TabIndex = 9;
            this.PulseTactor1Button.Text = "Pulse Tactor 1";
            this.PulseTactor1Button.UseVisualStyleBackColor = true;
            this.PulseTactor1Button.Click += new System.EventHandler(this.PulseTactor1Button_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(15, 89);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(257, 44);
            this.ConnectButton.TabIndex = 8;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // DiscoverButton
            // 
            this.DiscoverButton.Location = new System.Drawing.Point(15, 39);
            this.DiscoverButton.Name = "DiscoverButton";
            this.DiscoverButton.Size = new System.Drawing.Size(257, 44);
            this.DiscoverButton.TabIndex = 7;
            this.DiscoverButton.Text = "Discover";
            this.DiscoverButton.UseVisualStyleBackColor = true;
            this.DiscoverButton.Click += new System.EventHandler(this.DiscoverButton_Click);
            // 
            // ComPortLabel
            // 
            this.ComPortLabel.AutoSize = true;
            this.ComPortLabel.Location = new System.Drawing.Point(12, 15);
            this.ComPortLabel.Name = "ComPortLabel";
            this.ComPortLabel.Size = new System.Drawing.Size(50, 13);
            this.ComPortLabel.TabIndex = 6;
            this.ComPortLabel.Text = "Com Port";
            // 
            // ComPortComboBox
            // 
            this.ComPortComboBox.FormattingEnabled = true;
            this.ComPortComboBox.Location = new System.Drawing.Point(68, 12);
            this.ComPortComboBox.Name = "ComPortComboBox";
            this.ComPortComboBox.Size = new System.Drawing.Size(204, 21);
            this.ComPortComboBox.TabIndex = 5;
            // 
            // ConsoleOutputRichTextBox
            // 
            this.ConsoleOutputRichTextBox.Location = new System.Drawing.Point(12, 189);
            this.ConsoleOutputRichTextBox.Name = "ConsoleOutputRichTextBox";
            this.ConsoleOutputRichTextBox.ReadOnly = true;
            this.ConsoleOutputRichTextBox.Size = new System.Drawing.Size(259, 157);
            this.ConsoleOutputRichTextBox.TabIndex = 10;
            this.ConsoleOutputRichTextBox.Text = "";
            // 
            // ResponseCallbackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 357);
            this.Controls.Add(this.ConsoleOutputRichTextBox);
            this.Controls.Add(this.PulseTactor1Button);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.DiscoverButton);
            this.Controls.Add(this.ComPortLabel);
            this.Controls.Add(this.ComPortComboBox);
            this.Name = "ResponseCallbackForm";
            this.Text = "Response Callback Turtorial";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ResponseCallbackForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PulseTactor1Button;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DiscoverButton;
        private System.Windows.Forms.Label ComPortLabel;
        private System.Windows.Forms.ComboBox ComPortComboBox;
        private System.Windows.Forms.RichTextBox ConsoleOutputRichTextBox;
    }
}

