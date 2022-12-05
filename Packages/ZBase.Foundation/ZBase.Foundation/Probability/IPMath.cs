namespace ZBase.Foundation.Probability
{
    /// <summary>
    /// Math functions used for <see cref="PseudoProbability{TMath, TRandom}"/>.
    /// </summary>
    public interface IPMath
    {
        float Abs(float f);

        int CeilToInt(float f);

        int RoundToInt(float f);

        float Min(float a, float b);

        float Clamp(float value, float min, float max);

        int Clamp(int value, int min, int max);
    }
}
