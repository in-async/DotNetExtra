using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Inasync.Benchmark {

    internal class Program {

        private static void Main(string[] args) {
            var config = ManualConfig.Create(DefaultConfig.Instance)
                //.With(RPlotExporter.Default)
                .With(MarkdownExporter.GitHub)
                .With(MemoryDiagnoser.Default)
                //.With(StatisticColumn.Min)
                //.With(StatisticColumn.Max)
                //.With(RankColumn.Arabic)
                .With(Job.Core)
                .With(Job.Clr)
                //.With(Job.ShortRun)
                //.With(Job.ShortRun.With(BenchmarkDotNet.Environments.Platform.X64).WithWarmupCount(1).WithIterationCount(1))
                .WithArtifactsPath(null)
                ;

            BenchmarkRunner.Run<Benchmark>(config);
        }
    }

    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    public class Benchmark {
        private readonly byte[] _bytes;
        private readonly string _encoded;

        public Benchmark() {
            _bytes = Rand.Bytes(1024);
            _encoded = Base64Url.Encode(_bytes);
        }

        [BenchmarkCategory("Encode"), Benchmark(Baseline = true)]
        public string Encode() => Base64Url.Encode(_bytes);

        [BenchmarkCategory("Decode"), Benchmark(Baseline = true)]
        public byte[] Decode() => Base64Url.Decode(_encoded);
    }
}
