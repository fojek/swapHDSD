using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using swapHDSD;
using System.IO;

namespace swapHDSD
{
    public partial class editerForm: Form
    {
        Projet projet;
        string projetSel = "";
        enum MODE { EDITER, CREER };
        MODE mode;
        public editerForm(Projet projet)
        {
            InitializeComponent();
            this.projet = projet;
            sauvegarderButton.Text = "Creer";
            mode = MODE.CREER;
        }

        public editerForm(Projet projet, string projetSel)
        {
            InitializeComponent();
            this.projet = projet;
            this.projetSel = projetSel;
            mode = MODE.EDITER;
        }

        private void sdhd_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (mode == MODE.EDITER)
            {
                projet.chargeProjet(projetSel);
                projet.getNoms(nomTextbox, pathTextbox);
                Console.WriteLine("Fenêtre d'édition ouverte, " + projetSel + " sélectionné.");
            }
            else
            {
                Console.WriteLine("Fenêtre d'édition ouverte en mode création.");
            }

        }

        // Édition du path
        private void parcourirButton_Click(object sender, EventArgs e)
        {
            if (mode == MODE.EDITER)
                MessageBox.Show("Le chemin du projet sera changé, mais les dossiers du chemin précédent existeront toujours.");

            BrowserDialog.ShowDialog();

            if (!Directory.Exists(BrowserDialog.SelectedPath))
            {
                MessageBox.Show("Le répertoire sélectionné n'existe pas : \n\'{0}\'", BrowserDialog.SelectedPath);
            }
            else
            {
                pathTextbox.Text = BrowserDialog.SelectedPath;
            }
        }

        // On sauvegarde selon l'état de la fenêtre
        private void sauvegarderButton_Click(object sender, EventArgs e)
        {
            if (mode == MODE.EDITER)
            {
                // Sauvegarde le projet en cours
                if (projet.update(nomTextbox.Text, pathTextbox.Text))
                    this.Close(); 
            }
            else
            {
                if (projet.creer(nomTextbox.Text, pathTextbox.Text))
                    this.Close();
            }
        }

        // On s'assure que du texte a été entré
        private void verifValide(object sender, EventArgs e)
        {
            if(nomTextbox.Text == "" || pathTextbox.Text == "")
            {
                sauvegarderButton.Enabled = false;
            }
            else
            {
                sauvegarderButton.Enabled = true;
            }
        }

    }
}