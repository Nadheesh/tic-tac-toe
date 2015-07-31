using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TicTacToe {
    class Board {
        private Marker[][] cells;
        private List<TicTacToeBoardObserver> observers;
        private bool isEmpty;
        /// <summary>
        /// Initialize board
        /// </summary>
        public Board() {
            cells = new Marker[3][]{new Marker[3]{Marker.Empty, Marker.Empty, Marker.Empty},
                                    new Marker[3]{Marker.Empty, Marker.Empty, Marker.Empty},
                                    new Marker[3]{Marker.Empty, Marker.Empty, Marker.Empty}};
            isEmpty = true;
            observers = new List<TicTacToeBoardObserver>();
        }
        /// <summary>
        /// // Adds the given observer.
        /// </summary>
        /// <param name="obs">The observer to be added</param>
        public void addObserver(TicTacToeBoardObserver obs) {
            observers.Add(obs);
        }
        /// <summary>
        /// Removes the given observer.
        /// </summary>
        /// <param name="obs">The observer to be removed.</param>
        public void removeObserver(TicTacToeBoardObserver obs) {
            observers.Remove(obs);
        }
        /// <summary>
        /// set the marker of given cell to 'marker'
        /// </summary>
        /// <param name="row">row of the board</param>
        /// <param name="column">column of the board</param>
        /// <param name="marker">marker that should be placed</param>
        /// <returns></returns>
        internal void setMarkerAt(int row, int column, Marker marker) {
            isEmpty = false;
            cells[row][column] = marker;
            notifyObservers(0);
        }

        /// <summary>
        /// set the marker of given cell to 'marker'
        /// </summary>
        /// <param name="row">row of the board</param>
        /// <param name="column">column of the board</param>
        /// <param name="marker">marker that should be placed</param>
        /// <returns></returns>
        internal void setTempMarkerAt(int row, int column, Marker marker) {
            cells[row][column] = marker;
        }
        /// <summary>
        /// returns the marker of given cell
        /// </summary>
        /// <param name="row">row of the board</param>
        /// <param name="column">column of the board</param>
        /// <returns></returns>
        public Marker getMarkerAt(int row, int column) {
            return cells[row][column];
        }

        /// <summary>
        /// Notify all Observers
        /// </summary>
        /// <param name="wht">1=>Change of cell in the board</param>
        private void notifyObservers(int wht) {
            foreach (TicTacToeBoardObserver obs in observers) {
                if (wht == 0)
                    obs.boardCellChanged((Marker[][])cells.Clone());
                if (wht == 1)
                    obs.boardCleared((Marker[][])cells.Clone());
            }
        }

        internal void reset() {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++) {
                    cells = new Marker[3][]{new Marker[3]{Marker.Empty, Marker.Empty, Marker.Empty},
                                    new Marker[3]{Marker.Empty, Marker.Empty, Marker.Empty},
                                    new Marker[3]{Marker.Empty, Marker.Empty, Marker.Empty}};
                }
            isEmpty = true;
            notifyObservers(1);
        }


        internal Boolean IsEmpty() {
            return isEmpty;
        }
    }
}
