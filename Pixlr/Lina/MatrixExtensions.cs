namespace Pixlr.Lina
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra;

    public static class MatrixExtensions
    {
        public static IEnumerable<V> FlatMap<U, V>(this Matrix<U> self, Func<U, V> f)
              where U : struct, IEquatable<U>, IFormattable
              => self.EnumerateColumns()
                  .SelectMany(u => u.Enumerate())
                  .Select(u => f(u));

        public static Vector<double> Centroid(this Matrix<double> self)
        {
            var col = self.EnumerateColumns()
                .Select(c => c.Centroid())
                .ToVector()
                .Centroid();

            var row = self.EnumerateRows()
                .Select(c => c.Centroid())
                .ToVector()
                .Centroid();

            return Vector<double>.Build.Dense(
                (int)Math.Round(row),
                (int)Math.Round(col));
        }

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