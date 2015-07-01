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
//TDK Windows C# Discover Controller Tutorial
//This project show how the discovery system work. When the user presses 
//the discover button it calls discover on the tdk.
//The combobox is then populated with the found devices.
//The user can select wich device to connect to in the combo box and press
//the connect button to connect to it.
//The user is able to send a pulse command to the board by pressing the 
//pulse tactor 1 button
//-------------------------------------------------------------------------//

namespace DiscoverController
{
	public partial class DiscoverControllerForm : Form
	{
		private int ConnectedBoardID = -1;

		public DiscoverControllerForm()
		{
			InitializeComponent();
			//To initialize the TDKInterface we need to call InitializeTI before we use any
			//of its functionality
            WriteMessageToGUIConsole("InitializeTI\n");
			CheckTDKErrors(Tdk.TdkInterface.InitializeTI());
		}
  
		private void DiscoverButton_Click(object sender, EventArgs e)
		{
			WriteMessageToGUIConsole("Discover Started...\n");
			//Discovers all serial tactor devices and returns the amount found
			int ret = Tdk.TdkInterface.Discover((int)Tdk.TdkDefines.DeviceTypes.Serial);
			if (ret > 0)
			{
				WriteMessageToGUIConsole("Discover Found:\n");
				//populate combo box with discovered names
				for (int i = 0; i < ret; i++)
				{
					//Gets the discovered device name at the index i
					System.IntPtr discoveredNamePTR =  Tdk.TdkInterface.GetDiscoveredDeviceName(i);
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
			WriteMessageToGUIConsole("\nConnecting to com port " + selectedComPort + "\n");
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
			WriteMessageToGUIConsole("Pulse tactor 1\n");
			//This sends a command to the tactor controller to pulse tactor 1 for 250 milliseconds
			CheckTDKErrors(Tdk.TdkInterface.Pulse(ConnectedBoardID, 1, 250, 0));
		}

		private void WriteMessageToGUIConsole(string msg)
		{
			ConsoleOutputRichTextBox.AppendText(msg);
		}

		private void DiscoverControllerForm_FormClosed(object sender, FormClosedEventArgs e)
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
	}
}
