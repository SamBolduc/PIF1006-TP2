using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PIF1006_tp2
{
    public class Matrix2D
    {
        private double[,] Matrix { get; }
        private string Name { get; }

        public Matrix2D(double[,] matrix, string name)
        {
            Matrix = matrix;
            Name = name;
        }

        public Matrix2D Transpose()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice qui est la transposée de celle de l'objet

            var rowCount = GetRowCount();
            var colsCount = GetColCount();

            if (!IsSquare())
            {
                //TODO: Message d'erreur??
                return null;
            }

            var result = new double[rowCount, colsCount];
            for (var i = 0; i < rowCount; i++)
            {
                for (var j = 0; j < colsCount; j++)
                {
                    result[i, j] = Matrix[j, i];
                }
            }

            return new Matrix2D(result, $"Transposer de la matrice {Name}");
        }

        public bool IsSquare()
        {
            // À compléter (0.25 pt)
            // Doit retourner vrai si la matrice est une matrice carrée, sinon faux
            var rowCount = GetRowCount();
            var colCount = GetColCount();
            return rowCount != 0 && rowCount == colCount;
        }

        public double Determinant()
        {
            // À compléter (2 pts)
            // Aura sans doute des méthodes suppl. privée à ajouter,
            // notamment de nature récursive. La matrice doit être carrée de prime à bord.
            if (!IsSquare())
            {
                //TODO: Message d'erreur??
                return 0;
            }

            return GetDeterminant(Matrix, GetRowCount(), GetColCount());
        }

        public Matrix2D Comatrix()
        {
            // À compléter (1 pt)
            // Doit retourner une matrice qui est la comatrice de celle de l'objet
            if (!IsSquare())
            {
                //TODO: Message d'erreur??
                return null;
            }

            Matrix2D result = new Matrix2D(new double[this.GetRowCount(), this.GetColCount()], "result");
            Matrix2D box;

            for (int i = 0; i < this.GetRowCount(); i++)
            {
                for (int j = 0; j < this.GetColCount(); j++)
                {
                    box = this.SousMatrice(i, j);
                    if ((i + j) % 2 == 0) 
                    { 
                        result.Matrix[i, j] = box.GetDeterminant(box.Matrix, box.GetRowCount(),box.GetColCount()); 
                    } else 
                    { 
                        result.Matrix[i, j] = -1 * box.GetDeterminant(box.Matrix, box.GetRowCount(), box.GetColCount()); 
                    }
                }
            }
            return result;
        }

        public Matrix2D Inverse()
        {
            Matrix2D result = new Matrix2D(new double[this.GetRowCount(), this.GetColCount()], "result");
            // À compléter (0.25 pt)
            // Doit retourner une matrice qui est l'inverse de celle de l'objet;
            // Si le déterminant est nul ou la matrice non carrée, on retourne null.
            if (!IsSquare())
            {
                //TODO: Message d'erreur??
                return null;
            }

            var det = this.GetDeterminant(this.Matrix, this.GetRowCount(), this.GetColCount());
            //var det = this.DeterminantM();

            if (det == null)
            {
                //TODO: Message d'erreur??
                return null;
            }

            result = this.Comatrix();
            result = result.Transpose();
            result.Division(det);
            return result;
        }
        
        // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus 
        public Matrix2D Gauss(int rowCount, int colCount)
        {
            var result = new Matrix2D(new double[rowCount, colCount], "result");

            return result;
        }

        public void Division(double det)
        {
            for (var row = 0; row < this.GetRowCount(); row++)
            {
                for (var col = 0; col < this.GetColCount(); col++)
                {
                    this.Matrix[row, col] = this.Matrix[row, col] * (1 / det);
                }
            }
        }

        private IEnumerable<string> AsPrintable()
        {
            var ret = new List<string>();

            for (var row = 0; row < GetRowCount(); row++)
            {
                var line = new StringBuilder();
                for (var col = 0; col < GetColCount(); col++)
                {
                    var val = Matrix[row, col];
                    line.Append(string.Format("{0:0.##\t}", val));
                }

                ret.Add(line.ToString());
                ret.Add("");
            }

            return ret;
        }

        public bool IsHomogeneous()
        {
            for (var row = 0; row < GetRowCount(); row++)
            {
                for (var col = 0; col < GetColCount(); col++)
                {
                    var val = this.Matrix[row, col];
                    if (val != 0) return false;
                }
            }

            return true;
        }

        public int GetRowCount()
        {
            return Matrix.GetLength(0);
        }

        public int GetColCount()
        {
            return this.Matrix.GetLength(1);
        }

        private double GetDeterminant(double[,] matrix, int initialSize, int size)
        {
            double result = 0;
            Matrix2D matCalcul;


            if (size == 1) return matrix[0, 0];
            if (size == 2 ) return (matrix[0,0]*matrix[1,1] - matrix[0,1]*matrix[1,0] );

            var sign = 1;
            for (var i = 0; i < size; i++)
            {
                matCalcul = this.SousMatrice(0,i);
                result += GetDeterminant(matCalcul.Matrix, initialSize, size - 1) * matrix[0, i] * sign;
                sign *= -1;
            }

            return result;
        }

        public Matrix2D SousMatrice(int countRow, int countCol)
        {
            Matrix2D matriceCalcul = new Matrix2D(new double[this.GetRowCount() - 1, this.GetColCount() - 1], "sousMatrice");
            int rowM = 0, columnM = 0;
            for (int i = 0; i < this.GetRowCount(); i++)
            {
                for (int j = 0; j < this.GetColCount(); j++)
                {
                    if (i != (countRow) && j != (countCol))
                    {
                        matriceCalcul.Matrix[rowM, columnM] = this.Matrix[i, j];
                        if (columnM < matriceCalcul.GetRowCount() - 1)
                        {
                            columnM++;
                        } else 
                        {
                            columnM = 0; rowM++; 
                        }
                    }
                }
            }
            return matriceCalcul;
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

            var lines = this.AsPrintable();
            return string.Join("\n", lines);
        }
    }
}