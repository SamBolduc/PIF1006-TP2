namespace PIF1006_tp2
{
    public static class MatrixUtil
    {
        public static double GetDeterminant(Matrix2D matrix, int row, int col)
        {
            double result = 0;

            if (col == 1) return matrix.Matrix[0, 0];

            var tmpMatrix = new Matrix2D(new double[row, col], "tmp");

            var sign = 1;
            for (var i = 0; i < col; i++) //Iteration sur l'ensemble des colonnes
            {
                tmpMatrix = GetSubMatrix(matrix, tmpMatrix, 0, i, col); //On option la sous matrice
                result += GetDeterminant(tmpMatrix, row, col - 1) * matrix.Matrix[0, i] * sign; //Incrémente le résultat
                sign *= -1; //On alterne le multiplicateur, 1 et -1
            }

            return result;
        }

        public static Matrix2D GetSubMatrix(Matrix2D matrix, Matrix2D res, int skippedRow, int skippedCol,
            int matrixSize)
        {
            var row = 0;
            var col = 0;

            for (var i = 0; i < matrixSize; i++) //Iteration sur les row
            {
                for (var j = 0; j < matrixSize; j++) //Iteration sur les cols
                {
                    if (i == skippedRow || j == skippedCol) continue; //Si on est sur la row ou la col spécifié on ignore la ligne

                    res.Matrix[row, col++] = matrix.Matrix[i, j];
                    if (col == matrixSize - 1)
                    {
                        col = 0;
                        row++;
                    }
                }
            }

            return res;
        }
    }
}