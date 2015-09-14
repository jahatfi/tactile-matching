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
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
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
        private bool referencePlayed = false;
        private bool lowVariablePlayed = false;
        private bool highVariablePlayed = false;
        //Variables to pass to the tactor functions
        private int gain = 1;
        private int frequency = 2500;
        int color = 0;

        //Irrklang documentation: http://www.ambiera.com/irrklang/docunet/index.html
        //Volume is float between 0f and 1f
        private float volume = 0f;
        private float audioFrequency = 0f;
        private int ConnectedBoardID = -1;
        private int trial = 0;
        private int trials = 0;
        private int subtrial = 1;
        private int subtrials = 2;
        private int min, max = 0;
        private int prevMin, prevMax = 0;
        private float trackValue = 0;
        private List<int> start_modality = new List<int>();
        private List<int> match_modality = new List<int>();
        private List<int> start_intensity = new List<int>();
        private string participant_name = "";
        private string outputfile = "";

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

        //Cache references to images
        Image speakerImage;
        Image tactorImage;

        public Binary_MultiModalMatchingForm()
        {
            this.Text = "Low - High Intensity Matching Program";

            //Start up the irrklang engine
            mySoundEngine = new ISoundEngine();

            mySoundEngine.Play2D("tone.wav");

            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            Console.AppendText("Initializing Tactor Interface...\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
            Console.AppendText("Success\n");

            //this.PrimaryTabControl.TabPages.Remove(this.MatchingTab);
            //this.PrimaryTabControl.TabPages.Remove(this.Instructions);

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
            Cursor = Cursors.WaitCursor;
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

                Console.AppendText("Connecting to com port " + selectedComPort + "...\n");
                //Connect connects to the tactor controller via serial with the given name
                //we should be hooking up a response callback but for simplicity of the 
                //tutorial we wont be. Reference the ResponseCallback tutorial for more information
                int ret = Tdk.TdkInterface.Connect(selectedComPort,
                                                   (int)Tdk.TdkDefines.DeviceTypes.Serial,
                                                    System.IntPtr.Zero);
                if (ret >= 0)
                {
                    Console.AppendText("Successfully connected.\n");
                    ConnectedBoardID = ret;
                    DiscoverButton.Enabled = false;
                    ConnectButton.Enabled = false;
                    Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
                    connected = true;
                    if (fileLoaded) StartButton.Enabled = true;

                    //discoverradio.Checked = true;
                }
                else
                {
                    Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString()+"\n");
                }
            }
            else MessageBox.Show("Please select a Connection Mode!");

            Cursor = Cursors.Default;
            if(fileLoaded) StartButton.Enabled = true; 
            
        }

        private void PlayVariableButton_Click(object sender, EventArgs e)
        {
            //Sanity Check
            if (!radioButtonA.Checked && !radioButtonB.Checked)
            {
                MessageBox.Show("You must select A or B to continue.");
                return;
            }

            if (match_mode == 0)
            {

                //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
                //Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 500, 0));
                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 2, 500, 0));

                //Enable the next button if neither the stimulus nor the match mode are "auditory",
                //or if the sound has already been played
                //if (!EnableNextButton() && mode != 2 && match_mode != 2) NextButton.Enabled = true;
            }

            else {
                if (volumeButton.Checked)
                {
                    mySoundEngine.SoundVolume = volume;
                    mySoundEngine.Play2D("tone.wav");
                }
                else
                {
                    string path = "tones/tone" + (audioFrequency*100).ToString() + ".wav";
                    mySoundEngine.Play2D(path);
                }
                //Enable the next button if neither the stimulus nor the match mode are "tactile",
                //or if the tactor has already been pulse
                //if (!EnableNextButton() && mode != 0 && match_mode != 0) NextButton.Enabled = true;
            }
            EnableNextButton();

        }

        //This function has the side effect of enabling the "Next" button if the reference has been 
        //played, as well both the low and high variables.  It returns true if this is the case, thereby "shortcircuiting"
        // (https://en.wikipedia.org/wiki/Short-circuit_evaluation) the if() statement from which it's called.  
        //If it returns false, more checks are required to determine if the Next button should be enabled.
        private bool EnableNextButton()
        {
            if (radioButtonA.Checked) lowVariablePlayed = true;
            else highVariablePlayed = true;

            if (referencePlayed && lowVariablePlayed && highVariablePlayed)
            {
                NextButton.Enabled = true;
                return true;
            }

            return false;
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
                Console.AppendText(Tdk.TdkDefines.GetLastEAIErrorString() + "\n");
            }
            //else Console.AppendText("Success\r\n");
        }

        private void RefreshControls()
        {
            NextButton.Enabled = false;
            mode = start_modality[trial];
            match_mode = match_modality[trial];
            referencePlayed = false;
            lowVariablePlayed = false;
            highVariablePlayed = false;
            //Radio Button 'A' will be selected initially
            radioButtonA.Select();

            //Image is grey by default
            ReferenceImage.Image = null;
            ReferenceImage.BackColor = SystemColors.ControlLight;
            VariableImage.Image = null;
            VariableImage.BackColor = SystemColors.ControlLight;

            //Disable and hide elements
            //The relevant controls will be enabled as needed
            playVariableButton.Enabled = false;
            playVariableButton.Visible = false;
            VariableImage.Visible = false;
            ReferenceImage.Visible = false;

            //Set these to true
            playReferenceButton.Visible = true;
            playReferenceButton.Enabled = true;

            //Stimulus: Tactile
            if (mode == 0)
            {
                ReferenceImage.Image = tactorImage;
                ReferenceImage.Visible = true;

                gain = (int)(1 + (float)(start_intensity[trial] * 2.54));
                //frequency = (int)(300 + (float)(start_intensity[count] * 22));

                //Match mode: Visual
                if (match_mode == 1)
                {
                    //No need to hit play to show color, so lower... is true.
                    lowVariablePlayed = true;
                    VariableImage.BackColor = Color.FromArgb(color, color, color);
                    VariableImage.Visible = true;

                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b brightness \b0which best corresponds to the intensity of the \b vibration\b0.}";
                }

                //Match Mode: Auditory
                else if (match_mode == 2)
                {
                    playVariableButton.Enabled = true;
                    playVariableButton.Visible = true;
                    VariableImage.Image = speakerImage;
                    VariableImage.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b volume \b0which best corresponds to the intensity of the \b vibration\b0.}";
                }
            }

            //Stimulus: Visual
            else if (mode == 1)
            {
                referencePlayed = true;
                playReferenceButton.Visible = false;
                playReferenceButton.Enabled = false;
                color = (int)((start_intensity[trial] * 2.54));
                ReferenceImage.Image = null;
                ReferenceImage.BackColor = Color.FromArgb(color, color, color);
                ReferenceImage.Visible = true;
                playVariableButton.Enabled = true;
                playVariableButton.Visible = true;

                //Match Mode: Tactile
                if (match_mode == 0)
                {
                    VariableImage.Image = tactorImage;
                    VariableImage.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b vibration \b0which best corresponds to the intensity of the \b brightness\b0.}";

                }

                //Match Mode: Auditory
                else if (match_mode == 2)
                {
                    VariableImage.Image = speakerImage;
                    VariableImage.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b volume \b0which best corresponds to the intensity of the \b brightness\b0.}";
                }
            }

            //Stimulus: Auditory
            else if (mode == 2)
            {
                if (volumeButton.Checked) volume = (float)start_intensity[trial] / 100f;
                else audioFrequency = (float)start_intensity[trial] / 100f;
                ReferenceImage.Image = speakerImage;
                ReferenceImage.Visible = true;

                //Match Mode: Tactile
                if (match_mode == 0)
                {
                    variableLabel1.Visible = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b vibration \b0which best corresponds to the intensity of the \b volume\b0.}";
                    playVariableButton.Enabled = true;
                    playVariableButton.Visible = true;
                    VariableImage.Image = tactorImage;
                    VariableImage.Visible = true;
                }

                //Match mode: Visual
                if (match_mode == 1)
                {
                    VariableImage.BackColor = Color.FromArgb(color, color, color);
                    VariableImage.Visible = true;
                    lowVariablePlayed = true;
                    richTextBox1.Rtf = @"{\rtf1\ansi Select the intensity of the \b brightness \b0which best corresponds to the intensity of the \b volume\b0.}";
                }
            }
        }

        private void Initialize()
        {
            //Reset values 
            min = 0;
            max = 100;

            //Set all values to their minimum
            gain = 1;
            volume = 0;
            audioFrequency = 0;
            color = 0;
            RefreshControls();
            
        }



        private void Load_File_Click(object sender, EventArgs e)
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



            //Disable all controls on this tab:
            loadFileButton.Enabled = false;
            browseInput.Enabled = false;
            FileName.Enabled = false;

            trials = start_intensity.Count;
            Console.AppendText("trials: " + trials + "\n");           
            
            fileLoaded = true;
            if (connected) StartButton.Enabled = true;
            return;

        }//button3_Click

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (ConnectButton.Enabled)
            {
                MessageBox.Show("Please connect to the tactor to continue.");
                return;
            }

            if (ParticipantNumber.Text == "")
            {
                MessageBox.Show("Please enter the participant number to continue");
                return;
            }

            if (!(Int32.TryParse(SubtrialsNumber.Text, out subtrials)))
            {
                MessageBox.Show("Please enter the number of subtrials.");
                return;
            }

            if ((subtrials < 1) || (subtrials > 10))
            {
                MessageBox.Show("Please enter a valid number between 1 - 10.");
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

                outputfile = outputFolder.Text + outputFile.Text;
                if (!outputfile.EndsWith(".txt")) outputfile += ".txt";

                //this.PrimaryTabControl.TabPages.Add(this.Instructions);
                //Disable all controls on this tab
                browseOutput.Enabled = false;
                DiscoverButton.Enabled = false;
                StartButton.Enabled = false;
                volumeButton.Enabled = false;
                frequencyButton.Enabled = false;

                participant_name = ParticipantNumber.Text;
                //Enable all controls on the practice panel:
                practiceTrackbar.Enabled = true;
                //PlaySoundPracticeButton.Enabled = true;
                //PulseTactorPracticeButton.Enabled = true;
                SoundModePracticeRadio.Enabled = true;
                TactorModePracticeRadio.Enabled = true;
                ColorModePracticeRadio.Enabled = true;
                LetsGetStartedButton.Enabled = true;
                PrimaryTabControl.SelectedIndex = 1;


                TrialLabel.Text = "Trial " + (trial + 1) + " of " + trials;
                SubTrialLabel3.Text = "of " + subtrials;
                

                //this.PrimaryTabControl.TabPages.Remove(this.ConfigureTab);
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
            prevMax = max;
            prevMin = min;

            //If we're still in the same trial, we just increment the subtrial counter
            if (subtrial != subtrials)
            {
                if (radioButtonA.Checked) max = min + (max - min) / 2;
                else min = min + (max - min) / 2;
                subtrial++;
                SubTrialLabel2.Text = subtrial.ToString();
                NextButton.Enabled = false;
                lowVariablePlayed = false;
                highVariablePlayed = false;
                referencePlayed = false;
                if (mode == 1) referencePlayed = true;
                if (match_mode == 1)
                {
                    if (radioButtonA.Checked) lowVariablePlayed = true;
                    else highVariablePlayed = true;
                }
                
            }

            //This means we just completed a trial.  Store the result, increment the count, and check to see if we're done.
            else
            {
                if (radioButtonA.Checked) results.Add(min);
                else results.Add(max);
                trial++;

                //Finished  
                if (trial == trials) ProcessResults();

                //Not finished - reset the variables.
                else
                {
                    subtrial = 1;
                    TrialLabel.Text = "Trial " + (trial + 1) + " of " + trials;
                    SubTrialLabel2.Text = "1";
                    NextButton.Enabled = false;
                    Initialize();
                }
            }
            goBackButton.Enabled = true;

        }



        private void ProcessResults(){
            referenceLabel.Visible = false;
            variableLabel1.Visible = false;
            NextButton.Enabled = false;
            VariableImage.Visible = false;
            radioButtonA.Visible = false;
            radioButtonB.Visible = false;
            playVariableButton.Visible = false;
            NextButton.Visible = false;
            TrialLabel.Visible = false;
            SubTrialLabel1.Visible = false;
            SubTrialLabel2.Visible = false;
            SubTrialLabel3.Visible = false;
            ReferenceImage.Visible = false;
            playReferenceButton.Visible = false;
            richTextBox1.Visible = false;
            label4.Visible = false;

            goBackButton.Visible = false;
            FinishedLabel.Text = "Finished!  Thanks for participating! \r\n Note to experimentor: " +
                "The results file is stored in " + outputfile;
            FinishedLabel.Visible = true;


            using (StreamWriter sw = new StreamWriter(outputfile, true))
            {
                sw.WriteLine("\nResults for " + participant_name + " on " + DateTime.Now);
                sw.WriteLine("Presentation Order\tStart Modality\tStart Intensity\tMatch Modality\tMatch Intensity\n");
                for (int i = 0; i < trials; i++)
                {
                    if (start_modality[i] == 0)
                    {
                        if (match_modality[i] == 1) sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tV\t\t" + results[i]);
                        else
                        {
                            if (volumeButton.Checked) sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + results[i]);
                            else sw.WriteLine((i + 1).ToString() + "\t\t\tT\t\t" + start_intensity[i].ToString() + "\t\tA\t\t" + (200 + results[i] * 18).ToString() + " Hz");
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
                    //sw.WriteLine("\n");
                }
            }
        }

        private void radioButtonB_Click(object sender, EventArgs e)
        {

            //Matching: Tactile
            if (match_mode == 0) gain = (int)(1 + (float)(max * 2.54));


            //Matching: Visual
            if (match_mode == 1)
            {
                color = (int)(max * 2.54);
                VariableImage.BackColor = Color.FromArgb(color, color, color);
                highVariablePlayed = true;
                if (referencePlayed && lowVariablePlayed) NextButton.Enabled = true;
            }

            //Matching: Auditory
            else if (match_mode == 2)
            {
                if (volumeButton.Checked) volume = (float)max / 100f;
                else audioFrequency = (float)max / 100f;
            }


        }

        private void radioButtonA_Click(object sender, EventArgs e)
        {
            //Matching: Tactile
            if (match_mode == 0) gain = (int)(1 + (float)(min * 2.54));


            //Matching: Visual
            else if (match_mode == 1)
            {
                color = (int)(min * 2.54);
                VariableImage.BackColor = Color.FromArgb(color, color, color);
                lowVariablePlayed = true;
                if (referencePlayed && highVariablePlayed) NextButton.Enabled = true;
            } 
            
            //Matching: Auditory
            else if (match_mode == 2)
            {
                if (volumeButton.Checked) volume = min / 100F;
                else audioFrequency = min / 100F;
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
                PraticeLabel.Text = "Gain: " + gain;

            }//TACTOR MODE

            //COLOR MODE: USER CAN ONLY CHANGE THE BRIGHTNESS OF THE IMAGE
            else if(ColorModePracticeRadio.Checked)
            {
                //RGB values are all equal for a greyscale color
                //Scale up to 255 
                color = (int)(trackValue / maxTrackValue * 255);
                PracticeImage.BackColor = Color.FromArgb(color, color, color);
                PraticeLabel.Text = "Image Color Intensity: " + color;
            }//COLOR MODE

            // v should probaly just be an else{}
            else if (SoundModePracticeRadio.Checked)
            {
                if (volumeButton.Checked)
                {
                    volume = (trackValue / maxTrackValue);
                    PraticeLabel.Text = "Volume: " + (volume * 100).ToString() + '%';
                }
                else 
                {
                    audioFrequency = (trackValue / maxTrackValue);
                    PraticeLabel.Text = "Frequency: " + (200 + audioFrequency * 1800) + " Hz";
                }
            }
        }

        private void PulseTactorPracticeButton_Click(object sender, EventArgs e)
        {
            //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
            //Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, 1, frequency, 0);
            Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 500, 0));
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 2, 500, 0));

        }

        private void PlaySoundPracticeButton_Click(object sender, EventArgs e)
        {
            if (volumeButton.Checked)
            {
                //Remember: the volume is on a scale of 0 - 1
                mySoundEngine.SoundVolume = volume;
                mySoundEngine.Play2D("tone.wav");
            }
            else
            {
                string path = "tones/tone" + (audioFrequency*100).ToString() + ".wav";
                mySoundEngine.Play2D(path);
            }

        }

        private void TactorModePracticeRadio_Click(object sender, EventArgs e)
        {
            trackValue = practiceTrackbar.Value;
            //This allows us to calculate the scale factor rather than hardcoding it.
            gain = (int)(1 + (trackValue / practiceTrackbar.Maximum) * 254);
            PraticeLabel.Text = "Gain: " + gain;
            //was 1250 + ... (not sure why)
            //Max out frequency at 2500
            //frequency = (int)(300 + (trackValue / practiceTrackbar.Maximum) * 2200);
            //InstGainLabel.Text = "Gain:" + gain;
            //InstFrequencyLabel.Text = "Frequency: " + frequency;
            PulseTactorPracticeButton.Enabled = true;
            PlaySoundPracticeButton.Enabled = false;

        }

        private void ColorModePracticeRadio_Click(object sender, EventArgs e)
        {
            trackValue = practiceTrackbar.Value;

            color = (int)(trackValue / practiceTrackbar.Maximum * 255);
            PracticeImage.BackColor = Color.FromArgb(color, color, color);
            PraticeLabel.Text = "Image Color Intensity: " + color;
            PracticeImage.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;
            PlaySoundPracticeButton.Enabled = false;
        }

        private void SoundModePracticeRadio_Click(object sender, EventArgs e)
        {
            trackValue = practiceTrackbar.Value;
            volume = trackValue / (float)practiceTrackbar.Maximum;
            audioFrequency = volume;

            PlaySoundPracticeButton.Enabled = true;
            PulseTactorPracticeButton.Enabled = false;
            if(volumeButton.Checked) PraticeLabel.Text = "Volume: " + (volume * 100) + '%';
            else PraticeLabel.Text = "Frequency: " + (200 + audioFrequency * 1800) + " Hz";

        }

        private void LetsGetStartedButton_Click(object sender, EventArgs e)
        {
            TactorModePracticeRadio.Enabled = false;
            SoundModePracticeRadio.Enabled = false;
            ColorModePracticeRadio.Enabled = false;
            PulseTactorPracticeButton.Enabled = false;
            PlaySoundPracticeButton.Enabled = false;
            PraticeLabel.Visible = false;
            practiceTrackbar.Enabled = false;
            LetsGetStartedButton.Enabled = false;
            PrimaryTabControl.SelectedIndex = 2;
            playReferenceButton.Enabled = true;
            Initialize();
        }

        private void PlayReferenceButton_Click(object sender, EventArgs e)
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
            else {
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 1, gain, 0);
                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, 2, gain, 0);
                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 500, 0));
                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 2, 500, 0));
            }
            //Enable the next button if the both the low and high variables have been pulsed
            if (lowVariablePlayed && highVariablePlayed) NextButton.Enabled = true;
        }

        private void Binary_MultiModalMatchingForm_Resize(object sender, EventArgs e)
        {
            PrimaryTabControl.Width = this.Width - 25;
            PrimaryTabControl.Height = this.Height - 50;
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            goBackButton.Enabled = false;
            min = prevMin;
            max = prevMax;

            if (subtrial != 1)
            {
                subtrial -= 1;
                radioButtonA_Click(sender, e);
            }

            else
            {
                results.Remove(results.Count - 1);
                subtrial = subtrials;
                trial -= 1;
                TrialLabel.Text = "Trial " + (trial + 1) + " of " + trials;
                RefreshControls();

            }
            SubTrialLabel2.Text = subtrial.ToString();
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

        private void MatchingTab_Click(object sender, EventArgs e)
        {

        }

    }
}