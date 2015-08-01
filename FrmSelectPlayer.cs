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
                if (Parent.NoRequest == false) {
                    txtPlayerName1.Enabled = false;
                }
                else {
                    txtPlayerName2.Enabled = false;
                }
            }
            else if (Parent.GameMode == 2) {
                txtPlayerName1.Enabled = false;
            }

            else if (Parent.GameMode == 1) {
                txtPlayerName2.Enabled = false;
            }
        }

        private void FrmSelectPlayer_FormClosed(object sender, FormClosedEventArgs e) {
            Parent.Enabled = true;
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
                if (Parent.GameMode == 3 && Parent.NoRequest) {
                    Parent.PlayerNames[0] = txtPlayerName1.Text;
                    Parent.NetworkName = txtPlayerName1.Text;
                    Parent.SendNetworkCommand("Request " + txtPlayerName1.Text);
                    this.Close();
                }
                else if (Parent.GameMode == 3 && !Parent.NoRequest) {
                    Parent.PlayerNames[1] = txtPlayerName2.Text;
                    Parent.NetworkName = txtPlayerName2.Text;
                    Parent.SendNetworkCommand("Accept " + txtPlayerName2.Text);
                    Parent.NoRequest = true;
                    Parent.StartNewGame();
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
}
