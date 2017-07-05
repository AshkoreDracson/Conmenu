using System;
namespace Conmenu
{
    public class Button : Control
    {
        /// <summary>
        /// The action of the button upon trigger
        /// </summary>
        public Action Action { get; set; }
        /// <summary>
        /// The text of this button
        /// </summary>
        public string Text { get; set; }

        public Button(Menu parent, string text, Action action = null) : base(parent)
        {
            Action = action;
            Text = text;

            OnEnter += Button_OnEnter;
        }

        private void Button_OnEnter()
        {
            Action?.Invoke();
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            string finalText = (new string(' ', Padding) + Text).PadRight(cri.Width);

            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;

            System.Console.Write(finalText);
            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }
    }
}
