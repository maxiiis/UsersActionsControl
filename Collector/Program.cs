using System;

namespace Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Count:");
            int count;
            string input = Console.ReadLine();

            bool result = int.TryParse(input, out count);
            while (!result)
            {
                input = Console.ReadLine();
                result = int.TryParse(input, out count);
            }

            Console.WriteLine("Start:");
            int start;
            input = Console.ReadLine();

            result = int.TryParse(input, out start);
            while (!result)
            {
                input = Console.ReadLine();
                result = int.TryParse(input, out start);
            }
            try
            {
                new SAPExtractor("", "").ExtractData(count, start);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
