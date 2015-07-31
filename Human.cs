using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe {
    class Human : Player{
        private Boolean seekingNextMove;
        /// <summary>
        /// Initialize a Human player
        /// </summary>
        /// <param name="gameBoard">referance to game board</param>
        public Human(ref Board gameBoard, Marker marker, String name) {
            this.refGameBoard = gameBoard;
            this.marker = marker;
            this.name = name;
        }
        /// <summary>
        /// The game is seeking a move from the player.
        /// Wait for Human player to give a respond.
        /// </summary>
        public override void seekNextMove() {
            seekingNextMove = true;
        }
        /// <summary>
        /// human player respond through this method.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Boolean respond(int row, int column) {
            if (seekingNextMove) {
                seekingNextMove = false;
                if (isMoveValid(row, column, ref refGameBoard)) {
                    refGameBoard.setMarkerAt(row, column, this.marker);
                    return true;
                }
                else {
                    // Move is invalid!!
                    seekingNextMove = true;
                }
                // return;
            }
            return false;
        }
    }
}
