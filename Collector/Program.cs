using System;

namespace Collector
{
    class Program
    {
        static void Main(string[] args)
        {
            new SAPExtractor("", "").ExtractData();
        }
    }
}
