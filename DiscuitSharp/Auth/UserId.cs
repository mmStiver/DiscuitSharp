using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Auth
{
    public record struct UserId(string Value)
    {
        public override string ToString() { return Value; }
    }
}
