using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel {

	/// <summary>
	/// Defines three possible finish states : Won, Lost, Draw
	/// </summary>
	public enum GameFinishState {
		Won,
		Lost,
		Draw
	}

	/// <summary>
	/// Object to transmit and receive the result of a game
	/// </summary>
	public class GameResult {
		/// <summary>
		/// userName of player
		/// </summary>
		public string userName {
			set;
			get;
		}

		/// <summary>
		/// The result of game for the user
		/// </summary>
		public GameFinishState result {
			set;
			get;
		}

		/// <summary>
		/// Constructor with username and result
		/// </summary>
		/// <param name="userName">user name of the player</param>
		/// <param name="result">GameFinishState</param>
		public GameResult(String userName, GameFinishState result) {
			this.userName = userName;
			this.result = result;
		}
	}

}
