using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEnum.Test
{
    [MatchingExtensions]
    [FlagsExtensions]
    public enum TestFlag
    {
        None = 0,
        A = 1 << 0,
        B = 1 << 1,
        C = 1 << 2,
    }
}
