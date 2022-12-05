using System;

namespace ZBase.Foundation.Probability
{
    /// <summary>
    /// <see cref="System"/>.<see cref="Math"/> functions used for <see cref="PseudoProbability"/>.
    /// </summary>
    public readonly struct PMath : IPMath
    {
        public float Abs(float f)
            => Math.Abs(f);

        public int CeilToInt(float f)
            => (int)Math.Ceiling(f);

        public float Clamp(float value, float min, float max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }

        public int Clamp(int value, int min, int max)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }

        public float Min(float a, float b)
            => a < b ? a : b;

        public int RoundToInt(float f)
            => (int)Math.Round(f);
    }
}
