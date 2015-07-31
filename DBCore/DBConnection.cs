using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using log4net;
using System.Reflection;

namespace DBBridge {

	/// <summary>
	/// Singleton class to create a Database connection
	/// </summary>
	class DBConnection {
		private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static DBConnection dbConnection;
		private MySqlConnection mySQLConnection;

		private DBConnection() {
			Log.Debug("DBBridge.DBConnection Constructor Invoked");

			try {
				string server = "localhost";
				string database = "tic_tac_toe";
				string uid = "admin";
				string password = "password";

				string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
				mySQLConnection = new MySqlConnection(connectionString);
				Log.Info("DBBridge.DB initialized");
			} catch (Exception ex) {
				Log.Error("DBBridge.Init Error : " + ex.Message);
			}
		}

		/// <summary>
		/// Create a DB Connector
		/// </summary>
		/// <returns></returns>
		public static DBConnection GetDBConnection() {
			Log.Debug("DBBridge.GetDBConnection Invoked");

			if (dbConnection == null) {
				Log.Info("DBBridge.Creating new DBConnection");
				dbConnection = new DBConnection();
			}
			return dbConnection;
		}

		/// <summary>
		/// Get a MySQL database connection 
		/// </summary>
		/// <returns></returns>
		public MySqlConnection GetConnection() {
			Log.Debug("DBBridge.GetConnection Invoked");

			return mySQLConnection;
		}
	}
}
