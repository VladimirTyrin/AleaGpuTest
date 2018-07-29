## Config

CPU: Intel Core i7-6820HK (4 cores / 8 threads)
GPU: NVidia GeForce GTX 1080 (2560 CUDA cores)
Runtime: .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3160.0, GC=Concurrent Workstation

## Result

Find average of 256 * 1024 * 1024 (2^28) floats:

```
                    Method |         Mean |      Error |     StdDev | Scaled | ScaledSD |
-------------------------- |-------------:|-----------:|-----------:|-------:|---------:|
               LinqAverage | 1,407.438 ms | 27.4731 ms | 31.6381 ms |   4.82 |     0.11 |
             SimpleAverage |   292.107 ms |  1.8202 ms |  1.5200 ms |   1.00 |     0.00 |
           ParallelAverage |    42.152 ms |  0.8299 ms |  1.3635 ms |   0.14 |     0.00 |
         VectorizedAverage |    54.029 ms |  0.7476 ms |  0.6243 ms |   0.18 |     0.00 |
 ParallelVectorizedAverage |    35.965 ms |  0.2593 ms |  0.2425 ms |   0.12 |     0.00 |
                GpuAverage |     6.038 ms |  0.0802 ms |  0.0751 ms |   0.02 |     0.00 |
```