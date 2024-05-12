using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitDotNet.Core.Common
{
    public class PagerState<TResult>
    {
        public TResult? Outcome { get; set; }
        private Dictionary<string, object?> data = new();

        public PagerState() { }

        public PagerState(TResult? Outcome)
        {
            this.Outcome = Outcome;
        }

        public object? this[string key]
        {
            get {
                if (data.TryGetValue(key, out object? value))
                    return value;
                return null;
            }
            set {
                if(!data.ContainsKey(key)) 
                    data.Add(key, value);
                data[key] = value;
            }
        }
    }
}
