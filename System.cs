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

        private bool IsValid()
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

            //Validation que la matrice respecte bien les critères
            if (!IsValid())
                throw new ArgumentException(
                    "Conditions de validité non respectées...\n(A.IsSquare() && B.GetColCount() == 1 && B.GetRowCount() == A.GetRowCount()) == false");

            var detA = A.Determinant();

            //Si le determinant est 0, on ne peut pas utiliser cramer.
            if (detA == 0)
            {
                Console.WriteLine(B.IsHomogeneous()
                    ? "IL Y A UNE INFINITÉ DE SOLUTIONS"
                    : "IL Y A SOIT UNE INFINITÉ DE SOLUTIONS, SOIT AUCUNE SOLUTION");

                throw new ArgumentException(
                    "Voir message ci-haut.");
            }


            var res = new double[B.GetRowCount(), B.GetColCount()]; //Array des resultats

            for (var i = 0; i < B.GetRowCount(); i++)
            {
                var tmp = new Matrix2D(new double[A.GetRowCount(), A.GetColCount()],
                    "tmp"); //Matrice temporaire pour les calculs.
                for (var index = 0; index < A.GetRowCount(); index++)
                {
                    for (var j = 0; j < A.GetColCount(); j++)
                    {
                        tmp.Matrix[index, j] = A.Matrix[index, j];
                    }

                    tmp.Matrix[index, i] = B.Matrix[index, 0];
                }

                res[i, 0] = tmp.Determinant() / detA; //On stock le resultat du detX / detA
            }


            return new Matrix2D(res, A.Name);
        }

        public Matrix2D SolveUsingInverseMatrix()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus 

            if (!IsValid())
                throw new ArgumentException(
                    "Conditions de validité non respectées...\n(A.IsSquare() && B.GetColCount() == 1 && B.GetRowCount() == A.GetRowCount()) == false");

            var result = A.Inverse();

            //Vérification que la matrice inverse de la matrice A n'est pas null
            if (result == null)
                return null;

            var resultCol = result.GetColCount();
            var resultRow = result.GetRowCount();
            var bCol = B.GetColCount();
            var bRow = B.GetRowCount();
            var resultM = result.Matrix;
            var bM = B.Matrix;

            //Vérification que la matrice inverse à le même nombre de colone que le nombre de ligne de la matrice B 
            if (resultCol != bRow)
                return null;

            //On multiplie les éléments des colones de la matrice A avec les éléments des lignes de la matrice B
            //Ensuite nous les additionnons pour former une nouvelle matrice
            var newMatrix = new double[resultRow, bCol];
            for (var i = 0; i < resultRow; i++)
            {
                for (var y = 0; y < bCol; y++)
                {
                    double total = 0;
                    for (var z = 0; z < resultCol; z++)
                    {
                        total += resultM[i, z] * bM[z, y];
                    }

                    newMatrix[i, y] = total;
                }
            }

            return new Matrix2D(newMatrix, A.Name);
        }

        public Matrix2D SolveUsingGauss()
        {
            if (!IsValid())
                throw new ArgumentException(
                    "Conditions de validité non respectées...\n(A.IsSquare() && B.GetColCount() == 1 && B.GetRowCount() == A.GetRowCount()) == false");

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
            var result = new double[calcs.Matrix.GetLength(0), 1];
            for (var i = 0; i < calcs.Matrix.GetLength(0); i++)
                result[i, 0] = calcs.Matrix[i, calcs.Matrix.GetLength(1) - 1];

            return new Matrix2D(result, A.Name);
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

            try
            {
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
            }
            catch (Exception)
            {
                Console.WriteLine(
                    "Impossible d'imprimer le système entier sous forme 'ax + by + ... = Z' car les conditions de validité ne sont pas respectées...");
                Console.WriteLine(
                    "(A.IsSquare() && B.GetColCount() == 1 && B.GetRowCount() == A.GetRowCount()) == false");
                return null;
            }

            return string.Join("\n", result);
        }
    }
}