``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1348 (21H1/May2021Update)
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT  [AttachedDebugger]
  Job-VQGXHZ : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  MediumRun  : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT


```
|              Method |        Job | IterationCount | LaunchCount | RunStrategy | UnrollFactor | WarmupCount |            Mean |           Error |          StdDev | Skewness | Kurtosis | Rank |     Gen 0 |   Gen 1 |   Gen 2 |   Allocated |
|-------------------- |----------- |--------------- |------------ |------------ |------------- |------------ |----------------:|----------------:|----------------:|---------:|---------:|-----:|----------:|--------:|--------:|------------:|
|  **AddNewTaskTemplate** | **Job-VQGXHZ** |        **Default** |     **Default** |  **Monitoring** |            **1** |     **Default** |     **14,570.0 ns** |     **8,108.48 ns** |     **5,363.26 ns** |   **0.7755** |    **1.914** |    **3** |         **-** |       **-** |       **-** |     **1,112 B** |
|  AddNewTaskTemplate |  MediumRun |             15 |           2 |     Default |           16 |          10 |        749.0 ns |        44.83 ns |        64.30 ns |  -0.0784 |    2.120 |    2 |    0.0248 |  0.0105 |  0.0010 |       152 B |
| **RemoveSelectedTasks** | **Job-VQGXHZ** |        **Default** |     **Default** |  **Monitoring** |            **1** |     **Default** |     **16,490.0 ns** |    **10,826.99 ns** |     **7,161.39 ns** |   **2.0160** |    **5.773** |    **3** |         **-** |       **-** |       **-** |     **1,664 B** |
| RemoveSelectedTasks |  MediumRun |             15 |           2 |     Default |           16 |          10 |        116.3 ns |         9.72 ns |        13.95 ns |   1.0425 |    3.285 |    1 |    0.0305 |       - |       - |       128 B |
|    **SaveTaskTemplate** | **Job-VQGXHZ** |        **Default** |     **Default** |  **Monitoring** |            **1** |     **Default** |  **5,346,900.0 ns** | **2,469,937.68 ns** | **1,633,710.84 ns** |  **-0.2397** |    **1.779** |    **4** |         **-** |       **-** |       **-** |    **61,984 B** |
|    SaveTaskTemplate |  MediumRun |             15 |           2 |     Default |           16 |          10 | 15,699,333.2 ns | 2,845,997.23 ns | 4,081,644.19 ns |   0.0149 |    1.362 |    5 | 1406.2500 | 93.7500 | 31.2500 | 5,867,970 B |
