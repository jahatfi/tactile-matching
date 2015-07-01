namespace AdvancedActions
{
    partial class AdvancedActionsForm
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
            this.ComPortComboBox = new System.Windows.Forms.ComboBox();
            this.ComPortLabel = new System.Windows.Forms.Label();
            this.DiscoverButton = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.PulseTactor1Button = new System.Windows.Forms.Button();
            this.AdvancedAction1Button = new System.Windows.Forms.Button();
            this.AdvancedAction2Button = new System.Windows.Forms.Button();
            this.ConsoleOutputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.StoreTActionButton = new System.Windows.Forms.Button();
            this.PlayStoredTActionButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ComPortComboBox
            // 
            this.ComPortComboBox.FormattingEnabled = true;
            this.ComPortComboBox.Location = new System.Drawing.Point(68, 13);
            this.ComPortComboBox.Name = "ComPortComboBox";
            this.ComPortComboBox.Size = new System.Drawing.Size(204, 21);
            this.ComPortComboBox.TabIndex = 0;
            // 
            // ComPortLabel
            // 
            this.ComPortLabel.AutoSize = true;
            this.ComPortLabel.Location = new System.Drawing.Point(12, 16);
            this.ComPortLabel.Name = "ComPortLabel";
            this.ComPortLabel.Size = new System.Drawing.Size(50, 13);
            this.ComPortLabel.TabIndex = 1;
            this.ComPortLabel.Text = "Com Port";
            // 
            // DiscoverButton
            // 
            this.DiscoverButton.Location = new System.Drawing.Point(15, 40);
            this.DiscoverButton.Name = "DiscoverButton";
            this.DiscoverButton.Size = new System.Drawing.Size(257, 44);
            this.DiscoverButton.TabIndex = 2;
            this.DiscoverButton.Text = "Discover";
            this.DiscoverButton.UseVisualStyleBackColor = true;
            this.DiscoverButton.Click += new System.EventHandler(this.DiscoverButton_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Enabled = false;
            this.ConnectButton.Location = new System.Drawing.Point(15, 90);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(257, 44);
            this.ConnectButton.TabIndex = 3;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // PulseTactor1Button
            // 
            this.PulseTactor1Button.Enabled = false;
            this.PulseTactor1Button.Location = new System.Drawing.Point(15, 140);
            this.PulseTactor1Button.Name = "PulseTactor1Button";
            this.PulseTactor1Button.Size = new System.Drawing.Size(257, 44);
            this.PulseTactor1Button.TabIndex = 4;
            this.PulseTactor1Button.Text = "Pulse Tactor 1";
            this.PulseTactor1Button.UseVisualStyleBackColor = true;
            this.PulseTactor1Button.Click += new System.EventHandler(this.PulseTactor1Button_Click);
            // 
            // AdvancedAction1Button
            // 
            this.AdvancedAction1Button.Enabled = false;
            this.AdvancedAction1Button.Location = new System.Drawing.Point(15, 190);
            this.AdvancedAction1Button.Name = "AdvancedAction1Button";
            this.AdvancedAction1Button.Size = new System.Drawing.Size(257, 44);
            this.AdvancedAction1Button.TabIndex = 5;
            this.AdvancedAction1Button.Text = "Advanced Action 1";
            this.AdvancedAction1Button.UseVisualStyleBackColor = true;
            this.AdvancedAction1Button.Click += new System.EventHandler(this.AdvancedAction1Button_Click);
            // 
            // AdvancedAction2Button
            // 
            this.AdvancedAction2Button.Enabled = false;
            this.AdvancedAction2Button.Location = new System.Drawing.Point(15, 240);
            this.AdvancedAction2Button.Name = "AdvancedAction2Button";
            this.AdvancedAction2Button.Size = new System.Drawing.Size(257, 44);
            this.AdvancedAction2Button.TabIndex = 6;
            this.AdvancedAction2Button.Text = "Advanced Action 2";
            this.AdvancedAction2Button.UseVisualStyleBackColor = true;
            this.AdvancedAction2Button.Click += new System.EventHandler(this.AdvancedAction2Button_Click);
            // 
            // ConsoleOutputRichTextBox
            // 
            this.ConsoleOutputRichTextBox.Location = new System.Drawing.Point(15, 404);
            this.ConsoleOutputRichTextBox.Name = "ConsoleOutputRichTextBox";
            this.ConsoleOutputRichTextBox.ReadOnly = true;
            this.ConsoleOutputRichTextBox.Size = new System.Drawing.Size(259, 157);
            this.ConsoleOutputRichTextBox.TabIndex = 7;
            this.ConsoleOutputRichTextBox.Text = "";
            // 
            // StoreTActionButton
            // 
            this.StoreTActionButton.Enabled = false;
            this.StoreTActionButton.Location = new System.Drawing.Point(15, 290);
            this.StoreTActionButton.Name = "StoreTActionButton";
            this.StoreTActionButton.Size = new System.Drawing.Size(257, 44);
            this.StoreTActionButton.TabIndex = 8;
            this.StoreTActionButton.Text = "Store TAction";
            this.StoreTActionButton.UseVisualStyleBackColor = true;
            this.StoreTActionButton.Click += new System.EventHandler(this.StoreTActionButton_Click);
            // 
            // PlayStoredTActionButton
            // 
            this.PlayStoredTActionButton.Enabled = false;
            this.PlayStoredTActionButton.Location = new System.Drawing.Point(15, 340);
            this.PlayStoredTActionButton.Name = "PlayStoredTActionButton";
            this.PlayStoredTActionButton.Size = new System.Drawing.Size(257, 44);
            this.PlayStoredTActionButton.TabIndex = 9;
            this.PlayStoredTActionButton.Text = "Play Stored TAction";
            this.PlayStoredTActionButton.UseVisualStyleBackColor = true;
            this.PlayStoredTActionButton.Click += new System.EventHandler(this.PlayStoredTActionButton_Click);
            // 
            // AdvancedActionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 573);
            this.Controls.Add(this.PlayStoredTActionButton);
            this.Controls.Add(this.StoreTActionButton);
            this.Controls.Add(this.ConsoleOutputRichTextBox);
            this.Controls.Add(this.AdvancedAction2Button);
            this.Controls.Add(this.AdvancedAction1Button);
            this.Controls.Add(this.PulseTactor1Button);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.DiscoverButton);
            this.Controls.Add(this.ComPortLabel);
            this.Controls.Add(this.ComPortComboBox);
            this.Name = "AdvancedActionsForm";
            this.Text = "Advanced Actions Tutorial";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdvancedActionsForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComPortComboBox;
        private System.Windows.Forms.Label ComPortLabel;
        private System.Windows.Forms.Button DiscoverButton;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button PulseTactor1Button;
        private System.Windows.Forms.Button AdvancedAction1Button;
        private System.Windows.Forms.Button AdvancedAction2Button;
        private System.Windows.Forms.RichTextBox ConsoleOutputRichTextBox;
        private System.Windows.Forms.Button StoreTActionButton;
        private System.Windows.Forms.Button PlayStoredTActionButton;
    }
}

