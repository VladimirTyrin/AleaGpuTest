using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace AleaGpuTest
{
    public class FindPrimesBenchmark
    {
        private const int PrimesToFind = 100_000;

        [Benchmark]
        public int[] FindPrimesLinq()
        {
            return NaturalSequence().Where(IsPrime).Take(PrimesToFind).ToArray();
        }

        [Benchmark]
        public int[] FindPrimesCpuBlockParallel()
        {
            var cpuCount = Environment.ProcessorCount;
            const int blockSize = 128 * 1024;
            var buffers = new List<int>[cpuCount];
            var result = new List<int>();

            void InCpuRange(Action<int> action)
            {
                for (var i = 0; i < cpuCount; ++i)
                {
                    action(i);
                }
            }

            InCpuRange(i => buffers[i] = new List<int>());
            
            var offset = 0;

            while (result.Count < PrimesToFind)
            {
                InCpuRange(i => buffers[i].Clear());

                var offsetCopy = offset;
                Parallel.For(0, cpuCount, cpu =>
                {
                    var lowerInclusive = offsetCopy + (cpu * blockSize);

                    buffers[cpu].AddRange(Enumerable.Range(lowerInclusive, blockSize).Where(IsPrime));
                });

                InCpuRange(i => result.AddRange(buffers[i]));
                offset += cpuCount * blockSize;
            }

            return result.Take(PrimesToFind).ToArray();
        }

        private static IEnumerable<int> NaturalSequence()
        {
            var current = 1;
            while (true)
            {
                yield return current;
                ++current;
            }
        }

        private static bool IsPrime(int number)
        {
            var sqrt = (int) Math.Sqrt(number);

            for (var i = 2; i <= sqrt; ++i)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }
    }
}
