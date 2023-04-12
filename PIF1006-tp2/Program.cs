//PIF1006-TP2-Program.cs
//MEMBRES DE L'ÉQUIPE NO 3
//Jiadong Jin JINJ86100000
using System;
using System.IO;
using System.Linq;

namespace PIF1006_tp2
{

    // - Répartition des points -:
    // Program.cs: 2 pts
    // Matrix.cs: 4 pts
    // System.cs: 3 pts
    // Rapport + guide: 1 pt)

    class Program
    {
        static void Main(string[] args)
        {
            string filePathA = @"..\..\..\Data\matrix1.txt";
            string filePathB = @"..\..\..\Data\matrix2.txt";

            bool menu_continue = true;
            while (menu_continue)
            {
                //Créer deux instances de matrice
                Matrix2D matrixA = new("A");
                Matrix2D matrixB = new("B");
                //Chargement des données dans des matrices à partir des fichiers textes
                MatrixLoader loadA = new(filePathA);
                //Si le format est invalide, il faut retourner null ou l'équivalent et cela doit être indiqué à l'utilisateur; on affiche le système chargé
                if (!loadA.IsMatrix())
                {
                    Console.WriteLine("Les données valides dans le fichier pour ce chemin (" + filePathA + ") sont les suivantes :");
                    Console.WriteLine(loadA.ToString());
                    Console.WriteLine("Ce n'est pas une matrice valide, on va donc charger le premier fichier.\r\n");
                    filePathA = @"..\..\..\Data\matrix1.txt";
                    loadA = new(filePathA);
                }
                matrixA.Load(loadA.ToMatrix());
                MatrixLoader loadB = new(filePathB);
                if (!loadB.IsMatrix())
                {
                    Console.WriteLine("Les données valides dans le fichier pour ce chemin (" + filePathB + ") sont les suivantes :");
                    Console.WriteLine(loadB.ToString());
                    Console.WriteLine("Ce n'est pas une matrice valide, on va donc charger le premier fichier.\r\n");
                    filePathB = @"..\..\..\Data\matrix2.txt";
                    loadB = new(filePathB);
                }
                matrixB.Load(loadB.ToMatrix());

                System system = new System(matrixA, matrixB);
                Matrix2D matrixX;

                Console.WriteLine("Les fichiers de matrices actuellement chargées sont: " + filePathA + " et " + filePathB);
                Console.WriteLine("\r\nMenu");
                Console.WriteLine("Veuillez entrer un numéro pour sélectionner la fonction que vous voulez utiliser:");
                Console.WriteLine("(1) Charger deux fichiers de matrices en spécifiant le chemin (relatif) du fichier.");
                Console.WriteLine("(2) Afficher le système et les matrices en vue matrices qui composent les équiations.");
                Console.WriteLine("(3) Résoudre avec Cramer.");
                Console.WriteLine("(4) Résoudre avec la méthode de la matrice inverse.");
                Console.WriteLine("(5) Résoudre avec Gauss.");
                Console.WriteLine("(6) Quitter l'application.");
                switch ((char)Console.ReadKey().KeyChar)
                {
                    case '1':
                        Console.WriteLine("\r\nEntrer en spécifiant le chemin (relatif) du fichier de la matrice A:");
                        string pathAGet = Console.ReadLine();
                        //Vérifier le chemin relatif de l'entrée
                        if (File.Exists(pathAGet))
                        {
                            filePathA = pathAGet;
                            Console.WriteLine("Chargé avec succès\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Chemin relatif incorrect.\r\n");
                            break;
                        }
                        Console.WriteLine("Entrer en spécifiant le chemin (relatif) du fichier de la matrice B:");
                        string pathBGet = Console.ReadLine();
                        //Vérifier le chemin relatif de l'entrée
                        if (File.Exists(pathBGet))
                        {
                            filePathB = pathBGet;
                            Console.WriteLine("Chargé avec succès\r\n");
                        }
                        else
                        {
                            Console.WriteLine("Chemin relatif incorrect.\r\n");
                        }
                        break;
                    case '2':
                        //Afficher proprement la liste des états et la liste des transitions
                        Console.WriteLine("\r\n");
                        Console.WriteLine(matrixA.ToString());
                        Console.WriteLine(matrixB.ToString());
                        if (system.IsValid())
                        {
                            Console.WriteLine(system.ToString());
                        }
                        else
                        {
                            Console.WriteLine("Le mode d'équation n'est pas valide");
                        }
                        break;
                    case '3':
                        Console.WriteLine("\r\n");
                        if (system.IsValid())
                        {
                            matrixX = system.SolveUsingCramer();
                            if (matrixX != null)
                            {
                                Console.WriteLine(matrixX.ToString());
                            }
                            else {
                                Console.WriteLine("Pas de solution!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Le mode d'équation n'est pas valide!");
                        }
                        break;
                    case '4':
                        Console.WriteLine("\r\n");
                        if (system.IsValid())
                        {
                            matrixX = system.SolveUsingInverseMatrix();
                            if (matrixX != null)
                            {
                                Console.WriteLine(matrixX.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Pas de solution!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Le mode d'équation n'est pas valide!");
                        }
                        break;
                    case '5':
                        Console.WriteLine("\r\n");
                        if (system.IsValid())
                        {
                            matrixX = system.SolveUsingGauss();
                            if (matrixX != null)
                            {
                                Console.WriteLine(matrixX.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Pas de solution!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Le mode d'équation n'est pas valide!");
                        }
                        break;
                    case '6':
                        //Quitter l'application
                        menu_continue = false;
                        break;
                    default:
                        Console.WriteLine("\r\n" + "Caractère non valide.\r\n");
                        break;
                }

            }
        }

        
    }
}
