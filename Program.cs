using System;
using System.IO;
using Newtonsoft.Json;
// Coéquipiers: Samuel Bolduc, Benôit Légaré et Alexis Michaud

namespace PIF1006_tp2
{
    // - Répartition des points -:
    // Program.cs: 2 pts
    // Matrix.cs: 4 pts
    // System.cs: 3 pts
    // Rapport + guide: 1 pt)

    class Program
    {
        private static System _system;

        static void Main(string[] args)
        {
            //---- exemple --- à ne pas utiliser dans votre remise

            // On peut créer une matrice comme ceci:
            // Matrix2D matrixA = new Matrix2D("A", 3, 3);
            // Matrix2D matrixB = new Matrix2D("B", 3, 1);
            //
            // // Et on devrait pouvoir construire un système typiquement commec:
            // System system = new System(matrixA, matrixB);
            // _system = system;
            // // Puis résoudre selon différentes méthodes:
            // Matrix2D matrixX;
            // matrixX = system.SolveUsingCramer();
            // matrixX = system.SolveUsingGauss();
            // matrixX = system.SolveUsingInverseMatrix();


            // Vous pouvez vous injecter un exemple de système si vous le souhaitez pour vous aider,
            // mais ultimement tout comme au TP1 vous devrez avoir une méthode pour charger un fichier

            //-------- fin exemple ---------

            // À compléter: 2 pts (0.5 menu / 1.5 chargement)
            /* Vous devez avoir un menu utilisateur avec l'arboresence de menus suivants:
             * 1) Charger un fichier de système -> doit être un fichier structuré ou semi structurée qui décrit
             *    2 matrices; vous pouvez choisir de demander de charger 2 fichiers de matrices séparées (A et B)
             *    si vous préférez;
             *    
             *    ex. de format en "plain text" potentiel:
             *    
             *    3 1 5
             *    4 2 -1
             *    0 -6 4
             *    0
             *    4
             *    6
             *    
             *    Il faut ensuite "parser" ligne par ligne et déduire la taille de la matrice carrée (plusieurs
             *    façons de vérifier cela).  Créez le chargement dans une classe à part ou dans une méthode privée ici.
             *    Si le format est invalide, il faut retourner null ou l'équivalent et cela doit être
             *    indiqué à l'utilisateur; on affiche le système chargé (en appelant implicitement le TOString() du système);
             *    on retourne au menu dans tous les cas;
             *    
             *    Conseil: utilisez du JSON pour vous pratiquer
             *    
             *    Vous pouvez avoir un fichier chargé par défaut; je vous conseille d'avoir plusieurs fichiers de sy`stèmes sous la main prêt
             *    à être chargés.
             *    
             * 2) Afficher le système (note: et le ToString() du système en "mode équation", et les matrices en vue matrices qui composent les équiations
             * 3) Résoudre avec Cramer
             * 4) Résoudre avec la méthode de la matrice inverse : si dét. nul, on recoit nul et on doit afficher un message à l'utilisateur
             * 5) Résoudre avec Gauss
             * 6) Résoudre
             * 
             * Après chaque option on revient au menu utilisateur, sauf pour quitter bien évidemment.
             * 
             */

            // Si le fichier est mal formaté, alors le programme quitte et demande à l'utilisateur un fichier bien formatté.
            if (!LoadFromFile("./sys_3x3_3x1.json")) 
                return;
            
            var showMenu = true;

            //On affiche le menu principal aussi longtemps qu'on a pas sélectionné l'option de quitter (4)
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        //Affichage du menu principal.
        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choisi une option:");
            Console.WriteLine("1) Charger un automate");
            Console.WriteLine("2) Afficher le système");
            Console.WriteLine("3) Résoudre avec Cramer");
            Console.WriteLine(
                "4) Résoudre avec la méthode de la matrice inverse");
            Console.WriteLine("5) Résoudre avec Gauss");
            Console.WriteLine("6) Quitter l'application");
            Console.Write("\r\nSélectionner une option: ");

            try
            {
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine("Veuillez entrer le chemin vers votre fichier json (sys_3x3_3x1.json par défaut)");
                        LoadFromFile(Console.ReadLine());
                        return true;
                    
                    case "2":
                        Console.WriteLine("\nSystème entier:");
                        Console.WriteLine(_system.ToString());
                        Console.WriteLine("-------------------------------------------------------\n");
                        Console.WriteLine(_system.A.ToString());
                        Console.WriteLine("-------------------------------------------------------\n");
                        Console.WriteLine(_system.B.ToString());
                        Console.ReadLine();
                        return true;
                    
                    case "3":
                        Console.WriteLine($"Cramer de {_system.SolveUsingCramer()}");
                        Console.ReadLine();
                        return true;
                    
                    case "4":
                        //TODO: Résoudre avec la méthode de la matrice inverse : si dét. nul, on recoit nul et on doit afficher un message à l'utilisateur
                        Console.WriteLine($"Matrice inverse de {_system.SolveUsingInverseMatrix()}");
                        Console.ReadLine();
                        return true;
                    
                    case "5":
                        Console.WriteLine($"Gauss de {_system.SolveUsingGauss()}");
                        Console.ReadLine();
                        return true;
                    
                    case "6":
                        Console.WriteLine("Bye !");
                        return false;
                    
                    default:
                        return true;
                }
            }
            catch(Exception ex)
            {
                Console.Write($"ERREUR...\n{ex.Message}");
                Console.ReadLine();
            }
            return true;
        }

        private static bool LoadFromFile(string path)
        {
            try
            {
                var lines = File.ReadAllText(path); //Lecture du fichier
                var system = JsonConvert.DeserializeObject<System>(lines); //Tentative de parser les lignes du fichier en la classe System
                _system = system;
                
                Console.WriteLine("Le fichier a correctement été chargé.");
                Console.WriteLine("Appuyez sur ENTER pour continuer...");
                Console.ReadLine();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors du chargement du fichier... \"{e.Message}\"");
                Console.WriteLine("Veuillez fournir un fichier valide et correctement formatté, puis relancer l'application.");
                Console.WriteLine("Appuyez sur ENTER pour quitter.");
                Console.ReadLine();
                return false;
            }
        }
    }
}