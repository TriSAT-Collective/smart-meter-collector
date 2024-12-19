using System;
using System.Threading.Tasks;

namespace SmartMeterCollector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var collector = new MessageCollector();
            await collector.StartCollectingAsync();

            // Keep the application running
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            await collector.DisposeAsync();
        }
    }
}