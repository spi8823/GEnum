using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace GEnum.Benchmark
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class MatchingExtensionsBenchmark
    {
        public int loopCount = 100;
        public TestFlag flag = TestFlag.A;
        public TestFlag A = TestFlag.A;
        public TestFlag B = TestFlag.B;

        [Benchmark]
        public bool Equals()
        {
            for(var i = 0;i < loopCount;i++)
            {
                if (flag == B)
                    return false;
            }
            return true;
        }

        [Benchmark]
        public bool GEnum_Is()
        {
            for(var i = 0;i < loopCount;i++)
            {
                if (flag.IsB())
                    return false;
            }
            return true;
        }
    }
}
