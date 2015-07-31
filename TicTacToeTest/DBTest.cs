using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBController;
using DBModel;

namespace TicTacToeTest {
	/// <summary>
	/// Summary description for DBTest
	/// </summary>
	[TestClass]
	public class DBTest {
		//[TestMethod]
		public void initilizeDBHndler() {
			//GameScoreController DBTest = new GameScoreController();
		}

		[TestMethod]
		public void addUser() {
            bool result = GameScoreController.AddNewUser("jnj");
			Console.WriteLine(result? "user added":"adding user failed");
		}
		[TestMethod]
		public void updateScore() {
			bool result = GameScoreController.UpdateUserScore(new GameResult("jnj", GameFinishState.Won));
			Console.WriteLine(result?"update successfull":"update failed");
		}

		[TestMethod]
		public void getScore() {
			ScoreCard card = GameScoreController.GetUserScore("jnj");
			Console.WriteLine("User Name : " + card.userName + " | Won  : " + card.won + " | Lost : " + card.lost + " | Draw : " + card.draw);
		}

		[TestMethod]
		public void getAllScores() {
			ArrayList scores = GameScoreController.GetAllScores();
			foreach (ScoreCard userScore in scores) {
				Console.WriteLine("User Name : " + userScore.userName + " | Won  : " + userScore.won + " | Lost : " + userScore.lost + " | Draw : " + userScore.draw);
			}
		}
        
		//[TestMethod]
		public void deleteUser() {
			bool result = GameScoreController.DeleteUser("abc");
			Console.WriteLine(result ? "delete successfull" : "delete failed");
		}

		//[TestMethod]
		public void deleAll() {
			bool result = GameScoreController.DeleteAll();
			Console.WriteLine(result ? "delete all successfull" : "delete all failed");
		}
	}
}
