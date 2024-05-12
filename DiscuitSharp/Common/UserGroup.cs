using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Common
{
    public enum UserGroup
    {
        [Description("normal")] Normal,
        [Description("mods")] Moderator,
        [Description("admins")] Administrator
    }
}
