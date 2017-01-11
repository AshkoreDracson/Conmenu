using System;

namespace Conmenu
{
    public class Checkbox : Control
    {
        public bool Checked { get; set; }
        public string Label { get; set; }

        public Checkbox(Menu parent, string label, bool value = false) : base(parent)
        {
            Checked = value;
            Label = label;

            OnEnter += Checkbox_OnEnter;
        }

        private void Checkbox_OnEnter()
        {
            Checked = !Checked;
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            string finalText = (new string(' ', Padding) + $"[{GetCheckedChar()}] {Label}").PadRight(cri.Width);

            System.Console.BackgroundColor = (Selected ? SelectedBackColor : BackColor);
            System.Console.ForegroundColor = (Selected ? SelectedForeColor : ForeColor);
            System.Console.Write(finalText);
            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }

        char GetCheckedChar()
        {
            return (Checked ? 'x' : ' ');
        }
    }
}
