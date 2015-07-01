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
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Resources;
using System.Web;
using System.Threading;
using System.Reflection;
//-------------------------------------------------------------------------//
//TDK Windows C# Connecting Tutorial
//This project shows how to connect to a tactor controller with a given name.
//The Pulse button allows the user to send a pulse command to the board
//-------------------------------------------------------------------------//

namespace Connecting
{


	public partial class ConnectingForm : Form
	{
		private int ConnectedBoardID = -1;
        delegate void SetTextCallback(string text);
        static ParameterizedThreadStart PTS;

        //Array of threads
        static Thread[] Tactor_Threads = new Thread[9];

        //Array of lists to hold the variables for each thread.
        static List<int>[] Tactor_variables = new List<int>[9];

        /*Array of bools to mark whether the thread has executed or not.
        All values are initialized to "false" by default.*/
        static bool[] Tactor_is_running = new bool[9];

        //Indicates whether this is the first time this thread has been run
        static bool[] not_first_time = new bool[9];

        Dictionary<int, System.Windows.Forms.TextBox> delay = new Dictionary<int,TextBox>();
        Dictionary<int, System.Windows.Forms.TextBox> duration = new Dictionary<int, TextBox>();
        Dictionary<int, System.Windows.Forms.TextBox> gain = new Dictionary<int, TextBox>();
        Dictionary<int, System.Windows.Forms.TextBox> frequency = new Dictionary<int, TextBox>();

        public ConnectingForm()
        {
            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            WriteMessageToGUIConsole("IntializeTI\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
            // Create the thread object, passing in the TTC.Pulse method
            // via a ThreadStart delegate. This does not start the thread.
            PTS = new ParameterizedThreadStart(Thread_Method);
            //Initialize the arrays and dictionaries
            string delay_box = "tactortime";
            string duration_box = "tactorduration";
            string gain_box = "tactorgain";
            string freq_box = "tactorfreq";
            string temp_string;
            TextBox tempTexbox = new TextBox();
            for (int j = 1; j <= 8; ++j)
            {
                //Tactor_is_running[j] = false;
                //WriteMessageToGUIConsole(delay_box+j+"\n");
                Tactor_variables[j] = new List<int>();
                Tactor_Threads[j] = (new Thread(PTS));
                
                temp_string = delay_box + j;
                tempTexbox = (TextBox)this.Controls.Find((delay_box + j), true)[0];
                delay[j] = tempTexbox;

                temp_string = duration_box + j;
                tempTexbox = (TextBox)this.Controls.Find((duration_box + j), true)[0];
                duration[j] = tempTexbox;

                temp_string = gain_box + j;
                tempTexbox = (TextBox)this.Controls.Find((gain_box + j), true)[0];
                gain[j] = tempTexbox;

                temp_string = freq_box + j;
                tempTexbox = (TextBox)this.Controls.Find((freq_box + j), true)[0];
                frequency[j] = tempTexbox;

            }
        }

        private void Thread_Method(object myObject)
        {
            //The contents of the list are thus:
            // ConnectedBoardID, tactor #, duration, delay

            List<int> list = (List<int>)myObject;

            while (Tactor_is_running[list[1]])
            {
                //int threadID = (int)AppDomain.GetCurrentThreadId();
                this.SetText(/*"In thread " + threadID + */"\nConnectionID: " + list[0] +
                        "\nTactor_ID: " + list[1] + "\nduration:" + list[2] + "\ndelay: " + list[3]);

                CheckTDKErrors(Tdk.TdkInterface.Pulse(list[0], list[1], list[2], list[3]));
                SetText("\nTactor " + list[1] + " pulsed!");
                //Thread.CurrentThread.Join();
                Tactor_is_running[list[1]] = false;
            }


        }
        
        //Use this method for cross-thread calls!
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.ConsoleOutputRichTextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.ConsoleOutputRichTextBox.AppendText(text);
            }
        }

		private void ConnectButton_Click(object sender, EventArgs e)
		{

			string givenComPort = ComPortTextBox.Text;
			WriteMessageToGUIConsole("Connecting to com port " + givenComPort + "\n");
			//Connect connects to the tactor controller via serial with the given name
			//we should be hooking up a response callback but for simplicity of the 
			//tutorial we wont be. Reference the ResponseCallback tutorial for more information
			int ret = Tdk.TdkInterface.Connect(givenComPort,
											   (int)Tdk.TdkDefines.DeviceTypes.Serial,
												IntPtr.Zero);
			if (ret >= 0)
			{
				ConnectedBoardID = ret;
				ConnectButton.Enabled = false;
				StartButton.Enabled = true;
                WriteMessageToGUIConsole("Success!\n");
			}
			else
			{
				WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
			}
		}
        private void PulseTactor1Button_Click(object sender, EventArgs e)
		{
           bool rerun = false;
           string checkbox_name = "tactor";
           CheckBox temp_cb;
           for (int i = 1; i <= 8; i++)
           {
               string g = i.ToString();
               System.Windows.Forms.Control[] test = this.Controls.Find(checkbox_name + g, true);
               temp_cb = (CheckBox)this.Controls.Find(checkbox_name + g, true)[0];
               if (temp_cb.Checked)
               {
                   if (!Tactor_is_running[i])
                   {
                       TextBox temp;
                       /*We could add these variables to the list and pass it the the Thread_Method method,
                       but I fear that would create more overhead, so I just went with this approach.*/
                       frequency.TryGetValue(i, out temp);
                       Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(temp.Text)), 0);
                       gain.TryGetValue(i, out temp);
                       Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(temp.Text)), 0);

                       Tactor_variables[i].Add(ConnectedBoardID);
                       Tactor_variables[i].Add(i);
                       duration.TryGetValue(i, out temp);
                       Tactor_variables[i].Add((Int32.Parse(temp.Text)));
                       delay.TryGetValue(i, out temp);
                       Tactor_variables[i].Add((Int32.Parse(temp.Text)));

                       Tactor_is_running[i] = true;
                       if (!not_first_time[i])
                       {
                           Tactor_Threads[i].Start(Tactor_variables[i]);
                           not_first_time[i] = true;
                       }

                   }//if
                   else rerun = true;
               }//if
           }//for
           if (rerun)
           {
               string temp = "Could not pulse tactors: ";
               for (int i = 1; i < 9; ++i) if (Tactor_is_running[i]) temp += i.ToString() + " ";
               temp += "\nbecause they have already been pulsed, \nor are waiting to do so based the time delay.\n[Please restart this application if you wish to start over.]";
               MessageBox.Show(temp);
           }//if
		}//PulseTactorButton_Click

		private void WriteMessageToGUIConsole(string msg)
		{
			ConsoleOutputRichTextBox.AppendText(msg);
		}

		private void ConnectingForm_FormClosed(object sender, FormClosedEventArgs e)
		{
            for(int i = 0; i < 8; ++i) Thread.CurrentThread.Join();
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
				WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
			}
		}

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            tactortime1.Text = "0";
            tactortime2.Text = "0";
            tactortime3.Text = "0";
            tactortime4.Text = "0";
            tactortime5.Text = "0";
            tactortime6.Text = "0";
            tactortime7.Text = "0";
            tactortime8.Text = "0";

            tactorgain1.Text = "0";
            tactorgain2.Text = "0";
            tactorgain3.Text = "0";
            tactorgain4.Text = "0";
            tactorgain5.Text = "0";
            tactorgain6.Text = "0";
            tactorgain7.Text = "0";
            tactorgain8.Text = "0";

            tactorfreq1.Text = "0";
            tactorfreq2.Text = "0";
            tactorfreq3.Text = "0";
            tactorfreq4.Text = "0";
            tactorfreq5.Text = "0";
            tactorfreq6.Text = "0";
            tactorfreq7.Text = "0";
            tactorfreq8.Text = "0";

            tactorduration1.Text = "0";
            tactorduration2.Text = "0";
            tactorduration3.Text = "0";
            tactorduration4.Text = "0";
            tactorduration5.Text = "0";
            tactorduration6.Text = "0";
            tactorduration7.Text = "0";
            tactorduration8.Text = "0";
                         
        }
	}
}
