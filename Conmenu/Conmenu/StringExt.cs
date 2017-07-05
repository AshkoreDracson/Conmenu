namespace Conmenu
{
    static class StringExt
    {
        public static string Clamp(this string s, int length)
        {
            return s.Length > length ? s.Substring(0, length) : s;
        }
    }
}
