using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiscuitSharp.Core.Common
{
    public class Cursor<T>
    {
        public List<T>? Records { get; set; }
        public string? Next { get; set; }

        public static implicit operator List<T>?(Cursor<T> cursor) => cursor.Records;
        public static implicit operator string?(Cursor<T> cursor) => cursor.Next;
        public void Deconstruct(out List<T>? Records, out string? Next)
        {
             Records=this.Records;
             Next=this.Next;
        }
    }
}


