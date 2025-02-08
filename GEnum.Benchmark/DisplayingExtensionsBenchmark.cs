using BenchmarkDotNet.Attributes;

namespace GEnum.Benchmark
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DisplayingExtensionsBenchmark
    {
        public int loopCount = 100;
        public TestFlag A = TestFlag.A;
        [Benchmark]
        public string Enum_ToString()
        {
            for (var i = 0; i < loopCount; i++)
                A.ToString();
            return A.ToString();
        }

        [Benchmark]
        public string GEnum_GetDisplayName()
        {
            for (var i = 0; i < loopCount; i++)
                A.GetDisplayName();
            return A.GetDisplayName();
        }
    }
}
