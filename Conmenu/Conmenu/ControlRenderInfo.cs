namespace Conmenu
{
    public struct ControlRenderInfo
    {
        /// <summary>
        /// The line index
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// The width
        /// </summary>
        public int Width { get; set; }

        public ControlRenderInfo(int line, int width)
        {
            Line = line;
            Width = width;
        }
    }
}
