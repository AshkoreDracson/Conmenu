using System;
namespace Conmenu
{
    public class TextField : Control
    {
        private int _cursorPosition;
        public int CursorPosition
        {
            get => _cursorPosition;
            set
            {
                _cursorPosition = value;
                textStartIndex = (CursorPosition + Padding * 2 - textMaxVisualLength).Clamp(0, int.MaxValue);
            }
        }
        public string Label { get; set; }
        public bool Masked { get; set; }
        public int MaxLength { get; set; } = ushort.MaxValue;
        public string Text { get; set; }
        public int VisualCursorPosition => leftOffset + (CursorPosition - textStartIndex);

        private int textStartIndex;

        private int leftOffset => Padding + Label.Length + 2;
        private int textMaxVisualLength => ParentMenu.Width - leftOffset;

        public TextField(Menu parent, string label, string text = "") : base(parent)
        {
            Label = label;
            Text = text;
            RenderCursor = true;

            OnKeyPress += TextField_OnKeyPress;
            OnSelect += TextField_OnSelect;
        }

        /*
         * Friendly reminder that TextField update cycle goes like this:
         * OnSelect event
         * OnKeyPress event
         * OnRender virtual void
         */

        protected override void OnRender(ControlRenderInfo cri)
        {
            string finalText = (new string(' ', Padding) + ($"{Label}: {GetText()}")).PadRight(cri.Width);

            System.Console.BackgroundColor = Selected ? SelectedBackColor : BackColor;
            System.Console.ForegroundColor = Selected ? SelectedForeColor : ForeColor;
            System.Console.Write(finalText);
            System.Console.BackgroundColor = ParentMenu.BackColor;
            System.Console.ForegroundColor = ParentMenu.ForeColor;

            SetCorrectCursorPosition(cri.Line);
        }

        private void TextField_OnKeyPress(ConsoleKeyInfo kInfo)
        {
            switch (kInfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    PrevCursorPosition();
                    break;
                case ConsoleKey.RightArrow:
                    NextCursorPosition();
                    break;
                case ConsoleKey.Backspace:
                    Backspace();
                    break;
                case ConsoleKey.Delete:
                    Delete();
                    break;
                case ConsoleKey.Home:
                    GotoHome();
                    break;
                case ConsoleKey.End:
                    GotoEnd();
                    break;
                default:
                    AddChar(kInfo.KeyChar);
                    break;
            }
        }

        private void TextField_OnSelect()
        {
            CursorPosition = Text.Length;
        }

        void AddChar(char c)
        {
            Text = Text.Insert(CursorPosition, c.ToString());
            NextCursorPosition();
        }
        void Backspace()
        {
            if (CursorPosition > 0 && Text.Length > 0)
            {
                Text = Text.Remove(CursorPosition - 1, 1);
                PrevCursorPosition();
            }
        }
        void Delete()
        {
            if (CursorPosition < Text.Length && Text.Length > 0)
            {
                Text = Text.Remove(CursorPosition, 1);
            }
        }
        string GetText()
        {
            if (Text.Length <= textMaxVisualLength)
            {
                if (Masked)
                    return new string('*', Text.Length);

                return Text;
            }

            string finalText = Text.Substring(textStartIndex).Clamp(textMaxVisualLength);

            if (Masked)
                finalText = new string('*', finalText.Length);

            if (CursorPosition > textMaxVisualLength)
                finalText = "..." + finalText.Substring(3);

            if (VisualCursorPosition < textMaxVisualLength)
                finalText = finalText.Substring(0, finalText.Length - 3) + "...";

            return finalText;
        }
        void GotoHome()
        {
            CursorPosition = 0;
        }
        void GotoEnd()
        {
            CursorPosition = Text.Length;
        }
        void PrevCursorPosition()
        {
            if (CursorPosition > 0)
                CursorPosition--;
        }
        void NextCursorPosition()
        {
            if (CursorPosition < Text.Length)
                CursorPosition++;
        }
        void SetCorrectCursorPosition(int line)
        {
            CursorPosition = CursorPosition.Clamp(0, Text.Length);

            System.Console.CursorLeft = VisualCursorPosition;
            System.Console.CursorTop = line;
        }
    }
}
