using BenchmarkDotNet.Running;

namespace AleaGpuTest
{
    internal static class Program
    {
        private const int ArraySize = 128 * 1024 * 1024;

        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<AverageComputationBenchmark>();
        }
    }
}
