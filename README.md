# swapHDSD

Gestionnaire de proxy d'images fixes pour Edius permettant d'animer en temps réel de très grandes images.

## Manuel

### Démarrage
Pour le démarrer, il faut trouver `swapHDSD\swapHDSD\bin\Release\swapHDSD.exe`, et le démarrer **en tant qu'Administrateur**. Sinon Windows ne permet pas de faire le transfert des répertoires. 

### Utilisation
#### Icone de notification
L'icone "HD" ou "SD" apparaît dans les icones de notifications. Le dernier projet utilisé est chargé en mémoire, et l'icone est représentative de l'état du dossier `path\proxy` (HD s'il contient le fichier bidon `hd`, SD sinon). Si aucun projet n'est sélectionné, ce qui est possible uniquement si la base de donnée est vide ou que le projet en cours a été supprimé, l'icone indique HD par défaut.)

En laissant ta souris sur l'icone, un tooltip indique le nom du projet en cours (ou 'aucun').

#### Doublic clic
Un Double-clic sur l'icone effectue l'échange HD->SD si un projet valide est chargé.

#### Clic droit
Un clic droit ouvre le menu avec quatre options : 
 * `Gérer ...` : ouvrir l'interface de gestion des projets. 
 * `Parcourir HD ...` et `Parcourir SD ...` : ouvrent le dossier correspondant du projet en cours.
 * Quitter

#### Interface de gestion des projets
Sert à créer / editer / supprimer les projets, et finalement à charger le projet sélectionné dans la liste. Les projets sont sauvegardés dans une base de données SQLite3  (`\swapHDSD\swapHDSD\bin\Release\ressource\projets.db`) On peut également spécifier le dossier source Photoshop, qui est sauvegardé automatiquement.
 
(Notes : lors de la création d'un projet, il faut choisir un chemin existant (ie. le dossier racine doit déjà exister. On peut toutefois le créer dans le browser de dossier. Il est possible d'éditer le projet une fois créé, mais les dossiers précédemment créés ne sont pas supprimés.)

#### Chargement, création des dossiers et copie des fichiers
En appuyant sur charger, si c'est la première fois que le projet est ouvert, le programme crée les dossiers `path\proxy` et `path\proxy-standby`, et crée un fichier `hd` dans proxy. Les fichiers contenus dans le chemin source Photoshop (`pathPhotoshop\proxy` et `pathPhotoshop\proxy-`) sont copiés respectivement dans les bons répertoires. Un message s'affiche pour indiquer le nombre d'éléments copiés. Le dossier `path\proxy` s'ouvre dans l'explorateur Windows. 

Le programme roule toujours : il suffit de double-cliquer sur l'icone pour intervertir les dossiers.

## Révisions

 * `v0.9.1` (2017-09-04) : Ajout de la copie des fichiers, et meilleure gestion des projets. Ajout du manuel. 
 * `v0.9.0` (2017-09-01) : Pour commentaires
