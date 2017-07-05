using System;

namespace Conmenu
{
    public class NumericBox : Control
    {
        private double _value;
        public double Value
        {
            get
            {
                _value = _value.Clamp(Minimum, Maximum);
                return _value;
            }
            set
            {
                _value = value.Clamp(Minimum, Maximum);
                ValueChanged?.Invoke();
            }
        }

        public double Minimum { get; set; } = 0.0;
        public double Maximum { get; set; } = 10.0;

        public double Step { get; set; } = 1.0;

        public delegate void OnValueChanged();
        public event OnValueChanged ValueChanged;

        public string Format { get; set; } = string.Empty;

        public NumericBox(Menu parent) : base(parent)
        {
            OnKeyPress += SelectionBox_OnKeyPress;
        }

        public void Increment()
        {
            Value += Step;
        }

        public void Decrement()
        {
            Value -= Step;
        }

        private void SelectionBox_OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            ConsoleKey key = keyInfo.Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    Decrement();
                    break;
                case ConsoleKey.RightArrow:
                    Increment();
                    break;
            }
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            string text = Value.ToString(Format).Clamp(cri.Width - Padding * 2 - 2);
            string finalText = (((new string(' ', Padding) + '<').PadRight(cri.Width / 2 - text.Length / 2) + text).PadRight(cri.Width - Padding - 1) + '>').PadRight(cri.Width); // Really ugly code

            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;

            System.Console.Write(finalText);

            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }
    }
}