namespace Crunchy
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSrchSrs = new System.Windows.Forms.Button();
            this.btnBrwsPth = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbQlty = new System.Windows.Forms.ComboBox();
            this.btnFrmUrl = new System.Windows.Forms.Button();
            this.lbSrsTtl = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbATE = new System.Windows.Forms.Label();
            this.lbAT = new System.Windows.Forms.Label();
            this.lbUid = new System.Windows.Forms.Label();
            this.lbUname = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnLgn = new System.Windows.Forms.Button();
            this.btnRfrshTkns = new System.Windows.Forms.Button();
            this.btnDltTkns = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbDownloadProgress = new System.Windows.Forms.Label();
            this.cmbSeasons = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Series: ";
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(93, 135);
            this.txtPath.Name = "txtPath";
            this.txtPath.ReadOnly = true;
            this.txtPath.Size = new System.Drawing.Size(448, 23);
            this.txtPath.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Directory: ";
            // 
            // btnSrchSrs
            // 
            this.btnSrchSrs.Location = new System.Drawing.Point(435, 47);
            this.btnSrchSrs.Name = "btnSrchSrs";
            this.btnSrchSrs.Size = new System.Drawing.Size(106, 25);
            this.btnSrchSrs.TabIndex = 4;
            this.btnSrchSrs.Text = "Search for series";
            this.btnSrchSrs.UseVisualStyleBackColor = true;
            // 
            // btnBrwsPth
            // 
            this.btnBrwsPth.Location = new System.Drawing.Point(466, 165);
            this.btnBrwsPth.Name = "btnBrwsPth";
            this.btnBrwsPth.Size = new System.Drawing.Size(75, 25);
            this.btnBrwsPth.TabIndex = 5;
            this.btnBrwsPth.Text = "Browse";
            this.btnBrwsPth.UseVisualStyleBackColor = true;
            this.btnBrwsPth.Click += new System.EventHandler(this.btnBrwsPth_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "Quality: ";
            // 
            // cmbQlty
            // 
            this.cmbQlty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbQlty.FormattingEnabled = true;
            this.cmbQlty.Items.AddRange(new object[] {
            "Poorest",
            "360p",
            "480p",
            "720p",
            "1080p",
            "Best"});
            this.cmbQlty.Location = new System.Drawing.Point(93, 193);
            this.cmbQlty.Name = "cmbQlty";
            this.cmbQlty.Size = new System.Drawing.Size(121, 24);
            this.cmbQlty.TabIndex = 7;
            // 
            // btnFrmUrl
            // 
            this.btnFrmUrl.Location = new System.Drawing.Point(359, 47);
            this.btnFrmUrl.Name = "btnFrmUrl";
            this.btnFrmUrl.Size = new System.Drawing.Size(70, 25);
            this.btnFrmUrl.TabIndex = 8;
            this.btnFrmUrl.Text = "From url";
            this.btnFrmUrl.UseVisualStyleBackColor = true;
            this.btnFrmUrl.Click += new System.EventHandler(this.btnFrmUrl_Click);
            // 
            // lbSrsTtl
            // 
            this.lbSrsTtl.AutoSize = true;
            this.lbSrsTtl.Location = new System.Drawing.Point(93, 22);
            this.lbSrsTtl.Name = "lbSrsTtl";
            this.lbSrsTtl.Size = new System.Drawing.Size(39, 16);
            this.lbSrsTtl.TabIndex = 9;
            this.lbSrsTtl.Text = "label4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 16);
            this.label5.TabIndex = 10;
            this.label5.Text = "Account: ";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.74792F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.25208F));
            this.tableLayoutPanel1.Controls.Add(this.lbATE, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbAT, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbUid, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbUname, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(93, 237);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 36.92308F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 63.07692F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 115);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // lbATE
            // 
            this.lbATE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbATE.Location = new System.Drawing.Point(143, 94);
            this.lbATE.Name = "lbATE";
            this.lbATE.Size = new System.Drawing.Size(299, 18);
            this.lbATE.TabIndex = 7;
            this.lbATE.Text = "...";
            this.lbATE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbAT
            // 
            this.lbAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAT.Location = new System.Drawing.Point(143, 58);
            this.lbAT.Name = "lbAT";
            this.lbAT.Size = new System.Drawing.Size(299, 33);
            this.lbAT.TabIndex = 6;
            this.lbAT.Text = "...";
            this.lbAT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbUid
            // 
            this.lbUid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUid.Location = new System.Drawing.Point(143, 24);
            this.lbUid.Name = "lbUid";
            this.lbUid.Size = new System.Drawing.Size(299, 31);
            this.lbUid.TabIndex = 5;
            this.lbUid.Text = "...";
            this.lbUid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbUname
            // 
            this.lbUname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbUname.Location = new System.Drawing.Point(143, 3);
            this.lbUname.Name = "lbUname";
            this.lbUname.Size = new System.Drawing.Size(299, 18);
            this.lbUname.TabIndex = 4;
            this.lbUname.Text = "...";
            this.lbUname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(6, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Username:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(6, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 31);
            this.label7.TabIndex = 1;
            this.label7.Text = "Session Token: ";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Location = new System.Drawing.Point(6, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 33);
            this.label8.TabIndex = 2;
            this.label8.Text = "Auth Token: ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(6, 94);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(128, 18);
            this.label9.TabIndex = 3;
            this.label9.Text = "Token expires at: ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLgn
            // 
            this.btnLgn.Location = new System.Drawing.Point(466, 358);
            this.btnLgn.Name = "btnLgn";
            this.btnLgn.Size = new System.Drawing.Size(75, 25);
            this.btnLgn.TabIndex = 12;
            this.btnLgn.Text = "Login";
            this.btnLgn.UseVisualStyleBackColor = true;
            this.btnLgn.Click += new System.EventHandler(this.btnLgn_Click);
            // 
            // btnRfrshTkns
            // 
            this.btnRfrshTkns.Location = new System.Drawing.Point(359, 358);
            this.btnRfrshTkns.Name = "btnRfrshTkns";
            this.btnRfrshTkns.Size = new System.Drawing.Size(101, 25);
            this.btnRfrshTkns.TabIndex = 13;
            this.btnRfrshTkns.Text = "Refresh Tokens";
            this.btnRfrshTkns.UseVisualStyleBackColor = true;
            // 
            // btnDltTkns
            // 
            this.btnDltTkns.Location = new System.Drawing.Point(252, 358);
            this.btnDltTkns.Name = "btnDltTkns";
            this.btnDltTkns.Size = new System.Drawing.Size(101, 25);
            this.btnDltTkns.TabIndex = 14;
            this.btnDltTkns.Text = "Delete tokens";
            this.btnDltTkns.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(404, 399);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(56, 25);
            this.button1.TabIndex = 16;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(466, 399);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 25);
            this.button2.TabIndex = 15;
            this.button2.Text = "Download";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbDownloadProgress
            // 
            this.lbDownloadProgress.Location = new System.Drawing.Point(96, 395);
            this.lbDownloadProgress.Name = "lbDownloadProgress";
            this.lbDownloadProgress.Size = new System.Drawing.Size(302, 33);
            this.lbDownloadProgress.TabIndex = 18;
            this.lbDownloadProgress.Text = "...";
            this.lbDownloadProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbSeasons
            // 
            this.cmbSeasons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSeasons.FormattingEnabled = true;
            this.cmbSeasons.Location = new System.Drawing.Point(93, 91);
            this.cmbSeasons.Name = "cmbSeasons";
            this.cmbSeasons.Size = new System.Drawing.Size(342, 24);
            this.cmbSeasons.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Season: ";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(451, 93);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(85, 20);
            this.checkBox1.TabIndex = 21;
            this.checkBox1.Text = "All seasons";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(41, 403);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 16);
            this.label10.TabIndex = 22;
            this.label10.Text = "Status: ";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(553, 438);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.cmbSeasons);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbDownloadProgress);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnDltTkns);
            this.Controls.Add(this.btnRfrshTkns);
            this.Controls.Add(this.btnLgn);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbSrsTtl);
            this.Controls.Add(this.btnFrmUrl);
            this.Controls.Add(this.cmbQlty);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnBrwsPth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSrchSrs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPath);
            this.Font = new System.Drawing.Font("Microsoft PhagsPa", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Crunchyroll Downloader";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSrchSrs;
        private System.Windows.Forms.Button btnBrwsPth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbQlty;
        private System.Windows.Forms.Button btnFrmUrl;
        private System.Windows.Forms.Label lbSrsTtl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbATE;
        private System.Windows.Forms.Label lbAT;
        private System.Windows.Forms.Label lbUid;
        private System.Windows.Forms.Label lbUname;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnLgn;
        private System.Windows.Forms.Button btnRfrshTkns;
        private System.Windows.Forms.Button btnDltTkns;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbDownloadProgress;
        private System.Windows.Forms.ComboBox cmbSeasons;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label10;
    }
}