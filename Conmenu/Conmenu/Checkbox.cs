namespace Conmenu
{
    public class Checkbox : Control
    {
        /// <summary>
        /// The default checked char for any new checkbox control
        /// </summary>
        public static char DefaultCheckedChar { get; set; } = 'X';

        /// <summary>
        /// Determines if this checkbox is checked
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// The label of this checkbox
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// The checked char of this checkbox
        /// </summary>
        public char CheckedChar { get; set; }

        public Checkbox(Menu parent, string label, bool value = false) : base(parent)
        {
            Checked = value;
            Label = label;

            CheckedChar = DefaultCheckedChar;

            OnEnter += Checkbox_OnEnter;
        }

        private void Checkbox_OnEnter()
        {
            Checked = !Checked;
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            string finalText = (new string(' ', Padding) + $"[{GetCheckedChar()}] {Label}").PadRight(cri.Width);

            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;
            System.Console.Write(finalText);
            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }

        char GetCheckedChar()
        {
            return Checked ? CheckedChar : ' ';
        }
    }
}
