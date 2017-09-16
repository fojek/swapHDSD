using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace swapHDSD
{
    public class DossierSurveille
    {
        public FileSystemWatcher watcher;
        public event EventHandler fichierPret;
        internal string sousDossier;
        HDouSD type;
        System.Timers.Timer timer;

        List<FileInfo> fichiers;

        // Nombre de fichiers en attente
        public int EnAttente { get; set; }

        public DossierSurveille(string pSousDossier, HDouSD pType)
        {
            sousDossier = pSousDossier;
            type = pType;
            CreateWatcher();
            fichiers = new List<FileInfo>();

            // Timer
            timer = new System.Timers.Timer();
            timer.Interval = 2000;
            timer.Elapsed += new ElapsedEventHandler(essaieANouveau);
            timer.Enabled = false;
            timer.AutoReset = true;

            // Vérifie que le dossier est vide
            checkInitial();
        }

        void checkInitial()
        {
            // On vérifie que le dossier est vide, sinon, on notifie qu'il faut transférer les fichiers
            string[] fichiersEnAttente = Directory.GetFiles(sousDossier);

            if (fichiersEnAttente.Count() == 0)
            {
                Console.WriteLine("Dossier {0} vide.", sousDossier);
            }
            else
            {
                Console.WriteLine("{0} fichiers en attente dans {1}.", fichiersEnAttente.Count(), sousDossier);
                foreach (string nomFichier in fichiersEnAttente)
                {
                    fichiers.Add(new FileInfo(nomFichier));
                }

                // Pas super élégant, met en attente la copie initiale des fichiers
                timer.Enabled = true;
            }
        }

        // La première copie n'a pas fonctionné, on réessaie 2 secondes plus tard
        public void essaieANouveau(object sender, EventArgs e)
        {
            Console.WriteLine("Fichiers en attente : {0}", fichiers.Count());

            if (fichiers.Count == 0)
            {
                timer.Enabled = false;
                Console.WriteLine("Aucun fichier dans la pile!");
            }

            List<FileInfo> bidon = fichiers.ToList();

            foreach (FileInfo fichier in bidon)
            {
                if (!File.Exists(fichier.FullName))
                    fichiers.Remove(fichier);
                else if (IsFileLocked(fichier))
                {
                    Console.WriteLine("Fichier {0} toujours verrouillé.", fichier.FullName);
                    // timer.Enabled = true;
                }
                else
                {
                    Console.WriteLine("Fichier copié et retiré de la liste : {0}", fichier.FullName);
                    fichiers.Remove(fichier);
                    FichierLibre(fichier);
                }
            }
        }

        // Création du FileSystemWatcher au sous dossier Photoshop
        public void CreateWatcher()
        {
            //Create a new FileSystemWatcher.
            watcher = new FileSystemWatcher();

            //Subscribe to the Created event.
            watcher.Created += new FileSystemEventHandler(watcher_FileCreated);

            // Assigne le path au dossier Photoshop
            watcher.Path = sousDossier;

            //Enable the FileSystemWatcher events.
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Nouveau Watcher initialisé à {0}", sousDossier);
        }

        // Événement de fichier créé
        virtual internal void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            FileInfo fichier = new FileInfo(e.FullPath);

            if (IsFileLocked(fichier))
            {
                fichiers.Add(fichier);
                timer.Start();
                Console.WriteLine("Fichier {0} créé mais verrouillé, ajout dans la pile.", fichier.FullName);
            }
            else
            {
                FichierLibre(fichier);
            }
        }

        public void FichierLibre(FileInfo fichier)
        {
            if (fichierPret != null)
            {
                fichierPret(fichier, new FichierPretArgs(type, fichier));
                Console.WriteLine("Événement fichier pret dans {0}", sousDossier);
            }
        }

        // Pris sur https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use/937558#937558
        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (Exception e)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                Console.WriteLine(e.Message);
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }

    // Arguments pour l'événement custom
    public class FichierPretArgs : EventArgs
    {
        public HDouSD type { get; set; }
        public FileInfo fichier;

        public FichierPretArgs(HDouSD pType, FileInfo pFichier)
        {
            type = pType;
            fichier = pFichier;
        }
    }

    public class DossierProxy : DossierSurveille
    {
        public DossierProxy(string pSousDossier, HDouSD type) : base(pSousDossier, type)
        {}

        internal override void watcher_FileCreated(object sender, FileSystemEventArgs e)
        {
            ++EnAttente;
            Console.WriteLine("!! {0} fichiers en attente dans {1}", EnAttente, sousDossier);
        }
    }

    public class DossierParent
    {
        // Propriété d'instanciation
        public bool IsNull { get; private set; }

        public List<DossierSurveille> sousDossiers;

        // Property pour le path
        string photoshopPath;
        public string PhotoshopPath {
            get
            {
                if (IsNull)
                    return "(aucun)";
                else
                    return photoshopPath;
            }
            private set
            {
                if (Directory.Exists(value))
                    photoshopPath = value;
                else
                {
                    Console.WriteLine("Le dossier {0} n'existe pas.", value);
                    photoshopPath = "(aucun)";
                    IsNull = true;
                }

            }
        }

        // Constructeur
        public DossierParent(string pPath, Action<object, EventArgs> fonctionEvent)
        {
            string[] paths = new string[] { "\\proxy", "\\proxy-", "\\proxyExec" };
            PhotoshopPath = pPath;
            sousDossiers = new List<DossierSurveille>();

            if (!IsNull)
            {
                foreach (string path in paths)
                {
                    if (!Directory.Exists(PhotoshopPath + path))
                    {
                        Console.WriteLine("Dossier inexistant : " + path);

                        MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                        DialogResult result;

                        result = MessageBox.Show("Le dossier \'" + PhotoshopPath + path + "\' n'existe pas. Le créer ?", "Dossiers inexistants", buttons);

                        // Si l'utilisateur a accepté, on crée le dossier
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            try
                            {
                                Directory.CreateDirectory(PhotoshopPath + path);
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Erreur :\n" + e.Message);
                            }
                        }
                        else
                            IsNull = true;
                    }
                }
                
                if (!IsNull)
                {
                    sousDossiers.Add(new DossierSurveille(PhotoshopPath + paths[0], HDouSD.SD));
                    sousDossiers.Add(new DossierSurveille(PhotoshopPath + paths[1], HDouSD.HD));
                    sousDossiers.Add(new DossierProxy(PhotoshopPath + paths[2], HDouSD.HDSD));

                    // On passe la référence de la méthode fonctionEvent à l'événement fichier prêt
                    foreach (DossierSurveille ds in sousDossiers)
                    {
                        ds.fichierPret += new EventHandler(fonctionEvent);
                    }
                }
            }
        }
    }
}
