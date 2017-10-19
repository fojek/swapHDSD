using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace swapHDSD
{
    public enum HDouSD { HD, SD, HDSD };

    class ProjetActuel
    {
        int id;
        string nom;
        string path;

        // Charge un nouveau projet par # de id
        public void setProjet(int nId, string nNom, string nPath)
        {
            this.id = nId;
            this.nom = nNom;
            this.path = nPath;

            Console.WriteLine("Projet charge : " + nId + " | " + nNom + " | " + nPath);
        }

        public string getNom()
        {
            return nom;
        }

        public string getPath()
        {
            return path;
        }
        public int getId()
        {
            return id;
        }

        public bool existe()
        {
            return Directory.Exists(path);
        }

        internal void Clear(NotifyIcon icn)
        {
            setProjet(-1, "", "");
            icn.Text = "(aucun)";
        }

        internal bool IsNull()
        {
            return (id == -1);
        }
    }

    public class Projet
    {
        // Objet de la BDD
        public SQLiteConnection conn;

        // Projet actuel
        ProjetActuel projetActuel;

        // Proxy Photoshop
        DossierParent proxyPhotoshop;

        // Affichage
        Icon iconeHD;
        Icon iconeSD;
        NotifyIcon icone;

        // Timer pour l'affichage de la copie
        System.Timers.Timer timer;
        int fichiersCopiesRecemment;
        const int duree = 5000;

        // Fichiers
        string proxy;
        string proxy_standby;
        string proxy_temp;
        string bidon;

        // Constructeur
        public Projet(NotifyIcon icn)
        {
            // Timer
            timer = new System.Timers.Timer();
            timer.Interval = duree;
            timer.AutoReset = true;

            timer.Elapsed += resetCompte;

            // Icones
            iconeHD = new System.Drawing.Icon("ressource\\HD.ico");
            iconeSD = new System.Drawing.Icon("ressource\\SD.ico");

            icone = icn;

            projetActuel = new ProjetActuel();
            // Connection à la BDD
            conn = new SQLiteConnection("Data Source=ressource\\projets.db;Version=3;");
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void resetCompte(object sender, System.Timers.ElapsedEventArgs e)
        {
            fichiersCopiesRecemment = 0;
            timer.Stop();
        }

        internal string nom()
        {
            return projetActuel.getNom();
        }

        internal bool supprimer(string nom)
        {
            // Si le projet supprimé était le projet en cours
            projetActuel.Clear(icone);

            string sql = "delete from projets where nom=\"" + nom + "\";";
            SQLiteCommand command = new SQLiteCommand(sql, this.conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur:\n " + e.Message);
                return false;
            }
            Console.WriteLine(sql);
            return true;
        }

        internal void setProxy(string selectedPath)
        {
            proxyPhotoshop = new DossierParent(selectedPath, FichierEnAttente);

            string sql = "update dossierproxy set path=\"" + proxyPhotoshop.PhotoshopPath + "\" ;";
            SQLiteCommand command = new SQLiteCommand(sql, this.conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur!\n" + e.Message);
            }
            Console.WriteLine(sql);
        }

        internal void ouvrirHD()
        {
            if (projetActuel.getId() > 0)
            {
                if (File.Exists(bidon))
                    Process.Start(proxy);
                else
                    Process.Start(proxy_standby);
            }
        }

        internal void ouvrirSD()
        {
            if (projetActuel.getId() > 0)
            {
                if (File.Exists(bidon))
                    Process.Start(proxy_standby);
                else
                    Process.Start(proxy);
            }
        }

        internal string path()
        {
            return projetActuel.getPath();
        }

        internal bool swapProxy()
        {
            if (projetActuel.IsNull())
            {
                MessageBox.Show("Aucun projet n'est chargé actuellement! " + projetActuel.getId());
                return false;
            }
            else if (proxyPhotoshop.IsNull)
            {
                MessageBox.Show("Aucun proxy Photoshop n'est chargé actuellement! ");
                return false;
            }

            try
            {
                Directory.Move(proxy, proxy_temp);
                Directory.Move(proxy_standby, proxy);
                Directory.Move(proxy_temp, proxy_standby);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            UpdateIcone();

            return true;
        }

        // Effectue la transaction pour avoir le nouveau projet demandé
        private void charger(string sql)
        {
            Console.WriteLine(sql);
            DataSet ds = new DataSet();
            var da = new SQLiteDataAdapter(sql, conn);
            da.Fill(ds);

            try
            {
                projetActuel.setProjet(Convert.ToInt32(ds.Tables[0].Rows[0][0]), ds.Tables[0].Rows[0][1].ToString(), ds.Tables[0].Rows[0][2].ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                projetActuel.Clear(icone);
            }

            // Dossier proxy Photoshop
            DataSet dw = new DataSet();

            sql = "select * from dossierproxy;";
            var du = new SQLiteDataAdapter(sql, conn);
            du.Fill(dw);

            string photoshopPath = dw.Tables[0].Rows[0][1].ToString();

            proxyPhotoshop = new DossierParent(photoshopPath, FichierEnAttente);
            Console.WriteLine("Dossier proxy chargé : " + proxyPhotoshop.PhotoshopPath);

            sql = "update projetActuel set id=" + projetActuel.getId() + ";";
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            icone.Text = projetActuel.getNom();

            // Balloon Tooltip
            icone.BalloonTipText = "Projet chargé : " + projetActuel.getNom() + "\nProxy photoshop : " + proxyPhotoshop.PhotoshopPath;
            icone.ShowBalloonTip(2000);
        }

        // Charge le dernier projet utilisé, (aucun) sinon
        public void chargeProjet()
        {
            charger("select * from projets where id = (select * from projetActuel);");
        }

        // Charge par nom
        public void chargeProjet(string nom)
        {
            charger("select * from projets where nom = \"" + nom + "\";");
        }

        // Met la liste des projets dans une liste reçue par référence
        public void getListe(ListBox Liste, Label labelnom, Label labelpath, Label pathSourceLabel)
        {
            labelnom.Text = "Projet actuel : " + projetActuel.getNom();
            labelpath.Text = projetActuel.getPath();
            pathSourceLabel.Text = proxyPhotoshop.PhotoshopPath;

            Liste.Items.Clear();
            try
            {
                DataSet ds = new DataSet();
                var da = new SQLiteDataAdapter("select * from projets", conn);
                da.Fill(ds);

                foreach (DataTable table in ds.Tables)
                {
                    foreach (DataRow dr in table.Rows)
                    {
                        Liste.Items.Add(dr[1].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            for (int i=0; i<Liste.Items.Count; ++i)
                if (Liste.Items[i].ToString() == nom())
                    Liste.SetSelected(i, true);
        }

        // Met à jour le projet actuel
        public bool update(string nom, string path)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Le dossier spécifié n'existe pas, ou le chemin entré est invalide.");
                return false;
            }

            string sql = "update projets set nom=\"" + nom + "\", path =\"" + path + "\" where id=" + projetActuel.getId() + ";";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show("Nom déjà existant!\n" + e.Message);
                return false;
            }
            Console.WriteLine(sql);
            return true;
        }        
        
        // Crée un nouveau projet
        public bool creer(string nom, string path)
        {
            string sql = "insert into projets (nom, path) values (\"" + nom +  "\",\"" + path + "\");";
            SQLiteCommand command = new SQLiteCommand(sql, conn);
            try
            {
                command.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show("Création impossible!\n" + e.Message);
                return false;
            }
    
            Console.WriteLine(sql);

            return true;
        }

        // Met les noms dans les objets en référence
        internal void getNoms(TextBox nomTextbox, TextBox pathTextbox)
        {
            nomTextbox.Text = projetActuel.getNom();
            pathTextbox.Text = projetActuel.getPath();

            Console.WriteLine("Projet : " + projetActuel.getNom() + " " + projetActuel.getPath());
        }

        // Vérifie l'existence du path, sinon crée les dossiers
        public bool verifierDossiers()
        {
            // Dossiers et fichiers
            string path = projetActuel.getPath();
            proxy = path + "\\proxy";
            proxy_standby = path + "\\proxy-standby";
            proxy_temp = path + "\\proxy-temp";
            bidon = proxy + "\\hd";
            
            // Le dossier principal existe
            if (Directory.Exists(path))
            {
                // Les dossiers secondaires existent déjà
                if (Directory.Exists(proxy) && Directory.Exists(proxy_standby))
                {
                    // Update l'icone
                    UpdateIcone();
                    return true;
                }

                // S'ils n'existent pas, on les crée
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show("Voulez-vous créer les dossiers du projet ?", "Dossiers inexistants", buttons);

                    // Si l'utilisateur a accepté, on crée les dossiers et le fichier bidon
                    if (result == DialogResult.Yes)
                    {
                        // Crée le répertoires et le fichier
                        Directory.CreateDirectory(proxy);
                        Directory.CreateDirectory(proxy_standby);
                        File.CreateText(bidon);

                        // On ouvre l'explorateur Windows :
                        Process.Start(proxy);

                        // Met par défaut l'icone HD
                        icone.Icon = iconeHD;
                        return true;
                    }
                    // Sinon on retourne à la fenêtre de choix
                    else
                    {
                        projetActuel.Clear(icone);
                        return false;
                    }
                }
            }
            // Le dossier principal n'existe pas, erreur! On retourne à la fenêtre de choix
            else
            {
                MessageBox.Show("Le dossier projet " + path + " n'existe pas!");
                projetActuel.Clear(icone);
                return false;
            }
        }

        internal HDouSD testHDouSD()
        {
            // (condition) ? (si vrai) : (si faux)
            return File.Exists(bidon) ? HDouSD.HD : HDouSD.SD;
        }

        public void UpdateIcone()
        {
            if (!projetActuel.IsNull())
            {
                // On teste l'existence du fichier bidon afin de savoir dans quel état on est
                if (testHDouSD() == HDouSD.HD)
                    icone.Icon = iconeHD;
                else
                    icone.Icon = iconeSD;
            }
            else
                icone.Icon = SystemIcons.Exclamation;
        }

        // Copie des fichiers dans le bon répertoire (Événement de dossier surveillé)
        public void FichierEnAttente(object sender, EventArgs ev)
        {
            FichierPretArgs fichierPretArgs = ev as FichierPretArgs;
            FileSystemWatcher fichier = sender as FileSystemWatcher;
            string dest;
            if (fichierPretArgs.type == HDouSD.HD)
            {
                Console.WriteLine("!!Nouveau fichier dans le dossier HD");
                if(testHDouSD() == HDouSD.HD)
                    dest = proxy + '\\' + fichierPretArgs.fichier.Name;
                else
                    dest = proxy_standby + '\\' + fichierPretArgs.fichier.Name;
            }
            else if(fichierPretArgs.type == HDouSD.SD)
            {
                Console.WriteLine("!!Nouveau fichier dans le dossier SD");
                if (testHDouSD() == HDouSD.HD)
                    dest = proxy_standby + '\\' + fichierPretArgs.fichier.Name;
                else
                    dest = proxy + '\\' + fichierPretArgs.fichier.Name;
            }
            else
            {
                Console.WriteLine("Nouveau fichier dans le dossier Exec");
                dest = "";
            }

            // déplacement du fichier
            try
            {
                // Supprime le fichier destination s'il existe, avec 10 essais
                if (File.Exists(dest))
                {
                    int essais = 0;
                    do
                    {
                        try
                        {
                            File.Delete(dest);
                            essais = 10;
                        }
                        catch
                        {
                            MessageBox.Show("(" + essais + ") Veuillez libérer le fichier " + dest);
                            ++essais;
                        }
                    } while (essais<10);
                }

                File.Move(fichierPretArgs.fichier.FullName, dest);

                ++fichiersCopiesRecemment;

                icone.BalloonTipText = fichiersCopiesRecemment + " fichier(s) transférés";
                icone.ShowBalloonTip(duree);

                // Réinitialise le compteur
                timer.Stop();
                timer.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                Console.WriteLine(e.Message);
            }
        }
    }
}
