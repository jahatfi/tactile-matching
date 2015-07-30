namespace TenPointMatching
{
    partial class TenPointMatchingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TenPointMatchingForm));
            this.MatchingTab = new System.Windows.Forms.TabPage();
            this.PlaySoundButton = new System.Windows.Forms.Button();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.GainLabel = new System.Windows.Forms.Label();
            this.CountLabel = new System.Windows.Forms.Label();
            this.FinishedLabel = new System.Windows.Forms.Label();
            this.InstructionLabel = new System.Windows.Forms.Label();
            this.NextButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.PulseTactorButton = new System.Windows.Forms.Button();
            this.ConfigureTab = new System.Windows.Forms.TabPage();
            this.TrackbarPrecision = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ParticipantNumber = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.connectradio = new System.Windows.Forms.RadioButton();
            this.discoverradio = new System.Windows.Forms.RadioButton();
            this.comportselection = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.EnterButton = new System.Windows.Forms.Button();
            this.ComPortLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Console = new System.Windows.Forms.RichTextBox();
            this.FileName = new System.Windows.Forms.TextBox();
            this.ComPortComboBox = new System.Windows.Forms.ComboBox();
            this.DiscoverButton = new System.Windows.Forms.Button();
            this.StartButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.InstructionsTab = new System.Windows.Forms.TabPage();
            this.PracticePanel = new System.Windows.Forms.Panel();
            this.SoundModePracticeRadio = new System.Windows.Forms.RadioButton();
            this.PlaySoundPracticeButton = new System.Windows.Forms.Button();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.practiceLabel = new System.Windows.Forms.Label();
            this.LetsGetStartedButton = new System.Windows.Forms.Button();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.TactorModePracticeRadio = new System.Windows.Forms.RadioButton();
            this.ColorModePracticeRadio = new System.Windows.Forms.RadioButton();
            this.PracticeImage = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.PulseTactorPracticeButton = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.MatchingTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.ConfigureTab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.InstructionsTab.SuspendLayout();
            this.PracticePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PracticeImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // MatchingTab
            // 
            this.MatchingTab.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MatchingTab.Controls.Add(this.PlaySoundButton);
            this.MatchingTab.Controls.Add(this.pictureBox3);
            this.MatchingTab.Controls.Add(this.FrequencyLabel);
            this.MatchingTab.Controls.Add(this.GainLabel);
            this.MatchingTab.Controls.Add(this.CountLabel);
            this.MatchingTab.Controls.Add(this.InstructionLabel);
            this.MatchingTab.Controls.Add(this.NextButton);
            this.MatchingTab.Controls.Add(this.label4);
            this.MatchingTab.Controls.Add(this.label6);
            this.MatchingTab.Controls.Add(this.label7);
            this.MatchingTab.Controls.Add(this.pictureBox1);
            this.MatchingTab.Controls.Add(this.pictureBox2);
            this.MatchingTab.Controls.Add(this.trackBar1);
            this.MatchingTab.Controls.Add(this.PulseTactorButton);
            this.MatchingTab.Controls.Add(this.FinishedLabel);
            this.MatchingTab.Location = new System.Drawing.Point(4, 25);
            this.MatchingTab.Name = "MatchingTab";
            this.MatchingTab.Padding = new System.Windows.Forms.Padding(3);
            this.MatchingTab.Size = new System.Drawing.Size(924, 549);
            this.MatchingTab.TabIndex = 2;
            this.MatchingTab.Text = "Matching";
            // 
            // PlaySoundButton
            // 
            this.PlaySoundButton.Enabled = false;
            this.PlaySoundButton.Location = new System.Drawing.Point(146, 242);
            this.PlaySoundButton.Name = "PlaySoundButton";
            this.PlaySoundButton.Size = new System.Drawing.Size(89, 29);
            this.PlaySoundButton.TabIndex = 44;
            this.PlaySoundButton.Text = "Play Sound";
            this.PlaySoundButton.UseVisualStyleBackColor = true;
            this.PlaySoundButton.Click += new System.EventHandler(this.PlaySoundButton_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(128, 92);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(128, 134);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 43;
            this.pictureBox3.TabStop = false;
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.AutoSize = true;
            this.FrequencyLabel.Location = new System.Drawing.Point(368, 463);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(83, 17);
            this.FrequencyLabel.TabIndex = 42;
            this.FrequencyLabel.Text = "Frequency: ";
            this.FrequencyLabel.Visible = false;
            // 
            // GainLabel
            // 
            this.GainLabel.AutoSize = true;
            this.GainLabel.Location = new System.Drawing.Point(368, 433);
            this.GainLabel.Name = "GainLabel";
            this.GainLabel.Size = new System.Drawing.Size(42, 17);
            this.GainLabel.TabIndex = 41;
            this.GainLabel.Text = "Gain:";
            this.GainLabel.Visible = false;
            // 
            // CountLabel
            // 
            this.CountLabel.AutoSize = true;
            this.CountLabel.Location = new System.Drawing.Point(256, 400);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(0, 17);
            this.CountLabel.TabIndex = 40;
            // 
            // FinishedLabel
            // 
            this.FinishedLabel.AutoSize = true;
            this.FinishedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FinishedLabel.Location = new System.Drawing.Point(218, 125);
            this.FinishedLabel.Name = "FinishedLabel";
            this.FinishedLabel.Size = new System.Drawing.Size(468, 80);
            this.FinishedLabel.TabIndex = 39;
            this.FinishedLabel.Text = "Finished!  Thanks for participating!\r\n\r\nNote to experimentor: The results file is" +
                " stored on James\' desktop\r\nunder the name <Participant#>Results.txt";
            this.FinishedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.FinishedLabel.Visible = false;
            // 
            // InstructionLabel
            // 
            this.InstructionLabel.AutoSize = true;
            this.InstructionLabel.Location = new System.Drawing.Point(255, 372);
            this.InstructionLabel.Name = "InstructionLabel";
            this.InstructionLabel.Size = new System.Drawing.Size(537, 17);
            this.InstructionLabel.TabIndex = 38;
            this.InstructionLabel.Text = "Use the slider to match the intensity of the brightness to the intensity of the v" +
                "ibration";
            // 
            // NextButton
            // 
            this.NextButton.Enabled = false;
            this.NextButton.Location = new System.Drawing.Point(423, 400);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 23);
            this.NextButton.TabIndex = 36;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(347, 375);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 17);
            this.label4.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(365, 359);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(0, 17);
            this.label6.TabIndex = 33;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(446, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 17);
            this.label7.TabIndex = 32;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(386, 92);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(148, 134);
            this.pictureBox1.TabIndex = 31;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(659, 92);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(128, 134);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 30;
            this.pictureBox2.TabStop = false;
            // 
            // trackBar1
            // 
            this.trackBar1.LargeChange = 1;
            this.trackBar1.Location = new System.Drawing.Point(27, 311);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(894, 45);
            this.trackBar1.TabIndex = 29;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // PulseTactorButton
            // 
            this.PulseTactorButton.Enabled = false;
            this.PulseTactorButton.Location = new System.Drawing.Point(685, 245);
            this.PulseTactorButton.Name = "PulseTactorButton";
            this.PulseTactorButton.Size = new System.Drawing.Size(89, 29);
            this.PulseTactorButton.TabIndex = 28;
            this.PulseTactorButton.Text = "Pulse Tactor";
            this.PulseTactorButton.UseVisualStyleBackColor = true;
            this.PulseTactorButton.Click += new System.EventHandler(this.PulseTactor1Button_Click);
            // 
            // ConfigureTab
            // 
            this.ConfigureTab.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ConfigureTab.Controls.Add(this.TrackbarPrecision);
            this.ConfigureTab.Controls.Add(this.label8);
            this.ConfigureTab.Controls.Add(this.ParticipantNumber);
            this.ConfigureTab.Controls.Add(this.NameLabel);
            this.ConfigureTab.Controls.Add(this.connectradio);
            this.ConfigureTab.Controls.Add(this.discoverradio);
            this.ConfigureTab.Controls.Add(this.comportselection);
            this.ConfigureTab.Controls.Add(this.ConnectButton);
            this.ConfigureTab.Controls.Add(this.EnterButton);
            this.ConfigureTab.Controls.Add(this.ComPortLabel);
            this.ConfigureTab.Controls.Add(this.label5);
            this.ConfigureTab.Controls.Add(this.Console);
            this.ConfigureTab.Controls.Add(this.FileName);
            this.ConfigureTab.Controls.Add(this.ComPortComboBox);
            this.ConfigureTab.Controls.Add(this.DiscoverButton);
            this.ConfigureTab.Controls.Add(this.StartButton);
            this.ConfigureTab.Location = new System.Drawing.Point(4, 25);
            this.ConfigureTab.Name = "ConfigureTab";
            this.ConfigureTab.Padding = new System.Windows.Forms.Padding(3);
            this.ConfigureTab.Size = new System.Drawing.Size(924, 549);
            this.ConfigureTab.TabIndex = 1;
            this.ConfigureTab.Text = "Configure";
            // 
            // TrackbarPrecision
            // 
            this.TrackbarPrecision.Location = new System.Drawing.Point(126, 181);
            this.TrackbarPrecision.Name = "TrackbarPrecision";
            this.TrackbarPrecision.Size = new System.Drawing.Size(67, 23);
            this.TrackbarPrecision.TabIndex = 52;
            this.TrackbarPrecision.Text = "10";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 147);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(291, 17);
            this.label8.TabIndex = 51;
            this.label8.Text = "Set the precision of the track bar. [10-10000]";
            // 
            // ParticipantNumber
            // 
            this.ParticipantNumber.Location = new System.Drawing.Point(96, 83);
            this.ParticipantNumber.Name = "ParticipantNumber";
            this.ParticipantNumber.Size = new System.Drawing.Size(106, 23);
            this.ParticipantNumber.TabIndex = 50;
            this.ParticipantNumber.Text = "3";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(93, 56);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(133, 17);
            this.NameLabel.TabIndex = 49;
            this.NameLabel.Text = "Participant Number:";
            // 
            // connectradio
            // 
            this.connectradio.AutoSize = true;
            this.connectradio.Checked = true;
            this.connectradio.Location = new System.Drawing.Point(645, 88);
            this.connectradio.Name = "connectradio";
            this.connectradio.Size = new System.Drawing.Size(14, 13);
            this.connectradio.TabIndex = 48;
            this.connectradio.TabStop = true;
            this.connectradio.UseVisualStyleBackColor = true;
            // 
            // discoverradio
            // 
            this.discoverradio.AutoSize = true;
            this.discoverradio.Location = new System.Drawing.Point(645, 60);
            this.discoverradio.Name = "discoverradio";
            this.discoverradio.Size = new System.Drawing.Size(14, 13);
            this.discoverradio.TabIndex = 47;
            this.discoverradio.UseVisualStyleBackColor = true;
            // 
            // comportselection
            // 
            this.comportselection.Location = new System.Drawing.Point(666, 85);
            this.comportselection.Name = "comportselection";
            this.comportselection.Size = new System.Drawing.Size(204, 23);
            this.comportselection.TabIndex = 46;
            this.comportselection.Text = "COM4";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(709, 170);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(116, 44);
            this.ConnectButton.TabIndex = 45;
            this.ConnectButton.Text = "Connect!";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // EnterButton
            // 
            this.EnterButton.Location = new System.Drawing.Point(400, 109);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(116, 44);
            this.EnterButton.TabIndex = 44;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_Click);
            // 
            // ComPortLabel
            // 
            this.ComPortLabel.AutoSize = true;
            this.ComPortLabel.Location = new System.Drawing.Point(663, 41);
            this.ComPortLabel.Name = "ComPortLabel";
            this.ComPortLabel.Size = new System.Drawing.Size(66, 17);
            this.ComPortLabel.TabIndex = 42;
            this.ComPortLabel.Text = "Com Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(349, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(290, 34);
            this.label5.TabIndex = 43;
            this.label5.Text = "Enter the name of the configuration file here:\r\n(Note: it must be placed on James" +
                "\' desktop)";
            // 
            // Console
            // 
            this.Console.Location = new System.Drawing.Point(302, 305);
            this.Console.Name = "Console";
            this.Console.Size = new System.Drawing.Size(316, 122);
            this.Console.TabIndex = 41;
            this.Console.Text = "";
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(360, 78);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(204, 23);
            this.FileName.TabIndex = 39;
            this.FileName.Text = "config.txt";
            // 
            // ComPortComboBox
            // 
            this.ComPortComboBox.FormattingEnabled = true;
            this.ComPortComboBox.Location = new System.Drawing.Point(666, 57);
            this.ComPortComboBox.Name = "ComPortComboBox";
            this.ComPortComboBox.Size = new System.Drawing.Size(204, 24);
            this.ComPortComboBox.TabIndex = 36;
            // 
            // DiscoverButton
            // 
            this.DiscoverButton.Location = new System.Drawing.Point(709, 120);
            this.DiscoverButton.Name = "DiscoverButton";
            this.DiscoverButton.Size = new System.Drawing.Size(116, 44);
            this.DiscoverButton.TabIndex = 37;
            this.DiscoverButton.Text = "Discover";
            this.DiscoverButton.UseVisualStyleBackColor = true;
            this.DiscoverButton.Click += new System.EventHandler(this.DiscoverButton_Click);
            // 
            // StartButton
            // 
            this.StartButton.Enabled = false;
            this.StartButton.Location = new System.Drawing.Point(400, 456);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(116, 46);
            this.StartButton.TabIndex = 38;
            this.StartButton.Text = "Start!";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ConfigureTab);
            this.tabControl1.Controls.Add(this.InstructionsTab);
            this.tabControl1.Controls.Add(this.MatchingTab);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(932, 578);
            this.tabControl1.TabIndex = 36;
            // 
            // InstructionsTab
            // 
            this.InstructionsTab.BackColor = System.Drawing.SystemColors.ControlLight;
            this.InstructionsTab.Controls.Add(this.PracticePanel);
            this.InstructionsTab.Controls.Add(this.label17);
            this.InstructionsTab.Controls.Add(this.label3);
            this.InstructionsTab.Location = new System.Drawing.Point(4, 25);
            this.InstructionsTab.Name = "InstructionsTab";
            this.InstructionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.InstructionsTab.Size = new System.Drawing.Size(924, 549);
            this.InstructionsTab.TabIndex = 3;
            this.InstructionsTab.Text = "Instructions";
            // 
            // PracticePanel
            // 
            this.PracticePanel.Controls.Add(this.SoundModePracticeRadio);
            this.PracticePanel.Controls.Add(this.PlaySoundPracticeButton);
            this.PracticePanel.Controls.Add(this.pictureBox5);
            this.PracticePanel.Controls.Add(this.practiceLabel);
            this.PracticePanel.Controls.Add(this.LetsGetStartedButton);
            this.PracticePanel.Controls.Add(this.trackBar2);
            this.PracticePanel.Controls.Add(this.TactorModePracticeRadio);
            this.PracticePanel.Controls.Add(this.ColorModePracticeRadio);
            this.PracticePanel.Controls.Add(this.PracticeImage);
            this.PracticePanel.Controls.Add(this.pictureBox4);
            this.PracticePanel.Controls.Add(this.PulseTactorPracticeButton);
            this.PracticePanel.Location = new System.Drawing.Point(21, 179);
            this.PracticePanel.Name = "PracticePanel";
            this.PracticePanel.Size = new System.Drawing.Size(875, 356);
            this.PracticePanel.TabIndex = 48;
            // 
            // SoundModePracticeRadio
            // 
            this.SoundModePracticeRadio.AutoSize = true;
            this.SoundModePracticeRadio.Location = new System.Drawing.Point(240, 229);
            this.SoundModePracticeRadio.Name = "SoundModePracticeRadio";
            this.SoundModePracticeRadio.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SoundModePracticeRadio.Size = new System.Drawing.Size(106, 21);
            this.SoundModePracticeRadio.TabIndex = 59;
            this.SoundModePracticeRadio.TabStop = true;
            this.SoundModePracticeRadio.Text = "Sound Mode";
            this.SoundModePracticeRadio.UseVisualStyleBackColor = true;
            this.SoundModePracticeRadio.Click += new System.EventHandler(this.SoundModePracticeRadio_Click);
            // 
            // PlaySoundPracticeButton
            // 
            this.PlaySoundPracticeButton.Enabled = false;
            this.PlaySoundPracticeButton.Location = new System.Drawing.Point(170, 184);
            this.PlaySoundPracticeButton.Name = "PlaySoundPracticeButton";
            this.PlaySoundPracticeButton.Size = new System.Drawing.Size(89, 29);
            this.PlaySoundPracticeButton.TabIndex = 58;
            this.PlaySoundPracticeButton.Text = "Play Sound";
            this.PlaySoundPracticeButton.UseVisualStyleBackColor = true;
            this.PlaySoundPracticeButton.Click += new System.EventHandler(this.PlaySoundPracticeButton_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox5.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox5.Image")));
            this.pictureBox5.Location = new System.Drawing.Point(150, 28);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(128, 134);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox5.TabIndex = 56;
            this.pictureBox5.TabStop = false;
            // 
            // practiceLabel
            // 
            this.practiceLabel.AutoSize = true;
            this.practiceLabel.Location = new System.Drawing.Point(629, 278);
            this.practiceLabel.Name = "practiceLabel";
            this.practiceLabel.Size = new System.Drawing.Size(155, 17);
            this.practiceLabel.TabIndex = 54;
            this.practiceLabel.Text = "Image Color Intensity: 0";
            // 
            // LetsGetStartedButton
            // 
            this.LetsGetStartedButton.Location = new System.Drawing.Point(353, 329);
            this.LetsGetStartedButton.Name = "LetsGetStartedButton";
            this.LetsGetStartedButton.Size = new System.Drawing.Size(138, 23);
            this.LetsGetStartedButton.TabIndex = 52;
            this.LetsGetStartedButton.Text = "Let\'s get started!";
            this.LetsGetStartedButton.UseVisualStyleBackColor = true;
            this.LetsGetStartedButton.Click += new System.EventHandler(this.LetsGetStartedButton_Click_1);
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(187, 278);
            this.trackBar2.Maximum = 50;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(427, 45);
            this.trackBar2.TabIndex = 51;
            this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // TactorModePracticeRadio
            // 
            this.TactorModePracticeRadio.AutoSize = true;
            this.TactorModePracticeRadio.Location = new System.Drawing.Point(542, 229);
            this.TactorModePracticeRadio.Name = "TactorModePracticeRadio";
            this.TactorModePracticeRadio.Size = new System.Drawing.Size(106, 21);
            this.TactorModePracticeRadio.TabIndex = 50;
            this.TactorModePracticeRadio.Text = "Tactor Mode";
            this.TactorModePracticeRadio.UseVisualStyleBackColor = true;
            this.TactorModePracticeRadio.Click += new System.EventHandler(this.TactorModePracticeRadio_Click);
            // 
            // ColorModePracticeRadio
            // 
            this.ColorModePracticeRadio.AutoSize = true;
            this.ColorModePracticeRadio.Checked = true;
            this.ColorModePracticeRadio.Location = new System.Drawing.Point(389, 229);
            this.ColorModePracticeRadio.Name = "ColorModePracticeRadio";
            this.ColorModePracticeRadio.Size = new System.Drawing.Size(98, 21);
            this.ColorModePracticeRadio.TabIndex = 49;
            this.ColorModePracticeRadio.TabStop = true;
            this.ColorModePracticeRadio.Text = "Color Mode";
            this.ColorModePracticeRadio.UseVisualStyleBackColor = true;
            this.ColorModePracticeRadio.Click += new System.EventHandler(this.ColorModePracticeRadio_Click);
            // 
            // PracticeImage
            // 
            this.PracticeImage.BackColor = System.Drawing.Color.Black;
            this.PracticeImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PracticeImage.Location = new System.Drawing.Point(353, 28);
            this.PracticeImage.Name = "PracticeImage";
            this.PracticeImage.Size = new System.Drawing.Size(148, 134);
            this.PracticeImage.TabIndex = 47;
            this.PracticeImage.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(583, 28);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(128, 134);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox4.TabIndex = 46;
            this.pictureBox4.TabStop = false;
            // 
            // PulseTactorPracticeButton
            // 
            this.PulseTactorPracticeButton.Enabled = false;
            this.PulseTactorPracticeButton.Location = new System.Drawing.Point(603, 184);
            this.PulseTactorPracticeButton.Name = "PulseTactorPracticeButton";
            this.PulseTactorPracticeButton.Size = new System.Drawing.Size(89, 29);
            this.PulseTactorPracticeButton.TabIndex = 45;
            this.PulseTactorPracticeButton.Text = "Pulse Tactor";
            this.PulseTactorPracticeButton.UseVisualStyleBackColor = true;
            this.PulseTactorPracticeButton.Click += new System.EventHandler(this.PulseTactorPracticeButton_Click);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(446, 29);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(0, 17);
            this.label17.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(759, 153);
            this.label3.TabIndex = 0;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // TenPointMatchingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 593);
            this.Controls.Add(this.tabControl1);
            this.Name = "TenPointMatchingForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MultiModalMatchingForm_FormClosed);
            this.MatchingTab.ResumeLayout(false);
            this.MatchingTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ConfigureTab.ResumeLayout(false);
            this.ConfigureTab.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.InstructionsTab.ResumeLayout(false);
            this.InstructionsTab.PerformLayout();
            this.PracticePanel.ResumeLayout(false);
            this.PracticePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PracticeImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage MatchingTab;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button PulseTactorButton;
        private System.Windows.Forms.TabPage ConfigureTab;
        private System.Windows.Forms.Label ComPortLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox Console;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.ComboBox ComPortComboBox;
        private System.Windows.Forms.Button DiscoverButton;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.Label InstructionLabel;
        private System.Windows.Forms.Label FinishedLabel;
        private System.Windows.Forms.Label CountLabel;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.RadioButton connectradio;
        private System.Windows.Forms.RadioButton discoverradio;
        private System.Windows.Forms.TextBox comportselection;
        private System.Windows.Forms.TextBox ParticipantNumber;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.TabPage InstructionsTab;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label FrequencyLabel;
        private System.Windows.Forms.Label GainLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox TrackbarPrecision;
        private System.Windows.Forms.Panel PracticePanel;
        private System.Windows.Forms.Button LetsGetStartedButton;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.RadioButton TactorModePracticeRadio;
        private System.Windows.Forms.RadioButton ColorModePracticeRadio;
        private System.Windows.Forms.PictureBox PracticeImage;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Button PulseTactorPracticeButton;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button PlaySoundButton;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Button PlaySoundPracticeButton;
        private System.Windows.Forms.RadioButton SoundModePracticeRadio;
        private System.Windows.Forms.Label practiceLabel;

    }
}

