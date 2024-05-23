using DiscuitSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public class DiscuitClientTest
    {
        IDiscuitClient client = new DiscuitClient();

        [Fact]
        public void GetCookies_NoServerInteraction_LocalSessionAndTokenNotSet()
        {
            Assert.Equal(string.Empty, client.CSRFtoken);
        }
    }
}
