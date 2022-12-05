namespace ZBase.Foundation.Probability
{
    /// <summary>
    /// Random functions used for <see cref="PseudoProbability{TMath, TRandom}"/>.
    /// </summary>
    public interface IPRandom
    {
        /// <summary>
        /// Returns a random number between 0.0 [inclusive] and 1.0 [inclusive]
        /// </summary>
        float Value { get; }

        /// <summary>
        /// Return a random float number between min [inclusive] and max [inclusive]
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        float Range(float min, float max);
    }
}
