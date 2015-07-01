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
using System.Resources;
using System.Web;

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

		public ConnectingForm()
		{
			InitializeComponent();
			//To initialize the TDKInterface we need to call InitializeTI before we use any
			//of its functionality
            WriteMessageToGUIConsole("IntializeTI\n");
			CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
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
				PulseTactor1Button.Enabled = true;
			}
			else
			{
				WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
			}
		}
        private void PulseTactor1Button_Click(object sender, EventArgs e)
		{
            int i = 0;
			//This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
			//CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 250, 0));
            foreach (var tactors in checkboxpanel.Controls)
            {
                i+=1;
                if (((CheckBox)tactors).Checked)
                {
                try{
                    switch (i)
                    {
                        
                            case 1:
                            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration1.Text), Int32.Parse(tactortime1.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq1.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain1.Text)), 0);
                                break;

                            case 2:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration2.Text), Int32.Parse(tactortime2.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq2.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain2.Text)), 0);
                                break;

                            case 3:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration3.Text), Int32.Parse(tactortime3.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq3.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain3.Text)), 0);
                                break;

                            case 4:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration4.Text), Int32.Parse(tactortime4.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq4.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain4.Text)), 0);
                                break;

                            case 5:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration5.Text), Int32.Parse(tactortime5.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq5.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain5.Text)), 0);
                                break;

                            case 6:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration6.Text), Int32.Parse(tactortime6.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq6.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain6.Text)), 0);
                                break;

                            case 7:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration7.Text), Int32.Parse(tactortime7.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq7.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain7.Text)), 0);
                                break;

                            case 8:
                                CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, i, Int32.Parse(tactorduration8.Text), Int32.Parse(tactortime8.Text)));
                                Tdk.TdkInterface.ChangeFreq(ConnectedBoardID, i, (Int32.Parse(tactorfreq8.Text)), 0);
                                Tdk.TdkInterface.ChangeGain(ConnectedBoardID, i, (Int32.Parse(tactorgain8.Text)), 0);
                                break;
                    
                            default:
                                WriteMessageToGUIConsole("Error!");
                                break;                        
                    }//switch

                    WriteMessageToGUIConsole("\nTactor " + i + " pulsed!");
                }//try
                catch(System.FormatException){
                             WriteMessageToGUIConsole("\nInvalid Value");
                        }
                }//if
              }//foreach
		}//PulseTactor Button_Click
		private void WriteMessageToGUIConsole(string msg)
		{
			ConsoleOutputRichTextBox.AppendText(msg);
		}

		private void ConnectingForm_FormClosed(object sender, FormClosedEventArgs e)
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
