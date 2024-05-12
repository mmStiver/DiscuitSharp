using DiscuitDotNet.Core.Auth;
using DiscuitDotNet.Core.Common;
using DiscuitDotNet.Core.Common.Exception;
using DiscuitDotNet.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace DiscuitDotNet.Test.IPagedResults
{
    public class PagerTest
    {

        [Fact]
        public void CurrentPage_DefaultState_PagerReturnsNullPage()
        {
            var pager = new Pager<int>();

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<NullPage<int>>(pager.CurrentPage);
        }

        [Fact]
        public void CurrentPage_PrependEmptyList_PagerReturnsEmptyDataPage()
        {
            var pager = new Pager<int>();
            int[] items = new int[0];

            pager.Prepend(items);

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<DataPage<int>>(pager.CurrentPage);
            Assert.Empty(pager);
        }
        [Fact]
        public void CurrentPage_AppendEmptyList_PagerReturnsEmptyDataPage()
        {
            var pager = new Pager<int>();
            int[] items = new int[0];

            pager.Append(items);

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<DataPage<int>>(pager.CurrentPage);
            Assert.Empty(pager);
        }
        [Fact]
        public void CurrentPage_PrependListOfItems_PagerReturnsDataPage()
        {
            var pager = new Pager<int>();
            int[] items = new int[1] { 1 };

            pager.Prepend(items);

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<DataPage<int>>(pager.CurrentPage);
            Assert.Collection<int>(pager, item => Assert.Equal(1, item));
        }
        [Fact]
        public void Iterate_PrependThreeListOfItems_PageCountIncreasesByThree()
        {
            var pager = new Pager<int>();
            int[] items1 = new int[1] { 1 };
            int[] items2 = new int[0] { };
            int[] items3 = new int[2] { 3, 4 };

            pager.Prepend(items1);
            pager.Prepend(items2);
            pager.Prepend(items3);

            Assert.Equal(3, pager.PageCount);
        }
        [Fact]
        public void Iterate_AppendThreeListOfItems_PageCountIncreasesByThree()
        {
            var pager = new Pager<int>();
            int[] items1 = new int[1] { 1 };
            int[] items2 = new int[0] { };
            int[] items3 = new int[2] { 4, 4 };

            pager.Append(items1);
            pager.Append(items2);
            pager.Append(items3);

            Assert.Equal(3, pager.PageCount);
        }
        [Fact]
        public void Iterate_AppendVariableLengthLists_PagerReturnsAllPagedItems()
        {
            var pager = new Pager<int>();

            pager.Append(new int[1] { 1, });
            pager.Append(new int[0] { });
            pager.Append(new int[3] { 2, 3, 5 });

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<DataPage<int>>(pager.CurrentPage);
            Assert.Collection<int>(pager,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item)

                );
        }
        [Fact]
        public void Iterate_PrependVariableLengthLists_PagerReturnsAllPagedItems()
        {
            var pager = new Pager<int>();

            pager.Prepend(new int[1] { 1, });
            pager.Prepend(new int[0] { });
            pager.Prepend(new int[3] { 2, 3, 5 });

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<DataPage<int>>(pager.CurrentPage);
            Assert.Collection<int>(pager,
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item),
                item => Assert.Equal(1, item)

                );
        }

        [Fact]
        public void State_Default_StateReady()
        {
            var pager = new Pager<int>();

            Assert.Equal(Pager.PagerSatus.Ready, pager.Status);
        }

        [Fact]
        public void State_AppendAsyncProcess_SetStateToProcessing()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 10 });
                return new PagerState<IEnumerable<int>>(response);
            });

            Assert.Equal(Pager.PagerSatus.AsyncProcessing, pager.Status);
        }

        [Fact]
        public void State_AsyncProcessing_AppendListThrowsException()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 10 });
                return new PagerState<IEnumerable<int>>(response);
            });

            Pager<int> act() => pager.Append(new int[1] { 10, }); ;

            var exception = Assert.Throws<PagerProcessingException>(act);
        }


        [Fact]
        public async Task State_FetchNextAsync_ResetStateToIsReady()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 10 });
                return new PagerState<IEnumerable<int>>(response);
            }); ;
            _ = await pager.FetchNextAsync();
            _ = pager.Append(new int[1] { 1, });

            Assert.Equal(Pager.PagerSatus.Ready, pager.Status);
        }

        [Fact]
        public async Task State_AppendDataAFterFetchNext_ReturnAllItems()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            });
            _ = await pager.FetchNextAsync();
            _ = pager.Append(new int[2] { 1, 5 });

            Assert.Collection<int>(pager,
                item => Assert.Equal(10, item),
                item => Assert.Equal(8, item),
                item => Assert.Equal(1, item),
                item => Assert.Equal(5, item)

                );
        }

        [Fact]
        public void Iterate_AppendAsyncronousPage_PagerReturnsDataPagedItems()
        {
            var pager = new Pager<int>();

            pager.Append(new int[1] { 1, });
            pager.Append(new int[0] { });
            pager.Append(new int[3] { 2, 3, 5 });
            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            });

            Assert.NotNull(pager.CurrentPage);
            Assert.IsType<DataPage<int>>(pager.CurrentPage);
            Assert.Collection<int>(pager,
                item => Assert.Equal(1, item),
                item => Assert.Equal(2, item),
                item => Assert.Equal(3, item),
                item => Assert.Equal(5, item)

                );
        }

        [Fact]
        public async Task Fetch_PagerPendingAsync_ResetStatusToReady()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            });

            await pager.FetchNextAsync();
            Assert.Equal(Pager.PagerSatus.Ready, pager.Status);
        }

        [Fact]
        public async Task Fetch_T_PagerPendingAsyncPassState_StatusPending()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            }); pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            });

            await pager.FetchNextAsync();
            Assert.Equal(Pager.PagerSatus.AsyncProcessing, pager.Status);
        }
        [Fact]
        public async Task Fetch_T_FetchAllAsyncPassState_StatusReady()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            });
            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[2] { 10, 8 });
                return new PagerState<IEnumerable<int>>(response);
            });
            await pager.FetchNextAsync();
            await pager.FetchNextAsync();
            Assert.Equal(Pager.PagerSatus.Ready, pager.Status);
        }

        [Fact]
        public async Task Fetch_EmptyPagerPendingAsync_ResolveFunctionAppendResult()
        {
            var pager = new Pager<int>();
            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 1 });
                return new PagerState<IEnumerable<int>>(response);
            });

            await pager.FetchNextAsync();

            Assert.Collection<int>(pager,
                item => Assert.Equal(1, item)
            );
        }

        [Fact]
        public async Task Fetch_FullPagerPendingAsync_ResolveFunctionAppendResult()
        {
            var pager = new Pager<int>();

            pager.Prepend(new int[1] { 2 });
            pager.Prepend(new int[0] { });
            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 1 });
                return new PagerState<IEnumerable<int>>(response);
            });

            await pager.FetchNextAsync();

            Assert.Collection<int>(pager,
                item => Assert.Equal(2, item),
                item => Assert.Equal(1, item)

                );
        }
        [Fact]
        public async Task Fetch_QueueTwoAsync_ResolveOneAsyncAppendResult()
        {
            var pager = new Pager<int>();
            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 10 });
                return new PagerState<IEnumerable<int>>(response);
            }); pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 21 });
                return new PagerState<IEnumerable<int>>(response);
            });

            await pager.FetchNextAsync();

            Assert.Collection<int>(pager,
                item => Assert.Equal(10, item)
                );
        }
        [Fact]
        public async Task Fetch_QueueTwoAsync_ResolveAllAsyncAppendResult()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 10 });
                return new PagerState<IEnumerable<int>>(response);
            }); pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 21 });
                return new PagerState<IEnumerable<int>>(response);
            });

            await pager.FetchNextAsync();
            await pager.FetchNextAsync();

            Assert.Collection<int>(pager,
                item => Assert.Equal(10, item),
                item => Assert.Equal(21, item)

                );
        }
        [Fact]
        public async Task Fetch_QueueAndUpdateStateInTask_IntakeStateInNextTask()
        {
            var pager = new Pager<int>();

            pager.Append(async () =>
            {
                var response = await Task.FromResult<IEnumerable<int>>(new int[1] { 10 });
                var state = new PagerState<IEnumerable<int>>(response);
                state["Next"] = 9;
                return state;
            });
            pager.Append(async (state) =>
            {
                if (state?["Next"] is null)
                    throw new ArgumentNullException();
                IEnumerable<int> data = Enumerable.Empty<int>();
                if (state["Next"] is int i)
                {
                    data = await Task.FromResult<IEnumerable<int>>(new int[1] { i });
                }
                return new PagerState<IEnumerable<int>>(data);
            });

            await pager.FetchNextAsync();
            await pager.FetchNextAsync();

            Assert.Collection<int>(pager,
                item => Assert.Equal(10, item),
                item => Assert.Equal(9, item)

                );
        }
    }
}


