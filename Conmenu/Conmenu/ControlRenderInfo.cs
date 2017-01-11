namespace Conmenu
{
    public struct ControlRenderInfo
    {
        public int Line { get; set; }
        public int Width { get; set; }

        public ControlRenderInfo(int line, int width)
        {
            Line = line;
            Width = width;
        }
    }
}
