﻿using DiscuitSharp.Core.Media;
using DiscuitSharp.Core.Member;

namespace DiscuitSharp.Core.Content
{
    public record struct PublicPostId(string Value)
    {
        public override string ToString() { return Value; }
    }
}
