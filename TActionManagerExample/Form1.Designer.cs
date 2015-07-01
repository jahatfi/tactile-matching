namespace TActionManagerExample
{
    partial class TActionManagerExampleForm
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
            this.ConsoleOutputRichTextBox = new System.Windows.Forms.RichTextBox();
            this.PulseTactor1Button = new System.Windows.Forms.Button();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DiscoverButton = new System.Windows.Forms.Button();
            this.ComPortLabel = new System.Windows.Forms.Label();
            this.ComPortComboBox = new System.Windows.Forms.ComboBox();
            this.TActionsListBox = new System.Windows.Forms.ListBox();
            this.PlayTActionButton = new System.Windows.Forms.Button();
            this.PlaySelectedTActionToSegment = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConsoleOutputRichTextBox
            // 
            this.ConsoleOutputRichTextBox.Location = new System.Drawing.Point(15, 391);
            this.ConsoleOutputRichTextBox.Name = "ConsoleOutputRichTextBox";
            this.ConsoleOutputRichTextBox.ReadOnly = true;
            this.ConsoleOutputRichTextBox.Size = new System.Drawing.Size(259, 157);
            this.ConsoleOutputRichTextBox.TabIndex = 16;
            this.ConsoleOutputRichTextBox.Text = "";
            // 
            // PulseTactor1Button
            // 
            this.PulseTactor1Button.Enabled = false;
            this.PulseTactor1Button.Location = new System.Drawing.Point(15, 139);
            this.PulseTactor1Button.Name = "PulseTactor1Button";
            this.PulseTactor1Button.Size = new System.Drawing.Size(257, 44);
            this.PulseTactor1Button.TabIndex = 15;
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
            this.ConnectButton.TabIndex = 14;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // DiscoverButton
            // 
            this.DiscoverButton.Location = new System.Drawing.Point(15, 39);
            this.DiscoverButton.Name = "DiscoverButton";
            this.DiscoverButton.Size = new System.Drawing.Size(257, 44);
            this.DiscoverButton.TabIndex = 13;
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
            this.ComPortLabel.TabIndex = 12;
            this.ComPortLabel.Text = "Com Port";
            // 
            // ComPortComboBox
            // 
            this.ComPortComboBox.FormattingEnabled = true;
            this.ComPortComboBox.Location = new System.Drawing.Point(68, 12);
            this.ComPortComboBox.Name = "ComPortComboBox";
            this.ComPortComboBox.Size = new System.Drawing.Size(204, 21);
            this.ComPortComboBox.TabIndex = 11;
            // 
            // TActionsListBox
            // 
            this.TActionsListBox.FormattingEnabled = true;
            this.TActionsListBox.Location = new System.Drawing.Point(15, 189);
            this.TActionsListBox.Name = "TActionsListBox";
            this.TActionsListBox.Size = new System.Drawing.Size(256, 95);
            this.TActionsListBox.TabIndex = 17;
            // 
            // PlayTActionButton
            // 
            this.PlayTActionButton.Enabled = false;
            this.PlayTActionButton.Location = new System.Drawing.Point(15, 290);
            this.PlayTActionButton.Name = "PlayTActionButton";
            this.PlayTActionButton.Size = new System.Drawing.Size(257, 44);
            this.PlayTActionButton.TabIndex = 18;
            this.PlayTActionButton.Text = "Play Selected TAction";
            this.PlayTActionButton.UseVisualStyleBackColor = true;
            this.PlayTActionButton.Click += new System.EventHandler(this.PlayTActionButton_Click);
            // 
            // PlaySelectedTActionToSegment
            // 
            this.PlaySelectedTActionToSegment.Enabled = false;
            this.PlaySelectedTActionToSegment.Location = new System.Drawing.Point(15, 341);
            this.PlaySelectedTActionToSegment.Name = "PlaySelectedTActionToSegment";
            this.PlaySelectedTActionToSegment.Size = new System.Drawing.Size(257, 44);
            this.PlaySelectedTActionToSegment.TabIndex = 19;
            this.PlaySelectedTActionToSegment.Text = "Play Selected To Segment";
            this.PlaySelectedTActionToSegment.UseVisualStyleBackColor = true;
            this.PlaySelectedTActionToSegment.Click += new System.EventHandler(this.PlaySelectedTActionToSegment_Click);
            // 
            // TActionManagerExampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 560);
            this.Controls.Add(this.PlaySelectedTActionToSegment);
            this.Controls.Add(this.PlayTActionButton);
            this.Controls.Add(this.TActionsListBox);
            this.Controls.Add(this.ConsoleOutputRichTextBox);
            this.Controls.Add(this.PulseTactor1Button);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.DiscoverButton);
            this.Controls.Add(this.ComPortLabel);
            this.Controls.Add(this.ComPortComboBox);
            this.Name = "TActionManagerExampleForm";
            this.Text = "TAction Manager Tutorial";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TActionManagerExampleForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox ConsoleOutputRichTextBox;
        private System.Windows.Forms.Button PulseTactor1Button;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DiscoverButton;
        private System.Windows.Forms.Label ComPortLabel;
        private System.Windows.Forms.ComboBox ComPortComboBox;
        private System.Windows.Forms.ListBox TActionsListBox;
        private System.Windows.Forms.Button PlayTActionButton;
        private System.Windows.Forms.Button PlaySelectedTActionToSegment;
    }
}

