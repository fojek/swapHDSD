using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using swapHDSD;
using System.IO;

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
            
            Projet projet = new Projet(icn);
            
            // Conteneur
            System.ComponentModel.Container components = new System.ComponentModel.Container();

            // Liste du clic-droit
            ToolStripMenuItem GererButton = new ToolStripMenuItem("Gerer ...");
            GererButton.Click += new EventHandler(Gerer_Click);
            ToolStripSeparator separateur1 = new ToolStripSeparator();
            ToolStripSeparator separateur2 = new ToolStripSeparator();
            ToolStripMenuItem ouvrirSDButton = new ToolStripMenuItem("Parcourir SD ...");
            ouvrirSDButton.Click += new EventHandler(ouvrirSD_Click);
            ToolStripMenuItem ouvrirHDButton = new ToolStripMenuItem("Parcourir HD ...");
            ouvrirHDButton.Click += new EventHandler(OuvrirHD_Click);
            ToolStripMenuItem exitButton = new ToolStripMenuItem("Quitter");
            exitButton.Click += new EventHandler(exit_Click);
            ContextMenuStrip Liste = new ContextMenuStrip(components);

            // Ajout des éléments dans la liste du clic-droit
            Liste.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                GererButton,
                separateur1,
                ouvrirSDButton,
                ouvrirHDButton,
                separateur2,
                exitButton
            });

            Liste.Name = "MenuClicDroit";
            Liste.Size = new System.Drawing.Size(135, 82);

            // Icone dans le tray de notifications
            icn.MouseDoubleClick += new MouseEventHandler(icn_Click);
            icn.Visible = true;

            // Chargement du projet
            projet.chargeProjet();
            projet.verifierDossiers();
            projet.UpdateIcone(); // Par défaut, HD

            // On met l'objet projet dans les objects ci-dessous pour pouvoir les utiliser dans leurs événements
            icn.Tag = projet;
            GererButton.Tag = projet;
            ouvrirHDButton.Tag = projet;
            ouvrirSDButton.Tag = projet;
            exitButton.Tag = icn;

            // Assigne la liste à l'icone de notification
            icn.ContextMenuStrip = Liste;

            // Les logs (pris sur https://stackoverflow.com/questions/4470700/how-to-save-console-writeline-output-to-text-file#4470751)
            FileStream ostrm;
            StreamWriter writer;
            TextWriter oldOut = Console.Out;
            try
            {
                ostrm = new FileStream("./log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(ostrm);
            }
            catch (Exception e)
            {
                Console.WriteLine("Impossible d'ouvrir le fichier log!");
                Console.WriteLine(e.Message);
                return;
            }
            Console.SetOut(writer);

            Console.WriteLine("Debut du log");

            Application.Run();

            Console.SetOut(oldOut);
            writer.Close();
            ostrm.Close();
            Console.WriteLine("Done");
        }

        // Élément de la liste OuvrirHD
        private static void OuvrirHD_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            Projet projet = btn.Tag as Projet;

            projet.ouvrirHD();
        }

        // Élément de la liste OuvrirSD
        private static void ouvrirSD_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            Projet projet = btn.Tag as Projet;

            projet.ouvrirSD();
        }

        // Double-clic
        static void icn_Click(object sender, EventArgs e)
        {
            // ici on appelle la fonction qui intervertit les répertoires HD et SD
            NotifyIcon icn = sender as NotifyIcon;
            Projet projet = icn.Tag as Projet;

            projet.swapProxy();

            Console.WriteLine("Changement HD -> SD");
        }

        // Élément de la liste Gérer
        static void Gerer_Click(object sender, EventArgs e)
        {
            // ici on appelle la fonction pour éditer / choisir le projet en cours
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            Projet projet = btn.Tag as Projet;

            Console.WriteLine("Creer / Editer projet");
            choisirForm Form = new choisirForm(projet);
            Form.Show();
        }

        // Élément de la liste Quitter
        static void exit_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem btn = sender as ToolStripMenuItem;
            NotifyIcon icone = btn.Tag as NotifyIcon;

            // Pour éviter que l'icone reste bêtement dans la barre des notifications!
            icone.Dispose();

            Application.Exit();
        }
    }
}
