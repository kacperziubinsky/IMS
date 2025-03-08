namespace Mobilka
{
    class Poisson
    {
        private Random random;

        public Poisson(Random random)
        {
            this.random = random;
        }

        private double GenU()
        {
            return random.NextDouble();
        }

        private double GenE()
        {
            return -Math.Log(GenU());
        }

        public int Generate(double lambda)
        {
            int x = -1;
            double s = 1;
            double q = Math.Exp(-lambda);

            while (s > q)
            {
                double u = GenU();
                s = s * u;
                x++;
            }

            return x;
        }
    }
}