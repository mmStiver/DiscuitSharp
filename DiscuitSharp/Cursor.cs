namespace DiscuitSharp.Core
{
    public record struct CursorIndex(string Value)
    {


        public static explicit operator string(CursorIndex Cursor) => Cursor.Value;
    }
    
}