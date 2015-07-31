using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DBController;
using DBModel;

namespace TicTacToeTest {

	[TestClass]
	 public class DBTest {

		/// <summary>
		/// Test when null passed
		/// </summary>
		[TestMethod]
		public void AddUser_Null_Test() {
			String inUser = null;
			bool expected = false;

			bool actual = GameScoreController.AddNewUser(inUser);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Add new non empty ,non existing user
		/// </summary>
		[TestMethod]
		public void AddUser_Positive_Test() {
			String inUser = "jnj";
			bool expected = true;

			bool actual = GameScoreController.AddNewUser(inUser);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Test when existing user passed
		/// </summary>
		[TestMethod]
		public void AddUser_Negetive_Test() {
			//
			GameScoreController.AddNewUser("abs");
			GameScoreController.AddNewUser("str");
			//

			String inUser = "str";
			bool expected = false;

			bool actual = GameScoreController.AddNewUser(inUser);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Test when null object is passed
		/// </summary>
		[TestMethod]
		public void UpdateScore_Null_Test() {
			GameResult inGameResult = null;
			bool expected = false;

			bool actual = GameScoreController.UpdateUserScore(inGameResult);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Test for update when existing user score is updated
		/// </summary>
		[TestMethod]
		public void UpdateScore_Positive_Test() {
			GameResult inGameResult = new GameResult("jnj", GameFinishState.Won);
			bool expected = true;

			bool actual = GameScoreController.UpdateUserScore(inGameResult);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Test when trying to update non existing user score
		/// </summary>
		[TestMethod]
		public void UpdateScore_Negetive_Test() {
			GameResult inGameResult = new GameResult("pqr", GameFinishState.Won);
			bool expected = false;

			bool actual = GameScoreController.UpdateUserScore(inGameResult);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Score of null
		/// </summary>
		[TestMethod]
		public void GetScore_Null_Test() {
			String inUser = null;
			ScoreCard expected= null;
			
			ScoreCard actual = GameScoreController.GetUserScore(inUser);

			Assert.AreEqual(expected, actual);
			
		}

		/// <summary>
		/// Score of existing user
		/// </summary>
		[TestMethod]
		public void GetScore_Positive_Test() {
			String inUser = "jnj";
			String expectedUser = "jnj";
			int expectedWon=1;
			int expectedLost=0;
			int expectedDraw=0;

			ScoreCard actual = GameScoreController.GetUserScore(inUser);

			Assert.AreEqual(expectedUser, actual.userName);
			Assert.AreEqual(expectedWon, actual.won);
			Assert.AreEqual(expectedLost, actual.lost);
			Assert.AreEqual(expectedDraw, actual.draw);
		}

		/// <summary>
		/// Score of non existing user
		/// </summary>
		[TestMethod]
		public void GetScore_Negetive_Test() {
			String inUser = "pqr";
			ScoreCard expected = null;

			ScoreCard actual = GameScoreController.GetUserScore(inUser);

			Assert.AreEqual(expected, actual);

		}

		/// <summary>
		/// Test if the get all score method return a not null arraylist object reference 
		/// </summary>
		[TestMethod]
		public void GetAllScore_Positive_Test() {
			ArrayList notExpected = null;

			ArrayList actual = GameScoreController.GetAllScores();

			Assert.AreNotEqual(notExpected, actual);
		}

		/// <summary>
		/// Try to delete null
		/// </summary>
		[TestMethod]
		public void DeleteUser_Null_Test() {
			String inUser = null;
			bool expected = false;

			bool actual = GameScoreController.DeleteUser(inUser);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Try to delete existing user
		/// </summary>
		[TestMethod]
		public void DeleteUser_Positive_Test() {
			String inUser = "jnj";
			bool expected = true;

			bool actual = GameScoreController.DeleteUser(inUser);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Try to delete non existing user
		/// </summary>
		[TestMethod]
		public void DeleteUser_Negetive_Test() {
			String inUser = "pqr";
			bool expected = false;

			bool actual = GameScoreController.DeleteUser(inUser);

			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Delete all users
		/// </summary>
		[TestMethod]
		public void DeleteAll_Positive_Test() {
			bool expected = true;

			bool actual = GameScoreController.DeleteAll();

			Assert.AreEqual(expected, actual);
		}
	}
}
