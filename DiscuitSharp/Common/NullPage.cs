using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Core.Common
{
    public sealed class NullPage<T> : Page<T>
    {
        public Page<T>? Previous { get; set; }
    }
}
