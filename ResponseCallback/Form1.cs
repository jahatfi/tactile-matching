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
//TDK Windows C# Response Callback Tutorial
//This projects shows how to recieve device packet data.
//This tutorial builds off the Discover Controllers tutorial.
//-------------------------------------------------------------------------//

namespace ResponseCallback
{
	public partial class ResponseCallbackForm : Form
	{
		private int ConnectedBoardID = -1;

		public ResponseCallbackForm()
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
			WriteMessageToGUIConsole("\nConnecting to com port " + selectedComPort + "\n");
			
			//This is the callback we will be using for the responce packet data from the tacotr device.
			Tdk.TdkDefines.DataCallbackDelegate incomingDataCallbackDelegate =
				new Tdk.TdkDefines.DataCallbackDelegate(DeviceIncomingDataCallback);
			System.IntPtr callback = Marshal.GetFunctionPointerForDelegate(incomingDataCallbackDelegate);

			//Connect connects to the tactor controller via serial with the given name
			//we are passing the callback to the device so we can get the device response data through it
			int ret = Tdk.TdkInterface.Connect(selectedComPort,
											   (int)Tdk.TdkDefines.DeviceTypes.Serial,
												callback);
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

		delegate void WriteMessageToGUIConsoleCallback(string msg);
		private void WriteMessageToGUIConsole(string msg)
		{
			if (this.ConsoleOutputRichTextBox.InvokeRequired)
			{
				WriteMessageToGUIConsoleCallback d = new WriteMessageToGUIConsoleCallback(WriteMessageToGUIConsole);
			   // this.Invoke(d, msg);
				this.BeginInvoke(d,msg);
			}
			else
			{
				this.ConsoleOutputRichTextBox.AppendText(msg);
			}
		}

		private void ResponseCallbackForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			//closes up the connection to the tactor device with ConnectedBoardID
			CheckTDKErrors(Tdk.TdkInterface.Close(ConnectedBoardID));
			//cleans up everyting associated witht the TActionManager. Unloads any TActions loaded
			CheckTDKErrors(Tdk.TdkInterface.ShutdownTI());
		}

		void PrintPacketMsg(String msg, float timer, byte[] buffer, int size)
		{
			String ret = "";
			ret += "[TDK] " + timer + "," + msg;

			for (int i = 0; i < size; ++i)
				ret += "" + buffer[i] + ":";
			ret += "\n";
			WriteMessageToGUIConsole(ret);
		}
		byte ComputeCheckSum(byte[] buf, int size)
		{
			byte checksum = 0x00;
			for (int i = 0; i < size; ++i)
			{
				checksum = (byte)(checksum ^ buf[i]);
			}
			checksum ^= 0xEA;

			return checksum;
		}

		public void DeviceIncomingDataCallback(int id, System.IntPtr bytes, int size)
		{
			byte[] packet = new byte[size];
			Marshal.Copy(bytes, packet, 0, size);

			byte type = (byte)packet[1];
			byte len = (byte)packet[2];

			byte[] data = new byte[len];
			for (int i = 0; i < len; i++)
				data[i] = packet[i + 3];
			byte chksum = packet[len + 3];

			byte computed_chksum = ComputeCheckSum(packet, len + 3);

			if (computed_chksum == chksum)
			{
				switch (type)
				{
					case (byte)0xc8:
						PrintPacketMsg("ACK WITH DATA", 0, packet, size);
						break;
					case (byte)0xc9:
						PrintPacketMsg("ACK", 0, packet, size);
						break;
					case (byte)0xc4:
						{
							PrintPacketMsg("NAK", 0, packet, size);
							byte responseTo = data[0];
							byte reasonCode = data[1];
							PrintPacketMsg(Tdk.TdkDefines.NakReasonToString(reasonCode),0,packet,size);                            
						}
						break;
					case (byte)0xEE:
						PrintPacketMsg("0xEE", 0, packet, size);
						break;
				}
			}
			else
			{
				PrintPacketMsg("checksum was bad", 0, packet, size);
			}
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
