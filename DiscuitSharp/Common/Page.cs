using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Core.Common
{
    public abstract class Page<T>
    {
    }
    public interface IPageable<T>
    {
        public Page<T> Next { get; set; }
    }
}
