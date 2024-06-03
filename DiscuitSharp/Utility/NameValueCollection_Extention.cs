using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Utils
{
    /// <summary>
    /// Provides extension methods for the NameValueCollection class.
    /// </summary>
    public static class NameValueCollection_Extention
    {
        /// <summary>
        /// Converts a NameValueCollection to a URI query string.
        /// </summary>
        /// <param name="nvc">The NameValueCollection to convert.</param>
        /// <returns>A string that represents the URI query component formed from the NameValueCollection.</returns>
        /// <remarks>
        /// This method iterates over all keys in the NameValueCollection. For each key, it iterates over its values.
        /// It constructs a query string by appending key-value pairs, formatted as 'key=value', separated by '&'.
        /// Each key and value is URL-encoded to ensure it is safe for use in URI query components.
        /// </remarks>
        public static string ToUriQuery(this NameValueCollection nvc)
        {
            bool firstParameter = true;
            StringBuilder querystring = new();
            foreach (string? key in nvc.AllKeys)
            {
                if(key is string keyValue) { 
                    string[]? allValues = nvc.GetValues(keyValue);
                    foreach (string? value in allValues ?? Array.Empty<string>())
                    {
                        if (!firstParameter)
                        {
                            querystring.Append("&");
                        }
                        querystring.Append($"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
                        firstParameter = false;
                    }
                }
            }
            return querystring.ToString();
        }
    }
}
