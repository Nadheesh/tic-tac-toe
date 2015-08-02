using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using UI;
using log4net;
using System.Reflection;

namespace Network {
	class Client {
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private FrmMain mainUI = null;// Reference for the  main screen

		private Thread readServerThread;// Thread for server reading

		private const int SERVER_PORT = 55650;// Server port - Index numbers of developers 556 and 650

		private String serverIPAddress;

		private volatile bool isReadingServer = true;// Loop control variables for server reading thread

		private NetworkStream clientSocketStream;// TCP  NetworkStream objects for client and server
		private TcpClient tcpClient;

		public Client(FrmMain ui) {
			Log.Debug("Client Constructor Invoked");
			this.mainUI = ui;
		}

		/// <summary>
		/// Connect to server
		/// </summary>
		/// <param name="serverIP">IP address of the server</param>
		public void ConnectToServer(String serverIPAddress) {
			Log.Debug("Client.ConnectToServer Invoked");

			this.serverIPAddress = serverIPAddress;
			// Connect to server
			readServerThread = new Thread(new ThreadStart(ReadFromServer));
			readServerThread.Start();
		}


		/// <summary>
		/// Get the Default gateway address of Ethernet port
		/// </summary>
		/// <returns></returns>
		private IPAddress GetDefaultGateway() {
			Log.Debug("Client.GetDefaultGateway Invoked");

			IPAddress result = null;
			var cards = NetworkInterface.GetAllNetworkInterfaces().ToList();
			if (cards.Any()) {
				foreach (var card in cards) {
					var props = card.GetIPProperties();
					if (props == null)
						continue;

					var gateways = props.GatewayAddresses;
					if (!gateways.Any())
						continue;

					var gateway =
						gateways.FirstOrDefault(g => g.Address.AddressFamily.ToString() == "InterNetwork");
					if (gateway == null)
						continue;
					result = gateway.Address;
					break;
				};
			}
			Log.Info("Client.Default gateway : " + result);
			return result;
		}

		/// <summary>
		/// Thread for receiving packets from server
		/// </summary>
		private void ReadFromServer() {
			Log.Debug("Client.ReadFromServer Invoked");

			try {
				Log.Info("Client.ReadFromServer Connecting to ServerIP - " + serverIPAddress + " ,Port - " + SERVER_PORT);
				tcpClient = new TcpClient(serverIPAddress, SERVER_PORT);
				clientSocketStream = tcpClient.GetStream();

				WriteToServer("Client : Connected");
				//mainUI.setStatusMessage("Connected to server");

				byte[] bytes = new byte[1024];
				int bytesReceived = 0;
				//clientSocketStream.ReadTimeout = 100;
				isReadingServer = true;

				while (isReadingServer) {
					// Thread is blocked until receives data
					try {
						bytesReceived = clientSocketStream.Read(bytes, 0, bytes.Length);
					} catch {
						isReadingServer = false;
						return;
					}
					// Processes network packet
					if (bytesReceived > 0) {
						//Call the RecieveNetworkCommand(String command) UI of the FrmMain
						//Ref - mainUI.setNetworkTxt(Encoding.ASCII.GetString(bytes, 0, bytesReceived));
						String command = Encoding.ASCII.GetString(bytes, 0, bytesReceived);
						Log.Info("Client.Bytes Recieved : " + command);
						mainUI.RecieveNetworkCommand(command);
					}
				}
				Log.Info("ReadFromServer - Done reading");
			} catch (Exception ex) {
				Log.Error("Client.ReadFromServer error ocurred: " + ex.Message, ex);
			}
		}

		/// <summary>
		/// Write to the server
		/// </summary>
		/// <param name="command">The text to be sent</param>
		public void WriteToServer(string command) {
			Log.Debug("Client.WriteToServer Invoked");

			if (clientSocketStream == null) {
				return;
			}
			try {
				if (clientSocketStream.CanWrite) {
					Log.Info("Client.Sending command : " + command);
					byte[] txtByte = Encoding.ASCII.GetBytes(command);
					clientSocketStream.Write(txtByte, 0, txtByte.Length);
					clientSocketStream.Flush();
				}
			} catch (Exception ex) {
				Log.Error("Client.WriteToServer error ocurred: " + ex.Message, ex);
				mainUI.DisconnectNetwork();
			}
		}

		/// <summary>
		/// Disconnect the connection
		/// </summary>
		public void Disconnect() {
			Log.Debug("Client.Disconnect Invoked");
			try {
				if (readServerThread != null) {
					//readServerThread.Abort();
					//readServerThread.Interrupt();
					isReadingServer = false;

					Log.Info("Client.readServerThread interrupted");
				}
				if (clientSocketStream != null) {
					clientSocketStream.Close();
					Log.Info("Client.clientSocketStream closed");
				}
				if (tcpClient != null) {
					tcpClient.Close();
					Log.Info("Client.tcpClient closed");
				}
			} catch (Exception ex) {
				Log.Error("Client.Disconnect error ocurred: " + ex.Message, ex);
			}

		}
	}
}
