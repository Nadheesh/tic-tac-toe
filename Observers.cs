using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe {
    interface Observer {

    }
    interface TicTacToeBoardObserver : Observer {
        void boardCellChanged(Marker[][] board);

        void boardCleared(Marker[][] marker);
    }

    interface TicTacToeGameObserver : Observer {
        void gameOver(string nameOfPlayerWon, int line);
    }
}
