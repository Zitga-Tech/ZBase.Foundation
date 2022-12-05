using System.Collections.Generic;

namespace ZBase.Foundation.Probability
{
    public sealed class PseudoProbability
        : PseudoProbability<PMath, PRandom>
    { }

    public class PseudoProbability<TMath, TRandom>
        : IPseudoProbability
        where TMath : IPMath, new()
        where TRandom : IPRandom, new()
    {
        private readonly Dictionary<int, float> _cValues;
        private readonly TMath _math;
        private readonly TRandom _rand;

        public PseudoProbability()
        {
            _math = new();
            _rand = new();
            _cValues = new();

            Initialize();
        }

        public void Initialize()
        {
            var cValues = _cValues;
            cValues.Clear();

            for (var p = 1; p < 1000; p++)
            {
                var c = PseudoRandomDistribution.GetCFromP(p / 1000f, _math);
                cValues.Add(p, c * 100f);
            }
        }

        /// <param name="chance">In the range of [0.0, 1.0]</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuck(float chance)
        {
            var r = _rand.Value;
            return r <= chance;
        }

        /// <param name="chance">In the range of [1.0, 100.0]</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuckInRange100(float chance)
        {
            var r = _rand.Range(1f, 101f);
            return r <= chance;
        }

        /// <param name="chance">In the range of [1, 100]</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuckInRange100(int chance)
        {
            var r = _rand.Range(1, 101);
            return r <= chance;
        }

        /// <param name="chance">In the range of [0.0, 1.0]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuck(float chance, int n, int n_0, out int n_1)
        {
            var thousand = _math.RoundToInt(_math.Clamp(chance, 0f, 1f) * 1000);

            if (thousand <= 0)
                thousand = 1;

            return FindLuck_Internal(thousand, n, n_0, out n_1);
        }

        /// <param name="chance">In the range of [1, 100]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuckInRange100(int chance, int n, int n_0, out int n_1)
        {
            var thousand = _math.Clamp(chance, 1, 100) * 10;
            return FindLuck_Internal(thousand, n, n_0, out n_1);
        }

        /// <param name="chance">In the range of [1.0, 100.0]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuckInRange100(float chance, int n, int n_0, out int n_1)
        {
            var thousand = _math.RoundToInt(_math.Clamp(chance, 1f, 100f) * 10);
            return FindLuck_Internal(thousand, n, n_0, out n_1);
        }

        /// <param name="chance">In the range of [1, 1000]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuckInRange1000(int chance, int n, int n_0, out int n_1)
        {
            var thousand = _math.Clamp(chance, 1, 1000);
            return FindLuck_Internal(thousand, n, n_0, out n_1);
        }

        /// <param name="chance">In the range of [1.0, 1000.0]</param>
        /// <param name="n"></param>
        /// <param name="n_0">Number of failed attempts</param>
        /// <param name="n_1">n_0 if succeeded or n+1 if failed</param>
        /// <returns>TRUE if found</returns>
        public bool FindLuckInRange1000(float chance, int n, int n_0, out int n_1)
        {
            var thousand = _math.RoundToInt(_math.Clamp(chance, 1f, 1000f));
            return FindLuck_Internal(thousand, n, n_0, out n_1);
        }

        private bool FindLuck_Internal(int thousand, int n, int n_0, out int n_1)
        {
            if (thousand < 1000)
            {
                var cValues = _cValues;
                var c = cValues[thousand];
                var p = c * n;
                var r = _rand.Value * 100f;

                if (r > p)
                {
                    n_1 = n + 1;
                    return false;
                }
            }

            n_1 = n_0;
            return true;
        }
    }
}
