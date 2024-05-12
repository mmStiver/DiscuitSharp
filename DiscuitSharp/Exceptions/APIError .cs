using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Exceptions
{
    public record struct APIError(int Status, string? Code, string Message);

}
