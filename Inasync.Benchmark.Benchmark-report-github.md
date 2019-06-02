``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 7 SP1 (6.1.7601.0)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
Frequency=3320380 Hz, Resolution=301.1703 ns, Timer=TSC
.NET Core SDK=2.2.101
  [Host] : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT
  Clr    : .NET Framework 4.7.2 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3416.0
  Core   : .NET Core 2.2.0 (CoreCLR 4.6.27110.04, CoreFX 4.6.27110.04), 64bit RyuJIT


```
|   Method |  Job | Runtime | Categories |     Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------- |----- |-------- |----------- |---------:|----------:|----------:|------:|-------:|------:|------:|----------:|
| EncodeV1 |  Clr |     Clr |     Encode | 4.623 us | 0.0236 us | 0.0221 us |  1.00 | 3.5172 |     - |     - |  10.83 KB |
| EncodeV2 |  Clr |     Clr |     Encode | 5.547 us | 0.0579 us | 0.0542 us |  1.20 | 2.6474 |     - |     - |   8.14 KB |
|   Encode |  Clr |     Clr |     Encode | 2.345 us | 0.0095 us | 0.0089 us |  0.51 | 1.7509 |     - |     - |   5.39 KB |
|          |      |         |            |          |           |           |       |        |       |       |           |
| DecodeV1 |  Clr |     Clr |     Decode | 6.936 us | 0.0350 us | 0.0327 us |  1.00 | 2.9755 |     - |     - |   9.15 KB |
| DecodeV2 |  Clr |     Clr |     Decode | 7.261 us | 0.0466 us | 0.0436 us |  1.05 | 2.9907 |     - |     - |   9.21 KB |
|   Decode |  Clr |     Clr |     Decode | 3.132 us | 0.0110 us | 0.0103 us |  0.45 | 0.3395 |     - |     - |   1.05 KB |
|          |      |         |            |          |           |           |       |        |       |       |           |
| EncodeV1 | Core |    Core |     Encode | 4.547 us | 0.0213 us | 0.0199 us |  1.00 | 3.5172 |     - |     - |  10.82 KB |
| EncodeV2 | Core |    Core |     Encode | 5.341 us | 0.0166 us | 0.0155 us |  1.17 | 2.6474 |     - |     - |   8.14 KB |
|   Encode | Core |    Core |     Encode | 2.447 us | 0.0140 us | 0.0131 us |  0.54 | 1.7509 |     - |     - |   5.39 KB |
|          |      |         |            |          |           |           |       |        |       |       |           |
| DecodeV1 | Core |    Core |     Decode | 5.648 us | 0.0229 us | 0.0214 us |  1.00 | 2.9755 |     - |     - |   9.15 KB |
| DecodeV2 | Core |    Core |     Decode | 6.188 us | 0.0243 us | 0.0227 us |  1.10 | 2.9907 |     - |     - |   9.21 KB |
|   Decode | Core |    Core |     Decode | 3.171 us | 0.0166 us | 0.0155 us |  0.56 | 0.3395 |     - |     - |   1.05 KB |
