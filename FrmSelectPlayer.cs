using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI {
    public partial class FrmSelectPlayer : Form {

        public FrmMain Parent { get; set; }

        public FrmSelectPlayer(FrmMain parent) {
            InitializeComponent();
            this.Parent = parent;
            this.txtPlayerName1.Text = parent.PlayerNames[0];
            this.txtPlayerName2.Text = parent.PlayerNames[1];
            if (Parent.GameMode == 3) {
                this.btnContinue.Text = "Connect";
            }
        }

        private void FrmSelectPlayer_FormClosed(object sender, FormClosedEventArgs e) {
            Parent.Enabled = true;
        }

        private void btnContinue_Click(object sender, EventArgs e) {
            if (Parent.GameMode == 3) {
                Parent.PlayerNames = new String[] { txtPlayerName1.Text, "" };
                Parent.NetworkName = txtPlayerName1.Text;
                Parent.SendNetworkCommand("Request " + txtPlayerName1.Text);
                this.Close();
            }
            else {
                Parent.PlayerNames = new String[] { txtPlayerName1.Text, txtPlayerName2.Text };
                Parent.StartNewGame();
                this.Close();
            }
        }

    }
}
