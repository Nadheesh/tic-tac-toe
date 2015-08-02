using System;
using System.Drawing;
using System.Windows.Forms;

using TicTacToe;
using DBController;
using DBModel;
using Network;
using log4net;
using System.Reflection;

namespace UI {
	public partial class FrmMain : Form, TicTacToeBoardObserver, TicTacToeGameObserver {
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public Pen DEFAULT_PEN = null;

		Game ticTacToeGame;


		private int DrawLine {
			get;
			set;
		}

		//Network
		private bool isClient;
		private bool isServer;

		private Server netServer;
		private Client netClient;
		//

		int mode;
		int compDifficulty;

		public string[] PlayerNames {
			get;
			set;
		}

		public string NetworkName {
			get;
			set;
		}

		public bool NoRequest {
			get;
			set;
		}
		public FrmMain() {
			DEFAULT_PEN = new Pen(Brushes.Orange, 5);
			InitializeComponent();
			ticTacToeGame = new Game();
			ticTacToeGame.addObserver((Observer)this);
			mode = 1;
			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = true;
			mnuModeHvH.Checked = false;
			mnuModeNetwork.Checked = false;
			compDifficulty = 1;
			mnuDifficultyEasy.Checked = false;
			mnuDifficultyHard.Checked = false;
			mnuDifficultyNormal.Checked = true;

			picGridBoard.BackColor = Color.Transparent;

			NoRequest = true;

			lblStatusPlayer.Text = "Not Started";

			PlayerNames = new string[2] { "P1", "P2" };

			DrawLine = 0;

			//network
			isClient = false;
			isServer = false;
			netServer = new Server(this);
			netClient = new Client(this);

			graphicsOverlay.Paint += graphicsOverlay_Paint;

			graphicsOverlay.Owner = this;

			UpdatePlayerDetails();
		}

		void graphicsOverlay_Paint(object sender, PaintEventArgs e) {
			switch (DrawLine) {

				case 1:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell1), CenterOf(picGridCell3));
					break;

				case 2:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell4), CenterOf(picGridCell6));
					break;

				case 3:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell7), CenterOf(picGridCell9));
					break;

				case 4:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell1), CenterOf(picGridCell7));
					break;

				case 5:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell2), CenterOf(picGridCell8));
					break;

				case 6:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell3), CenterOf(picGridCell9));
					break;

				case 7:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell1), CenterOf(picGridCell9));
					break;

				case 8:
					e.Graphics.DrawLine(DEFAULT_PEN, CenterOf(picGridCell3), CenterOf(picGridCell7));
					break;

				default:
					break;

			}
		}

		public Point CenterOf(PictureBox p) {
			Point l = p.Location;
			Size s = p.Size;
			return new Point(l.X + s.Width / 2, l.Y + s.Height / 2);
		}

		private void mnuExit_Click(object sender, EventArgs e) {
			this.Dispose();
		}

		private void mnuRestart_Click(object sender, EventArgs e) {
			System.Diagnostics.Process.Start(Assembly.GetExecutingAssembly().Location);
			Environment.Exit(0);
		}


		private void aboutToolStripMenuItem1_Click(object sender, EventArgs e) {
			AboutBox AboutBox = new AboutBox();
			AboutBox.ShowDialog();
		}

		private void picGridCell_Clicked(object sender, EventArgs e) {
			if (sender is PictureBox) {
				PictureBox p = (PictureBox)sender;
				switch (p.Name) {
					case "picGridCell1":
						PlayCell("", 0, 0);
						break;
					case "picGridCell2":
						PlayCell("", 0, 1);
						break;
					case "picGridCell3":
						PlayCell("", 0, 2);
						break;
					case "picGridCell4":
						PlayCell("", 1, 0);
						break;
					case "picGridCell5":
						PlayCell("", 1, 1);
						break;
					case "picGridCell6":
						PlayCell("", 1, 2);
						break;
					case "picGridCell7":
						PlayCell("", 2, 0);
						break;
					case "picGridCell8":
						PlayCell("", 2, 1);
						break;
					default:
						PlayCell("", 2, 2);
						break;
				}
			} else {
				throw new Exception("Error: call from non picture object!");
			}
		}

		private void PlayCell(string s, int row, int column) {
			if (this.mode == 3) {
				if (s == ticTacToeGame.GetCurrentPlayer())
					ticTacToeGame.HumanPlayerInput(row, column);
				else if (s == "" && NetworkName == ticTacToeGame.GetCurrentPlayer()) {
					ticTacToeGame.HumanPlayerInput(row, column);
					SendNetworkCommand(row + " " + column);
				}
			} else
				ticTacToeGame.HumanPlayerInput(row, column);

		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e) {
			AddNewPlayer();
		}

		internal void AddNewPlayer() {
			this.Enabled = false;
			FrmSelectPlayer temp = new FrmSelectPlayer(this);
			temp.Location = this.Location + new Size(this.Size.Width / 2, this.Size.Height / 2) - new Size(temp.Size.Width / 2, temp.Size.Height / 2);
			temp.Visible = true;
		}

		internal void StartNewGame() {
			this.DrawLine = 0;
			Refresh();

			ticTacToeGame.ResetGame();
			if (mode == 0) {
				ticTacToeGame.NewHumanPlayer(0, PlayerNames[0]);
				GameScoreController.AddNewUser(PlayerNames[0]);

				ticTacToeGame.NewHumanPlayer(1, PlayerNames[1]);
				GameScoreController.AddNewUser(PlayerNames[1]);
			} else if (mode == 1) {
				ticTacToeGame.NewHumanPlayer(0, PlayerNames[0]);
				GameScoreController.AddNewUser(PlayerNames[0]);
				if (compDifficulty == 0) {
					ticTacToeGame.NewComputerPlayer(1, Difficulty.Easy);
				} else if (compDifficulty == 1) {
					ticTacToeGame.NewComputerPlayer(1, Difficulty.Medium);
				} else {
					ticTacToeGame.NewComputerPlayer(1, Difficulty.Hard);
				}
				GameScoreController.AddNewUser("Computer");
			} else if (mode == 2) {
				if (compDifficulty == 0) {
					ticTacToeGame.NewComputerPlayer(0, Difficulty.Easy);
				} else if (compDifficulty == 1) {
					ticTacToeGame.NewComputerPlayer(0, Difficulty.Medium);
				} else {
					ticTacToeGame.NewComputerPlayer(0, Difficulty.Hard);
				}
				GameScoreController.AddNewUser("Computer");

				ticTacToeGame.NewHumanPlayer(1, PlayerNames[1]);
				GameScoreController.AddNewUser(PlayerNames[1]);
			} else {
				ticTacToeGame.NewHumanPlayer(0, PlayerNames[0]);
				GameScoreController.AddNewUser(PlayerNames[0]);

				ticTacToeGame.NewHumanPlayer(1, PlayerNames[1]);
				GameScoreController.AddNewUser(PlayerNames[1]);
			}
			ticTacToeGame.Start();
			this.UpdatePlayerDetails();
			updateWindowInfomation();
		}



		private void mnuHighscores_Click(object sender, EventArgs e) {
			this.Enabled = false;
			FrmHighscores temp = new FrmHighscores(this);
			temp.Location = this.Location + new Size(this.Size.Width / 2, this.Size.Height / 2) - new Size(temp.Size.Width / 2, temp.Size.Height / 2);
			temp.Visible = true;
		}

		private void updateWindowInfomation() {
			lblPlayerMode.Text = ticTacToeGame.GetGameMode();
			lblNextPlayer.Text = ticTacToeGame.GetCurrentPlayer();
			if (!ticTacToeGame.GameOver())
				lblStatusPlayer.Text = "Playing";
		}

		private void mnuDifficultyEasy_Click(object sender, EventArgs e) {
			compDifficulty = 0;
			mnuDifficultyEasy.Checked = true;
			mnuDifficultyHard.Checked = false;
			mnuDifficultyNormal.Checked = false;
		}

		private void mnuDifficultyNormal_Click(object sender, EventArgs e) {
			compDifficulty = 1;
			mnuDifficultyEasy.Checked = false;
			mnuDifficultyHard.Checked = false;
			mnuDifficultyNormal.Checked = true;
		}

		private void mnuDifficultyHard_Click(object sender, EventArgs e) {
			compDifficulty = 2;
			mnuDifficultyEasy.Checked = false;
			mnuDifficultyHard.Checked = true;
			mnuDifficultyNormal.Checked = false;
		}

		private void mnuModeHvH_Click(object sender, EventArgs e) {
			mode = 0;
			mnuDifficulty.Enabled = false;

			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = false;
			mnuModeHvH.Checked = true;
			mnuModeNetwork.Checked = false;

			DisableNetworkPlay();
		}
		private void mnuModeNetwork_Click(object sender, EventArgs e) {
			mode = 3;
			mnuDifficulty.Enabled = false;

			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = false;
			mnuModeHvH.Checked = false;
			mnuModeNetwork.Checked = true;

			//Enable network settings
			networkToolStripMenuItem.Enabled = true;

			//Disable start new game
			newToolStripMenuItem.Enabled = false;

		}

		private void mnuModeCvH_Click(object sender, EventArgs e) {
			mode = 2;
			mnuDifficulty.Enabled = true;

			mnuModeCvH.Checked = true;
			mnuModeHvC.Checked = false;
			mnuModeHvH.Checked = false;
			mnuModeNetwork.Checked = false;

			DisableNetworkPlay();
		}

		private void mnuModeHvC_Click(object sender, EventArgs e) {
			mode = 1;
			mnuDifficulty.Enabled = true;

			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = true;
			mnuModeHvH.Checked = false;
			mnuModeNetwork.Checked = false;

			DisableNetworkPlay();
		}

		private void DisableNetworkPlay() {
			//Disable network settings
			DisconnectNetwork();
			networkToolStripMenuItem.Enabled = false;

			disconnectToolStripMenuItem.Enabled = false;
			startServerToolStripMenuItem.Enabled = true;
			connectToServerToolStripMenuItem.Enabled = true;

			startServerToolStripMenuItem.Checked = false;
			connectToServerToolStripMenuItem.Checked = false;

			//Enable start new game
			newToolStripMenuItem.Enabled = true;
		}


		/////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// 
		/// </summary>
		/// <param name="board"></param>
		public void boardCellChanged(Marker[][] board) {
			// Changes UI Board
			for (int i = 0; i < 3; i++) {
				for (int j = 0; j < 3; j++) {
					Image cellValue = null;
					switch (board[i][j]) {
						case Marker.Cross:
							cellValue = global::TicTacToe.Properties.Resources.X;
							break;
						case Marker.Nought:
							cellValue = global::TicTacToe.Properties.Resources.O;
							break;
						default:
							break;
					}
					switch (i) {
						case 0:
							switch (j) {
								case 0:
									picGridCell1.Image = cellValue;
									break;
								case 1:
									picGridCell2.Image = cellValue;
									break;
								case 2:
									picGridCell3.Image = cellValue;
									break;
								default:
									break;
							}
							break;
						case 1:
							switch (j) {
								case 0:
									picGridCell4.Image = cellValue;
									break;
								case 1:
									picGridCell5.Image = cellValue;
									break;
								case 2:
									picGridCell6.Image = cellValue;
									break;
								default:
									break;
							}
							break;
						case 2:
							switch (j) {
								case 0:
									picGridCell7.Image = cellValue;
									break;
								case 1:
									picGridCell8.Image = cellValue;
									break;
								case 2:
									picGridCell9.Image = cellValue;
									break;
								default:
									break;
							}
							break;
						default:
							break;
					}
				}
				updateWindowInfomation();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="marker"></param>
		public void boardCleared(Marker[][] marker) {
			boardCellChanged(marker);
		}

		/// <summary>
		/// This is a state event of game
		/// </summary>
		/// <param name="eventArgs">Empty if game is drawn</param>
		public void gameOver(String eventArgs, int line) {
			if (eventArgs != "") {
				this.lblStatusPlayer.Text = eventArgs + " won";
				this.DrawLine = line;
				this.Refresh();
				GameScoreController.UpdateUserScore(new GameResult(eventArgs, GameFinishState.Won));
				GameScoreController.UpdateUserScore(new GameResult(ticTacToeGame.OtherPlayer(eventArgs), GameFinishState.Lost));
			} else {
				lblStatusPlayer.Text = "Draw";
				GameScoreController.UpdateUserScore(new GameResult(ticTacToeGame.OtherPlayer(eventArgs), GameFinishState.Draw));
				GameScoreController.UpdateUserScore(new GameResult(ticTacToeGame.OtherPlayer(ticTacToeGame.OtherPlayer(eventArgs)), GameFinishState.Draw));
			}
			this.UpdatePlayerDetails();
		}

		private void startServerToolStripMenuItem_Click(object sender, EventArgs e) {
			InititiateServer();

			startServerToolStripMenuItem.Checked = true;
			connectToServerToolStripMenuItem.Checked = false;

			//Disable connecting and enable disconnecting
			startServerToolStripMenuItem.Enabled = false;
			connectToServerToolStripMenuItem.Enabled = false;
			disconnectToolStripMenuItem.Enabled = true;

			//Enable new game
			newToolStripMenuItem.Enabled = true;
		}

		private bool IsIPv4(string value) {
			var quads = value.Split('.');

			// if we do not have 4 quads, return false
			if (!(quads.Length == 4))
				return false;

			// for each quad
			foreach (var quad in quads) {
				int q;
				// if parse fails 
				// or length of parsed int != length of quad string (i.e.; '1' vs '001')
				// or parsed int < 0
				// or parsed int > 255
				// return false
				if (!Int32.TryParse(quad, out q)
					|| !q.ToString().Length.Equals(quad.Length)
					|| q < 0
					|| q > 255) {
					return false;
				}

			}

			return true;
		}
		private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e) {

			String serverIPAddress = Microsoft.VisualBasic.Interaction.InputBox("What is the server Name/IP?", "Server name", "", -1, -1);
			///Validate ip address
			if (serverIPAddress.Trim() == "") {
				MessageBox.Show("IP cannot be empty", "Tic Tac Toe", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			if (!IsIPv4(serverIPAddress)) {
				MessageBox.Show("Invalid IP address", "Tic Tac Toe", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			ConnectToServer(serverIPAddress);

			startServerToolStripMenuItem.Checked = false;
			connectToServerToolStripMenuItem.Checked = true;

			//Disable connecting and enable disconnecting
			startServerToolStripMenuItem.Enabled = false;
			connectToServerToolStripMenuItem.Enabled = false;
			disconnectToolStripMenuItem.Enabled = true;

			//Enable new game
			newToolStripMenuItem.Enabled = true;
		}

		private void disconnectToolStripMenuItem_Click(object sender, EventArgs e) {
			DisconnectNetwork();

			//Disable disconnecting and enable connecting
			startServerToolStripMenuItem.Checked = false;
			connectToServerToolStripMenuItem.Checked = false;
			startServerToolStripMenuItem.Enabled = true;
			connectToServerToolStripMenuItem.Enabled = true;
			disconnectToolStripMenuItem.Enabled = false;

			//Enable new game
			newToolStripMenuItem.Enabled = false;
		}
		///////////////////////////////////Network Methods/////////////////////////////////////////////

		/// <summary>
		/// Start server
		/// </summary>
		private void InititiateServer() {
			Log.Debug("InititiateServer Invoked");

			if (!isClient && !isServer) {
				if (this.netServer != null) {
					netServer.StartServer();
					isServer = true;
					Log.Info("Tic tac toe : Server");
				} else {
					Log.Error("Null netServer");
				}
			}
		}

		/// <summary>
		/// Connect to server
		/// </summary>
		private void ConnectToServer(String serverIPAddress) {
			Log.Debug("ConnectToServer Invoked");

			if (this.netClient != null) {
				netClient.ConnectToServer(serverIPAddress);
				isClient = true;
				Log.Info("Tic tac toe : Client");
			} else {
				Log.Error("Null netClient");
			}
		}

		/// <summary>
		/// Send command
		/// </summary>
		/// <param name="command">The network command to be sent</param>
		public void SendNetworkCommand(String command) {
			Log.Debug("SendNetworkCommand Invoked");

			if (isServer && netServer != null) {
				netServer.WriteToClient(command);
				Log.Info("Server -> Client : " + command);
			} else if (isClient && netClient != null) {
				netClient.WriteToServer(command);
				Log.Info("Client -> Server : " + command);
			} else {
				Log.Error("Connection Error");
				MessageBox.Show("No connection", "Error", MessageBoxButtons.OK);
			}
		}

		/// <summary>
		/// Method to be called from server or client read threads
		/// </summary>
		public void RecieveNetworkCommand(String command) {
			Log.Debug("RecieveNetworkCommand Invoked");

			//this.Text += " " + command;

			String[] s = command.Split(new Char[] { ' ' });

			if (s.Length == 2) {
				Log.Info("s[0]= " + s[0]);
				Log.Info("s[1]= " + s[1]);
				if (s[0] == "Request") {
					PlayerNames[0] = s[1];

					this.mode = 3;

					this.NoRequest = false;

					this.Invoke((MethodInvoker)delegate {
						PlayFromNetwork();
					});

				} else if (s[0] == "Accept") {
					PlayerNames[1] = s[1];

					this.Invoke((MethodInvoker)delegate {
						this.StartNewGame();
					});

				} else {
					int column = int.Parse(s[1]);
					int row = int.Parse(s[0]);

					this.Invoke((MethodInvoker)delegate {
						this.PlayCell(ticTacToeGame.OtherPlayer(NetworkName), row, column);
					});

				}
			}
		}

		/// <summary>
		/// Start game in network mode 
		/// </summary>
		private void PlayFromNetwork() {
			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = false;
			mnuModeHvH.Checked = false;
			mnuModeNetwork.Checked = true;
			AddNewPlayer();
		}

		/// <summary>
		/// Diconnect from network
		/// </summary>
		public void DisconnectNetwork() {
			Log.Debug("DisconnectNetwork Invoked");

			if (isServer && netServer != null) {
				Log.Info("Server disconnected");
				netServer.Disconnect();
			} else if (isClient && netClient != null) {
				Log.Info("Client disconnected");
				netClient.Disconnect();
			}
			isServer = false;
			isClient = false;
			this.Text = "Tic tac toe";
		}

		public int GameMode {
			get {
				return this.mode;
			}
		}

		private void FrmMain_Load(object sender, EventArgs e) {

		}

		public void UpdatePlayerDetails() {
			String playerName1 = ticTacToeGame.GetCurrentPlayer();
			String playerName2 = ticTacToeGame.OtherPlayer(playerName1);
			lblPlayer1.Text = playerName1;
			lblPlayer2.Text = playerName2;
			ScoreCard sc1 = DBController.GameScoreController.GetUserScore(playerName1);
			ScoreCard sc2 = DBController.GameScoreController.GetUserScore(playerName2);
			if (sc1 != null) {
				lblPlayer1Draws.Text = sc1.draw.ToString();
				lblPlayer1Losses.Text = sc1.lost.ToString();
				lblPlayer1Wins.Text = sc1.won.ToString();
			} else {
				lblPlayer1Draws.Text = "";
				lblPlayer1Losses.Text = "";
				lblPlayer1Wins.Text = "";
			}

			if (sc2 != null) {
				lblPlayer2Draws.Text = sc2.draw.ToString();
				lblPlayer2Losses.Text = sc2.lost.ToString();
				lblPlayer2Wins.Text = sc2.won.ToString();
			} else {
				lblPlayer2Draws.Text = "";
				lblPlayer2Losses.Text = "";
				lblPlayer2Wins.Text = "";
			}

		}

		private void lblPlayer1Wins_Click(object sender, EventArgs e) {

		}

		private void lblPlayer1Draws_Click(object sender, EventArgs e) {

		}

		private void lblPlayer1Losses_Click(object sender, EventArgs e) {

		}


	}
}
