using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Utility
{
    public interface IMutableState<TValue>
    {
        public ReadOnlyDictionary<string, TValue?> MutatedState { get; }
    }
}
