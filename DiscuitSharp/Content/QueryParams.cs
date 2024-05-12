using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Content
{
    public enum Feed
    {
        Home,
        All,
        Community
    }

    public enum Filter
    {
        All, // Default value
        Deleted,
        Locked
    }

    public enum Sort
    {
        Latest,
        Hot,
        Activity,
        Day,
        Week,
        Month,
        Year,
        All
    }

}
