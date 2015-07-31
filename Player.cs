using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe {
    abstract class Player {
        // Player observes the game board.
        protected Board refGameBoard;
        // Marker of the player
        protected Marker marker;
        // Player's name
        protected String name;
        public String Name {
            get {
                return name;
            }
        }
        /// <summary>
        /// The game is seeking a move from the player.
        /// </summary>
        public abstract void seekNextMove();
        /// <summary>
        /// // Checks whether move is valid in the given board.
        /// </summary>
        /// <param name="row">Determines the row of the cell</param>
        /// <param name="column">Determines the column of the</param>
        /// <returns></returns>
        public static Boolean isMoveValid(int row, int column, ref Board gameBoard) {
            return (gameBoard.getMarkerAt(row, column) == Marker.Empty);
        }
    }
}
