namespace UI {
    partial class FrmSelectPlayer {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnContinue = new System.Windows.Forms.Button();
			this.txtPlayerName1 = new System.Windows.Forms.TextBox();
			this.txtPlayerName2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(13, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(123, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Player 1 Name : ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(13, 49);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(123, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Player 2 Name : ";
			// 
			// btnContinue
			// 
			this.btnContinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnContinue.Location = new System.Drawing.Point(302, 97);
			this.btnContinue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(100, 30);
			this.btnContinue.TabIndex = 2;
			this.btnContinue.Text = "Start game";
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
			// 
			// txtPlayerName1
			// 
			this.txtPlayerName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPlayerName1.Location = new System.Drawing.Point(172, 4);
			this.txtPlayerName1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtPlayerName1.Name = "txtPlayerName1";
			this.txtPlayerName1.Size = new System.Drawing.Size(232, 26);
			this.txtPlayerName1.TabIndex = 3;
			// 
			// txtPlayerName2
			// 
			this.txtPlayerName2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPlayerName2.Location = new System.Drawing.Point(172, 44);
			this.txtPlayerName2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.txtPlayerName2.Name = "txtPlayerName2";
			this.txtPlayerName2.Size = new System.Drawing.Size(232, 26);
			this.txtPlayerName2.TabIndex = 4;
			// 
			// FrmSelectPlayer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(417, 139);
			this.Controls.Add(this.txtPlayerName2);
			this.Controls.Add(this.txtPlayerName1);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "FrmSelectPlayer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Enter Name(s)";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmSelectPlayer_FormClosed);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.TextBox txtPlayerName1;
        private System.Windows.Forms.TextBox txtPlayerName2;
    }
}