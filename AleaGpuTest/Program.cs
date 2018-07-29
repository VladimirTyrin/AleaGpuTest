using BenchmarkDotNet.Running;

namespace AleaGpuTest
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<AverageComputationBenchmark>();
        }
    }
}
