using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using log4net;
using System.Reflection;

namespace Network {
	public class Server {
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		private FrmMain mainUI;// Reference for the  main screen

		private Thread readClientThread;// Thread for client reading

		private volatile bool isReadingClient;// Loop control variables for client reading thread

		private const int SERVER_PORT = 12790;// Server port

		private NetworkStream serverSocketStream;// TCP  NetworkStream objects for client and server

		private TcpListener tcpListener;
		private TcpClient tcpClient;

		public Server(FrmMain ui) {
			Log.Debug("Server Constructor Invoked");

			this.mainUI = ui;
			this.isReadingClient = true;
		}

		/// <summary>
		/// Start server
		/// </summary>
		public void StartServer() {
			Log.Debug("Server.StartServer Invoked");

			readClientThread = new Thread(new ThreadStart(ReadFromClient));
			readClientThread.Start();
		}

		/// <summary>
		/// Read data sent by the client
		/// </summary>
		private void ReadFromClient() {
			Log.Debug("Server.ReadFromClient Invoked");

			try {
				tcpListener = new TcpListener(SERVER_PORT);
				tcpListener.Start();
				// Thread is blocked until it gets a connection from client
				tcpClient = tcpListener.AcceptTcpClient();
				serverSocketStream = tcpClient.GetStream();

				WriteToClient("Server : Connected");

				byte[] bytes = new byte[1024];
				int bytesReceived = 0;
				serverSocketStream.ReadTimeout = 100;
				isReadingClient = true;

				while (isReadingClient) {
					// Thread is blocked until receives data

					try {
						bytesReceived = serverSocketStream.Read(bytes, 0, bytes.Length);
					} catch {
						isReadingClient = false;
						return;
					}
					// Processe network packet
					if (bytesReceived > 0) {
						String command = Encoding.ASCII.GetString(bytes, 0, bytesReceived);
						Log.Info("Server.Bytes Recieved : " + command);
						//Call the RecieveNetworkCommand(String command) UI of the FrmMain 
						//Ref - mainUI.setNetworkTxt(Encoding.ASCII.GetString(bytes, 0, bytesReceived));
						mainUI.RecieveNetworkCommand(command);
					}
				}
				Log.Info("ReadFromClient - Done reading");
			} catch (Exception ex) {
				Log.Error("Server.An error ocurred : " + ex.Message, ex);
				//mainUI.DisconnectNetwork();
			} finally {
				try {
					if (serverSocketStream != null) {
						serverSocketStream.Close();
						Log.Info("Server.serverSocketStream closed");
					}
					if (tcpClient != null) {
						tcpClient.Close();
						Log.Info("Server.tcpClient closed");
					}
					if (tcpListener != null) {
						tcpListener.Stop();
						Log.Info("Server.tcpListener closed");
					}
				} catch (Exception ex) {
					Log.Error("Server.An error ocurred: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Write to the client
		/// </summary>
		/// <param name="command">The command to be sent</param>
		public void WriteToClient(string command) {
			Log.Debug("Server.WriteToClient Invoked");

			if (serverSocketStream == null) {
				return;
			}
			try {
				if (serverSocketStream.CanWrite) {
					Log.Info("Server.Sending command : " + command);
					byte[] txtByte = Encoding.ASCII.GetBytes(command);
					serverSocketStream.Write(txtByte, 0, txtByte.Length);
					serverSocketStream.Flush();
				}
			} catch (Exception ex) {
				Log.Error("Server.An error ocurred : " + ex.Message, ex);
				mainUI.DisconnectNetwork();
			}
		}

		/// <summary>
		/// Disconnect the connection
		/// </summary>
		public void Disconnect() {
			Log.Debug("Server.Disconnect Invoked");
			try {
				if (readClientThread != null) {
					//readClientThread.Abort();
					isReadingClient = false;
					//readClientThread.Interrupt();
					Log.Info("Server.readClientThread interrupted");
				}
			} catch (Exception ex) {
				Log.Error("Server.An error ocurred : " + ex.Message);
			}
		}
	}
}
