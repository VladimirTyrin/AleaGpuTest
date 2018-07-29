using System;
using System.Linq;
using System.Threading.Tasks;
using Alea;
using Alea.Parallel;
using BenchmarkDotNet.Attributes;

namespace AleaGpuTest
{
    public class AverageComputationBenchmark
    {
        private readonly float[] _data;
        private const int DataSize = 256 * 1024 * 1024;

        public AverageComputationBenchmark()
        {
            _data = GenerateRandomArray(DataSize);
        }

        [Benchmark]
        public double SimpleAverage()
        {
            return RangeSum(_data, 0, DataSize) / DataSize;
        }

        [Benchmark]
        public double ParallelAverage()
        {
            var parts = Environment.ProcessorCount;
            var partSize = DataSize / parts;
            var averages = new double[parts];

            Parallel.For(0, parts, part =>
            {
                var lowerBound = partSize * part;
                var upperBound = lowerBound + partSize;
                averages[part] = RangeSum(_data, lowerBound, upperBound) / partSize;
            });

            return averages.Average();
        }

        [Benchmark]
        public double GpuAverage()
        {
            var gpu = Gpu.Default;
            return gpu.Average(_data);
        }

        private static double RangeSum(float[] array, int lowerInclusive, int upperExclusize)
        {
            var sum = 0d;
            for (var i = lowerInclusive; i < upperExclusize; ++i)
            {
                sum += array[i];
            }
            return sum;
        }

        private static float[] GenerateRandomArray(int length)
        {
            var random = new Random();
            var array = new float[length];
            for (var i = 0; i < length; i++)
            {
                array[i] = (float)random.NextDouble();
            }

            return array;
        }
    }
}
