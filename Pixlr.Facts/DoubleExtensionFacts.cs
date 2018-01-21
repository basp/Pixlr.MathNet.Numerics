namespace Pixlr.Facts
{
    using System;
    using System.Drawing;
    using Xunit;

    public class DoubleExtensionFacts
    {
        [Fact]
        public void ConvertToGrayScaleColor()
        {
            var cases = new[]
            {
                new { V = 0.0, C = Color.FromArgb(  0,   0,   0) },
                new { V = 0.5, C = Color.FromArgb(127, 127, 127) },
                new { V = 1.0, C = Color.FromArgb(255, 255, 255) },
            };

            Array.ForEach(cases, c => Assert.Equal(c.C, c.V.GS()));
        }
    }
}