using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PIF1006_tp2
{
    public class MatrixLoader
    {
        public List<List<string>> Inputmatrix { get; private set; }

        public MatrixLoader(string filePath)
        {
            string strline;
            string[] inputline;
            Inputmatrix = new();
            StreamReader sr = new StreamReader(filePath, Encoding.Default);
            //Balayer ligne par ligne et interprété en séparant chaque ligne en un tableau de strings
            int j = 0;
            while ((strline = sr.ReadLine()) != null)
            {
                inputline = strline.Trim().Split(" ");
                for (int i = 0; i < inputline.Length; i++)
                {
                    //S'il y a un élément dans le fichier qui ne peut pas être converti en double, il sera ignoré.
                    if (!double.TryParse(inputline[i], out _))
                    {
                        inputline = inputline.Skip(i).ToArray();
                    }
                }
                Inputmatrix.Add(inputline.ToList());
                j++;
            }

        }

        public bool IsMatrix()
        {
            //Il s'agit d'une matrice lorsque tous les nombres d'éléments valides de chaque ligne sont égaux.
            return Inputmatrix.Max(n => n.Count) == Inputmatrix.Min(n => n.Count);
        }

        public double[,] ToMatrix()
        {
            //Conversion d'une liste bidimensionnelle chargée en un tableau bidimensionnel
            double[,] matrix = new double[Inputmatrix.Count, Inputmatrix[0].Count];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    _ = double.TryParse(Inputmatrix[i][j], out double result);
                    matrix[i, j] = result;
                }
            }
            return matrix;
        }

        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < Inputmatrix.Count; i++)
            {
                for (int j = 0; j < Inputmatrix[i].Count; j++)
                {
                    result += Inputmatrix[i][j].ToString() + "\t";
                }
                result += "\n";
            }
            return result;
        }
    }
}
