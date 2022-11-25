using System;
using System.Collections.Generic;
using System.Text;

namespace PIF1006_tp2
{
    public class Matrix2D
    {
        public double[,] Matrix { get; }
        public string Name { get; }

        public Matrix2D(double[,] matrix, string name)
        {
            Matrix = matrix;
            Name = name;
        }

        public double[,] GetMatrix()
        {
            return Matrix;
        }

        private Matrix2D Transpose()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice qui est la transposée de celle de l'objet

            var rowCount = GetRowCount();
            var colsCount = GetColCount();

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
            if (!IsSquare())
            {
                throw new ArgumentException("La matrice doit être carrée.");
            }

            // À compléter (2 pts)
            // Aura sans doute des méthodes suppl. privée à ajouter,
            // notamment de nature récursive. La matrice doit être carrée de prime à bord.
            return MatrixUtil.GetDeterminant(this, GetRowCount(), GetColCount());
        }

        private Matrix2D Comatrix()
        {
            // À compléter (1 pt)
            // Doit retourner une matrice qui est la comatrice de celle de l'objet
            if (!IsSquare())
            {
                Console.WriteLine("La matrice n'est pas carré");
                return null;
            }

            var result = new Matrix2D(new double[GetRowCount(), GetColCount()], "result");
            var box = new Matrix2D(new double[GetRowCount() - 1, GetColCount() - 1], "b");

            for (var i = 0; i < GetRowCount(); i++)
            {
                for (var j = 0; j < GetColCount(); j++)
                {
                    box = MatrixUtil.GetSubMatrix(this, box, i, j, GetColCount());
                    if ((i + j) % 2 == 0)
                    {
                        result.Matrix[i, j] = box.Determinant();
                    }
                    else
                    {
                        result.Matrix[i, j] = -1 * box.Determinant();
                    }
                }
            }

            return result;
        }

        public Matrix2D Inverse()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice qui est l'inverse de celle de l'objet;
            // Si le déterminant est nul ou la matrice non carrée, on retourne null.
            if (!IsSquare())
            {
                Console.WriteLine("La matrice n'est pas carré");
                return null;
            }

            var det = Determinant();


            if (det == null)
            {
                Console.WriteLine("Le determinant de la matrice est null");
                return null;
            }

            Matrix2D result = Comatrix();
            if (result != null)
            {
                result = result.Transpose();
                result.Division(det);
            }
            return result;
        }

        private void Division(double det)
        {
            for (var row = 0; row < GetRowCount(); row++)
            {
                for (var col = 0; col < GetColCount(); col++)
                {
                    Matrix[row, col] *= (1 / det);
                }
            }
        }

        public bool IsHomogeneous()
        {
            for (var row = 0; row < GetRowCount(); row++)
            {
                for (var col = 0; col < GetColCount(); col++)
                {
                    var val = Matrix[row, col];
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
            return Matrix.GetLength(1);
        }

        private IEnumerable<string> AsPrintable()
        {
            var ret = new List<string> { $"{Name}:" };

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

        public override string ToString()
        {
            // À compléter (0.25 pt)
            // Doit retourner l'équivalent textuel/visuel d'une matrice.
            // P.ex.:
            // A:
            // | 3 5 7 |
            // | 6 2 5 |
            // | 5 4 5 |

            var lines = AsPrintable();
            return string.Join("\n", lines);
        }
    }
}