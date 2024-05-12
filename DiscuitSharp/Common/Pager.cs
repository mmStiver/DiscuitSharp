using DiscuitDotNet.Core.Common.Exception;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Core.Common
{
    public sealed class Pager<TResult> : Pager, IEnumerable<TResult>, IAsyncEnumerable<TResult>
    {
        private readonly NullPage<TResult> Null = new NullPage<TResult>();
        public Page<TResult> CurrentPage { get; private set; }
        private Page<TResult> LastPage;
        public PagerSatus Status { get; private set; } = PagerSatus.Ready;
        public int PageCount { get; private set; }

        public Pager() {
            this.CurrentPage = Null;
            this.LastPage = Null;
        }

        public Pager<TResult> Append(IEnumerable<TResult> items)
        {
            if (this.Status == PagerSatus.AsyncProcessing)
                throw new PagerProcessingException();

            DataPage<TResult> newPage = new(items, Null);
            if(this.CurrentPage == Null)
            {
                this.CurrentPage = this.LastPage = newPage;
            }
            else if (this.LastPage is DataPage<TResult> last)
            {
                DataPage<TResult> prev = last;
                this.LastPage = newPage;
                prev.Next = newPage;
            }
            PageCount += 1;
            return this;
        }

        public Pager<TResult> Append(Func<Task<PagerState<IEnumerable<TResult>>>> func)
        {
            this.Status = PagerSatus.AsyncProcessing;

            FuturePage<TResult> newPage = new(func, this.Null);
            if (this.CurrentPage == Null)
            {
                this.CurrentPage = this.LastPage = newPage;
            }
            else if (this.LastPage is DataPage<TResult> last)
            {
                DataPage<TResult> prev = last;
                this.LastPage = newPage;
                prev.Next = newPage;
            }
            else if (this.LastPage is TaskPage<TResult> lastFunc)
            {
                TaskPage<TResult> prev = lastFunc;
                this.LastPage = newPage;
                prev.Next = newPage;
            }
            PageCount += 1;
           return this;
        }
        public Pager<TResult> Append(Func<PagerState<IEnumerable<TResult>>?, Task<PagerState<IEnumerable<TResult>>>> func, object? InitialState = null)
        {
            this.Status = PagerSatus.AsyncProcessing;
            var state = new PagerState<IEnumerable<TResult>>();
            state["init"] = InitialState;
            PromisePage<TResult> newPage = new(func, state, this.Null);
            if (this.CurrentPage == Null)
            {
                this.CurrentPage = this.LastPage = newPage;
            }
            else if (this.LastPage is DataPage<TResult> last)
            {
                DataPage<TResult> prev = last;
                this.LastPage = newPage;
                prev.Next = newPage;
            }
            else if (this.LastPage is TaskPage<TResult> lastFunc)
            {
                TaskPage<TResult> prev = lastFunc;
                this.LastPage = newPage;
                prev.Next = newPage;
            }
            PageCount += 1;
           return this;
        }
        
        public async Task<Pager<TResult>> FetchNextAsync()
        {
            if (Status == PagerSatus.AsyncProcessing)
            {
                if (this.CurrentPage == Null)
                    goto end;

                Page<TResult>? page = CurrentPage;
                while (page is DataPage<TResult> dp && dp.Next is DataPage<TResult>)
                    page = dp.Next;

                DataPage<TResult>? prev = page as DataPage<TResult>;
                Page<TResult> taskPage = (page is DataPage<TResult> a) ? a.Next
                    : (CurrentPage is TaskPage<TResult>) ? CurrentPage : Null;

                if (taskPage is IFetchPageAsync<TResult> fp)
                {
                    var result = await fp.FetchAsync();
                    var data = result.Outcome;
                    Page<TResult> nextPage = this.Null;

                    if (fp is IPageable<TResult> iter)
                    {
                        if (iter.Next is PromisePage<TResult> promise)
                            promise.OperationState = result;
                        nextPage = iter.Next;
                    }
                    var newDataPage = new DataPage<TResult>(data ?? Enumerable.Empty<TResult>(), nextPage);
                    if (prev != null)
                        prev.Next = newDataPage;
                    else this.CurrentPage = newDataPage;

                    if (nextPage == Null)
                    {
                        this.LastPage = newDataPage;
                        goto end;
                    }
                    return this;
                }

            end:
                this.Status = PagerSatus.Ready;
            }
            
            return this;
        }

        //private async FutureState<TState, Task<IEnumerable<TResult>> Exec (FuturePage<TResult> func)
        //{
        //    return await func.Operation.Invoke();
        //}

        //private async FutureState<TState, Task<IEnumerable<TResult>> Exec<TState>(TState state, FuturePage<TState, TResult> func)
        //{
        //    return await func.Operation.Invoke(state);
        //}

        public Pager<TResult> Prepend(IEnumerable<TResult> items)
        { 
            if(CurrentPage is DataPage<TResult> dp)
                this.CurrentPage = new DataPage<TResult>(items, dp);
            else if (CurrentPage == this.Null)
                this.CurrentPage = this.LastPage = new DataPage<TResult>(items, this.CurrentPage);
            else
                this.CurrentPage = new DataPage<TResult>(items, this.CurrentPage);

            PageCount += 1;
            return this;
        }

        public IEnumerator<TResult> GetEnumerator()
        {
            var processingPage = this.CurrentPage;
            while(processingPage is DataPage<TResult> pp)
            {
                foreach (TResult item in pp.Value)
                {
                    yield return item;
                }
                processingPage = pp.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IAsyncEnumerator<TResult> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }
    public abstract class Pager
    {
        public enum PagerSatus
        {
            Ready = 0,
            AsyncProcessing = 1
        }
    }
}
