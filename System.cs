using System;
using System.Collections.Generic;
using System.Linq;

namespace PIF1006_tp2
{
    public class System
    {
        public Matrix2D A { get; }
        private Matrix2D B { get; }

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
            var mat = new double[B.GetRowCount(), B.GetColCount()];
            var res1 = new Matrix2D(mat, $"Cramer from {B.Name}");

            var detA = A.Determinant();
            if (detA == 0) {
                if (B.IsHomogeneous()) {
                    //TODO: IL Y A UNE INFINITÉ DE SOLUTIONS
                } else {
                    //TODO: IL Y A SOIT UNE INFINITÉ DE SOLUTIONS, SOIT AUCUNE SOLUTION
                }

                return null;
            }

            var res = new double[3, 1];
            for (var i = 0; i < 3; i++)
            {
                var tmp = ArrayUtil.CreateJaggedArray<double[][]>(3, 3);

                var firstMatrix = A.Matrix.ToJaggedArray();
                var secondMatrix = B.Matrix.ToJaggedArray();
                for (var index = 0; index < firstMatrix.Length; index++) {
                    // System.arraycopy(firstMatrix[index], 0, tmp[index], 0, firstMatrix[0].length);
                    Array.Copy(firstMatrix[index], 0, tmp[index], 0, firstMatrix[0].Length);
                    tmp[index][i] = secondMatrix[index][0];
                }

                res[i, 0] = A.getDeterminant(tmp, 3,3) / detA;
            }

            return new Matrix2D(res, $"Cramer from {B.Name}");;
        }

        public Matrix2D SolveUsingInverseMatrix()
        {
            // À compléter (0.25 pt)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus 
            return null;
        }
        
        public Matrix2D SolveUsingGauss()
        {
            // À compléter (1 pts)
            // Doit retourner une matrice X de même dimension que B avec les valeurs des inconnus 
            return A.Gauss(B.GetRowCount(), B.GetColCount());
        }

        public override string ToString()
        {
            // À compléter (0.5 pt)
            // Devrait retourner en format:
            // 
            // 3x1 + 5x2 + 7x3 = 9
            // 6x1 + 2x2 + 5x3 = -1
            // 5x1 + 4x2 + 5x3 = 5
            return A.ToString();
        }
    }
}