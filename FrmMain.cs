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
		Game ticTacToeGame;

		//Network
		private bool isClient;
		private bool isServer;

		private Server netServer;
		private Client netClient;
		//

		int mode;
		int compDifficulty;

		public FrmMain() {

			InitializeComponent();
			ticTacToeGame = new Game();
			ticTacToeGame.addObserver((Observer)this);
			mode = 1;
			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = true;
			mnuModeHvH.Checked = false;
			compDifficulty = 1;
			mnuDifficultyEasy.Checked = false;
			mnuDifficultyHard.Checked = false;
			mnuDifficultyNormal.Checked = true;

			lblStatusPlayer.Text = "Not Started";

			//network
			isClient = false;
			isServer = false;
			netServer = new Server(this);
			netClient = new Client(this);
		}

		private void mnuExit_Click(object sender, EventArgs e) {
			this.Dispose();
		}

		private void picGridCell_Clicked(object sender, EventArgs e) {
			if (sender is PictureBox) {
				PictureBox p = (PictureBox)sender;
				switch (p.Name) {
					case "picGridCell1":
						ticTacToeGame.humanPlayerInput(0, 0);
						break;
					case "picGridCell2":
						ticTacToeGame.humanPlayerInput(0, 1);
						break;
					case "picGridCell3":
						ticTacToeGame.humanPlayerInput(0, 2);
						break;
					case "picGridCell4":
						ticTacToeGame.humanPlayerInput(1, 0);
						break;
					case "picGridCell5":
						ticTacToeGame.humanPlayerInput(1, 1);
						break;
					case "picGridCell6":
						ticTacToeGame.humanPlayerInput(1, 2);
						break;
					case "picGridCell7":
						ticTacToeGame.humanPlayerInput(2, 0);
						break;
					case "picGridCell8":
						ticTacToeGame.humanPlayerInput(2, 1);
						break;
					default:
						ticTacToeGame.humanPlayerInput(2, 2);
						break;
				}
			} else {
				throw new Exception("Error: call from non picture object!");
			}
		}

		private void mnuModeHvC_Click(object sender, EventArgs e) {
			mode = 1;
			mnuDifficulty.Enabled = true;
			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = true;
			mnuModeHvH.Checked = false;
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e) {
			ticTacToeGame.resetGame();
			if (mode == 0) {
				ticTacToeGame.newHumanPlayer(0, "Player 1");
				ticTacToeGame.newHumanPlayer(1, "Player 2");
			} else if (mode == 1) {
				ticTacToeGame.newHumanPlayer(0, "Player 1");
				if (compDifficulty == 0) {
					ticTacToeGame.newComputerPlayer(1, Difficulty.Easy);
				} else if (compDifficulty == 1) {
					ticTacToeGame.newComputerPlayer(1, Difficulty.Medium);
				} else {
					ticTacToeGame.newComputerPlayer(1, Difficulty.Hard);
				}
			} else {
				if (compDifficulty == 0) {
					ticTacToeGame.newComputerPlayer(0, Difficulty.Easy);
				} else if (compDifficulty == 1) {
					ticTacToeGame.newComputerPlayer(0, Difficulty.Medium);
				} else {
					ticTacToeGame.newComputerPlayer(0, Difficulty.Hard);
				}
				ticTacToeGame.newHumanPlayer(1, "Player 1");
			}
			ticTacToeGame.start();
			updateWindowInfomation();
		}

		private void updateWindowInfomation() {
			lblPlayerMode.Text = ticTacToeGame.getGameMode();
			lblNextPlayer.Text = ticTacToeGame.getCurrentPlayer();
			if (!ticTacToeGame.gameOver())
				lblStatusPlayer.Text = "Playing";
		}

		private void mnuModeHvH_Click(object sender, EventArgs e) {
			mode = 0;
			;
			mnuDifficulty.Enabled = false;
			mnuModeCvH.Checked = false;
			mnuModeHvC.Checked = false;
			mnuModeHvH.Checked = true;
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

		private void mnuModeCvH_Click(object sender, EventArgs e) {
			mode = 2;
			;
			mnuDifficulty.Enabled = true;
			mnuModeCvH.Checked = true;
			mnuModeHvC.Checked = false;
			mnuModeHvH.Checked = false;
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
				lblStatusPlayer.Text = eventArgs + " won";
				drawWinningLine(line);
				GameScoreController.UpdateUserScore(new GameResult(eventArgs, GameFinishState.Won));
				GameScoreController.UpdateUserScore(new GameResult(ticTacToeGame.OtherPlayer(eventArgs), GameFinishState.Lost));
				UpdateUserScores();
			} else {
				lblStatusPlayer.Text = "Draw";
				GameScoreController.UpdateUserScore(new GameResult(eventArgs, GameFinishState.Draw));
				GameScoreController.UpdateUserScore(new GameResult(eventArgs, GameFinishState.Draw));
				UpdateUserScores();
			}
		}

		private void UpdateUserScores() {
			// throw new NotImplementedException();
		}

		private void drawWinningLine(int line) {
			// Graphics g = picGridBoard.CreateGraphics();
			// if (line == 1) g.DrawLine(Pens.Black, new Point(picGridCell1.Location.X, picGridCell1.Location.Y),
			//     new Point(picGridCell3.Location.X + picGridCell.Width, 50));
		}



		private void startServerToolStripMenuItem_Click(object sender, EventArgs e) {
			InititiateServer();
		}

		private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e) {
			ConnectToServer();
		}

		private void disconnectToolStripMenuItem_Click(object sender, EventArgs e) {
			DisconnectNetwork();
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
		private void ConnectToServer() {
			Log.Debug("ConnectToServer Invoked");

			if (this.netClient != null) {
				netClient.ConnectToServer();
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
		private void SendNetworkCommand(String command) {
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

			this.Text += " " + command;
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

	}
}
