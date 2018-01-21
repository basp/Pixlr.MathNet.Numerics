namespace Pixlr.Lina
{
    using System;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra;

    public static class MatrixExtensions
    {
        public static IConvolution2D<U> Convolution<U, T>(
            this Matrix<T> self,
            Accumulator<U, T> acc,
            Func<int, int, U> factory)
            where U : struct, IEquatable<U>, IFormattable
            where T : struct, IEquatable<T>, IFormattable
        {
            self.ValidateKernel();
            return new Convolution2D<U, T>(self, acc, factory);
        }

        private static void ValidateKernel<U>(this Matrix<U> self)
            where U : struct, IEquatable<U>, IFormattable
        {
            if (self.RowCount % 2 == 0)
            {
                var msg = $"You can only create a convolution kernel instance from a matrix with an odd size and this matrix has {self.RowCount} rows.";
                throw new ArgumentException(msg, nameof(self.RowCount));
            }

            if (self.ColumnCount % 2 == 0)
            {
                var msg = $"You can only create a convolution kernel instance from a matrix with an odd size and this matrix has {self.ColumnCount} columns.";
                throw new ArgumentException(msg, nameof(self.ColumnCount));
            }
        }
    }
}