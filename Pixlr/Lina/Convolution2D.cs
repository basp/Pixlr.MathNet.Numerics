namespace Pixlr.Lina
{
    using System;
    using System.Threading.Tasks;
    using MathNet.Numerics.LinearAlgebra;

    internal class Convolution2D<U, V> : IConvolution2D<U>
        where U : struct, IEquatable<U>, IFormattable
        where V : struct, IEquatable<V>, IFormattable
    {
        private readonly Matrix<V> v;   // kernel   
        private readonly Tuple<int, int> vc;
        private readonly Accumulator<U, V> acc;
        private readonly Func<int, int, U> factory;

        public Convolution2D(
            Matrix<V> v,
            Accumulator<U, V> acc,
            Func<int, int, U> factory)
        {
            this.v = v;
            this.vc = Tuple.Create(v.RowCount / 2, v.ColumnCount / 2);
            this.acc = acc;
            this.factory = factory;
        }

        public Matrix<U> Valid(Matrix<U> u)
        {
            var strat = new ConvolutionStrategy2D
            {
                StartInclusive = this.vc,

                StopExclusive = Tuple.Create(
                    u.RowCount - this.vc.Item1,
                    u.ColumnCount - this.vc.Item2),

                TargetSize = Tuple.Create(
                    u.RowCount - 2 * this.vc.Item1,
                    u.ColumnCount - 2 * this.vc.Item2),
            };

            return this.Convolve(u, strat);
        }

        public Matrix<U> Same(Matrix<U> u)
        {
            var strat = new ConvolutionStrategy2D
            {
                StartInclusive = Tuple.Create(0, 0),

                StopExclusive = Tuple.Create(
                    u.RowCount,
                    u.ColumnCount),

                TargetSize = Tuple.Create(
                    u.RowCount,
                    u.ColumnCount),
            };

            return this.Convolve(u, strat);
        }

        public Matrix<U> All(Matrix<U> u)
        {
            var strat = new ConvolutionStrategy2D
            {
                StartInclusive = Tuple.Create(
                    -this.vc.Item1,
                    -this.vc.Item2),

                StopExclusive = Tuple.Create(
                    u.RowCount + this.vc.Item1,
                    u.ColumnCount + this.vc.Item2),

                TargetSize = Tuple.Create(
                    u.RowCount + 2 * this.vc.Item1,
                    u.ColumnCount + 2 * this.vc.Item2),
            };

            return this.Convolve(u, strat);
        }

        internal U Accumulate(int row, int col, Matrix<U> u)
        {
            var s = default(U);
            for (var i = -this.vc.Item1; i <= this.vc.Item1; i++)
            {
                for (var j = -this.vc.Item2; j <= this.vc.Item2; j++)
                {
                    var ii = row + i;
                    var jj = col + j;

                    var vv = this.v[i + this.vc.Item1, j + this.vc.Item2];
                    var uv = ii < 0 || ii >= u.RowCount || jj < 0 || jj >= u.ColumnCount
                        ? this.factory(ii, jj)
                        : u[ii, jj];

                    s = this.acc(s, uv, vv);
                }
            }

            return s;
        }

        internal Matrix<U> Convolve(Matrix<U> u, ConvolutionStrategy2D strat)
        {
            var w = Matrix<U>.Build.Dense(
                strat.TargetSize.Item1,
                strat.TargetSize.Item2,
                (r, c) => default(U));

            Parallel.For(strat.StartInclusive.Item1, strat.StopExclusive.Item1, r => {
                for (var c = strat.StartInclusive.Item2; c < strat.StopExclusive.Item2; c++)
                {
                    var s = this.Accumulate(r, c, u);
                    w[r - strat.StartInclusive.Item1, c - strat.StartInclusive.Item2] = s;
                }
            });

            return w;
        }
    }
}