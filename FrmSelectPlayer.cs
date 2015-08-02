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

        public FrmMain frmMain { get; set; }

        public FrmSelectPlayer(FrmMain parent) {
            InitializeComponent();
            this.frmMain = parent;
            this.txtPlayerName1.Text = parent.PlayerNames[0];
            this.txtPlayerName2.Text = parent.PlayerNames[1];
            if (frmMain.GameMode == 3) {
                this.btnContinue.Text = "Connect";
                if (frmMain.NoRequest == false) {
                    txtPlayerName1.Enabled = false;
                }
                else {
                    txtPlayerName2.Enabled = false;
                }
            }
            else if (frmMain.GameMode == 2) {
                txtPlayerName1.Enabled = false;
            }

            else if (frmMain.GameMode == 1) {
                txtPlayerName2.Enabled = false;
            }
        }

        private void FrmSelectPlayer_FormClosed(object sender, FormClosedEventArgs e) {
            frmMain.Enabled = true;
        }

        private void btnContinue_Click(object sender, EventArgs e) {
            if (txtPlayerName1.Text == "") {
                txtPlayerName1.Text = "P1";
                txtPlayerName1.ForeColor = Color.Red;
            }
            else if (txtPlayerName2.Text == "") {
                txtPlayerName2.Text = "P2";
                txtPlayerName2.ForeColor = Color.Red;
            }
            else {
                if (frmMain.GameMode == 3 && frmMain.NoRequest) {
                    frmMain.PlayerNames[0] = txtPlayerName1.Text;
                    frmMain.NetworkName = txtPlayerName1.Text;
                    frmMain.SendNetworkCommand("Request " + txtPlayerName1.Text);
                    this.Close();
                }
                else if (frmMain.GameMode == 3 && !frmMain.NoRequest) {
                    frmMain.PlayerNames[1] = txtPlayerName2.Text;
                    frmMain.NetworkName = txtPlayerName2.Text;
                    frmMain.SendNetworkCommand("Accept " + txtPlayerName2.Text);
                    frmMain.NoRequest = true;
                    frmMain.StartNewGame();
                    this.Close();
                }
                else {
                    frmMain.PlayerNames = new String[] { txtPlayerName1.Text, txtPlayerName2.Text };
                    frmMain.StartNewGame();
                    this.Close();
                }
            }
        }

    }
}
