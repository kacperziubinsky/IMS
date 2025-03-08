namespace Mobilka
{
    class Gauss
    {
        private Random random;

        public Gauss(Random random)
        {
            this.random = random;
        }

        private double GenU()
        {
            return random.NextDouble();
        }

        public double Generate(double mean, double stdDev)
        {
            double v1, v2, s;

            do
            {
                v1 = 2 * GenU() - 1;
                v2 = 2 * GenU() - 1;
                s = v1 * v1 + v2 * v2;
            } while (s >= 1 || s == 0);

            double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);

            double standardNormal = v1 * multiplier;

            return mean + stdDev * standardNormal;
        }
    }
}