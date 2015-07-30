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
//using WMPLib;
//using Microsoft.DirectX.AudioVideoPlayback;
using IrrKlang;



//-------------------------------------------------------------------------//
//TDK Windows C# Discover Controller Tutorial
//This project show how the discovery system work. When the user presses 
//the discover button it calls discover on the tdk.
//The combobox is then populated with the found devices.
//The user can select wich device to connect to in the combo box and press
//the connect button to connect to it.
//The user is able to send a pulse command to the board by pressing the 
//pulse tactor 1 button
//-------------------------------------------------------------------------//

namespace Binary_MultiModalMatching
{
    public partial class Binary_MultiModalMatchingForm : Form
    {
        private bool connected = false;
        private bool fileLoaded = false;
        private bool soundPlayed = false;
        private bool tactorPulsed = false;
        //Variables to pass to the tactor functions
        private int gain = 1;
        private int frequency = 2500;
        int color = 0;

        //Irrklang documentation: http://www.ambiera.com/irrklang/docunet/index.html
        //Volume is float between 0f and 1f
        private float volume;
        private int ConnectedBoardID = -1;
        private int trials = 0;
        private int subtrial = 1;
        private int subtrials = 2;
        private int count = 0;
        private int min, max = 0;
        private float trackValue = 0;
        private List<int> start_modality = new List<int>();
        private List<int> match_modality = new List<int>();
        private List<int> start_intensity = new List<int>();
        private string participant_name = "";

        //private List<bool, int> configuration = new List<bool, int>();
        private List<int> results = new List<int>();

        //Determines which mode to use: tactile, visual, or auditory:
        //0: tactile (user adjusts the intensity of the vibration)
        //1: visual (user adjusts the intensity of the brightness)
        //2: auditory (user adjusts the volume of the sound)
        //Set a default value to avoid undefined behavior (See Meyer's text)
        private int mode = 0;
        private int match_mode = 0;
        //private Audio myAudio;  //For use with DirectX
        ISoundEngine mySoundEngine;

        public Binary_MultiModalMatchingForm()
        {
            //SOUND CODE
            //NOTE THAT THIS LINE TAKES A SHORT BIT TO EXECUTE - THE ERROR MESSAGE IS HARMLESS
            //myAudio = new Audio(@"C:/Users/Public/Documents/0.1.0.9.r25 API/tutorials/Windows/C#/Serial/chimes.wav", true);

            //Start up the irrklang engine
            mySoundEngine = new ISoundEngine();

            mySoundEngine.Play2D("../chimes.wav");

            //FOR USE WITH WMP - DISCOVERED WMP DOESN'T ALLOW VOLUME CONTROL
            //WMPLib.WindowsMediaPlayer soundPlayer = new WindowsMediaPlayer();
            //soundPlayer.URL = "C:/Users/Public/Documents/0.1.0.9.r25 API/tutorials/Windows/C#/Serial/chimes.wav";
            //soundPlayer.controls.play();

            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            Console.AppendText("Initializing Tactor Interface...\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());

            this.MainPanel.TabPages.Remove(this.MatchingTab);
            this.MainPanel.TabPages.Remove(this.Instructions);

            //For testing
            //Console.AppendText(myAudio.CurrentPosition.ToString());
            //myAudio.Play();
            //myAudio.SeekCurrentPosition(0.0, SeekPositionFlags.AbsolutePositioning);
            //Console.AppendText(myAudio.State.ToString()+"\n");
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
                {
                    if (ComPortComboBox.Items.Count == 0)
                    {
                        MessageBox.Show("No connection type selected.");
                        return;
                    }
                    selectedComPort = ComPortComboBox.SelectedItem.ToString();
                    
                }
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
                    PulseTactor1Button.Enabled = true;
                    ConnectButton.Enabled = false;
                    Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
                    connected = true;
                    if (fileLoaded) StartButton.Enabled = true;

                    //discoverradio.Checked = true;
                }
                else
                {
                    Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
                }
            }
            else MessageBox.Show("Please select a Connection Mode!");

            //Here we open the configuration file
        }

        private void PulseTactor1Button_Click(object sender, EventArgs e)
        {

            if (!radioButtonA.Checked && !radioButtonB.Checked)
            {
                MessageBox.Show("You must select A or B to continue.");
                return;
            }

            tactorPulsed = true;

            //Enable the next button if neither the stimulus nor the match mode are "auditory",
            //or if the sound has already been played
            if (soundPlayed) NextButton.Enabled = true;
            else if (mode != 2 && match_mode != 2) NextButton.Enabled = true;            

            //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
            //Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
            Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 1000, 0));
            //GainLabel.Text = "Gain:" + gain;
            //FrequencyLabel.Text = "Frequency: " + frequency;

        }

        private void ConsoleOutputRichTextBox(string msg)
        {
            Console.AppendText(msg);
        }

        private void Binary_MultiModalMatchingForm_FormClosed(object sender, FormClosedEventArgs e)
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
            if (ret < 0)
            {
                //the GetLastEAIErrorString returns a string that represents the last error code
                //recorded in the tdk interface.
                Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
            }
            else Console.AppendText("Success\r\n");
        }

        private void Initialize()
        {
            if (subtrial == 1)
            {
                min = 0;
                max = 100;
                mode = start_modality[count];
                match_mode = match_modality[count];
                soundPlayed = false;
                tactorPulsed = false;

                //Set all values to their minimum
                gain = 1;
                //frequency = 300;
                volume = 0;
                //Radio Button 'A' will be selected initially
                radioButtonA.Select();
                 

                //Disable all buttons and set image to black.  
                //The relevant buttons will be enabled as needed
                PulseTactor1Button.Enabled = false;
                PlaySoundButton.Enabled = false;
                pictureBox1.BackColor = Color.FromArgb(0,0,0);
                //GainLabel.Visible = false;
                //FrequencyLabel.Visible = false;
                //Sound_Image_Intensity.Visible = false;

                //Stimulus: Tactile
                if (mode == 0)
                {
                    gain = (int)(1 + (float)(start_intensity[count] * 2.54));
                    //frequency = (int)(300 + (float)(start_intensity[count] * 22));
                    PulseTactor1Button.Enabled = true;
                    //GainLabel.Text = "Gain:" + gain;
                    //FrequencyLabel.Text = "Frequency: " + frequency;
                    //GainLabel.Visible =  true;
                    //FrequencyLabel.Visible = true;

                    //Match mode: Visual
                    if (match_mode == 1)
                    {
                        InstructionLabel.Text = "Pick the intensity of the brightness which best corresponds to the intensity of the vibration.";
                    }

                    //Match Mode: Auditory
                    else if (match_mode == 2)
                    {
                        InstructionLabel.Text = "Pick the volume of sound which best corresponds to the intensity of the vibration.";
                        PlaySoundButton.Enabled = true;
                        //Sound_Image_Intensity.Text = "Volume: " + (volume * 100).ToString() + '%';
                        //Sound_Image_Intensity.Visible = true;
                    }
                }

                //Stimulus: Visual
                else if (mode == 1)
                {
                    color = (int)((start_intensity[count] * 2.54));
                    pictureBox1.BackColor = Color.FromArgb(color, color, color);

                    //Match Mode: Tactile
                    if (match_mode == 0)
                    {
                        InstructionLabel.Text = "Pick the intensity of the vibration which best corresponds to the brightness.";
                        PulseTactor1Button.Enabled = true;

                        //GainLabel.Text = "Gain:" + gain;
                        //FrequencyLabel.Text = "Frequency: " + frequency;
                        //GainLabel.Visible = true;
                        //FrequencyLabel.Visible = true;
                    }

                    //Match Mode: Auditory
                    else if (match_mode == 2)
                    {
                        InstructionLabel.Text = "Pick the volume of sound which best corresponds to the brightness";
                        PlaySoundButton.Enabled = true;
                        //Sound_Image_Intensity.Text = "Volume: " + (volume * 100).ToString() + '%';
                        //Sound_Image_Intensity.Visible = true;
                    }
                }

                //Stimulus: Auditory
                else if (mode == 2){
                    PlaySoundButton.Enabled = true;
                    volume = (float) start_intensity[count] / 100f;

                    //Sound_Image_Intensity.Text = "Volume: " + (volume * 100).ToString() + '%';
                    //Sound_Image_Intensity.Visible = true;

                    //Match Mode: Tactile
                    if (match_mode == 0)
                    {
                        InstructionLabel.Text = "Pick the intensity of the vibration which best corresponds to the volume.";
                        PulseTactor1Button.Enabled = true;

                        //GainLabel.Text = "Gain:" + gain;
                        //FrequencyLabel.Text = "Frequency: " + frequency;
                        //GainLabel.Visible = true;
                        //FrequencyLabel.Visible = true;
                    }

                    //Match mode: Visual
                    if (match_mode == 1)
                    {

                        InstructionLabel.Text = "Pick the intensity of the brightness which best corresponds to the volume.";
                    }                        
                }
            }//if

            else
            {
                if (radioButtonA.Checked == true)
                    max = min + (int)((max - min) / 2);
                else
                    min = min + (int)((max - min) / 2);
            }
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
                    while ((s = sr.ReadLine()) != null && s != "")
                    {
                        //Get the modality
                        string[] words = s.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
                        Console.AppendText(words[0] + ": " + words[1] + ", " + words[2] +  ", " + words[3] + '\n');

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
            FileName.Enabled = false;
            fileLoaded = true;
            if (connected) StartButton.Enabled = true;

        }//button3_Click

        private void StartButton_Click(object sender, EventArgs e)
        {
            if(ConnectButton.Enabled)
                MessageBox.Show("Please connect to the tactor to continue.");
            else if (ParticipantNumber.Text == "")
                MessageBox.Show("Please enter the participant number to continue");
            else if (EnterButton.Enabled == true)
                MessageBox.Show("Please select a configuration file before continuing.");
            else if (!(Int32.TryParse(SubtrialsNumber.Text, out subtrials)))
            {
                MessageBox.Show("Please enter the number of subtrials.");
            }
            else if((subtrials < 1) || (subtrials > 10))
                 MessageBox.Show("Please enter a valid number between 1 - 10.");
            else
            {
                this.MainPanel.TabPages.Add(this.Instructions);
                participant_name = ParticipantNumber.Text;
                EnterButton.Enabled = false;
                DiscoverButton.Enabled = false;
                StartButton.Enabled = false;
                MainPanel.SelectedIndex = 1;
                CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
                SubCountLabel.Text = "Subtrial " + (subtrial) + " of " + subtrials;

                this.MainPanel.TabPages.Remove(this.ConfigureTab);
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            //Sanity check
            if (!radioButtonA.Checked && !radioButtonB.Checked)
            {
                MessageBox.Show("You must select A or B to continue.");
                return;
            }
            //If we're still in the same trial, we just increment the subtrial counter
            if (subtrial != subtrials)
            {
                if (radioButtonA.Checked) max = min + (max - min) / 2;
                else min = min + (max - min) / 2;
                subtrial++;
                SubCountLabel.Text = "Subtrial " + (subtrial) + " of " + subtrials;
                NextButton.Enabled = false;
                tactorPulsed = false;
                soundPlayed = false;
            }

            //This means we just completed a subtrial.  Store the result, increment the count, and check to see if we're done.
            else
            {
                if (radioButtonA.Checked) results.Add(min);
                else results.Add(max);
                count++;

                //Finished  
                if (count == trials) ProcessResults();

                //Not finished - reset the variables.
                else
                {
                    subtrial = 1;
                    CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
                    NextButton.Enabled = false;
                    Initialize();
                }
            }
        }



        private void ProcessResults(){
            //GainLabel.Visible = false;
            //FrequencyLabel.Visible = false;
            NextButton.Enabled = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            radioButtonA.Visible = false;
            radioButtonB.Visible = false;
            InstructionLabel.Visible = false;
            PulseTactor1Button.Visible = false;
            NextButton.Visible = false;
            CountLabel.Visible = false;
            SubCountLabel.Visible = false;
            FinishedLabel.Visible = true;
            pictureBox3.Visible = false;
            PlaySoundButton.Visible = false;

            //results.Add((int)((float)((max-min)/2) * 6.67));
            string path = @"C:\Users\jahatfi\Desktop\" + ParticipantNumber.Text + "Results.txt";
            //FileStream file = File.Open(path, FileMode.Create);

            //TextWriter tw = new StreamWriter(path);

            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine("\nResults for " + participant_name + " on " + DateTime.Now);
                sw.WriteLine("Presentation Order\tStart Modality\tStart Intensity\tMatch Modality\tMatch Intensity\n");
                for (int i = 0; i < trials; i++)
                {
                    if (start_modality[i] == 0)
                        sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                    else if (start_modality[i] == 1)
                        sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tV\t\t" + results[i]);
                    else if (start_modality[i] == 2)
                        sw.WriteLine((i + 1).ToString() + "\t\t\tA\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + results[i]);
                }
                sw.WriteLine("\n\n");
            }
        }

        private void radioButtonB_Click(object sender, EventArgs e)
        {

            //Matching: Tactile
            if (match_mode == 0)
            {
                gain = (int)(1 + (float)(max * 2.54));
                //frequency = (int)(300 + (float)(max * 22));
                //GainLabel.Text = "Gain:" + gain;
                //FrequencyLabel.Text = "Frequency: " + frequency;
            }

            //Matching: Visual
            if (match_mode == 1)
            {
                color = (int)(max * 2.54);
                pictureBox1.BackColor = Color.FromArgb(color, color, color);
            }

            //Matching: Auditory
            else if (match_mode == 2)
            {
                volume = (float)max / 100f;
                //Sound_Image_Intensity.Text = "Volume: " + (volume * 100).ToString() + '%';
            }
        }

        private void radioButtonA_Click(object sender, EventArgs e)
        {
            //Matching: Tactile
            if (match_mode == 0)
            {
                gain = (int)(1 + (float)(min * 2.54));
                //frequency = (int)(300 + (float)(min * 22));
                //GainLabel.Text = "Gain:" + gain;
                //FrequencyLabel.Text = "Frequency: " + frequency;
            }

            //Matching: Visual
            else if (match_mode == 1)
            {
                color = (int)(min * 2.54);
                pictureBox1.BackColor = Color.FromArgb(color, color, color);
            } 
            
            //Matching: Auditory
            else if (match_mode == 2)
            {
                volume = min / 100F;
                //Sound_Image_Intensity.Text = "Volume: " + (volume * 100).ToString() + '%';
            }               
        }//radioButtonA_Click()
                    
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            float maxTrackValue = practiceTrackbar.Maximum;
            trackValue = practiceTrackbar.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.

            //IF TACTOR MODE IS SELECTED, USER CAN ONLY CHANGE THE INTENSITY OF VIBRATION ON THE TACTOR
            if (TactorModePracticeRadio.Checked)
            {
                gain = (int)(1 + (trackValue / maxTrackValue) * 254);
                //was 1250 + ... (not sure why)
                //frequency = (int)(300 + (trackValue / maxTrackValue) * 2200);

                //InstGainLabel.Text = "Gain:" + gain;
                //InstFrequencyLabel.Text = "Frequency: " + frequency;
            }//TACTOR MODE

            //COLOR MODE: USER CAN ONLY CHANGE THE BRIGHTNESS OF THE IMAGE
            else if(ColorModePracticeRadio.Checked)
            {
                //RGB values are all equal for a greyscale color
                //Scale up to 255 
                color = (int)(trackValue / maxTrackValue * 255);
                PracticeImage.BackColor = Color.FromArgb(color, color, color);
                ImgIntensityPractice.Text = "Image Color Intensity: " + color;
            }//COLOR MODE

            // v should probaly just be an else{}
            else if (SoundModePracticeRadio.Checked)
            {
                volume = (trackValue / maxTrackValue);
                ImgIntensityPractice.Text = "Volume: " + (volume * 100).ToString() + '%';
                //ImgIntensityPractice.Text = "Volume :"  + volume.ToString();
            }
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
            //Remember: the volume is on a scale of 0 - 1
            mySoundEngine.SoundVolume = volume;
            mySoundEngine.Play2D("../chimes.wav");
        }

        private void TactorModePracticeRadio_Click(object sender, EventArgs e)
        {
            trackValue = practiceTrackbar.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            gain = (int)(1 + (trackValue / practiceTrackbar.Maximum) * 254);
            //was 1250 + ... (not sure why)
            //Max out frequency at 2500
            //frequency = (int)(300 + (trackValue / practiceTrackbar.Maximum) * 2200);
            //InstGainLabel.Text = "Gain:" + gain;
            //InstFrequencyLabel.Text = "Frequency: " + frequency;

            PulseTactorPracticeButton.Enabled = true;
            PlaySoundPracticeButton.Enabled = false;
            PracticeImage.Enabled = false;
            //InstGainLabel.Visible = true;
            //InstFrequencyLabel.Visible = true;
            ImgIntensityPractice.Visible = false;
        }

        private void ColorModePracticeRadio_Click(object sender, EventArgs e)
        {
            trackValue = practiceTrackbar.Value;

            color = (int)(trackValue / practiceTrackbar.Maximum * 255);
            PracticeImage.BackColor = Color.FromArgb(color, color, color);
            ImgIntensityPractice.Text = "Image Color Intensity: " + color;
            PracticeImage.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;
            PlaySoundPracticeButton.Enabled = false;
            //InstGainLabel.Visible = false;
            //InstFrequencyLabel.Visible = false;
            ImgIntensityPractice.Visible = true;
        }

        private void SoundModePracticeRadio_Click(object sender, EventArgs e)
        {
            trackValue = practiceTrackbar.Value;
            volume = trackValue / (float)practiceTrackbar.Maximum;

            PlaySoundPracticeButton.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;
            //InstGainLabel.Visible = false;
            //InstFrequencyLabel.Visible = false;
            ImgIntensityPractice.Text = "Volume: " + (volume * 100) + '%';

            ImgIntensityPractice.Visible = true;
        }

        private void LetsGetStartedButton_Click(object sender, EventArgs e)
        {
            PracticePanel.Visible = false;
            this.MainPanel.TabPages.Remove(this.Instructions);
            this.MainPanel.TabPages.Add(this.MatchingTab);
            this.MainPanel.TabPages.Add(this.Instructions);
            //FrequencyLabel.Text = "Frequency : " + frequency.ToString();
            Initialize();
        }

        private void PlaySound1Button_Click(object sender, EventArgs e)
        {

            if (match_mode == 2 && !radioButtonA.Checked && !radioButtonB.Checked)
            {
                MessageBox.Show("You must select A or B to continue.");
                return;
            }
            mySoundEngine.SoundVolume = volume;
            mySoundEngine.Play2D("../chimes.wav");

            soundPlayed = true;

            //Enable the next button if neither the stimulus nor the match mode are "tactile",
            //or if the tactor has already been pulse
            if (tactorPulsed) NextButton.Enabled = true;
            else if (mode != 0 && match_mode != 0) NextButton.Enabled = true;  

        }
    }
}