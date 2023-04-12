using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PIF1006_tp2
{
    public class System
    {
        public Matrix2D A { get; private set; }
        public Matrix2D B { get; private set; }

        public System(Matrix2D a, Matrix2D b)
        {
            // Doit rester tel quel

            A = a;
            B = b;
        }

        public bool IsValid()
        {
            // À compléter (0.25 pt)
            // Doit vérifier si la matrix A est carrée et si B est une matrice avec le même nb
            // de ligne que A et une seule colonne, sinon cela retourne faux.
            // Avant d'agir avec le système, il faut toujours faire cette validation
            return A.IsSquare() && B.Matrix.GetLength(0) == A.Matrix.GetLength(0) && B.Matrix.GetLength(1) == 1;
        }

        public Matrix2D SolveUsingCramer()
        {
            // À compléter (1 pt)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus
            double det = A.Determinant();
            if (!IsValid() || det == 0)
            {
                return null;
            }
            double[,] x = new double[B.Matrix.GetLength(0), B.Matrix.GetLength(1)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                x[i, 0] = ReplaceColumn(i).Determinant() / det;
            }
            return new Matrix2D("X(Cramer)", x);
        }

        public Matrix2D ReplaceColumn(int r)
        {
            //Cette fonction peut remplacer une colonne de la matrice A par la matrice B,
            //ou combiner la matrice A et la matrice B et résoudre par le pivot de Gauss.
            double[,] replaced;
            if (r == A.Matrix.GetLength(1))
            {
                replaced = new double[A.Matrix.GetLength(0), r + 1];
            }
            else { replaced = new double[A.Matrix.GetLength(0), A.Matrix.GetLength(1)]; }

            for (int i = 0; i < replaced.GetLength(0); i++)
            {
                for (int j = 0; j < replaced.GetLength(1); j++)
                {
                    if (j == r)
                    {
                        replaced[i, j] = B.Matrix[i, 0];
                    }
                    else
                    {
                        replaced[i, j] = A.Matrix[i, j];
                    }
                }
            }
            return new Matrix2D("Replace Line" + r.ToString(), replaced);
        }

        public Matrix2D SolveUsingInverseMatrix()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus
            if (!IsValid()|| A.Determinant()==0)
            {
                return null;
            }
            double[,] inverse = A.Inverse().Matrix;
            double[,] x = new double[B.Matrix.GetLength(0), B.Matrix.GetLength(1)];
            for (int i = 0; i < x.GetLength(0); i++)
            {
                x[i, 0] = 0;
                for (int j = 0; j < inverse.GetLength(1); j++)
                {
                    x[i, 0] += inverse[i, j] * B.Matrix[j, 0];
                }
            }
            return new Matrix2D("X(InverseMatrix)", x);
        }

        public Matrix2D SolveUsingGauss()
        {
            // À compléter (1 pts)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus
            if (!IsValid()||A.Determinant()==0)
            {
                return null;
            }
            //
            return new Matrix2D("X(Gauss)", GaussElimination(A.Matrix.GetLength(0), ReplaceColumn(A.Matrix.GetLength(0)).Matrix));
        }
        public double[,] GaussElimination(int n, double[,] a)
        {
            //n: nombre d'inconnues a: matrice n*(n+1) synthétisées à partir des matrices A et B
            //Processus d'élimination
            for (int k = 0; k < n - 1; k++)
            {
                for (int i = k; i < n - 1; i++)
                {
                    //Calculer le facteur qui permet de l'éliminer
                    double m = a[i + 1, k] / a[k, k];
                    for (int j = k; j <= n; j++)
                    {
                        //Remplacer la ligne correspondante
                        a[i + 1, j] = a[i + 1, j] - m * a[k, j];
                    }
                }
            }
            double[,] x = new double[B.Matrix.GetLength(0), B.Matrix.GetLength(1)];
            //Processus de rétro génération
            for (int k = n - 1; k >= 0; k--)
            {
                double addResult = 0;
                for (int j = k; j < n - 1; j++)
                {
                    //La somme obtenue en faisant intervenir les inconnues connues
                    addResult += x[j + 1, 0] * a[k, j + 1];
                }
                x[k, 0] = (a[k, n] - addResult) / a[k, k];
            }
            return x;

        }

        public override string ToString()
        {
            // À compléter (0.5 pt)
            // Devrait retourner en format:
            // 
            // 3x1 + 5x2 + 7x3 = 9
            // 6x1 + 2x2 + 5x3 = -1
            // 5x1 + 4x2 + 5x3 = 5
            string result = "Ax=B :\n";
            for (int i = 0; i < A.Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < A.Matrix.GetLength(1); j++)
                {
                    result += "(" + A.Matrix[i, j].ToString() + ")x" + (j + 1).ToString();
                    if (j < A.Matrix.GetLength(1) - 1)
                    {
                        result += " + ";
                    }
                }
                result += " = " + B.Matrix[i, 0].ToString() + "\n";
            }
            return result;
        }

    }
}
