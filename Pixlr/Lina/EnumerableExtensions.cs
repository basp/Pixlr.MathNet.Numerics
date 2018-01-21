namespace Pixlr.Lina
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MathNet.Numerics.LinearAlgebra;

    public static class EnumerableExtensions
    {
        public static Vector<T> ToVector<T>(this IEnumerable<T> self)
            where T : struct, IEquatable<T>, IFormattable
            => Vector<T>.Build.Dense(self.ToArray());
    }
}