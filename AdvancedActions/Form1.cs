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
using System.Runtime.InteropServices;

//-------------------------------------------------------------------------//
//TDK Windows C# Advanced Actions Tutorial
//This project contains exampls of advanced commands such as RampGain and RampFreq.
//This tutorial builds off the Discover Controllers tutorial.
//-------------------------------------------------------------------------//

namespace AdvancedActions
{
    public partial class AdvancedActionsForm : Form
    {
        private int ConnectedDeviceID = -1;

        public AdvancedActionsForm()
        {
            InitializeComponent();
            //To initialize the TDKInterface we need to call InitializeTI before we use any
            //of its functionality
            WriteMessageToGUIConsole("IntializeTI\n");
            CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
        }

        private void DiscoverButton_Click(object sender, EventArgs e)
        {
            WriteMessageToGUIConsole("\nDiscover Started...\n");
            //Discovers all serial tactor devices and returns the amount found
            int ret = Tdk.TdkInterface.Discover((int)Tdk.TdkDefines.DeviceTypes.Serial);
            if (ret > 0)
            {
                WriteMessageToGUIConsole("Discover Found:\n");
                //populate combo box with discovered names
                for (int i = 0; i < ret; i++)
                {
                    //Gets the discovered device name at the index i
                    System.IntPtr discoveredNamePTR = Tdk.TdkInterface.GetDiscoveredDeviceName(i);
                    if (discoveredNamePTR != null)
                    {
                        string sComName = Marshal.PtrToStringAnsi(discoveredNamePTR);
                        WriteMessageToGUIConsole(sComName + "\n");
                        ComPortComboBox.Items.Add(sComName);
                    }
                    else
                        WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
                }
                ComPortComboBox.SelectedIndex = 0;
                DiscoverButton.Enabled = false;
                ConnectButton.Enabled = true;
            }
            else
            {
                WriteMessageToGUIConsole("Discover Failed:\n");
                WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string selectedComPort = ComPortComboBox.SelectedItem.ToString();
            WriteMessageToGUIConsole("Connecting to com port " + selectedComPort + "\n");
            //Connect connects to the tactor controller via serial with the given name
            //we should be hooking up a response callback but for simplicity of the 
            //tutorial we wont be. Reference the ResponseCallback tutorial for more information
            int ret = Tdk.TdkInterface.Connect(selectedComPort,
                                               (int)Tdk.TdkDefines.DeviceTypes.Serial,
                                                System.IntPtr.Zero);
            if (ret >= 0)
            {
                ConnectedDeviceID = ret;
                DiscoverButton.Enabled = false;
                ConnectButton.Enabled = false;
                PulseTactor1Button.Enabled = true;
                AdvancedAction1Button.Enabled = true;
                AdvancedAction2Button.Enabled = true;
                StoreTActionButton.Enabled = true;
                PlayStoredTActionButton.Enabled = true;
            }
            else
            {
                WriteMessageToGUIConsole(Tdk.TdkDefines.GetLastEAIErrorString());
            }
        }

        private void PulseTactor1Button_Click(object sender, EventArgs e)
        {
            WriteMessageToGUIConsole("Pulse tactor 1\n");
            //This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 1, 250, 0));
        }
        private void AdvancedAction1Button_Click(object sender, EventArgs e)
        {
            WriteMessageToGUIConsole("Advanced Action 1\n");
            //this entire set of commands ramps the gains of tactors 1 and 2 up, and ramps the frequency
            //of tactors 9 and 10 up, all while pulsing tactor 1,2,9,10
            int timeFactor = 10;
            //Ramps gain of tactor 1 from 10 to 255 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampGain(ConnectedDeviceID, 1, 10, 255, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //Ramps gain of tactor 2 from 10 to 255 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampGain(ConnectedDeviceID, 2, 10, 255, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //Ramps frequency of tactor 9 from 300 to 3500 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampFreq(ConnectedDeviceID, 9, 300, 3500, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //Ramps frequency of tactor 10 from 300 to 3500 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampFreq(ConnectedDeviceID, 10, 300, 3500, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //pulses tactor 1 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 1, 100 * timeFactor, 0));
            //pulses tactor 2 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 2, 100 * timeFactor, 0));
            //pulses tactor 9 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 9, 100 * timeFactor, 0));
            //pulses tactor 10 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 10, 100 * timeFactor, 0));
        }
        private void AdvancedAction2Button_Click(object sender, EventArgs e)
        {
            WriteMessageToGUIConsole("Advanced Action 2\n");
            //this entire set of commands ramps the gains of tactors 1 and 2 down, and ramps the frequency
            //of tactors 9 and 10 down, all while pulsing tactor 1,2,9,10
            int timeFactor = 10;
            //Ramps gain of tactor 1 from 255 to 10 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampGain(ConnectedDeviceID, 1, 255, 10, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //Ramps gain of tactor 2 from 255 to 10 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampGain(ConnectedDeviceID, 2, 255, 10, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //Ramps frequency of tactor 9 from 3500 to 300 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampFreq(ConnectedDeviceID, 9, 3500, 300, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //Ramps frequency of tactor 10 from 3500 to 300 for a duration of a 1000 milliseconds immediatly.
            CheckTDKErrors(Tdk.TdkInterface.RampFreq(ConnectedDeviceID, 10, 3500, 300, 100 * timeFactor, Tdk.TdkDefines.RampLinear, 0));
            //pulses tactor 1 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 1, 100 * timeFactor, 0));
            //pulses tactor 2 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 2, 100 * timeFactor, 0));
            //pulses tactor 9 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 9, 100 * timeFactor, 0));
            //pulses tactor 10 for a duration of 1000 milliseconds immediatly
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 10, 100 * timeFactor, 0));
        }
        private void StoreTActionButton_Click(object sender, EventArgs e)
        {
            //sets the device to store taction state for slot 1.
            WriteMessageToGUIConsole("Store TAction\n");
            CheckTDKErrors(Tdk.TdkInterface.BeginStoreTAction(ConnectedDeviceID,1));
            //stores the following set of commands as a TAction on the device.
            CheckTDKErrors(Tdk.TdkInterface.ChangeGain(ConnectedDeviceID, 0, 255, 0));
            CheckTDKErrors(Tdk.TdkInterface.ChangeFreq(ConnectedDeviceID, 0, 2500, 0));
            CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedDeviceID, 1, 250, 0));
            //stops recording
            CheckTDKErrors(Tdk.TdkInterface.FinishStoreTAction(ConnectedDeviceID));
        }

        private void PlayStoredTActionButton_Click(object sender, EventArgs e)
        {
            //plays the stored TAction on slot 1
            WriteMessageToGUIConsole("Playing Stored TAction\n");
            CheckTDKErrors(Tdk.TdkInterface.PlayStoredTAction(ConnectedDeviceID,0,1));
        }

        private void WriteMessageToGUIConsole(string msg)
        {
            ConsoleOutputRichTextBox.AppendText(msg);
        }

        private void AdvancedActionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //closes up the connection to the tactor device with ConnectedBoardID
			CheckTDKErrors(Tdk.TdkInterface.Close(ConnectedDeviceID));
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

      
    }
}
