using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GEnum;

namespace GEnum.Benchmark
{
    [MemoryDiagnoser]
    [ShortRunJob]
    public class FlagsExtensionsBenchmark
    {
        public int loopCount = 100;
        public TestFlag AB = TestFlag.A | TestFlag.B;
        public TestFlag A = TestFlag.A;
        public TestFlag B = TestFlag.B;

        [Benchmark]
        public bool And()
        {
            var result = true;
            for (var i = 0; i < loopCount; i++)
            {
                var f = AB;
                result &= (f & A) == A;
            }
            return result;
        }

        [Benchmark]
        public bool HasFlag()
        {
            var result = true;
            for (var i = 0; i < loopCount; i++)
            {
                var f = AB;
                result &= f.HasFlag(A);
            }
            return result;
        }

        [Benchmark]
        public bool GEnum_Contains()
        {
            var result = true;
            for (var i = 0; i < loopCount; i++)
            {
                var f = AB;
                result &= f.Contains(A);
            }
            return result;
        }

        [Benchmark]
        public TestFlag Or()
        {
            var result = A;
            for (var i = 0; i < loopCount; i++)
            {
                var f = B;
                result |= f;
            }

            return result;
        }

        [Benchmark]
        public TestFlag GEnum_Add()
        {
            var result = A;
            for (var i = 0; i < loopCount; i++)
            {
                var f = B;
                result.Add(f);
            }

            return result;
        }

        [Benchmark]
        public TestFlag GEnum_Remove()
        {
            var result = AB;
            for (var i = 0; i < loopCount; i++)
            {
                var f = B;
                result.Remove(f);
            }

            return result;
        }

        [Benchmark]
        public TestFlag GEnum_Clear()
        {
            var result = AB;
            for (var i = 0; i < loopCount; i++)
            {
                var f = AB;
                f.Clear();
            }

            return result;
        }
    }
}
