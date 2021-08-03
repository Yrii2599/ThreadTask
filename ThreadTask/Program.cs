using System;
using System.Diagnostics;
using System.Threading.Tasks;
using ThreadSquareEquation;

namespace ThreadTask
{
    class Program
    {
      
            static async Task Main(string[] args)
            {
                Stopwatch stopWatch = new Stopwatch();
                FileWrite f = new FileWrite(10);
                stopWatch.Start();
              f.SolveAndWrite();
                stopWatch.Stop();
                Console.WriteLine(value: $" seconds {stopWatch.Elapsed.Seconds}  ms {stopWatch.Elapsed.Milliseconds}");
            }
        
    }
}
