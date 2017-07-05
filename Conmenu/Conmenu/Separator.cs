namespace Conmenu
{
    public class Separator : Control
    {
        /// <summary>
        /// The text for this separator
        /// </summary>
        public string Text { get; set; }

        public Separator(Menu parent, string text = "") : base(parent)
        {
            Selectable = false;
            Text = text;
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;
            System.Console.CursorLeft = ParentMenu.Width / 2 - Text.Length / 2;
            System.Console.Write(Text);
            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }
    }
}
