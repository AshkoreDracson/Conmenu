using System;
namespace Conmenu
{
    public abstract class Control
    {
        /// <summary>
        /// The back color of the control
        /// </summary>
        public ConsoleColor BackColor { get; set; }
        /// <summary>
        /// The fore color of the control
        /// </summary>
        public ConsoleColor ForeColor { get; set; }
        /// <summary>
        /// The selected back color of the control
        /// </summary>
        public ConsoleColor SelectedBackColor { get; set; }
        /// <summary>
        /// The selected fore color of the control
        /// </summary>
        public ConsoleColor SelectedForeColor { get; set; }
        /// <summary>
        /// The padding of the control
        /// </summary>
        public ushort Padding { get; set; }
        /// <summary>
        /// Determines whetever we should render the console cursor while this control is selected
        /// </summary>
        public bool RenderCursor { get; set; }

        /// <summary>
        /// The parent menu of the control
        /// </summary>
        public Menu ParentMenu { get; private set; }

        /// <summary>
        /// Determines if the control is selected
        /// </summary>
        public bool Selected => Selectable && ParentMenu.SelectedControl == this;
        /// <summary>
        /// Determines if the control is selectable
        /// </summary>
        public bool Selectable { get; protected set; } = true;

        public delegate void OnSelectDelegate();
        public delegate void OnEnterDelegate();
        public delegate void OnKeyPressDelegate(ConsoleKeyInfo keyInfo);

        /// <summary>
        /// Called whenever we press the "Enter" key when the control is selected
        /// </summary>
        public event OnEnterDelegate OnEnter;
        /// <summary>
        /// Called whenever there is a key press when the control is selected
        /// </summary>
        public event OnKeyPressDelegate OnKeyPress;
        /// <summary>
        /// Called whenever this control is selected
        /// </summary>
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

        /// <summary>
        /// Gets the current back color of this control
        /// </summary>
        /// <returns>The current back color of the control</returns>
        public ConsoleColor GetCurrentBackColor()
        {
            return Selected ? SelectedBackColor : BackColor;
        }
        /// <summary>
        /// Gets the current fore color of this control
        /// </summary>
        /// <returns>The current fore color of the control</returns>
        public ConsoleColor GetCurrentForeColor()
        {
            return Selected ? SelectedForeColor : ForeColor;
        }

        /// <summary>
        /// Performs an OnEnter event
        /// </summary>
        public void PerformEnter()
        {
            OnEnter?.Invoke();
        }
        /// <summary>
        /// Performs an OnKeyPress event
        /// </summary>
        /// <param name="kInfo">The key info</param>
        public void PerformKeyPress(ConsoleKeyInfo kInfo)
        {
            OnKeyPress?.Invoke(kInfo);
        }
        /// <summary>
        /// Performs an OnSelect event
        /// </summary>
        public void PerformSelect()
        {
            OnSelect?.Invoke();
        }
        /// <summary>
        /// Renders this control
        /// </summary>
        /// <param name="cri">The control render info</param>
        public void Render(ControlRenderInfo cri)
        {
            System.Console.CursorLeft = 0;
            System.Console.CursorTop = cri.Line;
            OnRender(cri);
        }

        protected abstract void OnRender(ControlRenderInfo cri);
    }
}
