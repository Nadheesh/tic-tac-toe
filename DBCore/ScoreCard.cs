using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModel {
	/// <summary>
	/// Object to transmit and receive all the scores of a single user
	/// </summary>
	public class ScoreCard {
		/// <summary>
		/// userName of player
		/// </summary>
		public string userName {
			set;
			get;
		}
		/// <summary>
		/// No of wins for user
		/// </summary>
		public int won {
			set;
			get;
		}

		/// <summary>
		/// No of losses for user
		/// </summary>
		public int lost {
			set;
			get;
		}

		/// <summary>
		/// No of draws for user
		/// </summary>
		public int draw {
			set;
			get;
		}

		/// <summary>
		/// Constructor with all properties input
		/// </summary>
		/// <param name="username">user name of player</param>
		/// <param name="won">no of wins</param>
		/// <param name="lost">no of loses</param>
		/// <param name="draw">no of draws</param>
		public ScoreCard(String userName, int won, int lost, int draw) {
			this.userName = userName;
			this.won = won;
			this.lost = lost;
			this.draw = draw;
		}

		/// <summary>
		/// Constructor with only user name, all properties set to 0
		/// </summary>
		/// <param name="username">user name of player</param>
		public ScoreCard(String userName) {
			this.userName = userName;
			this.won = 0;
			this.lost = 0;
			this.draw = 0;
		}
	}
}
