namespace Mobilka
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Generator rozkładów dyskretnych i ciągłych");
            Console.WriteLine("=========================================");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nWybierz opcję:");
                Console.WriteLine("1. Generator rozkładu Poissona");
                Console.WriteLine("2. Generator rozkładu Normalnego (Gaussa)");
                Console.WriteLine("3. Wyjście");
                Console.Write("\nTwój wybór: ");

                if (int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            GeneratePoissonDistribution();
                            break;
                        case 2:
                            GenerateNormalDistribution();
                            break;
                        case 3:
                            exit = true;
                            break;
                        default:
                            Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                }
            }
        }

        private static void InitializeRandom(out Random random)
        {
            Console.Write("Czy chcesz użyć ziarna (t/n)? ");
            string answer = Console.ReadLine().ToLower();
            if (answer == "t")
            {
                Console.Write("Podaj wartość ziarna: ");
                if (int.TryParse(Console.ReadLine(), out int seed))
                {
                    random = new Random(seed);
                    Console.WriteLine($"Zainicjalizowano generator z ziarnem {seed}");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa wartość ziarna. Używam generatora bez ziarna.");
                    random = new Random();
                }
            }
            else
            {
                random = new Random();
                Console.WriteLine("Zainicjalizowano generator bez ziarna");
            }
        }

        private static void GeneratePoissonDistribution()
        {
            Console.WriteLine("\n=== Generator rozkładu Poissona ===");
            
            Console.Write("Podaj parametr λ: ");
            if (!double.TryParse(Console.ReadLine(), out double lambda) || lambda <= 0)
            {
                Console.WriteLine("Nieprawidłowa wartość. Używam domyślnej wartości λ = 5");
                lambda = 5;
            }
            
            Console.Write("Podaj liczbę próbek: ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Nieprawidłowa wartość. Używam domyślnej wartości 1000");
                count = 1000;
            }
            
            InitializeRandom(out Random random);
            Poisson poissonGenerator = new Poisson(random);
            
            List<int> samples = new List<int>();
            Console.WriteLine($"\nGeneruję {count} liczb z rozkładu Poissona (λ = {lambda})...");
            
            for (int i = 0; i < count; i++)
            {
                samples.Add(poissonGenerator.Generate(lambda));
            }
            
            DisplayHistogram(samples.ConvertAll(x => (double)x), true);
            
            Console.WriteLine("\nPrzykładowe wygenerowane liczby:");
            for (int i = 0; i < Math.Min(10, count); i++)
            {
                Console.Write($"{samples[i]} ");
            }
            Console.WriteLine();
            
            Console.WriteLine($"\nŚrednia: {samples.Average():F4}");
            Console.WriteLine($"Teoretyczna średnia: {lambda:F4}");
            double variance = samples.Select(x => Math.Pow(x - samples.Average(), 2)).Sum() / samples.Count;
            Console.WriteLine($"Wariancja: {variance:F4}");
            Console.WriteLine($"Teoretyczna wariancja: {lambda:F4}");
        }

        private static void GenerateNormalDistribution()
        {
            Console.WriteLine("\n=== Generator rozkładu Normalnego (Gaussa) ===");
            
            Console.Write("Podaj wartość średnią (μ): ");
            if (!double.TryParse(Console.ReadLine(), out double mean))
            {
                Console.WriteLine("Nieprawidłowa wartość. Używam domyślnej wartości μ = 0");
                mean = 0;
            }
            
            Console.Write("Podaj odchylenie standardowe (σ): ");
            if (!double.TryParse(Console.ReadLine(), out double stdDev) || stdDev <= 0)
            {
                Console.WriteLine("Nieprawidłowa wartość. Używam domyślnej wartości σ = 1");
                stdDev = 1;
            }
            
            Console.Write("Podaj liczbę próbek: ");
            if (!int.TryParse(Console.ReadLine(), out int count) || count <= 0)
            {
                Console.WriteLine("Nieprawidłowa wartość. Używam domyślnej wartości 1000");
                count = 1000;
            }
            
            InitializeRandom(out Random random);
            Gauss gaussGenerator = new Gauss(random);
            
            List<double> samples = new List<double>();
            Console.WriteLine($"\nGeneruję {count} liczb z rozkładu Normalnego (μ = {mean}, σ = {stdDev})...");
            
            for (int i = 0; i < count; i++)
            {
                samples.Add(gaussGenerator.Generate(mean, stdDev));
            }
            
            DisplayHistogram(samples, false);
            
            Console.WriteLine("\nPrzykładowe wygenerowane liczby:");
            for (int i = 0; i < Math.Min(10, count); i++)
            {
                Console.Write($"{samples[i]:F4} ");
            }
            Console.WriteLine();
            
            Console.WriteLine($"\nŚrednia: {samples.Average():F4}");
            Console.WriteLine($"Teoretyczna średnia: {mean:F4}");
            double variance = samples.Select(x => Math.Pow(x - samples.Average(), 2)).Sum() / samples.Count;
            Console.WriteLine($"Wariancja: {variance:F4}");
            Console.WriteLine($"Odchylenie standardowe: {Math.Sqrt(variance):F4}");
            Console.WriteLine($"Teoretyczna wariancja: {stdDev * stdDev:F4}");
        }

        private static void DisplayHistogram(List<double> samples, bool isDiscrete)
        {
            int numBins;
            double minValue = samples.Min();
            double maxValue = samples.Max();
            
            if (isDiscrete)
            {
                numBins = (int)(maxValue - minValue + 1);
            }
            else
            {
                numBins = (int)(Math.Ceiling(Math.Log(samples.Count, 2) + 1));
                numBins = Math.Min(numBins, 20);
            }
            
            double binWidth = (maxValue - minValue) / numBins;
            
            int[] frequencies = new int[numBins];
            foreach (double sample in samples)
            {
                int binIndex = (int)((sample - minValue) / binWidth);
                if (binIndex >= 0 && binIndex < numBins)
                {
                    frequencies[binIndex]++;
                }
            }
            
            int maxFrequency = frequencies.Max();
            int histogramWidth = 50;
            
            Console.WriteLine("\nHistogram:");
            Console.WriteLine(new string('-', 70));
            
            for (int i = 0; i < numBins; i++)
            {
                double binStart = minValue + i * binWidth;
                double binEnd = binStart + binWidth;
                
                if (isDiscrete)
                {
                    Console.Write($"{(int)binStart,3}: ");
                }
                else
                {
                    Console.Write($"{binStart,7:F2} - {binEnd,7:F2}: ");
                }
                
                int barLength = (int)Math.Round((double)frequencies[i] / maxFrequency * histogramWidth);
                Console.Write(new string('█', barLength));
                Console.WriteLine($" {frequencies[i]}");
            }
            
            Console.WriteLine(new string('-', 70));
        }
    }

   

    
}