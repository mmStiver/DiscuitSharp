using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Utils
{
    public static class NameValueCollection_Extention
    {
        public static string ToUriQuery(this NameValueCollection nvc)
        {
            bool firstParameter = true;
            StringBuilder querystring = new();
            foreach (string? key in nvc.AllKeys)
            {
                foreach (var value in nvc?.GetValues(key))
                {
                    if (!firstParameter)
                    {
                        querystring.Append("&");
                    }
                    querystring.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
                    firstParameter = false;
                }
            }
            return querystring.ToString();
        }
    }
}
