using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Core.Common
{
    public abstract class TaskPage<TResult> : Page<TResult> , IPageable<TResult>
    {
        public Page<TResult>? Next { get; set; }
    }

    public interface IFetchPageAsync<TResult>
    {
        Task<PagerState<IEnumerable<TResult>>> FetchAsync();
    }
    public class FuturePage<TResult> : TaskPage<TResult>, IFetchPageAsync<TResult>
    {
        public Func<Task<PagerState<IEnumerable<TResult>>>> Operation { get; init; }
        public FuturePage(Func<Task<PagerState<IEnumerable<TResult>>>> operation, Page<TResult>? Next)
        {
            this.Operation = operation;
            this.Next = Next;
        }

        public async Task<PagerState<IEnumerable<TResult>>> FetchAsync()
             => await Operation();
    }

    public class PromisePage<TResult> : TaskPage<TResult>, IFetchPageAsync<TResult>
    {
        public PagerState<IEnumerable<TResult>>? OperationState {get; set; }
        public Func<PagerState<IEnumerable<TResult>>?, Task<PagerState<IEnumerable<TResult>>>> Operation { get; init; }
        public PromisePage(Func<PagerState<IEnumerable<TResult>>?, Task<PagerState<IEnumerable<TResult>>>> operation, Page<TResult>? Next)
        {
            this.Operation = operation;
            this.OperationState = null;
            this.Next = Next;
        }
        public PromisePage(Func<PagerState<IEnumerable<TResult>>?, Task<PagerState<IEnumerable<TResult>>>> operation, PagerState<IEnumerable<TResult>>? OperationState, Page<TResult>? Next) {
            this.Operation = operation;
            this.OperationState = OperationState;
            this.Next = Next;
        }

        public async Task<PagerState<IEnumerable<TResult>>> FetchAsync()
             => await Operation(this.OperationState);
    }
}
