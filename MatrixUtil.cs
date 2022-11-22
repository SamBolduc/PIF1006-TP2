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
            for (var i = 0; i < col; i++)
            {
                tmpMatrix = GetSubMatrix(matrix, tmpMatrix, 0, i, col);
                result += GetDeterminant(tmpMatrix, row, col - 1) * matrix.Matrix[0, i] * sign;
                sign *= -1;
            }

            return result;
        }

        public static Matrix2D GetSubMatrix(Matrix2D matrix, Matrix2D res, int skippedRow, int skippedCol,
            int matrixSize)
        {
            var row = 0;
            var col = 0;

            for (var i = 0; i < matrixSize; i++)
            {
                for (var j = 0; j < matrixSize; j++)
                {
                    if (i == skippedRow || j == skippedCol) continue;

                    res.Matrix[row, col++] =
                        matrix.Matrix[i, j];
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