using System;
namespace Conmenu
{
    public abstract class Control
    {
        public ConsoleColor BackColor { get; set; }
        public ConsoleColor ForeColor { get; set; }
        public ConsoleColor SelectedBackColor { get; set; }
        public ConsoleColor SelectedForeColor { get; set; }
        public ushort Padding { get; set; }
        public bool RenderCursor { get; set; }

        public Menu ParentMenu { get; private set; }

        public bool Selected
        {
            get
            {
                return Selectable && ParentMenu.SelectedControl == this;
            }
        }
        public bool Selectable { get; protected set; } = true;

        public delegate void OnSelectDelegate();
        public delegate void OnEnterDelegate();
        public delegate void OnKeyPressDelegate(ConsoleKeyInfo keyInfo);

        public event OnEnterDelegate OnEnter;
        public event OnKeyPressDelegate OnKeyPress;
        public event OnSelectDelegate OnSelect;

        public Control(Menu parent)
        {
            ParentMenu = parent;

            BackColor = parent.BackColor;
            ForeColor = parent.ForeColor;
            SelectedBackColor = parent.SelectedBackColor;
            SelectedForeColor = parent.SelectedForeColor;
            Padding = parent.ControlPadding;
        }

        public ConsoleColor GetCurrentBackColor()
        {
            return (Selected ? SelectedBackColor : BackColor);
        }
        public ConsoleColor GetCurrentForeColor()
        {
            return (Selected ? SelectedForeColor : ForeColor);
        }

        public void PerformEnter()
        {
            OnEnter?.Invoke();
        }
        public void PerformKeyPress(ConsoleKeyInfo kInfo)
        {
            OnKeyPress?.Invoke(kInfo);
        }
        public void PerformSelect()
        {
            OnSelect?.Invoke();
        }
        public void Render(ControlRenderInfo cri)
        {
            System.Console.CursorLeft = 0;
            System.Console.CursorTop = cri.Line;
            OnRender(cri);
        }

        protected virtual void OnRender(ControlRenderInfo cri) { }
    }
}
