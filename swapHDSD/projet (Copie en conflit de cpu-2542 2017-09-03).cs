using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace swapHDSD
{
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
    }

    public class Projet
    {
        // Objet de la BDD
        public SQLiteConnection conn;

        // Projet actuel
        ProjetActuel projetActuel;

        // Affichage
        System.Drawing.Icon iconeHD;
        System.Drawing.Icon iconeSD;
        NotifyIcon icone;
        bool test;

        // Fichiers
        string proxy;
        string proxy_standby;
        string proxy_temp;
        string bidon;

        // Constructeur
        public Projet(NotifyIcon icn)
        {
            // Icones
            iconeHD = new System.Drawing.Icon("ressource\\HD.ico");
            iconeSD = new System.Drawing.Icon("ressource\\SD.ico");

            this.icone = icn;

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

        internal string nom()
        {
            return projetActuel.getNom();
        }

        internal bool supprimer(string nom)
        {
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

        internal bool swapProxy()
        {
            if (this.projetActuel.getId() == -1)
            {
                MessageBox.Show("Aucun projet n'est chargé actuellement! " + this.projetActuel.getId());
                icone.Icon = iconeHD;
                return false;
            }

            Directory.Move(proxy, proxy_temp);
            Directory.Move(proxy_standby, proxy);
            Directory.Move(proxy_temp, proxy_standby);

            HDouSD();

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
                this.projetActuel.setProjet(Convert.ToInt32(ds.Tables[0].Rows[0][0]), ds.Tables[0].Rows[0][1].ToString(), ds.Tables[0].Rows[0][2].ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Aucun projet n'a été chargé.\n" + e.Message);
                this.projetActuel.setProjet(-1, "", "");
                icone.Text = "(aucun)";
            }

            sql = "update projetActuel set id=" + this.projetActuel.getId() + ";";
            Console.WriteLine(sql);
            SQLiteCommand command = new SQLiteCommand(sql, this.conn);
            command.ExecuteNonQuery();

            verifierDossiers();

            icone.Text = this.projetActuel.getNom();
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
        public void getListe(ListBox Liste, Label label)
        {
            label.Text = "Projet actuel : " + this.projetActuel.getNom();
            Liste.Items.Clear();
            try
            {
                DataSet ds = new DataSet();
                var da = new SQLiteDataAdapter("select * from projets", this.conn);
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
        }

        // Met à jour le projet actuel
        public bool update(string nom, string path)
        {
            string sql = "update projets set nom=\"" + nom + "\", path =\"" + path + "\" where id=" + this.projetActuel.getId() + ";";
            SQLiteCommand command = new SQLiteCommand(sql, this.conn);
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
            SQLiteCommand command = new SQLiteCommand(sql, this.conn);
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
            nomTextbox.Text = this.projetActuel.getNom();
            pathTextbox.Text = this.projetActuel.getPath();

            Console.WriteLine("Projet : " + this.projetActuel.getNom() + " " + this.projetActuel.getPath());
        }

        // Vérifie l'existence du path, sinon crée les dossiers
        public bool verifierDossiers()
        {
            // Dossiers et fichiers
            string path = this.projetActuel.getPath();
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
                    HDouSD();
                    return true;
                }
                // S'ils n'existent pas, on les crée
                else
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result;

                    result = MessageBox.Show("Voulez-vous créer les dossiers du projet?", "Dossiers inexistants", buttons);

                    // Si l'utilisateur a accepté, on crée les dossiers et le fichier bidon
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        // Crée le répertoires et le fichier
                        Directory.CreateDirectory(proxy);
                        Directory.CreateDirectory(proxy_standby);
                        File.CreateText(bidon);

                        // Met par défaut l'icone HD
                        icone.Icon = iconeHD;
                        return true;
                    }
                    // Sinon on retourne à la fenêtre de choix
                    else
                        return false;
                }
            }
            // Le dossier principal n'existe pas, erreur! On retourne à la fenêtre de choix
            else
            {
                MessageBox.Show("Le dossier spécifié dans le projet n'existe pas!");
                return false;
            }
        }

        public void HDouSD()
        {
            // On teste l'existence du fichier bidon afin de savoir dans quel état on est
            if (File.Exists(bidon))
                icone.Icon = iconeHD;
            else
                icone.Icon = iconeSD;
        }
    }
}
