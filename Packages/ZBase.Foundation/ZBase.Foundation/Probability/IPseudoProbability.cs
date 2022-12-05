namespace ZBase.Foundation.Probability
{
    public interface IPseudoProbability
    {
        void Initialize();

        bool FindLuck(float chance);

        bool FindLuck(float chance, int n, int n_0, out int n_1);

        bool FindLuckInRange100(float chance);

        bool FindLuckInRange100(float chance, int n, int n_0, out int n_1);

        bool FindLuckInRange100(int chance);

        bool FindLuckInRange100(int chance, int n, int n_0, out int n_1);

        bool FindLuckInRange1000(float chance, int n, int n_0, out int n_1);

        bool FindLuckInRange1000(int chance, int n, int n_0, out int n_1);
    }
}