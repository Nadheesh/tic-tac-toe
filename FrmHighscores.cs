using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DBController;
using DBModel;
using System.Collections;

namespace UI {
    public partial class FrmHighscores : Form {

        private FrmMain frmMain { get; set; }
		private ListViewColumnSorter lvwColumnSorter;

        public FrmHighscores(FrmMain parent) {
            InitializeComponent();

            this.frmMain = parent;
            parent.Enabled = false;

			// Create an instance of a ListView column sorter and assign it 
			// to the ListView control.
			lvwColumnSorter = new ListViewColumnSorter();
			this.lstScoreView.ListViewItemSorter = lvwColumnSorter;

			

        }

        private void btnClear_Click(object sender, EventArgs e) {
            DBController.GameScoreController.DeleteAll();
            UpdateScores();
        }

        private void UserScoresInterface_Load(object sender, EventArgs e) {
            UpdateScores();
        }

        private void FrmHighscores_FormClosed(object sender, FormClosedEventArgs e) {
            frmMain.Enabled = true;
        }
        private void btnOK_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void UpdateScores() {
            this.lstScoreView.Items.Clear();
            ArrayList ar = GameScoreController.GetAllScores();
            if (ar != null)
                foreach (DBModel.ScoreCard itm in ar) {
                    this.lstScoreView.Items.Add(new ListViewItem(new String[] { itm.userName, itm.won.ToString(), itm.draw.ToString(), itm.lost.ToString() }));
                }
		}

		private void lstScoreView_ColumnClick(object sender, ColumnClickEventArgs e) {
			// Determine if clicked column is already the column that is being sorted.
			if (e.Column == lvwColumnSorter.SortColumn) {
				// Reverse the current sort direction for this column.
				if (lvwColumnSorter.Order == SortOrder.Ascending) {
					lvwColumnSorter.Order = SortOrder.Descending;
				} else {
					lvwColumnSorter.Order = SortOrder.Ascending;
				}
			} else {
				// Set the column number that is to be sorted; default to ascending.
				lvwColumnSorter.SortColumn = e.Column;
				lvwColumnSorter.Order = SortOrder.Ascending;
			}

			// Perform the sort with these new sort options.
			this.lstScoreView.Sort();
		}

    }
}
