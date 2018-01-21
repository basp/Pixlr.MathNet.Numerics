namespace Pixlr.Lina.Facts
{
    using MathNet.Numerics.LinearAlgebra;
    using Xunit;

    public class Convolution1DTest
    {
        private readonly Vector<double> source;
        private readonly Vector<double> kernel;
        private readonly IConvolution1D<double> convolution;

        public Convolution1DTest()
        {
            this.source = Vector<double>.Build.Dense(new double[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            this.kernel = Vector<double>.Build.Dense(new double[] { 0, 1, 0 });
            this.convolution = this.kernel.Convolution((s, u, v) => s + (u * v), i => 0.0);
        }

        [Fact]
        public void ValidIsSmallerThanSource()
        {
            var w = this.convolution.Valid(this.source);
            Assert.Equal(this.source.Count - 2 * (this.kernel.Count / 2), w.Count);
        }

        [Fact]
        public void SameHasEqualLengthToSource()
        {
            var w = this.convolution.Same(this.source);
            Assert.Equal(this.source.Count, w.Count);
        }

        [Fact]
        public void AllHasLargerLengthThanSource()
        {
            var w = this.convolution.All(this.source);
            Assert.Equal(this.source.Count + 2 * (this.kernel.Count / 2), w.Count);
        }
    }
}