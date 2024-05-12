using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscuitSharp.Test
{
    internal interface Maybe<T> { }
    
    public readonly record struct Nothing<T> : Maybe<T>;
    public readonly record struct Something<T>(T Value) : Maybe<T>;
    public readonly record struct Fault<T>(string Message) : Maybe<T>;
    public readonly record struct Error<T>(Exception CapturedException) : Maybe<T>;
}
