namespace swapHDSD
{
    partial class choisirForm
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
            this.Liste = new System.Windows.Forms.ListBox();
            this.editerButton = new System.Windows.Forms.Button();
            this.SupprimerButton = new System.Windows.Forms.Button();
            this.nouveauButton = new System.Windows.Forms.Button();
            this.BrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.chargerButton = new System.Windows.Forms.Button();
            this.projetActuelLabel = new System.Windows.Forms.Label();
            this.pathLabel = new System.Windows.Forms.Label();
            this.pathSourceLabel = new System.Windows.Forms.Label();
            this.parcourirBouton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Liste
            // 
            this.Liste.FormattingEnabled = true;
            this.Liste.Location = new System.Drawing.Point(12, 94);
            this.Liste.Name = "Liste";
            this.Liste.Size = new System.Drawing.Size(222, 160);
            this.Liste.TabIndex = 0;
            this.Liste.SelectedIndexChanged += new System.EventHandler(this.chargerSelection);
            // 
            // editerButton
            // 
            this.editerButton.Location = new System.Drawing.Point(88, 260);
            this.editerButton.Name = "editerButton";
            this.editerButton.Size = new System.Drawing.Size(70, 23);
            this.editerButton.TabIndex = 3;
            this.editerButton.Text = "Éditer";
            this.editerButton.UseVisualStyleBackColor = true;
            this.editerButton.Click += new System.EventHandler(this.editerButton_Click);
            // 
            // SupprimerButton
            // 
            this.SupprimerButton.Location = new System.Drawing.Point(12, 260);
            this.SupprimerButton.Name = "SupprimerButton";
            this.SupprimerButton.Size = new System.Drawing.Size(70, 23);
            this.SupprimerButton.TabIndex = 4;
            this.SupprimerButton.Text = "Supprimer";
            this.SupprimerButton.UseVisualStyleBackColor = true;
            this.SupprimerButton.Click += new System.EventHandler(this.SupprimerButton_Click);
            // 
            // nouveauButton
            // 
            this.nouveauButton.Location = new System.Drawing.Point(164, 260);
            this.nouveauButton.Name = "nouveauButton";
            this.nouveauButton.Size = new System.Drawing.Size(70, 23);
            this.nouveauButton.TabIndex = 5;
            this.nouveauButton.Text = "Nouveau";
            this.nouveauButton.UseVisualStyleBackColor = true;
            this.nouveauButton.Click += new System.EventHandler(this.nouveauButton_Click);
            // 
            // chargerButton
            // 
            this.chargerButton.Location = new System.Drawing.Point(164, 289);
            this.chargerButton.Name = "chargerButton";
            this.chargerButton.Size = new System.Drawing.Size(70, 23);
            this.chargerButton.TabIndex = 6;
            this.chargerButton.Text = "Charger";
            this.chargerButton.UseVisualStyleBackColor = true;
            this.chargerButton.Click += new System.EventHandler(this.chargerButton_Click);
            // 
            // projetActuelLabel
            // 
            this.projetActuelLabel.AutoSize = true;
            this.projetActuelLabel.Location = new System.Drawing.Point(12, 63);
            this.projetActuelLabel.Name = "projetActuelLabel";
            this.projetActuelLabel.Size = new System.Drawing.Size(75, 13);
            this.projetActuelLabel.TabIndex = 7;
            this.projetActuelLabel.Text = "Projet actuel : ";
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(12, 76);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(54, 13);
            this.pathLabel.TabIndex = 8;
            this.pathLabel.Text = "pathLabel";
            // 
            // pathSourceLabel
            // 
            this.pathSourceLabel.AutoSize = true;
            this.pathSourceLabel.Location = new System.Drawing.Point(12, 26);
            this.pathSourceLabel.Name = "pathSourceLabel";
            this.pathSourceLabel.Size = new System.Drawing.Size(88, 13);
            this.pathSourceLabel.TabIndex = 9;
            this.pathSourceLabel.Text = "pathSourceLabel";
            // 
            // parcourirBouton
            // 
            this.parcourirBouton.Location = new System.Drawing.Point(201, 19);
            this.parcourirBouton.Name = "parcourirBouton";
            this.parcourirBouton.Size = new System.Drawing.Size(29, 20);
            this.parcourirBouton.TabIndex = 10;
            this.parcourirBouton.Text = "...";
            this.parcourirBouton.UseVisualStyleBackColor = true;
            this.parcourirBouton.Click += new System.EventHandler(this.parcourirBouton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Dossier proxy Photoshop :";
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(0, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(250, 2);
            this.label2.TabIndex = 12;
            this.label2.Text = "Dossier proxy Photoshop :";
            // 
            // choisirForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 323);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.parcourirBouton);
            this.Controls.Add(this.pathSourceLabel);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.projetActuelLabel);
            this.Controls.Add(this.chargerButton);
            this.Controls.Add(this.nouveauButton);
            this.Controls.Add(this.SupprimerButton);
            this.Controls.Add(this.editerButton);
            this.Controls.Add(this.Liste);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "choisirForm";
            this.Text = "Choisir projet ...";
            this.Load += new System.EventHandler(this.choisirForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox Liste;
        private System.Windows.Forms.Button editerButton;
        private System.Windows.Forms.Button SupprimerButton;
        private System.Windows.Forms.Button nouveauButton;
        private System.Windows.Forms.FolderBrowserDialog BrowserDialog;
        private System.Windows.Forms.Button chargerButton;
        private System.Windows.Forms.Label projetActuelLabel;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Label pathSourceLabel;
        private System.Windows.Forms.Button parcourirBouton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}