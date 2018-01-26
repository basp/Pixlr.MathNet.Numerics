namespace Pixlr.Lina
{
    using System;

    internal class ConvolutionStrategy2D
    {
        public ConvolutionStrategy2D()
        {
        }

        public Tuple<int, int> StartInclusive { get; set; }

        public Tuple<int, int> StopExclusive { get; set; }

        public Tuple<int, int> TargetSize { get; set; }
    }
}