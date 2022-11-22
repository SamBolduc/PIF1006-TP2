using System;
using System.Collections.Generic;
using System.Text;

namespace PIF1006_tp2
{
    public class System
    {
        public Matrix2D A { get; }
        public Matrix2D B { get; }

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
            return A.IsSquare() && B.GetColCount() == 1 && B.GetRowCount() == A.GetRowCount();
        }

        public Matrix2D SolveUsingCramer()
        {
            // À compléter (1 pt)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus

            var detA = A.Determinant();
            if (detA == 0)
            {
                if (B.IsHomogeneous())
                {
                    Console.WriteLine("IL Y A UNE INFINITÉ DE SOLUTIONS");
                }
                else
                {
                    Console.WriteLine("IL Y A SOIT UNE INFINITÉ DE SOLUTIONS, SOIT AUCUNE SOLUTION");
                }

                return null;
            }

            var res = new double[3, 1];
            for (var i = 0; i < 3; i++)
            {
                var tmp = new Matrix2D(new double[3, 3], "tmp");
                for (var index = 0; index < A.GetColCount(); index++)
                {
                    for (var j = 0; j < A.GetColCount(); j++)
                    {
                        tmp.Matrix[index, j] = A.Matrix[index, j];
                    }

                    tmp.Matrix[index, i] = B.Matrix[index, 0];
                }

                res[i, 0] = tmp.Determinant() / detA;
            }


            return new Matrix2D(res, $"Cramer from {B.Name}");
        }

        public Matrix2D SolveUsingInverseMatrix()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus 
            if (!IsValid())
                return null;

            Matrix2D result = A.Inverse();
            int resultCol = result.GetColCount();
            int resultRow = result.GetRowCount();
            int bCol = B.GetColCount();
            int bRow = B.GetRowCount();
            double[,] resultM = result.getMatrix();
            double[,] bM = B.getMatrix();


            if (resultCol == bRow)
            {
                double total = 0;
                double[,] nouvelMat = new double[resultRow, bCol];

                for (int i = 0; i < resultRow; i++)
                {
                    for (int y = 0; y < bCol; y++)
                    {
                        total = 0;
                        for (int z = 0; z < resultCol; z++)
                        {
                            total += resultM[i, z] * bM[z, y];
                        }

                        nouvelMat[i, y] = total;
                    }
                }

                return new Matrix2D(nouvelMat, "resultat");
            }
            else
            {
                return null;
            }

            return null;
        }

        public Matrix2D SolveUsingGauss()
        {
            if (!IsValid())
                return null;

            // La matrice "calcs" contiendra la matrice A ainsi que la matrice B dans la dernière colonne
            var calcs = new Matrix2D(new double[A.GetRowCount(), A.GetColCount() + B.GetColCount()], "calculations");

            // Copier les matrices A et B dans la matrice "calcs"
            for (var row = 0; row < calcs.Matrix.GetLength(0); row++)
            {
                for (var col = 0; col < calcs.Matrix.GetLength(1); col++)
                {
                    // Pour la dernière colonne qui contient la matrice B
                    if (col == calcs.Matrix.GetLength(1) - 1)
                        calcs.Matrix[row, col] = B.Matrix[row, 0];
                    else
                        calcs.Matrix[row, col] = A.Matrix[row, col];
                }
            }


            // Opérations élémentaires
            for (var pivot = 0; pivot < calcs.Matrix.GetLength(0); pivot++)
            {
                /* int overUnderPivot -> valeur à partir d'où il faut mettre les valeurs(matrix[row, col]) à 0
                   Peut être une valeur pour au-dessus ou en-dessous du pivot */
                void PivotValuesTo0(int overUnderPivot)
                {
                    // Calculer le scalaire qui pourra mettre les valeurs en-dessous/au-dessus du pivot à 0
                    var overUnderPivotTo0 = -calcs.Matrix[overUnderPivot, pivot] / calcs.Matrix[pivot, pivot];

                    // Mettre 0 à tous les endroits(matrix[row, col]) nécessaires
                    for (var pivotRow = pivot; pivotRow < calcs.Matrix.GetLength(1); pivotRow++)
                    {
                        calcs.Matrix[overUnderPivot, pivotRow] += overUnderPivotTo0 * calcs.Matrix[pivot, pivotRow];
                    }
                }

                // Mettre les valeurs en-dessous du pivot à 0
                for (var underPivot = pivot + 1; underPivot < calcs.Matrix.GetLength(0); underPivot++)
                {
                    PivotValuesTo0(underPivot);
                }

                // Mettre les valeurs au-dessus du pivot à 0
                for (var overPivot = pivot - 1; overPivot >= 0; overPivot--)
                {
                    PivotValuesTo0(overPivot);
                }

                // Calculer le scalaire qui pourra mettre le pivot à 1
                var pivotTo1 = 1 / calcs.Matrix[pivot, pivot];
                for (var nextPivotRow = pivot; nextPivotRow < calcs.Matrix.GetLength(1); nextPivotRow++)
                {
                    calcs.Matrix[pivot, nextPivotRow] *= pivotTo1;
                }
            }

            // Retourne seulement la dernière colonne de la matrice (qui est en soi la matrice B, bref, l'ensemble des valeurs recherchées)
            return new Matrix2D(
                new[,]
                {
                    { calcs.Matrix[0, calcs.Matrix.GetLength(1) - 1] },
                    { calcs.Matrix[1, calcs.Matrix.GetLength(1) - 1] },
                    { calcs.Matrix[2, calcs.Matrix.GetLength(1) - 1] },
                }, "Result");
        }

        public override string ToString()
        {
            // À compléter (0.5 pt)
            // Devrait retourner en format:
            // 
            // 3x1 + 5x2 + 7x3 = 9
            // 6x1 + 2x2 + 5x3 = -1
            // 5x1 + 4x2 + 5x3 = 5
            var result = new List<string>();

            // Copier les matrices A et B dans la matrice "calcs"
            for (var row = 0; row < A.Matrix.GetLength(0); row++)
            {
                var line = new StringBuilder();
                for (var col = 0; col < A.Matrix.GetLength(1) + 1; col++)
                {
                    // Pour la dernière colonne qui contient la matrice B
                    line.Append(col == A.Matrix.GetLength(1)
                            ? $"=\t{B.Matrix[row, 0]}" // " = 6"
                            : $"{A.Matrix[row, col]}x{col + 1}\t{(col == A.Matrix.GetLength(1) - 1 ? "" : "+\t")}" // "1x1 + "
                    );
                }

                result.Add(line.ToString());
                result.Add("");
            }

            return string.Join("\n", result);
        }
    }
}