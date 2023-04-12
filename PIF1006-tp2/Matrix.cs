using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp2
{
    public class Matrix2D
    {
        public double[,] Matrix { get; private set; }
        public string Name { get; private set; }

        public Matrix2D(string name, double[,] matrix)
        {
            // Doit rester tel quel

            Matrix = matrix;
            Name = name;
        }

        public Matrix2D(string name)
        {
            // Doit rester tel quel

            Name = name;
        }

        public void Load(double[,] matrix)
        {
            Matrix = matrix;
        }

        public Matrix2D Transpose()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice qui est la transposée de celle de l'objet
            double[,] trans = new double[Matrix.GetLength(1), Matrix.GetLength(0)];
            for (int i = 0; i < Matrix.GetLength(1); i++)
            {
                for (int j = 0; j < Matrix.GetLength(0); j++)
                {
                    trans[i, j] = Matrix[j, i];
                }
            }
            return new Matrix2D("(" + Name + ")T", trans);
        }

        public bool IsSquare()
        {
            // À compléter (0.25 pt)
            // Doit retourner vrai si la matrice est une matrice carrée, sinon faux
            return Matrix.GetLength(0) == Matrix.GetLength(1);
        }

        public double Determinant()
        {
            // À compléter (2 pts)
            // Aura sans doute des méthodes suppl. privée à ajouter,
            // notamment de nature récursive. La matrice doit être carrée de prime à bord.
            if (!this.IsSquare())
            {
                return 0;
            }
            else if (Matrix.GetLength(0) == 1)
            {
                return Matrix[0, 0];
            }
            double[,] matrix = Matrix;
            return DetCalculer(new List<double[,]>() { matrix }, new List<double>() { 1 });
        }

        public double DetCalculer(List<double[,]> listmatrix, List<double> listparam)
        {
            double result = 0;
            //Si les matrices acceptées sont 2*2, alors le résultat est calculé
            if (listmatrix.FirstOrDefault().GetLength(0) == 2)
            {
                for (int i = 0; i < listmatrix.Count; i++)
                {
                    result += listparam[i] * (listmatrix[i][0, 0] * listmatrix[i][1, 1] - listmatrix[i][0, 1] * listmatrix[i][1, 0]);
                }
                return result;
            }
            //Si la matrice acceptée n'est pas 2*2, alors transformez les n*n matrices en une liste de (n-1)*(n-1) matrices
            //et une liste d'arguments
            List<double[,]> submatrix = new() { };
            List<double> subparam = new() { };
            for (int i = 0; i < listmatrix.Count; i++)
            {
                for (int j = 0; j < listmatrix[i].GetLength(0); j++)
                {
                    //Calculer les paramètres et les ajouter à la liste
                    subparam.Add(listparam[i] * Math.Pow(-1, j) * listmatrix[i][0, j]);
                    submatrix.Add(GetSubmatrix(listmatrix[i],0, j));
                }
            }
            //Effectuer une récursion
            return DetCalculer(submatrix, subparam);
        }

        public double[,] GetSubmatrix(double[,] matrix, int i, int j) {
            double[,] submatrix = new double[matrix.GetLength(0) - 1, matrix.GetLength(1) - 1];
            //En ignorant les lignes et les colonnes dans lesquelles se trouvent les éléments
            //qui sont des paramètres, on obtient les matrices (n-1)*(n-1) et on les ajoute à la liste
            for (int m = 0; m < matrix.GetLength(0); m++)
            {
                for (int n = 0; n < matrix.GetLength(1); n++)
                {
                    if (n == j || m == i)
                    {
                        continue;
                    }
                    if (m > i && n > j)
                    {
                        submatrix[m - 1, n - 1] = matrix[m, n];
                    }
                    else if (m > i && n < j)
                    {
                        submatrix[m - 1, n] = matrix[m, n];
                    }
                    else if (m < i && n > j)
                    {
                        submatrix[m, n - 1] = matrix[m, n];
                    }
                    else { submatrix[m, n] = matrix[m, n]; }
                }
            }
            return submatrix;
        }
        public Matrix2D Comatrix()
        {
            // À compléter (1 pt)
            // Doit retourner une matrice qui est la comatrice de celle de l'objet
            if (!this.IsSquare())
            {
                return null;
            }
            double[,] comatrix = new double[Matrix.GetLength(0), Matrix.GetLength(1)];
            for (int i = 0; i < comatrix.GetLength(0); i++)
            {
                for (int j = 0; j < comatrix.GetLength(1); j++)
                {
                    List<double[,]> submatrix = new() { };
                    submatrix.Add(GetSubmatrix(Matrix,i,j));
                    comatrix[i, j] = Math.Pow(-1, i + j) *DetCalculer(submatrix, new List<double>() { 1 });
                }
            }
            return new Matrix2D("adj(" + Name + ")", new Matrix2D("cof",comatrix).Transpose().Matrix);
        }

        public Matrix2D Inverse()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice qui est l'inverse de celle de l'objet;
            // Si le déterminant est nul ou la matrice non carrée, on retourne null.
            if (!this.IsSquare())
            {
                return null;
            }
            double det = this.Determinant();
            double[,] cotrans = this.Comatrix().Matrix;
            double[,] inverse = new double[Matrix.GetLength(0), Matrix.GetLength(1)];
            for (int i=0; i < inverse.GetLength(0); i++) {
                for (int j = 0; j < inverse.GetLength(1); j++) {
                    inverse[i, j] = cotrans[i, j] / det;
                }
            }
            return new Matrix2D("(" + Name + ")*(-1)", inverse);
        }

        public override string ToString()
        {
            // À compléter (0.25 pt)
            // Doit retourner l'équivalent textuel/visuel d'une matrice.
            // P.ex.:
            // A:
            // | 3 5 7 |
            // | 6 2 5 |
            // | 5 4 5 |
            string result = "Matrix " + Name + " :\n\n";
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                result += "|\t";
                for (int j = 0; j < Matrix.GetLength(1); j++)
                {
                    result += Matrix[i, j].ToString() + "\t";
                }
                result += "|\n\n";
            }
            return result;
        }
    }
}
