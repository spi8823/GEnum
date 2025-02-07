using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GEnum.Test
{
    [MatchingExtensions]
    [FlagsExtensions]
    [DisplayingExtensions]
    public enum TestFlag
    {
        None,
        [DisplayName("DisplayA")] A = 1 << 0,
        [DisplayName("DisplayB")] B = 1 << 1,
        C = 1 << 2,
    }
}
