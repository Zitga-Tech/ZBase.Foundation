using System;

namespace ZBase.Foundation.Probability
{
    /// <summary>
    /// <see cref="System"/>.<see cref="Random"/> functions used for <see cref="PseudoProbability"/>.
    /// </summary>
    public readonly struct PRandom : IPRandom
    {
        public float Value => (float)s_rand.NextDouble();

        public float Range(float min, float max)
            => (float)(s_rand.NextDouble() * (max - min) + min);

        private static readonly Random s_rand = new();
    }
}
