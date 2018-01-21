namespace Pixlr.Facts
{
    using System;
    using System.Drawing;
    using Xunit;

    public class ColorExtensionFacts
    {
        [Fact]
        public void CalculateRelativeLuminosity()
        {
            // rlum = 0.2126R + 0.7152G + 0.0722B

            var cases = new[]
            {
                new { C = Color.FromArgb(  0,   0,   0), L = 0.0 },
                new { C = Color.FromArgb( 20,  20,  20), L = 0.1 },
                new { C = Color.FromArgb(128, 128, 128), L = 0.5 },
                new { C = Color.FromArgb(255, 255, 255), L = 1.0 },
            };

            Array.ForEach(cases, c => Assert.Equal(c.L, Math.Round(c.C.Lum(), 1)));
        }
    }
}