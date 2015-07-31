using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using MySql.Data.MySqlClient;
using DBModel;
using DBBridge;
using log4net;
using System.Reflection;

namespace DBController {

	/// <summary>
	/// Static class to interact with the MySQl database
	/// </summary>
	public static class GameScoreController {
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// Open a connection to database
		/// </summary>
		/// <param name="MySQLConnection">The DB connector to use</param>
		/// <returns></returns>
		private static bool OpenConnection(MySqlConnection MySQLConnection) {
			try {
				MySQLConnection.Open();
				Log.Info("GameScoreController.DB Connected");
				return true;
			} catch (MySqlException ex) {
				//0: Cannot connect to server.
				//1045: Invalid user name and/or password.
				switch (ex.Number) {
					case 0:
						Log.Error("GameScoreController.DB Connect Error : Contact administrator");
						break;
					case 1045:
						Log.Error("GameScoreControllerDB Connect Error : Invalid username/password");
						break;
				}
				return false;
			}
		}

		/// <summary>
		///  Close connection
		/// </summary>
		/// <param name="MySQLConnection">The DB connector to use</param>
		/// <returns></returns>
		private static bool CloseConnection(MySqlConnection MySQLConnection) {
			try {
				MySQLConnection.Close();
				Log.Info("GameScoreController.DB Closed");
				return true;
			} catch (MySqlException ex) {
				Log.Error("GameScoreController.DB Connect Error : " + ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Insert new user to default table
		/// </summary>
		/// <param name="userName">New user name of the player. Must be unique and non empty</param>
		/// <returns></returns>
		public static bool AddNewUser(String userName) {
			if (userName == null) {
				return false;
			}
			MySqlConnection MySQLConnection = DBConnection.GetDBConnection().GetConnection();
			if (OpenConnection(MySQLConnection)) {
				try {
					String query = "INSERT INTO score (user_name) VALUES(@user)";
					MySqlCommand cmd = new MySqlCommand(query, MySQLConnection);
					cmd.Prepare();
					cmd.Parameters.AddWithValue("@user", userName);
					return cmd.ExecuteNonQuery() == 1;
				} catch (MySqlException ex) {
					Log.Error("GameScoreController.DB Query Error : " + ex.Message);
				} finally {
					CloseConnection(MySQLConnection);
				}
			}
			return false;
		}

		/// <summary>
		/// Get score of an user
		/// </summary>
		/// <param name="username">User name of the player whose scores are needed</param>
		/// <returns>A score card containing user scores</returns>
		public static ScoreCard GetUserScore(String userName) {
			if (userName == null) {
				return null;
			}
			MySqlConnection MySQLConnection = DBConnection.GetDBConnection().GetConnection();
			if (OpenConnection(MySQLConnection)) {
				MySqlDataReader dataReader = null;
				try {
					String query = "SELECT * FROM score WHERE user_name=@user";
					MySqlCommand cmd = new MySqlCommand(query, MySQLConnection);
					cmd.Prepare();
					cmd.Parameters.AddWithValue("@user", userName);

					dataReader = cmd.ExecuteReader();
					if (!dataReader.HasRows) {
						throw new Exception("Empty Set");
					}
					ScoreCard userScore = new ScoreCard(userName);
					while (dataReader.Read()) {
						userScore.won = int.Parse(dataReader["won"].ToString());
						userScore.lost = int.Parse(dataReader["lost"].ToString());
						userScore.draw = int.Parse(dataReader["draw"].ToString());
					}

					return userScore;
				} catch (MySqlException ex) {
					Log.Error("GameScoreController.DB Query Error : " + ex.Message);
				} catch (Exception err) {
					Log.Error("GameScoreController.DB Query Error : " + err.Message);
				} finally {
					if (dataReader != null) {
						dataReader.Close();
					}
					CloseConnection(MySQLConnection);
				}
			}
			return null;
		}

		/// <summary>
		/// Get a score sheet for all users
		/// </summary>
		/// <returns>An array list of ScoreCard/s</returns>
		public static ArrayList GetAllScores() {
			MySqlConnection MySQLConnection = DBConnection.GetDBConnection().GetConnection();
			if (OpenConnection(MySQLConnection)) {
				MySqlDataReader dataReader = null;
				try {
					String query = "SELECT * FROM score";
					MySqlCommand cmd = new MySqlCommand(query, MySQLConnection);
					dataReader = cmd.ExecuteReader();
					if (!dataReader.HasRows) {
						throw new Exception("Empty Set");
					}
					ArrayList scoreSheet = new ArrayList();
					while (dataReader.Read()) {

						ScoreCard scoreCard = new ScoreCard(
							dataReader["user_name"].ToString(),
							int.Parse(dataReader["won"].ToString()),
							int.Parse(dataReader["lost"].ToString()),
							int.Parse(dataReader["draw"].ToString())
						);

						scoreSheet.Add(scoreCard);
					}

					return scoreSheet;
				} catch (MySqlException ex) {
					Log.Error("GameScoreController.DB Query Error : " + ex.Message);
				} catch (Exception err) {
					Log.Error("GameScoreController.DB Query Error : " + err.Message);
				} finally {
					if (dataReader != null) {
						dataReader.Close();
					}
					CloseConnection(MySQLConnection);
				}
			}
			return null;
		}

		/// <summary>
		/// Update score of a player
		/// </summary>
		/// <param name="gameResult">The result of a game for a user</param>
		/// <returns>Returns whether the user score was updated</returns>
		public static bool UpdateUserScore(GameResult gameResult) {
			if (gameResult == null) {
				return false;
			}
			MySqlConnection MySQLConnection = DBConnection.GetDBConnection().GetConnection();
			if (OpenConnection(MySQLConnection)) {
				try {
					String query = null;
					switch (gameResult.result) {

						case GameFinishState.Won:
							Log.Info("GameScoreController.Won Query");
							query = "UPDATE score SET won=won+1  WHERE user_name=@user";
							break;
						case GameFinishState.Lost:
							Log.Info("GameScoreController.Lost Query");
							query = "UPDATE score SET lost=lost+1  WHERE user_name=@user";
							break;
						case GameFinishState.Draw:
							Log.Info("GameScoreController.Draw Query");
							query = "UPDATE score SET draw=draw+1  WHERE user_name=@user";
							break;
						default:
							Log.Error("No matching Query found for : " + gameResult.result);
							return false;
					}
					MySqlCommand cmd = new MySqlCommand(query, MySQLConnection);
					cmd.Prepare();
					cmd.Parameters.AddWithValue("@user", gameResult.userName);
					return cmd.ExecuteNonQuery() == 1;

				} catch (MySqlException ex) {
					Log.Error("GameScoreController.DB Query Error : " + ex.Message);
				} finally {
					CloseConnection(MySQLConnection);
				}

			}
			return false;
		}

		/// <summary>
		/// Delete an user
		/// </summary>
		/// <param name="userName">user name to be deleted</param>
		/// <returns>Returns whether the user was deleted</returns>
		public static bool DeleteUser(String userName) {
			if (userName == null) {
				return false;
			}
			MySqlConnection MySQLConnection = DBConnection.GetDBConnection().GetConnection();
			if (OpenConnection(MySQLConnection)) {
				try {
					String query = "DELETE FROM score WHERE user_name=@user";
					MySqlCommand cmd = new MySqlCommand(query, MySQLConnection);
					cmd.Prepare();
					cmd.Parameters.AddWithValue("@user", userName);
					return cmd.ExecuteNonQuery() == 1;
				} catch (MySqlException ex) {
					Log.Error("GameScoreController.DB Query Error : " + ex.Message);
				} finally {
					CloseConnection(MySQLConnection);
				}
			}
			return false;
		}

		/// <summary>
		/// Delete all users. Warning : all records will be deleted
		/// </summary>
		/// <returns>Return if all users were deleted</returns>
		public static bool DeleteAll() {
			MySqlConnection MySQLConnection = DBConnection.GetDBConnection().GetConnection();
			if (OpenConnection(MySQLConnection)) {
				try {
					String query = "DELETE FROM score";
					MySqlCommand cmd = new MySqlCommand(query, MySQLConnection);
					return cmd.ExecuteNonQuery() > 0;
				} catch (MySqlException ex) {
					Log.Error("GameScoreController.DB Query Error : " + ex.Message);
				} finally {
					CloseConnection(MySQLConnection);
				}
			}
			return false;
		}
	}
}