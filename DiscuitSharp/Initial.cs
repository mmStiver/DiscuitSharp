using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscuitSharp.Core.Admin;
using DiscuitSharp.Core.Auth;
using DiscuitSharp.Core.Group;

namespace DiscuitSharp.Core
{
    public class Initial
    {
        public Credentials? User;

        public List<ReportReason>? ReportReasons { get; init; }
        public List<Community>? Communities { get; init; }
        public List<Community>? BannedFrom { get; init; }
        public int noUsers { get; set; }
    }
}
