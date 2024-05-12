using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Core.Common
{
    public class DataPage<T> : Page<T>, IPageable<T>
    {
        public IEnumerable<T> Value { get; set; }
        public Page<T> Next { get; set; }
        public DataPage(IEnumerable<T> Value, Page<T> Next) {
            this.Value = Value;
            this.Next = Next;
        }
    }
}
