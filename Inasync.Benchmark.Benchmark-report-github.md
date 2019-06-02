``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
Frequency=3320380 Hz, Resolution=301.1703 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
| Method |  Job | Runtime | Categories |     Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------- |----- |-------- |----------- |---------:|----------:|----------:|------:|-------:|------:|------:|----------:|
| Encode |  Clr |     Clr |     Encode | 4.899 us | 0.0685 us | 0.0641 us |  1.00 | 3.5172 |     - |     - |  10.83 KB |
|        |      |         |            |          |           |           |       |        |       |       |           |
| Decode |  Clr |     Clr |     Decode | 6.894 us | 0.0996 us | 0.0932 us |  1.00 | 2.9755 |     - |     - |   9.15 KB |
|        |      |         |            |          |           |           |       |        |       |       |           |
| Encode | Core |    Core |     Encode | 4.668 us | 0.0482 us | 0.0427 us |  1.00 | 3.5172 |     - |     - |  10.82 KB |
|        |      |         |            |          |           |           |       |        |       |       |           |
| Decode | Core |    Core |     Decode | 5.497 us | 0.0985 us | 0.0921 us |  1.00 | 2.9755 |     - |     - |   9.15 KB |
