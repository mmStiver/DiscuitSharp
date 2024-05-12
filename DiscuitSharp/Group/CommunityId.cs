using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Group
{
    public record struct CommunityId(string Id)
    {
        public override string ToString() { return Id; }
    };
}
