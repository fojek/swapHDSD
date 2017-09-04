using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using swapHDSD;

namespace swapHDSD
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            NotifyIcon icn = new NotifyIcon();

            Projet projet = new swapHDSD.Projet(icn);
            projet.chargeProjet();
            projet.verifierDossiers();

            // Par défaut, SD
            projet.HDouSD();

            // Conteneur
            System.ComponentModel.Container components = new System.ComponentModel.Container();

            // Liste du clic-droit
            System.Windows.Forms.ToolStripMenuItem GererButton = new System.Windows.Forms.ToolStripMenuItem("Gerer ...");
            GererButton.Click += new EventHandler(Gerer_Click);
            System.Windows.Forms.ToolStripSeparator separateur1 = new System.Windows.Forms.ToolStripSeparator();
            System.Windows.Forms.ToolStripSeparator separateur2 = new System.Windows.Forms.ToolStripSeparator();
            System.Windows.Forms.ToolStripMenuItem ouvrirSDButton = new System.Windows.Forms.ToolStripMenuItem("Parcourir SD ...");
            ouvrirSDButton.Click += new EventHandler(ouvrirSD_Click);
            System.Windows.Forms.ToolStripMenuItem ouvrirHDButton = new System.Windows.Forms.ToolStripMenuItem("Parcourir HD ...");
            ouvrirHDButton.Click += new EventHandler(OuvrirHD_Click);
            System.Windows.Forms.ToolStripMenuItem exitButton = new System.Windows.Forms.ToolStripMenuItem("Quitter");
            exitButton.Click += new EventHandler(exit_Click);
            System.Windows.Forms.ContextMenuStrip Liste = new System.Windows.Forms.ContextMenuStrip(components);

            Liste.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                GererButton,
                separateur1,
                ouvrirSDButton,
                ouvrirHDButton,
                separateur2,
                exitButton
            });

            Liste.Name = "contextMenuStrip1";
            Liste.Size = new System.Drawing.Size(135, 82);

            // Icone dans le tray de notifications
            icn.MouseDoubleClick += new MouseEventHandler(icn_Click);
            icn.Visible = true;

            // On met l'objet projet dans les objects ci-dessous pour pouvoir les utiliser dans leurs événements
            icn.Tag = projet;
            GererButton.Tag = projet;
            ouvrirHDButton.Tag = projet;
            ouvrirSDButton.Tag = projet;

            icn.ContextMenuStrip = Liste;

            Application.Run();
        }

        private static void OuvrirHD_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            Projet projet = btn.Tag as Projet;

            projet.ouvrirHD();
        }

        private static void ouvrirSD_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            Projet projet = btn.Tag as Projet;

            projet.ouvrirSD();
        }

        static void icn_Click(object sender, EventArgs e)
        {
            // ici on appelle la fonction qui intervertit les répertoires HD et SD
            NotifyIcon icn = sender as NotifyIcon;
            Projet projet = icn.Tag as Projet;

            projet.swapProxy();
            Console.WriteLine("Changement HD -> SD");
        }

        static void Gerer_Click(object sender, EventArgs e)
        {
            // ici on appelle la fonction pour éditer / choisir le projet en cours
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            Projet projet = btn.Tag as Projet;

            Console.WriteLine("Creer / Editer projet");
            choisirForm Form = new choisirForm(projet);
            Form.Show();
        }

        static void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
