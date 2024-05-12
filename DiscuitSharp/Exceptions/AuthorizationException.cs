using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Exceptions
{
    public class InsufficientPrivledgesException : InvalidOperationException
    {
        public InsufficientPrivledgesException()
        {
        }

        public InsufficientPrivledgesException(string message)
            : base(message)
        {
        }

        public InsufficientPrivledgesException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
