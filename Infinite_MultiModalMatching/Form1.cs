/* ---------------------------------------------------------------------------
** The software supplied herewith by Engineering Acoustics, Inc.
** (the Company) for its Tactor Development Kit is intended and
** supplied to you, the Company's customer, for use solely and
** exclusively on Engineering Acoustics, Inc. products. The
** software is owned by the Company and/or its supplier, and is
** protected under applicable copyright laws. All rights are reserved.
** Any use in violation of the foregoing restrictions may subject the
** user to criminal sanctions under applicable laws, as well as to
** civil liability for the breach of the terms and conditions of this
** license.
**
** THIS SOFTWARE IS PROVIDED IN AN AS IS CONDITION. NO WARRANTIES,
** WHETHER EXPRESS, IMPLIED OR STATUTORY, INCLUDING, BUT NOT LIMITED
** TO, IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
** PARTICULAR PURPOSE APPLY TO THIS SOFTWARE. THE COMPANY SHALL NOT,
** IN ANY CIRCUMSTANCES, BE LIABLE FOR SPECIAL, INCIDENTAL OR
** CONSEQUENTIAL DAMAGES, FOR ANY REASON WHATSOEVER.
**
**   Copyright 2015(c) Engineering Acoustics Inc. All rights reserved.   *
** -------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using IrrKlang;

namespace InfinitePrecisionMatching
{
    public partial class InfinitePrecisionMatchingForm : Form
    {
        private bool fileLoaded = false;
        private bool connected = false;

        private bool soundPlayed = false;
        private bool tactorPulsed = false;

        //Variables to pass to the tactor functions 
        private int gain = 65;
        private int frequency = 300;
        private int color = 0;

        //Irrklang documentation: http://www.ambiera.com/irrklang/docunet/index.html
        //Volume is float between 0f and 1f
        private float volume;
        private int ConnectedBoardID = -1;
        private int trials = 0;
        private int count = 0;
        private float trackValue;
        private float max = 10000f;
        private List<int> start_modality = new List<int>();
        private List<int> match_modality = new List<int>();
        private List<int> start_intensity = new List<int>();
        private string participant_name = "";

        private List<int> results = new List<int>();

        //Determines which mode to use: tactile, visual, or auditory:
        //0: tactile (user adjusts the intensity of the vibration)
        //1: visual (user adjusts the intensity of the brightness)
        //2: auditory (user adjusts the volume of the sound)
        //Set a default value to avoid undefined behavior (See Meyer's text)
        private int mode = 0;
        private int match_mode = 0;
        //private Audio myAudio; 
        private ISoundEngine mySoundEngine;

        public InfinitePrecisionMatchingForm()
        {
            //Sound Code
            mySoundEngine = new ISoundEngine();
            mySoundEngine.Play2D("../chimes.wav");

            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            Console.AppendText("InitializeTI\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
            //this.tabControl1.TabPages.Remove(this.MatchingTab);
            //this.tabControl1.TabPages.Remove(this.tabPage1);
        }

        private void DiscoverButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;

            Console.AppendText("Discover Started...\n");
            //Discovers all serial tactor devices and returns the amount found
            int ret = Tdk.TdkInterface.Discover((int)Tdk.TdkDefines.DeviceTypes.Serial);
            if (ret > 0)
            {
                Console.AppendText("Discover Found:\n");
                //populate combo box with discovered names
                for (int i = 0; i < ret; i++)
                {
                    //Gets the discovered device name at the index i
                    System.IntPtr discoveredNamePTR = Tdk.TdkInterface.GetDiscoveredDeviceName(i);
                    if (discoveredNamePTR != null)
                    {
                        string sComName = Marshal.PtrToStringAnsi(discoveredNamePTR);
                        Console.AppendText(sComName + "\n");
                        ComPortComboBox.Items.Add(sComName);
                    }
                    else
                        Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
                }
                ComPortComboBox.SelectedIndex = 0;
                DiscoverButton.Enabled = false;
                ConnectButton.Enabled = true;
                StartButton.Enabled = true;

            }
            else
            {
                Console.AppendText("Discover Failed:\n");
                Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
            }


        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string selectedComPort;
            if (discoverradio.Checked || connectradio.Checked)
            {
                if (discoverradio.Checked)
                    selectedComPort = ComPortComboBox.SelectedItem.ToString();
                else selectedComPort = comportselection.Text;

                Console.AppendText("\nConnecting to com port " + selectedComPort + "\n");
                //Connect connects to the tactor controller via serial with the given name
                //we should be hooking up a response callback but for simplicity of the 
                //tutorial we wont be. Reference the ResponseCallback tutorial for more information
                int ret = Tdk.TdkInterface.Connect(selectedComPort,
                                                   (int)Tdk.TdkDefines.DeviceTypes.Serial,
                                                    System.IntPtr.Zero);
                if (ret >= 0)
                {
                    ConnectedBoardID = ret;
                    DiscoverButton.Enabled = false;
                    PulseTactorButton.Enabled = true;
                    discoverradio.Checked = true;
                    StartButton.Enabled = true;
                    connected = true;
                }
                else
                {
                    Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
                }
            }
            else MessageBox.Show("Please select a Connection Mode!");

        }

        private void PulseTactor1Button_Click(object sender, EventArgs e)
        {
            tactorPulsed = true;
            //Enable the next button if neither the stimulus nor the match mode are "auditory",
            //or if the sound has already been played
            if (soundPlayed) NextButton.Enabled = true;
            else if (mode != 2 && match_mode != 2) NextButton.Enabled = true; 

            //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
            Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
            Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 1000, 0));
            GainLabel.Text = "Gain:" + gain;
            FrequencyLabel.Text = "Frequency: " + frequency;

        }

        private void ConsoleOutputRichTextBox(string msg)
        {
            Console.AppendText(msg);
        }

        private void MultiModalMatchingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //closes up the connection to the tactor device with ConnectedBoardID
            CheckTDKErrors(Tdk.TdkInterface.Close(ConnectedBoardID));
            //cleans up everyting associated witht the TActionManager. Unloads any TActions loaded
            CheckTDKErrors(Tdk.TdkInterface.ShutdownTI());
        }
        private void CheckTDKErrors(int ret)
        {
            //if a tdk method returns less then zero then we should display the last error
            //in the tdk interface
            if (ret < 0) Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
        }

        //This method hold the majority of the code.
        private void initialize()
        {
            //Disable labels and buttons and reset flags
            NextButton.Enabled = false;
            GainLabel.Visible = false;
            FrequencyLabel.Visible = false;
            PulseTactorButton.Enabled = false;
            PlaySoundButton.Enabled = false;
            soundPlayed = false;
            tactorPulsed = false;

            //Image is black unless it's part of the test
            pictureBox1.BackColor = Color.FromArgb(0, 0, 0);

            //Update modes
            mode = start_modality[count];
            match_mode = match_modality[count];

            //Stimulus: Tactile
            if (mode == 0)
            {
                //Enable buttons and labels
                GainLabel.Visible = true;
                FrequencyLabel.Visible = true;
                PulseTactorButton.Enabled = true;

                //Get the gain and freq.  start_intensity[] was populated from the config file.
                gain = 64 + (int)((float)start_intensity[count] * 1.91);
                frequency = 300 + (int)((float)start_intensity[count] * 32.5);
                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);

                //Reset labels
                GainLabel.Text = "Gain:" + gain;
                FrequencyLabel.Text = "Frequency: " + frequency;

                //Matching: Visual
                if (match_mode == 1)
                {
                    color = (int)(trackValue / max * 255);
                    pictureBox1.BackColor = Color.FromArgb(color, color, color);
                    InstructionLabel.Text = "Pick the intensity of the brightness which best corresponds to the intensity of the vibration.";
                }

                //Matching: Auditory
                else if (match_mode == 2)
                {
                    volume = trackValue / max;
                    mySoundEngine.SoundVolume = volume;
                    InstructionLabel.Text = "Pick the volume of sound which best corresponds to the intensity of the vibration.";
                    PlaySoundButton.Enabled = true;

                }
            }

            //Stimulus: Visual
            if (mode == 1)
            {
                color = (int)((start_intensity[count] * 2.55));
                pictureBox1.BackColor = Color.FromArgb(color, color, color);

                //Matching: Tactile
                if (match_mode == 0)
                {
                    gain = (int)(64 + (trackValue / max) * 191);
                    frequency = (int)(300 + (trackValue / max) * 2500);
                    GainLabel.Text = "Gain:" + gain;
                    FrequencyLabel.Text = "Frequency: " + frequency;
                    PlaySoundButton.Enabled = false;
                    GainLabel.Visible = true;
                    FrequencyLabel.Visible = true;
                    InstructionLabel.Text = "Pick the intensity of the vibration which best corresponds to the brightness.";
                    PulseTactorButton.Enabled = true;
                }
                //Matching: Auditory
                else if (match_mode == 2)
                {
                    volume = trackValue / max;
                    mySoundEngine.SoundVolume = volume;
                    InstructionLabel.Text = "Pick the volume of sound which best corresponds to the brightness";
                    PlaySoundButton.Enabled = true;
                }
            }

            //Stimulus: auditory
            if (mode == 2)
            {
                volume = (float)start_intensity[count] / 100f;
                mySoundEngine.SoundVolume = volume;
                PlaySoundButton.Enabled = true;
                //Matching: tactile
                if (match_mode == 0)
                {
                    GainLabel.Text = "Gain:" + gain;
                    FrequencyLabel.Text = "Frequency: " + frequency;
                    GainLabel.Visible = true;
                    FrequencyLabel.Visible = true;
                    InstructionLabel.Text = "Pick the intensity of the vibration which best corresponds to the volume.";
                    PulseTactorButton.Enabled = true;
                }

                //Matching: Visual
                else if (match_mode == 1)
                {
                    color = (int)(trackValue / max * 255);
                    pictureBox1.BackColor = Color.FromArgb(color, color, color);
                    InstructionLabel.Text = "Pick the intensity of the brightness which best corresponds to the volume.";
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
             
            trackValue = trackBar1.Value;
            //This allows us to calculate the scale factor rather than hard coding it.

            if (match_mode == 0)
            {
                gain = (int)(64 + (trackValue / max) * 191);
                //was 1250 + ... (not sure why)
                frequency = (int)(300 + (trackValue / max) * 3250);
                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
                GainLabel.Text = "Gain:" + gain;
                FrequencyLabel.Text = "Frequency: " + frequency;
            }

            else if (match_mode == 1)
            {
                int color = (int)(trackValue / max * 255);
                pictureBox1.BackColor = Color.FromArgb(color, color, color);
            }

            else if (match_mode == 2)
            {
                volume = trackValue / max;
                mySoundEngine.SoundVolume = volume;
                ImgIntensityPractice.Text = "Volume: " + (volume * 100).ToString() + '%';
            }

            //GainLabel.Text = "Gain:" + gain;
            //FrequencyLabel.Text = "Frequency: " + frequency;
        }


        private void EnterButton_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\jahatfi\Desktop\" + FileName.Text;
            if (!File.Exists(path))
            {
                Console.AppendText("Cannot find the file.  Please Try Again.\n");
                return;
            }

            Console.AppendText("Found the file.\r\n");
            EnterButton.Enabled = false;
            // Open the file to read from. 
            using (StreamReader sr = File.OpenText(path))
            {
                char[] delimiterChars = { ',', ' ', '\t' };
                string s = "";
                if ((s = sr.ReadLine()) == null)
                {
                    Console.AppendText("File is empty, please try again.\r\n");
                    EnterButton.Enabled = true;
                }
                else
                {
                    while ((s = sr.ReadLine()) != null)
                    {
                        //Get the modality
                        string[] words = s.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        Console.AppendText(words[0] + ": " + words[1] + ", " + words[2] + ", " + words[3] + '\n');

                        //Get the starting modality
                        switch (words[1])
                        {
                            case "T":
                                start_modality.Add(0);
                                break;
                            case "V":
                                start_modality.Add(1);
                                break;
                            case "A":
                                start_modality.Add(2);
                                break;
                            default:
                                Console.AppendText("Invalid mode in configuration file.  Please check the file and start over.\r\n");
                                match_modality.Clear();
                                start_intensity.Clear();
                                start_modality.Clear();
                                return;
                        }

                        //Get the matching modality
                        switch (words[2])
                        {
                            case "T":
                                match_modality.Add(0);
                                break;
                            case "V":
                                match_modality.Add(1);
                                break;
                            case "A":
                                match_modality.Add(2);
                                break;
                            default:
                                Console.AppendText("Invalid mode in configuration file.  Please check the file and start over.");
                                match_modality.Clear();
                                start_intensity.Clear();
                                start_modality.Clear();
                                return;
                        }

                        //Get the intensity
                        int intensity = 0;
                        if (int.TryParse(words[3], out intensity)) start_intensity.Add(intensity);
                        else
                        {
                            Console.AppendText("One or more of the intensity values is invalid.  Please correct it in the configuration file and start over.");
                            start_modality.Clear();
                            start_intensity.Clear();
                            break;
                        }

                    }//while

                    if (match_modality.Count != start_intensity.Count)
                        MessageBox.Show("Each start modality must have a corresponding matched modality.  Please correct your configuration file.");
                    else
                    {
                        for (int i = 0; i < match_modality.Count; ++i)
                        {
                            if (match_modality[i] == start_modality[i])
                            {
                                MessageBox.Show("A start modality is the same as it's matched modality.  Please make sure they are different and try again.");
                                break;
                            }
                        }
                    }

                    //FOR TESTING ONLY 
                    /*
                    for (int index = 0; index < start_intensity.Count; index++)
                    {
                        Console.AppendText("Mode:" + start_modality[index] + "Intensity" + start_intensity[index]);
                    }
                    */

                }//else
                //sr.Dispose();
            }//using
            trials = start_intensity.Count;
            Console.AppendText("trials: " + trials);
            StartButton.Enabled = true;
            FileName.Enabled = false;
            fileLoaded = true;

        }//EnterButton_click

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (ParticipantNumber.Text == "")
                MessageBox.Show("Please enter a participant number to continue");
            else if (!fileLoaded)
                MessageBox.Show("Please select a configuration file before continuing.");
            else if(!connected)
                MessageBox.Show("Please connect to the tactor bpard before continuing.");
            else
            {
                participant_name = ParticipantNumber.Text;
                EnterButton.Enabled = false;
                DiscoverButton.Enabled = false;
                StartButton.Enabled = false;
                //tabControl1.SelectedIndex = 1;
                tabControl1.SelectedIndex = 1;
                CountLabel.Text = "Round " + (count + 1) + " of " + trials;
                initialize();

            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            ++count;
            if (count == trials) ProcessResults();

            else
            {
                results.Add((int)(trackValue / max));
                initialize();
                CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
            }
            if (count + 1 == trials) NextButton.Text = "Finish";
        }

        private void ProcessResults()
        {
            NextButton.Enabled = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            trackBar1.Visible = false;
            InstructionLabel.Visible = false;
            PulseTactorButton.Visible = false;
            NextButton.Visible = false;
            CountLabel.Visible = false;
            FinishedLabel.Visible = true;
            pictureBox3.Visible = false;
            PlaySoundButton.Visible = false;

            results.Add((int)(trackValue / max * 100f));
            string path = @"C:\Users\jahatfi\Desktop\" + ParticipantNumber.Text + "Results.txt";

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("\nResults for " + participant_name + " on " + DateTime.Now);
                sw.WriteLine("Presentation Order\tStart Modality\tStart Intensity\tMatch Modality\tMatch Intensity\n");
                for (int i = 0; i < trials; i++)
                {
                    if (start_modality[i] == 0)
                    {
                        if (match_modality[i] == 1) sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tV\t\t" + results[i]);
                        else sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + results[i]);
                    }
                    else if (start_modality[i] == 1)
                    {
                        if (match_modality[i] == 0) sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                        else sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + results[i]);
                    }

                    else
                    {
                        if (match_modality[i] == 0) sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                        else sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                    }
                }
                sw.WriteLine("\n\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Selected: " + trackBar1.Value);
        }

        private void LetsGetStartedButton_Click(object sender, EventArgs e)
        {
            LetsGetStartedButton.Enabled = false;
            tabControl1.SelectedIndex = 2;
            /*
            gain = 0;
            frequency = 0;
            volume = 0;
            color = 0;
             * */
            initialize();

        }

        private void PulseTactorPracticeButton_Click(object sender, EventArgs e)
        {
            //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
            Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
            Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 1000, 0));
            GainLabel.Text = "Gain:" + gain;
            FrequencyLabel.Text = "Frequency: " + frequency;
        }

        private void LetsGetStartedButton_Click_1(object sender, EventArgs e)
        {
            PracticePanel.Visible = false;
            this.tabControl1.TabPages.Remove(this.InstructionsTab);
            this.tabControl1.TabPages.Add(this.MatchingTab);
            this.tabControl1.TabPages.Add(this.InstructionsTab);
            initialize();
        }

        private void TactorModePracticeRadio_Click(object sender, EventArgs e)
        {
            float intensity = ((float)trackBar2.Value / (float)trackBar2.Maximum);

            PlaySoundPracticeButton.Enabled = false;
            PulseTactorPracticeButton.Enabled = true;

            //255 - 64 = 191
            gain = (int)(64 + intensity * 191);
            //was 1250 + ... (not sure why)
            frequency = (int)(300 + intensity * 2500);

            InstGainLabel.Text = "Gain:" + gain;
            InstFrequencyLabel.Text = "Frequency: " + frequency;

            InstGainLabel.Visible = true;
            InstFrequencyLabel.Visible = true;
            ImgIntensityPractice.Visible = false;
        }

        private void ColorModePracticeRadio_Click(object sender, EventArgs e)
        {
            int color = (int)((float)trackBar2.Value / (float)trackBar2.Maximum * 255);

            PulseTactorPracticeButton.Enabled = false;
            PlaySoundPracticeButton.Enabled = false;

            InstGainLabel.Visible = false;
            InstFrequencyLabel.Visible = false;

            PracticeImage.BackColor = Color.FromArgb(color, color, color);
            ImgIntensityPractice.Text = "Image Color Intensity: " + color;
            ImgIntensityPractice.Visible = true;
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            float practiceTrackValue = trackBar2.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float practiceMax = trackBar2.Maximum;

            //IF TACTOR MODE IS SELECTED, USER CAN ONLY CHANGE THE INTENSITY OF VIBRATION ON THE TACTOR
            if (TactorModePracticeRadio.Checked)
            {
                //255 - 64 = 191
                gain = (int)(64 + (practiceTrackValue / practiceMax) * 191);
                //was 1250 + ... (not sure why)
                frequency = (int)(300 + (practiceTrackValue / practiceMax) * 2500);

                InstGainLabel.Text = "Gain:" + gain;
                InstFrequencyLabel.Text = "Frequency: " + frequency;
            }//TACTOR MODE

            //COLOR MODE: USER CAN ONLY CHANGE THE BRIGHTNESS OF THE IMAGE
            else if (ColorModePracticeRadio.Checked)
            {
                //RGB values are all equal for a greyscale color
                //Scale up to 255 
                color = (int)(practiceTrackValue / practiceMax * 255);
                PracticeImage.BackColor = Color.FromArgb(color, color, color);
                ImgIntensityPractice.Text = "Image Color Intensity: " + color;
            }//COLOR MODE

            else
            {
                volume = practiceTrackValue / practiceMax;
                mySoundEngine.SoundVolume = volume;
                ImgIntensityPractice.Text = "Volume: " + (volume * 100).ToString() + '%';

            }
        }

        private void SoundModePracticeRadio_Click(object sender, EventArgs e)
        {
            float intensity = ((float)trackBar2.Value / (float)trackBar2.Maximum);

            PlaySoundPracticeButton.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;

            InstGainLabel.Visible = false;
            InstFrequencyLabel.Visible = false;
            ImgIntensityPractice.Visible = true;
            volume = trackBar2.Value / (float)trackBar2.Maximum;
            mySoundEngine.SoundVolume = volume;
            ImgIntensityPractice.Text = "Volume: " + (volume * 100).ToString() + '%';
        }

        private void PlaySoundPracticeButton_Click(object sender, EventArgs e)
        {
            mySoundEngine.Play2D("../chimes.wav");
        }

        private void PlaySoundButton_Click(object sender, EventArgs e)
        {

            mySoundEngine.Play2D("../chimes.wav");
            soundPlayed = true;

            //Enable the next button if neither the stimulus nor the match mode are "tactile",
            //or if the tactor has already been pulse
            if (tactorPulsed) NextButton.Enabled = true;
            else if (mode != 0 && match_mode != 0) NextButton.Enabled = true;
        }
    }
}