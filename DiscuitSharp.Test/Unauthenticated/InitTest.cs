//using DiscuitSharp.Test.Unauthenticated.Unauthorized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test.Unauthenticated
{
    public class InitTest : IClassFixture<NoAuthTestFixture>
    {
        readonly NoAuthTestFixture context;
        public InitTest(NoAuthTestFixture fixture)
        {
            context = fixture;
        }

        [Fact]
        public async Task GetInitial_ServerOK_ReturnInitialResponse()
        {
            var client = context.CreateClient();

            var initial = await client.GetInitial();
            Assert.NotNull(initial);
        }

        [Fact]
        public async Task GetInitial_ServerOK_UserNotInResponse()
        {
            var client = context.CreateClient();

            var initial = await client.GetInitial();
            Assert.Null(initial?.User);
        }

        [Fact]
        public async Task GetInitial_ServerOK_IncludeReportReasonsInResponse()
        {
            var client = context.CreateClient();

            var initial = await client.GetInitial();
            Assert.NotNull(initial?.ReportReasons);
        }

        [Fact]
        public async Task GetInitial_ServerOK_IncludeCommunitiesInResponse()
        {
            var client = context.CreateClient();

            var initial = await client.GetInitial();
            Assert.NotNull(initial?.Communities);
        }

        [Fact]
        public async Task GetInitial_ServerOK_BannedNotInResponse()
        {
            var client = context.CreateClient();

            var initial = await client.GetInitial();
            Assert.Null(initial?.BannedFrom);
        }

        [Fact]
        public async Task GetInitial_ServerOK_StoreCSRFTokenCookie()
        {
            var client = context.CreateClient();

            var _ = await client.GetInitial();
            Assert.NotNull(client.CSRFtoken);
        }
    }
}
