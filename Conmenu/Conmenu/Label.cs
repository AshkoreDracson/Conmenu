namespace Conmenu
{
    public class Label : Control
    {
        public string Text { get; set; }
        public TextAlignement TextAlignement { get; set; } = TextAlignement.Left;

        public Label(Menu parent, string text) : base(parent)
        {
            Selectable = false;
            Text = text;
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;

            if (TextAlignement == TextAlignement.Left)
                System.Console.CursorLeft = Padding;
            if (TextAlignement == TextAlignement.Middle)
                System.Console.CursorLeft = ParentMenu.Width / 2 - Text.Length / 2;
            else if (TextAlignement == TextAlignement.Right)
                System.Console.CursorLeft = ParentMenu.Width - Text.Length - Padding;

            System.Console.Write(Text);
            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }
    }
}