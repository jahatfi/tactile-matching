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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using IrrKlang;

namespace TenPointMatching
{
    public partial class TenPointMatchingForm : Form
    {
        private bool fileLoaded = false;
        private bool connected = false;

        private bool referencePlayed = false;
        private bool variablePlayed = false;

        //Variables to pass to the tactor functions
        private int gain = 65;
        private int frequency = 2500;
        private int color = 0;

        //Irrklang documentation: http://www.ambiera.com/irrklang/docunet/index.html
        //Volume is float between 0f and 1f
        private float volume;
        private float audioFrequency;
        private int ConnectedBoardID = -1;
        private int trials = 0;
        private int count = 0;
        private float lastVal = -1;
        private float trackValue;
        private float max;
        private List<int> start_modality = new List<int>();
        private List<int> match_modality = new List<int>();
        private List<int> start_intensity = new List<int>();
        private string participant_name = "";
        private string outputfile;

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

        //Cache references to images
        Image speakerImage;
        Image tactorImage;

        public TenPointMatchingForm()
        {
            //SOUND CODE
            mySoundEngine = new ISoundEngine();
            mySoundEngine.Play2D("tone.wav");

            //Visual Studio code - do not edit
            InitializeComponent();
            this.Text = "Variable Precision Matching Program";

            //To initialize the TDKInterface we need to call InitializeTI before we use any of its functionality
            Console.AppendText("Initializing Tactor Interface...\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
            Console.AppendText("Success \n");


            //Make the other two tabs invisible for now
            //this.PrimaryTabControl.TabPages.Remove(this.MatchingTab);
            //this.PrimaryTabControl.TabPages.Remove(this.InstructionsTab);

            speakerImage = Image.FromFile("speaker.jpg");
            tactorImage = Image.FromFile("tactor.jpg");
        }

        private void DiscoverButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            StartButton.Enabled = false;

            Console.AppendText("Discover Started...\n");
            //Discovers all serial tactor devices and returns the amount found
            int ret = Tdk.TdkInterface.Discover((int)Tdk.TdkDefines.DeviceTypes.Serial);
            Cursor.Current = Cursors.Default;

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
                        Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString() + "\n");
                }
                ComPortComboBox.SelectedIndex = 0;
                DiscoverButton.Enabled = false;
                ConnectButton.Enabled = true;
                StartButton.Enabled = true;

            }
            else
            {
                Console.AppendText("Discover Failed:\n");
                Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString() + "\n");
            }
     
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string selectedComPort;
            if (discoverradio.Checked || connectradio.Checked)
            {
                if (discoverradio.Checked)
                    selectedComPort = ComPortComboBox.SelectedItem.ToString();
                else  selectedComPort = comportselection.Text;

                Console.AppendText("Attempting to connect to com port " + selectedComPort + "...\n");
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
                    playVariableButton.Enabled = true;
                    ConnectButton.Enabled = false;
                    if(fileLoaded) StartButton.Enabled = true;
                    connected = true;
                    Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
                    Console.AppendText("Success! \n");
                }
                else
                {
                    Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString() + "\n");
                }
            }
            else MessageBox.Show("Please select a Connection Mode!");
            Cursor = Cursors.Default;
        }

        private void playVariableButton_Click(object sender, EventArgs e)
        {
            if (lastVal < 0) lastVal = trackValue;
            else if (lastVal != trackValue)
            {
                variablePlayed = true;
                //Enable the next button if the reference has already been played
                if (referencePlayed) NextButton.Enabled = true;
            }

            if (match_mode == 0) CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 1000, 0));

            else
            {
                if (volumeButton.Checked)
                {
                    mySoundEngine.SoundVolume = volume;
                    mySoundEngine.Play2D("tone.wav");
                }
                else
                {
                    string path = "tones/tone" + (audioFrequency * 100).ToString() + ".wav";
                    mySoundEngine.Play2D(path);
                }
            }
        }

        private void ConsoleOutputRichTextBox(string msg)  { Console.AppendText(msg); }

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
            if (ret < 0) Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString() + "\n");

        }

        //This method holds the majority of the code.
        private void Initialize()
        {
            //Reset the slider to 0
            matchingTrackBar.Value = 0;
            trackValue = 0;
            color = 0;
            gain = 1;
            audioFrequency = 0;
            Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            volume = 0f;
            mySoundEngine.SoundVolume = volume;
            audioFrequency = 0f;

            //Update modes
            mode = start_modality[count];
            match_mode = match_modality[count];
            
            //Disable button and reset flags
            NextButton.Enabled = false;
            referencePlayed = false;
            variablePlayed = false;


            //Reference image is grey by default
            ReferenceImage.Image = null;
            ReferenceImage.BackColor = SystemColors.ControlLight;

            VariableImage.Image = null;
            VariableImage.BackColor = SystemColors.ControlLight;

            //Disable/hide variable controls
            playVariableButton.Enabled = false;
            playVariableButton.Visible = false;
            VariableImage.Visible = false;

            //Set these to true
            playReferenceButton.Visible = true;
            playReferenceButton.Enabled = true;

            //Stimulus: Tactile
            if (mode == 0)
            {
                ReferenceImage.Image = tactorImage;
                ReferenceImage.Visible = true;

                //Get the gain. start_intensity[] was populated from the config file.
                gain = (int)(1 + (float)(start_intensity[count] * 2.54));
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);

                //Reset labels
                //GainLabel.Text = "Gain:" + gain;
                //FrequencyLabel.Text = "Frequency: " + frequency;

                //Matching: Visual
                if (match_mode == 1)
                {
                    //color = (int)(trackValue / max * 255);
                    VariableImage.BackColor = Color.FromArgb(color, color, color);
                    VariableImage.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b brightness \b0which best corresponds to the intensity of the \b vibration\b0.}";
                }

                //Matching: Auditory
                else if (match_mode == 2)
                {
                    VariableImage.Image = speakerImage;
                    VariableImage.Visible = true;
                    variableLabel1.Visible = true;
                    //volume = trackValue / max;
                    //mySoundEngine.SoundVolume = volume;
                    //instructionLabel.Text = "Pick the volume of sound which best corresponds to the intensity of the vibration.";
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b volume \b0which best corresponds to the intensity of the \b vibration\b0.}";
                    playVariableButton.Enabled = true;
                    playVariableButton.Visible = true;
                }
            }

            //Stimulus: Visual
            else if (mode == 1)
            {
                //Set these to true
                playReferenceButton.Visible = false;
                playReferenceButton.Enabled = false;
                color = (int)((start_intensity[count] * 2.55));
                ReferenceImage.BackColor = Color.FromArgb(color, color, color);

                variableLabel1.Visible = true;
                playVariableButton.Enabled = true;
                playVariableButton.Visible = true;
                referencePlayed = true;

                //Matching: Tactile
                if (match_mode == 0)
                {
                    VariableImage.Image = tactorImage;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b vibration \b0which best corresponds to the intensity of the \b brightness\b0.}";
                }
                //Matching: Auditory
                else if (match_mode == 2)
                {
                    VariableImage.Image = speakerImage;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b volume \b0which best corresponds to the intensity of the \b brightness\b0.}";
                }
                VariableImage.Visible = true;
            }

            //Stimulus: auditory
            else if(mode == 2){

                if(volumeButton.Checked) volume = (float)start_intensity[count] / 100f;
                else audioFrequency = (float)start_intensity[count] / 100f;
                mySoundEngine.SoundVolume = volume;
                ReferenceImage.Image = speakerImage;
                ReferenceImage.Visible = true;

                //Matching: tactile
                if (match_mode == 0)
                {
                    VariableImage.Image = tactorImage;
                    VariableImage.Visible = true;
                    variableLabel1.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b vibration \b0which best corresponds to the intensity of the \b volume\b0.}";
                    playVariableButton.Enabled = true;
                    playVariableButton.Visible = true;
                }

                //Matching: Visual
                else if (match_mode == 1)
                {
                    VariableImage.BackColor = Color.FromArgb(color, color, color);
                    variableLabel1.Visible = true;
                    VariableImage.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b brightness \b0which best corresponds to the intensity of the \b volume\b0.}";
                }
            }
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {

            trackValue = matchingTrackBar.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.

            if (match_mode == 0)
            {
                gain = (int)(1 + (trackValue / max) * 254);
                frequency = (int)(300 + (trackValue / max) * 3250);
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            }

            else if (match_mode == 1)
            {
                int color = (int)(trackValue / max * 255);
                VariableImage.BackColor = Color.FromArgb(color, color, color);
                variablePlayed = true;
                if (referencePlayed) NextButton.Enabled = true;
            }

            else if (match_mode == 2)
            {
                if (volumeButton.Checked) volume = trackValue / max;
                else audioFrequency = trackValue / max;
            }
        }


        private void LoadFile(object Sender, EventArgs e)
        {
            string path = FileName.Text;
            if (!File.Exists(path))
            {
                Console.AppendText("Cannot find the file.  Please Try Again.\n");
                return;
            }

            Console.AppendText("Found the file.\r\n");
            // Open the file to read from. 
            using (StreamReader sr = File.OpenText(path))
            {
                char[] delimiterChars = { ',', ' ', '\t' };
                string s = "";
                if ((s = sr.ReadLine()) == null)
                {
                    Console.AppendText("File is empty, please try again.\r\n");
                }
                else
                {
                    while ((s = sr.ReadLine()) != null  && s != "")
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
            loadFileButton.Enabled = false;
            browseInput.Enabled = false;
            trials = start_intensity.Count;
            Console.AppendText("trials: " + trials + "\n");
            FileName.Enabled = false;
            fileLoaded = true;
            if (connected) StartButton.Enabled = true;
            return;

        }//button3_Click

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (ParticipantNumber.Text == "")
            {
                MessageBox.Show("Please enter a participant number to continue");
                return;
            }
            if(!connected)
            {
                MessageBox.Show("Please connect to the tactor board before continuing.");
                return;
            }
            if (!(float.TryParse(TrackbarPrecision.Text, out max)))
            {
                MessageBox.Show("Please enter a precision for the trackbar between 10 and 10,000.");
            }
            if ((max < 10) || (max > 10000))
            {
                MessageBox.Show("Please enter a valid number between 10 and 10000.");
                return;
            }
            if (outputFile.Text == ""  || outputFile.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > 0)
            {
                MessageBox.Show("Please enter a valid name for the log file.");
                return;
            }
            else
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(outputFolder.Text + outputFile.Text, true)) { }
                }
                catch (IOException)
                {
                    MessageBox.Show("Please enter a valid name for the log file.");
                    return;
                }
                volumeButton.Enabled = frequencyButton.Enabled = false;
                outputfile = outputFolder.Text + outputFile.Text;
                if (!outputfile.EndsWith(".txt")) outputfile += ".txt";
                browseOutput.Enabled = false;
                matchingTrackBar.Maximum = (int)max;
                participant_name = ParticipantNumber.Text;
                DiscoverButton.Enabled = false;
                StartButton.Enabled = false;
                PrimaryTabControl.SelectedIndex = 1;
                CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
                //this.PrimaryTabControl.TabPages.Add(this.InstructionsTab);
                //this.PrimaryTabControl.TabPages.Remove(this.ConfigureTab);
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            count++;
            GoBackButton.Enabled = true;
            lastVal = -1;

            if (count == trials) ProcessResults();
            else
            {
                float result = trackValue / max * 100f;
                results.Add((int)result);
                Initialize();
                CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
            }
            if (count + 1 == trials) NextButton.Text = "Finish";
        }

        private void ProcessResults() {
            NextButton.Enabled = false;
            VariableImage.Visible = false;
            matchingTrackBar.Visible = false;
            //instructionLabel.Visible = false;
            playVariableButton.Visible = false;
            NextButton.Visible = false;
            CountLabel.Visible = false;
           
            ReferenceImage.Visible = false;
            playReferenceButton.Visible = false;
            richTextBox1.Visible = false;
            referenceLabel.Visible = false;
            variableLabel1.Visible = false;
            GoBackButton.Visible = false;

            FinishedLabel.Text = "Finished!  Thanks for participating! \r\n Note to experimentor: " +
                "The results file is stored in " + outputfile;
            FinishedLabel.Visible = true;

            results.Add((int)(trackValue / max * 100f));
            string path = outputfile;

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("\nResults for participant #" + participant_name + " on " + DateTime.Now);
                sw.WriteLine("Presentation Order\tStart Modality\tStart Intensity\tMatch Modality\tMatch Intensity\n");
                for (int i = 0; i < trials; i++)
                {
                    if (start_modality[i] == 0)
                    {
                        if(match_modality[i] == 1)  sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tV\t\t" + results[i]);
                        else
                        {
                            if (volumeButton.Checked) sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + results[i]);
                            else sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + (200 + results[i]* 18).ToString() + " Hz");
                        }
                    }
                    else if (start_modality[i] == 1)
                    {
                        if (match_modality[i] == 0) sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                        else
                        {
                            if (volumeButton.Checked) sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + results[i]);
                            else sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + (200 + results[i] * 18).ToString() + " Hz");
                        }
                    }

                    else
                    {
                        if (match_modality[i] == 0)
                        {
                            if (volumeButton.Checked) sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                            else sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + (200 + start_intensity[i] * 18).ToString() + " Hz\t\tT\t\t" + results[i]);
                        }

                        else
                        {
                            if (volumeButton.Checked) sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + start_intensity[i].ToString() + "\t\tV\t\t" + results[i]);
                            else sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + (200 + start_intensity[i] * 18).ToString() + "\t\tV\t\t" + results[i]);
                        }
                    }
                }
                sw.WriteLine("\n\n");
            }
        }

        private void LetsGetStartedButton_Click(object sender, EventArgs e)
        {
            Initialize();
            this.PrimaryTabControl.TabPages.Add(this.MatchingTab);
            this.PrimaryTabControl.TabPages.Remove(this.InstructionsTab);
        }

        private void PulseTactorPracticeButton_Click(object sender, EventArgs e)
        {
            //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
            //Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
            Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 1000, 0));
            //GainLabel.Text = "Gain:" + gain;
            //FrequencyLabel.Text = "Frequency: " + frequency;
        }

        private void PlaySoundPracticeButton_Click(object sender, EventArgs e)
        {
            if (volumeButton.Checked)
            {
                mySoundEngine.SoundVolume = volume;
                mySoundEngine.Play2D("tone.wav");
                
            }
            else
            {
                string path = "tones/tone" + (audioFrequency * 100).ToString() + ".wav";
                mySoundEngine.Play2D(path);
            }
        }

        private void LetsGetStartedButton_Click_1(object sender, EventArgs e)
        {
            //PracticePanel.Visible = false;
            //this.PrimaryTabControl.TabPages.Remove(this.InstructionsTab);
            //this.PrimaryTabControl.TabPages.Add(this.MatchingTab);
            //this.PrimaryTabControl.TabPages.Add(this.InstructionsTab);
            SoundModePracticeRadio.Enabled = false;
            ColorModePracticeRadio.Enabled = false;
            TactorModePracticeRadio.Enabled = false;
            matchingTrackBar.Enabled = true;
            LetsGetStartedButton.Enabled = false;
            practiceTrackbar.Enabled = false;
            PulseTactorPracticeButton.Enabled = false;
            PlaySoundPracticeButton.Enabled = false;
            playReferenceButton.Enabled = true;
            gain = 1;
            volume = 0f;
            if (frequencyButton.Checked) mySoundEngine.SoundVolume = 1f;
            color = 0;
            PrimaryTabControl.SelectedIndex = 2;
            Initialize();
        }

        private void TactorModePracticeRadio_Click(object sender, EventArgs e)
        {
            float intensity = ((float)practiceTrackbar.Value / (float)practiceTrackbar.Maximum);

            PlaySoundPracticeButton.Enabled = false;
            PulseTactorPracticeButton.Enabled = true;
            
            gain = (int)(1 + intensity * 254);
            practiceLabel.Text = "Gain: " + gain;
            practiceLabel.Visible = true;
        }

        private void ColorModePracticeRadio_Click(object sender, EventArgs e)
        {
            int color = (int)((float)practiceTrackbar.Value / (float)practiceTrackbar.Maximum * 255);

            PulseTactorPracticeButton.Enabled = false;
            PlaySoundPracticeButton.Enabled = false;

            //InstGainLabel.Visible = false;
            //InstFrequencyLabel.Visible = false;

            PracticeImage.BackColor = Color.FromArgb(color, color, color);
            practiceLabel.Text = "Image Color Intensity: " + color;
            practiceLabel.Visible = true;
        }

        private void SoundModePracticeRadio_Click(object sender, EventArgs e)
        {
            float intensity = ((float)practiceTrackbar.Value / (float)practiceTrackbar.Maximum);

            PlaySoundPracticeButton.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;

            //InstGainLabel.Visible = false;
            //InstFrequencyLabel.Visible = false;
            practiceLabel.Visible = true;
            volume = practiceTrackbar.Value / (float)practiceTrackbar.Maximum;
            audioFrequency = volume;
            if (volumeButton.Checked)  practiceLabel.Text = "Volume: " + (volume * 100).ToString() + '%';
            else practiceLabel.Text = "Frequency: " + (200 + audioFrequency * 1800) + " Hz";
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            float practiceTrackValue = practiceTrackbar.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float practiceMax = practiceTrackbar.Maximum;

            //IF TACTOR MODE IS SELECTED, USER CAN ONLY CHANGE THE INTENSITY OF VIBRATION ON THE TACTOR
            if (TactorModePracticeRadio.Checked)
            {
                gain = (int)(1 + (practiceTrackValue / practiceMax) * 254);
                practiceLabel.Text = "Gain:" + gain;
                //was 1250 + ... (not sure why)
                //frequency = (int)(300 + (practiceTrackValue / practiceMax) * 2500);

                //InstGainLabel.Text = "Gain:" + gain;
                //InstFrequencyLabel.Text = "Frequency: " + frequency;
            }//TACTOR MODE

            //COLOR MODE: USER CAN ONLY CHANGE THE BRIGHTNESS OF THE IMAGE
            else if (ColorModePracticeRadio.Checked)
            {
                //RGB values are all equal for a greyscale color
                //Scale up to 255 
                int color = (int)(practiceTrackValue / practiceMax * 255);
                PracticeImage.BackColor = Color.FromArgb(color, color, color);
                practiceLabel.Text = "Image Color Intensity: " + color;
            }//COLOR MODE

            else
            {

                if (volumeButton.Checked)
                {
                    volume = practiceTrackValue / practiceMax;
                    practiceLabel.Text = "Volume: " + (volume * 100).ToString() + '%';
                }
                else
                {
                    audioFrequency = practiceTrackValue / practiceMax; ;
                    practiceLabel.Text = "Frequency: " + (200 + audioFrequency * 1800) + " Hz";
                }
            }
        }

        private void playReferenceButton_Click(object sender, EventArgs e)
        {
            referencePlayed = true;

            if (mode == 2)
            {
                if (volumeButton.Checked)
                {

                    mySoundEngine.SoundVolume = volume;
                    mySoundEngine.Play2D("tone.wav");
                }
                else
                {
                    string path = "tones/tone" + (audioFrequency * 100).ToString() + ".wav";
                    mySoundEngine.Play2D(path);
                }
            }
            else
            {
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 2, gain, 0);
                Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 500, 0);
                Tdk.TdkInterface.Pulse(ConnectedBoardID, 2, 500, 0);
            }

            //Enable the next button if the reference has already been played
            if (variablePlayed) NextButton.Enabled = true;
        }

        private void TenPointMatchingForm_Resize(object sender, EventArgs e)
        {
            PrimaryTabControl.Width = this.Width - 25;
            PrimaryTabControl.Height = this.Height - 50;
        }

        private void GoBackButton_Click(object sender, EventArgs e)
        {
            GoBackButton.Enabled = false;
            count--;
            results.RemoveAt(results.Count - 1);

            CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
            Initialize();
        }

        private void browseInput_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Public\Documents\"; // this is the path that you are checking.
            if (Directory.Exists(path))
            {
                openFileDialog1.InitialDirectory = path;
            }
            else
            {
                openFileDialog1.InitialDirectory = @"C:\";
            }

            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
                FileName.Text = file;
            }
        }

        private void browseOutput_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Public\Documents\"; // this is the path that you are checking.
            string directoryPath;
            if (Directory.Exists(path))
            {
                folderBrowserDialog1.SelectedPath = path;
            }
            else
            {
                folderBrowserDialog1.SelectedPath = @"C:\";
            }
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                directoryPath = folderBrowserDialog1.SelectedPath;

                outputFolder.Text = directoryPath + "\\";
            }
        }

        private void volumeButton_CheckedChanged(object sender, EventArgs e)
        {

        }





    }
}