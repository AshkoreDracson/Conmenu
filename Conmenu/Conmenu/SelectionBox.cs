using System;
using System.Collections.Generic;

namespace Conmenu
{
    public class SelectionBox : Control
    {
        private int _selectedIndex = -1;

        public int SelectedIndex
        {
            get
            {
                if (Items.Count > 0)
                    _selectedIndex = _selectedIndex.Clamp(0, Items.Count - 1);
                else
                    _selectedIndex = -1;

                return _selectedIndex;

            }
            set
            {
                if (Items.Count > 0)
                    _selectedIndex = value.Clamp(0, Items.Count - 1);
                else
                    _selectedIndex = -1;

                SelectionChanged?.Invoke();
            }
        }
        public object SelectedItem
        {
            get => Items[SelectedIndex];
            set => SelectedIndex = Items.IndexOf(value);
        }
        public List<object> Items { get; }

        public delegate void OnSelectionChanged();
        public event OnSelectionChanged SelectionChanged;

        public SelectionBox(Menu parent, params object[] items) : base(parent)
        {
            Items = new List<object>();
            Items.AddRange(items);

            SelectedIndex = 0;

            OnKeyPress += SelectionBox_OnKeyPress;
        }

        private void SelectionBox_OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            ConsoleKey key = keyInfo.Key;

            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    SelectedIndex--;
                    break;
                case ConsoleKey.RightArrow:
                    SelectedIndex++;
                    break;
            }
        }

        protected override void OnRender(ControlRenderInfo cri)
        {
            string text = (SelectedItem?.ToString() ?? "???").Clamp(cri.Width - Padding * 2 - 2);
            string finalText = (((new string(' ', Padding) + '<').PadRight(cri.Width / 2 - text.Length / 2) + text).PadRight(cri.Width - Padding - 1) + '>').PadRight(cri.Width); // Really ugly code

            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;

            System.Console.Write(finalText);

            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;
        }
    }
}