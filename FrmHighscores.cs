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

        private FrmMain Parent { get; set; }

        public FrmHighscores(FrmMain parent) {
            InitializeComponent();
            this.Parent = parent;
            parent.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e) {
            DBController.GameScoreController.DeleteAll();
            UpdateScores();
        }

        private void UserScoresInterface_Load(object sender, EventArgs e) {
            UpdateScores();
        }

        private void FrmHighscores_FormClosed(object sender, FormClosedEventArgs e) {
            Parent.Enabled = true;
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

    }
}
