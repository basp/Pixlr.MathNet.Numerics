namespace Pixlr.Lina
{
    using System;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra;

    public static class VectorExtensions
    {
        public static double Centroid(this Vector<double> self)
        {
            var t = self.Enumerate().Select((x, i) => (i + 1) * x).Sum();
            var s = self.Sum();
            return s > 0 ? (t / s) - 1 : 0;
        }

        public static IConvolution1D<U> Convolution<U, T>(
            this Vector<T> self,
            Accumulator<U, T> acc,
            Func<int, U> factory)
            where U : struct, IEquatable<U>, IFormattable
            where T : struct, IEquatable<T>, IFormattable
        {
            self.ValidateKernel();
            return new Convolution1D<U, T>(self, acc, factory);
        }

        private static void ValidateKernel<U>(this Vector<U> self)
            where U : struct, IEquatable<U>, IFormattable
        {
            if (self.Count % 2 == 0)
            {
                var msg = $"You can only create a convolution kernel instance from a vector with an odd size and this vector has {self.Count} elements.";
                throw new ArgumentException(msg, nameof(self.Count));
            }
        }
    }
}