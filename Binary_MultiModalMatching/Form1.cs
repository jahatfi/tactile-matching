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
        //Variables to pass to the tactor functions
        private int gain = 65;
        private int frequency = 1250;
        private int ConnectedBoardID = -1;
        private int trials = 0;
        private int subtrial = 1;
        private int subtrials = 2;
        private int count = 0;
        private int min, max = 0;
        private List<bool> start_modality = new List<bool>();
        private List<int> start_intensity = new List<int>();
        private string participant_name = "";

        //private List<bool, int> configuration = new List<bool, int>();
        private List<int> results = new List<int>();

        //Determines which mode to use: tactile or visual: false is tactile, and true is visual;
        //Set a default value to avoid undefined behavior (See Meyer's text)
        //If false, then match the brightness to a constant vibration, and vice-versa
        private bool visual_mode = false;

        public Binary_MultiModalMatchingForm()
        {
            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            Console.AppendText("InitializeTI\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());

            this.MainPanel.TabPages.Remove(this.MatchingTab);
            this.MainPanel.TabPages.Remove(this.tabPage1);
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
                    discoverradio.Checked = true;
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
            if (visual_mode)
            {
                if (!radioButtonA.Checked && !radioButtonB.Checked)
                {
                    MessageBox.Show("You must select A or B to continue.");
                    return;
                }
            }
            NextButton.Enabled = true;
            Console.AppendText("Pulse tactor 1\n");
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
        }

        private void initialize()
        {
            if (subtrial == 1)
            {
                min = 0;
                max = 100;
                visual_mode = start_modality[count];

                if (visual_mode)
                {
                    label1.Visible = true;
                    label2.Visible = false;

                    int color = (int)((start_intensity[count] * 2.55));

                    pictureBox1.BackColor = Color.FromArgb(color, color, color);
                    PulseTactor1Button.Enabled = true;
                }
                else
                {
                    label2.Visible = true;
                    label1.Visible = false;
                    gain = 64 + (int)((float)start_intensity[count] * 1.91);
                    frequency = 300 + (int)((float)start_intensity[count] * 32);
                }
                GainLabel.Text = "Gain:" + gain;
                FrequencyLabel.Text = "Frequency: " + frequency;
                return;
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
            if (!File.Exists(path)) Console.AppendText("Cannot find the file.  Please Try Again.\n");

            else
            {
                Console.AppendText("Found the file.\n");
                EnterButton.Enabled = false;
                // Open the file to read from. 
                using (StreamReader sr = File.OpenText(path))
                {
                    char[] delimiterChars = { ',', ' ', '\t' };
                    string s = "";
                    if ((s = sr.ReadLine()) == null)
                    {
                        Console.AppendText("File is empty, please try again.\n");
                        EnterButton.Enabled = true;
                    }
                    else
                    {
                        while ((s = sr.ReadLine()) != null)
                        {
                            //Get the modality
                            Console.AppendText(s + '\n');
                            string[] words = s.Split(delimiterChars);
                            //TESTING ONLY
                            /*
                            Console.AppendText(words[0] + '\n');
                            Console.AppendText(words[1] + '\n');
                            Console.AppendText(words[2] + '\n');
                            */
                            if (string.Compare(words[1], "V") == 0) start_modality.Add(true);
                            else if (string.Compare(words[1], "T") == 0) start_modality.Add(false);
                            else
                            {
                                Console.AppendText("Invalid data in configuration file.  Please check the file and start over.");
                                start_modality.Clear();
                                start_intensity.Clear();
                                break;
                            }
                            //Get the intensity
                            int intensity = 0;
                            if (int.TryParse(words[2], out intensity)) start_intensity.Add(intensity);
                            else
                            {
                                Console.AppendText("Invalid data in configuration file.  Please check the file and start over.");
                                start_modality.Clear();
                                start_intensity.Clear();
                                break;
                            }

                        }//while

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
                trials = start_intensity.Capacity;
                Console.AppendText("trials: " + trials);
                StartButton.Enabled = true;
                DiscoverButton.Enabled = true;
                ConnectButton.Enabled = true;
                FileName.Enabled = false;
            }//else
        }//button3_Click

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (ParticipantNumber.Text == "")
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
                this.MainPanel.TabPages.Add(this.tabPage1);
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
            if (!radioButtonA.Checked && !radioButtonB.Checked)
            {
                MessageBox.Show("You must select A or B to continue.");
                return;
            }
            if (count != trials)
            {
                if (subtrial != subtrials) subtrial++;
                else
                {
                    subtrial = 1;
                    count++;
                    results.Add(min + (max - min) / 2);
                    CountLabel.Text = "Trial " + (count + 1) + " of " + trials;
                }

                if (count != trials)
                {
                    initialize();

                    radioButtonA.Checked = false;
                    radioButtonB.Checked = false;
                }
                else
                {
                    GainLabel.Visible = false;
                    FrequencyLabel.Visible = false;
                    NextButton.Enabled = false;
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = false;
                    radioButtonA.Visible = false;
                    radioButtonB.Visible = false;
                    label1.Visible = false;
                    label2.Visible = false;
                    PulseTactor1Button.Visible = false;
                    NextButton.Visible = false;
                    CountLabel.Visible = false;
                    SubCountLabel.Visible = false;
                    FinishedLabel.Visible = true;

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
                            if (start_modality[i])
                                sw.WriteLine((i + 1).ToString() + "\t\t\tV\t\t" + start_intensity[i].ToString() + "\t\tT\t\t" + results[i]);
                            else
                                sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tV\t\t" + results[i]);
                        }
                        sw.WriteLine("\n\n");
                    }
                }
                SubCountLabel.Text = "Subtrial " + (subtrial) + " of " + subtrials;
            }
        }

        private void radioButtonB_Click(object sender, EventArgs e)
        {
            int color;
            if (visual_mode)
            {
                gain = (int)(64 + (float)(max * 1.91));
                frequency = (int)(300 + (float)(max * 32));
                GainLabel.Text = "Gain:" + gain;
                FrequencyLabel.Text = "Frequency: " + frequency;
            }
            else
            {
                color = (int)(max * 2.55);
                pictureBox1.BackColor = Color.FromArgb(color, color, color);
            }
        }

        private void radioButtonA_Click(object sender, EventArgs e)
        {
            int color;
            if (visual_mode)
            {
                gain = (int)(64 + (float)(min * 1.91));
                frequency = (int)(300 + (float)(min * 32));
                GainLabel.Text = "Gain:" + gain;
                FrequencyLabel.Text = "Frequency: " + frequency;
            }
            else
            {
                color = (int)(min * 2.55);
                pictureBox1.BackColor = Color.FromArgb(color, color, color);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            float trackValue = trackBar1.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float max = trackBar1.Maximum;

            //IF TACTOR MODE IS SELECTED, USER CAN ONLY CHANGE THE INTENSITY OF VIBRATION ON THE TACTOR
            if (TactorModePracticeRadio.Checked)
            {
                //255 - 64 = 191
                gain = (int)(64 + (trackValue / max) * 191);
                //was 1250 + ... (not sure why)
                frequency = (int)(300 + (trackValue / max) * 3200);

                InstGainLabel.Text = "Gain:" + gain;
                InstFrequencyLabel.Text = "Frequency: " + frequency;
            }//TACTOR MODE

            //COLOR MODE: USER CAN ONLY CHANGE THE BRIGHTNESS OF THE IMAGE
            else
            {
                //RGB values are all equal for a greyscale color
                //Scale up to 255 
                int color = (int)(trackValue / max * 255);
                PracticeImage.BackColor = Color.FromArgb(color, color, color);
                ImgIntensityPractice.Text = "Image Color Intensity: " + color;
            }//COLOR MODE
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

        private void TactorModePracticeRadio_Click(object sender, EventArgs e)
        {
            float trackValue = trackBar1.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float max = trackBar1.Maximum;
            //255 - 64 = 191
            gain = (int)(64 + (trackValue / max) * 191);
            //was 1250 + ... (not sure why)
            frequency = (int)(300 + (trackValue / max) * 3200);
            InstGainLabel.Text = "Gain:" + gain;
            InstFrequencyLabel.Text = "Frequency: " + frequency;

            PulseTactorPracticeButton.Enabled = true;
            PracticeImage.Enabled = false;
            InstGainLabel.Visible = true;
            InstFrequencyLabel.Visible = true;
            ImgIntensityPractice.Visible = false;
        }

        private void ColorModePracticeRadio_Click(object sender, EventArgs e)
        {
            PracticeImage.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;
            InstGainLabel.Visible = false;
            InstFrequencyLabel.Visible = false;
            ImgIntensityPractice.Visible = true;
        }

        private void LetsGetStartedButton_Click(object sender, EventArgs e)
        {
            PracticePanel.Visible = false;
            this.MainPanel.TabPages.Remove(this.tabPage1);
            this.MainPanel.TabPages.Add(this.MatchingTab);
            this.MainPanel.TabPages.Add(this.tabPage1);
            initialize();
        }
    }
}