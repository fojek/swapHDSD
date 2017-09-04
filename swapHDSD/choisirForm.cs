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

namespace swapHDSD
{
    public partial class choisirForm : Form
    {
        Projet projet;
        public choisirForm(Projet projet)
        {
            InitializeComponent();
            this.projet = projet;
        }

        private void editerButton_Click(object sender, EventArgs e)
        {
            if (Liste.SelectedIndex >= 0)
            {
                editerForm Form = new editerForm(projet, Liste.Items[Liste.SelectedIndex].ToString());
                Form.ShowDialog();
            }
            projet.getListe(Liste, projetActuelLabel, pathLabel, pathSourceLabel);
        }

        private void choisirForm_Load(object sender, EventArgs e)
        {
            projet.getListe(Liste, projetActuelLabel, pathLabel, pathSourceLabel);
        }

        private void nouveauButton_Click(object sender, EventArgs e)
        {
            editerForm Form = new editerForm(projet);
            Form.ShowDialog();
            projet.getListe(Liste, projetActuelLabel, pathLabel, pathSourceLabel);
        }

        private void SupprimerButton_Click(object sender, EventArgs e)
        {
            if(Liste.SelectedIndex >= 0)
                projet.supprimer(Liste.Items[Liste.SelectedIndex].ToString());
            projet.getListe(Liste, projetActuelLabel, pathLabel, pathSourceLabel);
        }

        private void chargerButton_Click(object sender, EventArgs e)
        {
            if (Liste.SelectedIndex >= 0)
            {
                projet.chargeProjet(Liste.Items[Liste.SelectedIndex].ToString());
                if(projet.verifierDossiers())
                    this.Close();
            }
        }

        private void chargerSelection(object sender, EventArgs e)
        {
            if (Liste.SelectedIndex >= 0)
            {
                projet.chargeProjet(Liste.Items[Liste.SelectedIndex].ToString());
                projetActuelLabel.Text = "Projet actuel : " + projet.nom();
                pathLabel.Text = projet.path();
            }
        }

        private void parcourirBouton_Click(object sender, EventArgs e)
        {
            BrowserDialog.ShowDialog();
            
            if (BrowserDialog.SelectedPath != "")
            {
                pathSourceLabel.Text = BrowserDialog.SelectedPath;
                projet.setProxy(pathSourceLabel.Text);
            }
        }
    }
}
