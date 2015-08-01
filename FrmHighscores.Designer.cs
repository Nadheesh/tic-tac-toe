namespace UI {
    partial class FrmHighscores {
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
            this.lstScoreView = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colWins = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDraws = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLosses = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstScoreView
            // 
            this.lstScoreView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colWins,
            this.colDraws,
            this.colLosses});
            this.lstScoreView.GridLines = true;
            this.lstScoreView.Location = new System.Drawing.Point(12, 12);
            this.lstScoreView.Name = "lstScoreView";
            this.lstScoreView.Size = new System.Drawing.Size(246, 203);
            this.lstScoreView.TabIndex = 3;
            this.lstScoreView.UseCompatibleStateImageBehavior = false;
            this.lstScoreView.View = System.Windows.Forms.View.Details;
            // 
            // colName
            // 
            this.colName.Text = "Name";
            // 
            // colWins
            // 
            this.colWins.Text = "Wins";
            // 
            // colDraws
            // 
            this.colDraws.Text = "Draws";
            // 
            // colLosses
            // 
            this.colLosses.Text = "Losses";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(102, 221);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(183, 221);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FrmHighscores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 256);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lstScoreView);
            this.Controls.Add(this.btnClear);
            this.Name = "FrmHighscores";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmHighscores_FormClosed);
            this.Load += new System.EventHandler(this.UserScoresInterface_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstScoreView;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colWins;
        private System.Windows.Forms.ColumnHeader colDraws;
        private System.Windows.Forms.ColumnHeader colLosses;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnOK;
    }
}