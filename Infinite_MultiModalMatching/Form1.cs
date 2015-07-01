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

namespace InfinitePrecisionMatching
{
    public partial class InfinitePrecisionMatchingForm : Form
    {
        //Variables to pass to the tactor functions
        private int gain = 65;
        private int frequency = 1250;
        private int ConnectedBoardID = -1;
        private int trials = 0;
        private int count = 0;
        private List<bool> start_modality = new List<bool>();
        private List<int> start_intensity = new List<int>();
        private string participant_name = "";

        //private List<bool, int> configuration = new List<bool, int>();
        private List<int> results = new List<int>();

        //Determines which mode to use: tactile or visual: false is tactile, and true is visual;
        //Set a default value to avoid undefined behavior (See Meyer's text)
        //If false, then match the brightness to a constant vibration, and vice-versa
        private bool visual_mode = false;

        public InfinitePrecisionMatchingForm()
        {
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
                    PulseTactor1Button.Enabled = true;
                    discoverradio.Checked = true;
                    StartButton.Enabled = true;
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
            if (ret < 0)
            {
                //the GetLastEAIErrorString returns a string that represents the last error code
                //recorded in the tdk interface.
                Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString());
            }
        }

        private void initialize()
        {
            visual_mode = start_modality[count];
            if (visual_mode)
            {
                label1.Visible = true;
                label2.Visible = false;

                float trackValue = trackBar1.Value;
                //This allows us to calculate the scale factor rather than hardcoding it.
                float max = trackBar1.Maximum;

                int color = (int)((start_intensity[count] * 2.55));

                pictureBox1.BackColor = Color.FromArgb(color, color, color);
                PulseTactor1Button.Enabled = true;
                gain = (int)(64 + (trackValue / max) * 191);
                frequency = (int)(300 + (trackValue / max) * 3250);
            }
            else
            {
                label2.Visible = true;
                label1.Visible = false;
                gain = 64 + (int)((float)start_intensity[count] * 1.91);
                frequency = 300 + (int)((float)start_intensity[count] * 32.5);
            }
            count += 1;
            GainLabel.Text = "Gain:" + gain;
            FrequencyLabel.Text = "Frequency: " + frequency;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            float trackValue = trackBar1.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float max = trackBar1.Maximum;

            //VISUAL MODE: ONLY CHANGE THE VIBRATION OF THE TACTOR
            if (visual_mode)
            {
                //255 - 64 = 191
                gain = (int)(64 + (trackValue / max) * 191);
                //was 1250 + ... (not sure why)
                frequency = (int)(300 + (trackValue / max) * 3250);

            }//VISUAL MODE

            //TACTILE MODE: ONLY CHANGE THE COLOR OF THE IMAGE/LED
            else
            {
                //RGB values are all equal for a greyscale color
                //Scale up to 255 
                int color = (int)(trackValue / max * 255);
                pictureBox1.BackColor = Color.FromArgb(color, color, color);

            }//TACTILE MODE
            GainLabel.Text = "Gain:" + gain;
            FrequencyLabel.Text = "Frequency: " + frequency;
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
                //StartButton.Enabled = true;
                DiscoverButton.Enabled = true;
                ConnectButton.Enabled = true;
                FileName.Enabled = false;
            }//else
        }//button3_Click

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (ParticipantNumber.Text == "")
                MessageBox.Show("Please enter a participant number to continue");
            else if (EnterButton.Enabled == true)
                MessageBox.Show("Please select a configuration file before continuing.");
            else
            {
                participant_name = ParticipantNumber.Text;
                EnterButton.Enabled = false;
                DiscoverButton.Enabled = false;
                StartButton.Enabled = false;
                //tabControl1.SelectedIndex = 1;
                tabControl1.SelectedIndex = 2;
                CountLabel.Text = "Round " + (count + 1) + " of " + trials;
                initialize();

            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (count == trials)
            {
                NextButton.Enabled = false;
                pictureBox1.Visible = false;
                pictureBox2.Visible = false;
                trackBar1.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                PulseTactor1Button.Visible = false;
                NextButton.Visible = false;
                //ShowColorButton.Visible = false;
                CountLabel.Visible = false;
                FinishedLabel.Visible = true;
                results.Add((int)((float)trackBar1.Value * 6.67));
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
            else
            {
                results.Add((int)(trackBar1.Value * 6.67));
                initialize();
                CountLabel.Text = "Trial " + (count) + " of " + trials;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Selected: " + trackBar1.Value);
        }

        private void LetsGetStartedButton_Click(object sender, EventArgs e)
        {
            LetsGetStartedButton.Enabled = false;
            tabControl1.SelectedIndex = 1;

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
            this.tabControl1.TabPages.Remove(this.tabPage1);
            this.tabControl1.TabPages.Add(this.MatchingTab);
            this.tabControl1.TabPages.Add(this.tabPage1);
            initialize();
        }

        private void TactorModePracticeRadio_Click(object sender, EventArgs e)
        {
            float trackValue = trackBar2.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float max = trackBar2.Maximum;
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

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            float trackValue = trackBar2.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            float max = trackBar2.Maximum;

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


    }
}