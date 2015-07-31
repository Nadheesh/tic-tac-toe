using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe {
    // Define markers used in the game.
    public enum Marker {
        Empty, Nought, Cross
    }
    class Game : TicTacToeBoardObserver {
        private Board gameBoard;
        private Player firstPlayer;
        private Player secondPlayer;
        private Player currentPlayer;
        private List<TicTacToeGameObserver> observers;
        private Dictionary<String, int> scoreTable;
        public static int[][][] winningPatterns = new int[8][][] {
                                                new int[3][] { new int[2]{0,0}, new int[2]{0,1}, new int[2] {0,2}},
                                                new int[3][] { new int[2]{1,0}, new int[2]{1,1}, new int[2] {1,2}},
                                                new int[3][] { new int[2]{2,0}, new int[2]{2,1}, new int[2] {2,2}},
                                                new int[3][] { new int[2]{0,0}, new int[2]{1,0}, new int[2] {2,0}},
                                                new int[3][] { new int[2]{0,1}, new int[2]{1,1}, new int[2] {2,1}},
                                                new int[3][] { new int[2]{0,2}, new int[2]{1,2}, new int[2] {2,2}},
                                                new int[3][] { new int[2]{0,0}, new int[2]{1,1}, new int[2] {2,2}},
                                                new int[3][] { new int[2]{0,2}, new int[2]{1,1}, new int[2] {2,0}}};
        /// <summary>
        /// Initialize game board.
        /// </summary>
        public Game() {
            gameBoard = new Board();
            scoreTable = new Dictionary<String, int>();
            observers = new List<TicTacToeGameObserver>();
            firstPlayer = null;
            secondPlayer = null;
            gameBoard.addObserver((TicTacToeBoardObserver)this);
        }
        /// <summary>
        /// Creates new players up to 2 players.
        /// First player is the first added player
        /// </summary>
        /// <param name="name">Name of the human</param>
        /// <param name="isHuman">false if player is computer</param>
        public void newHumanPlayer(int playOrder, String name) {
            if (playOrder == 0) {
                firstPlayer = new Human(ref gameBoard, Marker.Cross, name);
            }
            else {
                secondPlayer = new Human(ref gameBoard, Marker.Nought, name);
            }
        }
        /// <summary>
        /// Creates new computer player.
        /// </summary>
        /// <param name="difficulty">Difficulty level of the computer player</param>
        public void newComputerPlayer(int playOrder, Difficulty difficulty) {
            if (playOrder == 0) {
                firstPlayer = new Computer(difficulty, ref gameBoard, Marker.Cross, "Computer");
            }
            else {
                secondPlayer = new Computer(difficulty, ref gameBoard, Marker.Nought, "Computer");
            }
        }
        /// <summary>
        /// Starts the Game.
        /// </summary>
        public void start() {
            // Sets current player to player[0] if all board settings are done.
            if (firstPlayer != null && secondPlayer != null) {
                currentPlayer = firstPlayer;
                currentPlayer.seekNextMove();
            }
            else {
                // Error: Unable to start game without 2 players
            }
        }
        /// <summary>
        /// Resets the game.
        /// </summary>
        public void resetGame() {
            // Reset all players and board to initial conditions.
            gameBoard.reset();
            firstPlayer = null;
            secondPlayer = null;
        }
        /// <summary>
        /// Connects real human to the Human class.
        /// </summary>
        /// <param name="row">Determines the row of the cell selected by the player</param>
        /// <param name="column">Determines the column of the cell selected by the player</param>
        public void humanPlayerInput(int row, int column) {
            if (currentPlayer is Human) {
                ((Human)currentPlayer).respond(row, column);
            }
        }
        /// <summary>
        /// Toggles player when their turn is over
        /// </summary>
        private void togglePlayer() {
            // If the move is valid Next all the data in the game will be updated...
            int winLine = hasWinner(ref gameBoard);
            if (winLine != 0)
                notifyObservers(winLine); // Notify observers that the game is over
            else if (isDraw(ref gameBoard))
                notifyObservers(9); // Notify observers that the game is over
            else {
                // Toggle the current player.
                if (currentPlayer == firstPlayer)
                    currentPlayer = secondPlayer;
                else
                    currentPlayer = firstPlayer;
                currentPlayer.seekNextMove();
            }
        }
        /// <summary>
        /// Determines wether game is over
        /// </summary>
        /// <returns></returns>
        public Boolean gameOver() {
            return hasWinner(ref gameBoard) != 0 || isDraw(ref gameBoard);
        }

        /// <summary>
        /// Determines the game has a winner
        /// </summary>
        /// <returns></returns>
        public static int hasWinner(ref Board gameBoard) {
            int i = 1;
            foreach (int[][] winningPattern in winningPatterns) {
                int[] cell1 = winningPattern[0];
                if (gameBoard.getMarkerAt(cell1[0], cell1[1]) != Marker.Empty) {
                    int[] cell2 = winningPattern[1];
                    int[] cell3 = winningPattern[2];
                    if (gameBoard.getMarkerAt(cell1[0], cell1[1]) == gameBoard.getMarkerAt(cell2[0], cell2[1]) &&
                        gameBoard.getMarkerAt(cell1[0], cell1[1]) == gameBoard.getMarkerAt(cell3[0], cell3[1])) {
                        return i;
                    }
                }
                i += 1;
            }
            return 0;
        }
        /// <summary>
        /// Determines wether one with given marker is the winner
        /// </summary>
        /// <param name="gameBoard"></param>
        /// <param name="marker"></param>
        /// <returns></returns>
        public static Boolean isWinner(ref Board gameBoard, Marker marker) {
            foreach (int[][] winningPattern in winningPatterns) {
                int[] cell1 = winningPattern[0];
                if (gameBoard.getMarkerAt(cell1[0], cell1[1]) == marker) {
                    int[] cell2 = winningPattern[1];
                    int[] cell3 = winningPattern[2];
                    if (gameBoard.getMarkerAt(cell1[0], cell1[1]) == gameBoard.getMarkerAt(cell2[0], cell2[1]) &&
                        gameBoard.getMarkerAt(cell1[0], cell1[1]) == gameBoard.getMarkerAt(cell3[0], cell3[1])) {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Determines the game is a Draw
        /// </summary>
        /// <returns></returns>
        public static Boolean isDraw(ref Board gameBoard) {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (gameBoard.getMarkerAt(i, j) == Marker.Empty) return false;
            return true;
        }
        /// <summary>
        /// Redirect the board's observer mathods through Game class
        /// </summary>
        /// <param name="obs"></param>
        public void addObserver(Observer obs) {
            observers.Add((TicTacToeGameObserver)obs);
            gameBoard.addObserver((TicTacToeBoardObserver)obs);
        }
        /// <summary>
        /// Redirect the board's observer mathods through Game class
        /// </summary>
        /// <param name="obs"></param>
        public void removeObserver(Observer obs) {
            observers.Remove((TicTacToeGameObserver)obs);
            gameBoard.removeObserver((TicTacToeBoardObserver)obs);
        }
        /// <summary>
        /// Notify all observers
        /// </summary>
        /// <param name="wht">0 => Game Won, 1 => game draw</param>
        private void notifyObservers(int wht) {
            foreach (TicTacToeGameObserver obs in observers) {
                if (wht == 9)
                    obs.gameOver("" , 0);
                else
                    obs.gameOver(currentPlayer.Name, wht);
            }
        }
        /// <summary>
        /// Toggles player when there is a change in ther board
        /// </summary>
        /// <param name="nameOfPlayerWon"></param>
        public void boardCellChanged(Marker[][] board) {
            togglePlayer();
        }


        /// <summary>
        /// Gets Game Mode as String
        /// </summary>
        /// <returns>String of game Mode</returns>
        public String getGameMode() {
            if (firstPlayer is Computer) {
                if (secondPlayer is Computer) {
                    return "Computer Vs Computer";
                }
                else {
                    return "Computer Vs Human";
                }
            }
            else {
                if (secondPlayer is Computer) {
                    return "Human Vs Computer";
                }
                else {
                    return "Human Vs Human";
                }
            }
        }

        public void boardCleared(Marker[][] marker) {

        }

        public string getCurrentPlayer() {
            if (currentPlayer != null)
                return currentPlayer.Name;
            else
                return "";
        }

        internal string OtherPlayer(string nameOfPlayer) {
            if (firstPlayer.Name == nameOfPlayer) {
                return secondPlayer.Name;
            }
            else
                return firstPlayer.Name;
        }
    }
}
