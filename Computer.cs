using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe {
    // Define Difficulty levels of the computer as a player.
    enum Difficulty {
        Hard, Medium, Easy
    }
    class Computer : Player {
        private Marker oppMarker;
        private static readonly Random getRandom = new Random();
        private static readonly object syncLock = new object();
        /// <summary>
        /// sets the difficulty level of the computer.
        /// </summary>
        public Difficulty difficulty { set; get; }
        /// <summary>
        /// Creates new Computer player with the given difficulty.
        /// </summary>
        /// <param name="difficulty">Difficulty level of the computer {Hard, Medium, Easy}</param>
        public Computer(Difficulty difficulty, ref Board gameBoard, Marker marker, String name) {
            this.difficulty = difficulty;
            this.refGameBoard = gameBoard;
            this.marker = marker;
            this.name = name;
            if (marker == Marker.Cross)
                this.oppMarker = Marker.Nought;
            else
                this.oppMarker = Marker.Cross;
        }
        /// <summary>
        /// Creates new Computer player
        /// </summary>
        public Computer(ref Board gameBoard, Marker marker, String name) {
            this.difficulty = Difficulty.Medium;
            this.refGameBoard = gameBoard;
            this.marker = marker;
            this.name = name;
            if (marker == Marker.Cross)
                this.oppMarker = Marker.Nought;
            else
                this.oppMarker = Marker.Cross;
        }
        /// <summary>
        /// Computer genarated move
        /// </summary>
        public override void seekNextMove() {
            int[] aiMove = difficultyModeMove();
            refGameBoard.setMarkerAt(aiMove[0], aiMove[1], marker);
        }
        ///////////////////////////////////////////////////////////////**********************************
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int[] difficultyModeMove() {
            int[] result;
            if (difficulty == Difficulty.Easy) {
                result = random(); // depth, max turn
            }
            else if (difficulty == Difficulty.Medium) {
                if (refGameBoard.IsEmpty()) result = random();
                else result = minimax(2, marker); // depth, max turn
            }
            else {
                result = minimax(3, marker); // depth, max turn
            }
            return new int[] { result[1], result[2] };   // row, col
        }

        private int[] random() {
            // Generate possible next moves in a List of int[2] of {row, col}.
            List<int[]> nextMoves = generateMoves();
            int rndInt = rndInteger(nextMoves.Count);
            return new int[3] { 0, nextMoves[rndInt][0],
                nextMoves[rndInt][1] };
        }

        /** Recursive minimax at level of depth for either maximizing or minimizing player.
            Return int[3] of {score, row, col}  */
        private int[] minimax(int depth, Marker player) {
            // Generate possible next moves in a List of int[2] of {row, col}.
            List<int[]> nextMoves = generateMoves();

            // mySeed is maximizing; while oppSeed is minimizing
            int bestScore = (player == marker) ? int.MinValue : int.MaxValue;
            int currentScore;
            int bestRow = -1;
            int bestCol = -1;
            if (nextMoves.Count == 0 || depth == 0) {
                // Gameover or depth reached, evaluate score
                bestScore = evaluate() * (depth + 1);
            }
            else {
                foreach (int[] move in nextMoves) {
                    // Try this move for the current "player"
                    refGameBoard.setTempMarkerAt(move[0], move[1], player);
                    if (player == marker) {  // computer is maximizing player
                        currentScore = minimax(depth - 1, oppMarker)[0];
                        if (currentScore > bestScore) {
                            bestScore = currentScore;
                            bestRow = move[0];
                            bestCol = move[1];
                        }
                        else if (currentScore == bestScore && rndBoolean()) {
                            bestScore = currentScore;
                            bestRow = move[0];
                            bestCol = move[1];
                        }
                    }
                    else {  // opponent is minimizing player
                        currentScore = minimax(depth - 1, marker)[0];
                        if (currentScore < bestScore) {
                            bestScore = currentScore;
                            bestRow = move[0];
                            bestCol = move[1];
                        }
                        else if (currentScore == bestScore && rndBoolean()) {
                            bestScore = currentScore;
                            bestRow = move[0];
                            bestCol = move[1];
                        }
                    }
                    // Undo move
                    refGameBoard.setTempMarkerAt(move[0], move[1], Marker.Empty);
                }
            }
            return new int[] { bestScore, bestRow, bestCol };
        }
        /// <summary>
        /// Returns a random boolean
        /// </summary>
        /// <returns></returns>
        private Boolean rndBoolean() {
            lock (syncLock) { // synchronize
                return getRandom.NextDouble() >= 0.5;
            }
        }

        /// <summary>
        /// returns random integer < value, >= 0
        /// </summary>
        /// <returns></returns>
        private int rndInteger(int val) {
            lock (syncLock) { // synchronize
                return getRandom.Next(val);
            }
        }
        //////////////////////////////////////////////////////////////***********************************
        /// <summary>
        /// Genarates all possible moves
        /// </summary>
        /// <returns>List of loves: List<int[]></returns>
        private List<int[]> generateMoves() {
            List<int[]> nextMoves = new List<int[]>(); // allocate List

            // If gameover, i.e., no next move
            if (Game.IsWinner(ref refGameBoard, marker) || Game.IsWinner(ref refGameBoard, oppMarker)) {
                return nextMoves;   // return empty list
            }

            // Search for empty cells and add to the List
            for (int row = 0; row < 3; ++row) {
                for (int col = 0; col < 3; ++col) {
                    if (refGameBoard.getMarkerAt(row, col) == Marker.Empty) {
                        nextMoves.Add(new int[] { row, col });
                    }
                }
            }
            return nextMoves;
        }
        /// <summary>
        /// Evaluate each wining line and gives a score
        /// </summary>
        /// <returns></returns>
        private int evaluate() {
            int score = 0;
            // Evaluate score for each of the 8 lines (3 rows, 3 columns, 2 diagonals)
            score += evaluateLine(0, 0, 0, 1, 0, 2);  // row 0
            score += evaluateLine(1, 0, 1, 1, 1, 2);  // row 1
            score += evaluateLine(2, 0, 2, 1, 2, 2);  // row 2
            score += evaluateLine(0, 0, 1, 0, 2, 0);  // col 0
            score += evaluateLine(0, 1, 1, 1, 2, 1);  // col 1
            score += evaluateLine(0, 2, 1, 2, 2, 2);  // col 2
            score += evaluateLine(0, 0, 1, 1, 2, 2);  // diagonal
            score += evaluateLine(0, 2, 1, 1, 2, 0);  // alternate diagonal
            return score;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="row1">Row1</param>
        /// <param name="col1">Col1</param>
        /// <param name="row2">Row2</param>
        /// <param name="col2">Col2</param>
        /// <param name="row3">Row3</param>
        /// <param name="col3">Col3</param>
        /// <returns></returns>
        private int evaluateLine(int row1, int col1, int row2, int col2, int row3, int col3) {
            int score = 0;
            // First cell
            if (refGameBoard.getMarkerAt(row1, col1) == marker) {
                score = 1;
            }
            else if (refGameBoard.getMarkerAt(row1, col1) == oppMarker) {
                score = -1;
            }

            // Second cell
            if (refGameBoard.getMarkerAt(row2, col2) == marker) {
                if (score == 1) {   // cell1 is marker
                    score = 10;
                }
                else if (score == -1) {  // cell1 is oppMarker
                    return 0;
                }
                else {  // cell1 is empty
                    score = 1;
                }
            }
            else if (refGameBoard.getMarkerAt(row2, col2) == oppMarker) {
                if (score == -1) { // cell1 is marker
                    score = -10;
                }
                else if (score == 1) { // cell1 is oppMarker
                    return 0;
                }
                else {  // cell1 is empty
                    score = -1;
                }
            }

            // Third cell
            if (refGameBoard.getMarkerAt(row3, col3) == marker) {
                if (score > 0) {  // cell1 and/or cell2 is marker
                    score *= 10;
                }
                else if (score < 0) {  // cell1 and/or cell2 is oppMarker
                    return 0;
                }
                else {  // cell1 and cell2 are empty
                    score = 1;
                }
            }
            else if (refGameBoard.getMarkerAt(row3, col3) == oppMarker) {
                if (score < 0) {  // cell1 and/or cell2 is marker
                    score *= 10;
                }
                else if (score > 1) {  // cell1 and/or cell2 is oppMarker
                    return 0;
                }
                else {  // cell1 and cell2 are empty
                    score = -1;
                }
            }
            return score;
        }
    }
}
