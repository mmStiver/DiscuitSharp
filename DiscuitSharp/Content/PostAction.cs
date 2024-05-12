using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Content
{

    public enum PostAction
    {
        [Description("lock")]
        Lock,
        [Description("unlock")]
        Unlock,
        [Description("changeAsUser")]
        ChangeSpeaker,
        [Description("pin")]
        Pin,
        [Description("unpin")]
        Unpin
    }
}
