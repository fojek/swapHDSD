namespace swapHDSD
{
    partial class editerForm
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
            this.components = new System.ComponentModel.Container();
            this.Liste = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.nouveauToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.hdsd = new System.Windows.Forms.ToolStripMenuItem();
            this.sdhd = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.nomTextbox = new System.Windows.Forms.TextBox();
            this.pathTextbox = new System.Windows.Forms.TextBox();
            this.BrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.parcourirBouton = new System.Windows.Forms.Button();
            this.labelNom = new System.Windows.Forms.Label();
            this.labelPath = new System.Windows.Forms.Label();
            this.sauvegarderButton = new System.Windows.Forms.Button();
            this.Liste.SuspendLayout();
            this.SuspendLayout();
            // 
            // Liste
            // 
            this.Liste.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nouveauToolStripMenuItem,
            this.toolStripSeparator1,
            this.hdsd,
            this.sdhd,
            this.toolStripSeparator2});
            this.Liste.Name = "contextMenuStrip1";
            this.Liste.Size = new System.Drawing.Size(135, 82);
            // 
            // nouveauToolStripMenuItem
            // 
            this.nouveauToolStripMenuItem.Name = "nouveauToolStripMenuItem";
            this.nouveauToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.nouveauToolStripMenuItem.Text = "Nouveau ...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(131, 6);
            // 
            // hdsd
            // 
            this.hdsd.Name = "hdsd";
            this.hdsd.Size = new System.Drawing.Size(134, 22);
            this.hdsd.Text = "HD -> SD";
            // 
            // sdhd
            // 
            this.sdhd.Name = "sdhd";
            this.sdhd.Size = new System.Drawing.Size(134, 22);
            this.sdhd.Text = "SD -> HD";
            this.sdhd.Click += new System.EventHandler(this.sdhd_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(131, 6);
            // 
            // nomTextbox
            // 
            this.nomTextbox.Location = new System.Drawing.Point(63, 12);
            this.nomTextbox.Name = "nomTextbox";
            this.nomTextbox.Size = new System.Drawing.Size(304, 20);
            this.nomTextbox.TabIndex = 1;
            this.nomTextbox.TextChanged += new System.EventHandler(this.verifValide);
            // 
            // pathTextbox
            // 
            this.pathTextbox.ContextMenuStrip = this.Liste;
            this.pathTextbox.Enabled = false;
            this.pathTextbox.Location = new System.Drawing.Point(63, 38);
            this.pathTextbox.Name = "pathTextbox";
            this.pathTextbox.Size = new System.Drawing.Size(269, 20);
            this.pathTextbox.TabIndex = 2;
            this.pathTextbox.TextChanged += new System.EventHandler(this.verifValide);
            // 
            // parcourirBouton
            // 
            this.parcourirBouton.Location = new System.Drawing.Point(338, 38);
            this.parcourirBouton.Name = "parcourirBouton";
            this.parcourirBouton.Size = new System.Drawing.Size(29, 20);
            this.parcourirBouton.TabIndex = 3;
            this.parcourirBouton.Text = "...";
            this.parcourirBouton.UseVisualStyleBackColor = true;
            this.parcourirBouton.Click += new System.EventHandler(this.parcourirButton_Click);
            // 
            // labelNom
            // 
            this.labelNom.AutoSize = true;
            this.labelNom.Location = new System.Drawing.Point(12, 15);
            this.labelNom.Name = "labelNom";
            this.labelNom.Size = new System.Drawing.Size(35, 13);
            this.labelNom.TabIndex = 4;
            this.labelNom.Text = "Nom :";
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(12, 41);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(48, 13);
            this.labelPath.TabIndex = 5;
            this.labelPath.Text = "Chemin :";
            // 
            // sauvegarderButton
            // 
            this.sauvegarderButton.Enabled = false;
            this.sauvegarderButton.Location = new System.Drawing.Point(275, 64);
            this.sauvegarderButton.Name = "sauvegarderButton";
            this.sauvegarderButton.Size = new System.Drawing.Size(92, 23);
            this.sauvegarderButton.TabIndex = 6;
            this.sauvegarderButton.Text = "Sauvegarder";
            this.sauvegarderButton.UseVisualStyleBackColor = true;
            this.sauvegarderButton.Click += new System.EventHandler(this.sauvegarderButton_Click);
            // 
            // editerForm
            // 
            this.AcceptButton = this.sauvegarderButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 96);
            this.Controls.Add(this.sauvegarderButton);
            this.Controls.Add(this.labelPath);
            this.Controls.Add(this.labelNom);
            this.Controls.Add(this.parcourirBouton);
            this.Controls.Add(this.pathTextbox);
            this.Controls.Add(this.nomTextbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(1000, 1000);
            this.MinimumSize = new System.Drawing.Size(16, 38);
            this.Name = "editerForm";
            this.Text = "Editer Projet";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Liste.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip Liste;
        private System.Windows.Forms.ToolStripMenuItem hdsd;
        private System.Windows.Forms.ToolStripMenuItem sdhd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem nouveauToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TextBox nomTextbox;
        private System.Windows.Forms.TextBox pathTextbox;
        private System.Windows.Forms.FolderBrowserDialog BrowserDialog;
        private System.Windows.Forms.Button parcourirBouton;
        private System.Windows.Forms.Label labelNom;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Button sauvegarderButton;
    }
}

