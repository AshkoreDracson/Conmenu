namespace Conmenu
{
    static class StringExt
    {
        public static string Clamp(this string s, int length)
        {
            if (s.Length > length)
                return s.Substring(0, length);

            return s;
        }
    }
}
