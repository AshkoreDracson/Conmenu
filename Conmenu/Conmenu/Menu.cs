using System;
using System.Collections.Generic;
namespace Conmenu
{
    public class Menu
    {
        private static bool needsRerendering = false;

        /// <summary>
        /// The default width of menus
        /// </summary>
        public static int DefaultWidth { get; set; } = 80;
        /// <summary>
        /// The default height of menus
        /// </summary>
        public static int DefaultHeight { get; set; } = 25;
        /// <summary>
        /// The default padding of menus
        /// </summary>
        public static ushort DefaultPadding { get; set; } = 1;
        /// <summary>
        /// The default control padding of menus
        /// </summary>
        public static ushort DefaultControlPadding { get; set; } = 1;

        public delegate void OnShowingDelegate();
        public delegate void OnHidingDelegate();

        /// <summary>
        /// Called when the menu is showing
        /// </summary>
        public event OnShowingDelegate OnShowing;
        /// <summary>
        /// Called when the menu is hiding
        /// </summary>
        public event OnHidingDelegate OnHiding;

        /// <summary>
        /// The back color of this menu
        /// </summary>
        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
        /// <summary>
        /// The fore color of this menu
        /// </summary>
        public ConsoleColor ForeColor { get; set; } = ConsoleColor.Gray;
        /// <summary>
        /// The selected back color of this menu
        /// </summary>
        public ConsoleColor SelectedBackColor { get; set; } = ConsoleColor.Gray;
        /// <summary>
        /// The selected fore color of this menu
        /// </summary>
        public ConsoleColor SelectedForeColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// The padding of the controls inside the menu.
        /// </summary>
        public ushort ControlPadding { get; set; }
        /// <summary>
        /// Whetever the menu should auto-hide upon pressing the Escape key
        /// </summary>
        public bool HideOnEscape { get; set; } = true;
        /// <summary>
        /// The padding of the menu itself
        /// </summary>
        public ushort Padding { get; set; }
        /// <summary>
        /// Determines if the controls should be reset to their default values upon hiding
        /// </summary>
        public bool ResetControlsOnHide { get; set; } = false;
        /// <summary>
        /// Determines if the selected index will be reset to 0 upon hiding
        /// </summary>
        public bool ResetSelectedIndexOnHide { get; set; } = true;

        /// <summary>
        /// The list of controls
        /// </summary>
        public List<Control> Controls { get; }
        /// <summary>
        /// The selected control index
        /// </summary>
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                if (Controls.Count <= 0) return;
                if (value < 0 || value > Controls.Count - 1) throw new IndexOutOfRangeException();

                _selectedIndex = value;
                SelectedControl.PerformSelect();
            }
        }
        /// <summary>
        /// The selected control
        /// </summary>
        public Control SelectedControl
        {
            get => Controls[_selectedIndex];
            set
            {
                if (Controls.Count <= 0) return;

                SelectedIndex = Controls.IndexOf(value);
            }
        }
        /// <summary>
        /// The title of this menu
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Is this menu visible?
        /// </summary>
        public bool Visible { get; private set; }

        /// <summary>
        /// The width of the menu
        /// </summary>
        public int Width { get; private set; }
        /// <summary>
        /// The height of the menu
        /// </summary>
        public int Height { get; private set; }

        private int _selectedIndex;

        public Menu(string title)
        {
            Controls = new List<Control>();
            Title = title;

            ControlPadding = DefaultControlPadding;
            Padding = DefaultPadding;

            Width = DefaultWidth;
            Height = DefaultHeight;
        }
        public Menu(string title, int width, int height)
        {
            Controls = new List<Control>();
            Title = title;

            Padding = DefaultPadding;

            Width = width;
            Height = height;
        }

        /// <summary>
        /// Clears and re-renders the menu
        /// </summary>
        public void Clear()
        {
            System.Console.BackgroundColor = BackColor;
            System.Console.ForegroundColor = ForeColor;

            System.Console.Clear();
            RenderTitle();
            RenderLines();
        }
        /// <summary>
        /// Hides the menu
        /// </summary>
        public void Hide()
        {
            Visible = false;
            OnHiding?.Invoke();
        }
        /// <summary>
        /// Shows the menu
        /// </summary>
        public void Show()
        {
            Visible = true;
            System.Console.SetBufferSize(Width, Height);
            System.Console.SetWindowSize(Width, Height);
            OnShowing?.Invoke();
            DoMenu();
        }

        /// <summary>
        /// Resets the controls to their default value
        /// </summary>
        public void ResetControls()
        {
            foreach (Control c in Controls)
            {
                if (c is TextField tf)
                    tf.Text = "";
                if (c is Checkbox chk)
                    chk.Checked = false;
                if (c is SelectionBox sc)
                    sc.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Selects the previous control
        /// </summary>
        public void SelectPrevious()
        {
            if (Controls.Count <= 0) return;

            do
            {
                if (SelectedIndex <= 0)
                    SelectedIndex = Controls.Count - 1;
                else
                    SelectedIndex--;
            } while (!SelectedControl.Selectable);
        }
        /// <summary>
        /// Selects the next control
        /// </summary>
        public void SelectNext()
        {
            if (Controls.Count <= 0) return;

            do
            {
                if (SelectedIndex >= Controls.Count - 1)
                    SelectedIndex = 0;
                else
                    SelectedIndex++;
            } while (!SelectedControl.Selectable);
        }

        void DoMenu()
        {
            Clear();

            while (Visible)
            {
                if (needsRerendering)
                {
                    System.Console.SetBufferSize(Width, Height);
                    System.Console.SetWindowSize(Width, Height);
                    Clear();
                }

                System.Console.CursorVisible = Controls.Count > 0 && SelectedControl.RenderCursor;

                int prevSelectedIndex = SelectedIndex;
                ConsoleKeyInfo kInfo = System.Console.ReadKey(true);

                switch (kInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        SelectPrevious();
                        break;
                    case ConsoleKey.DownArrow:
                        SelectNext();
                        break;
                    case ConsoleKey.Tab:
                        SelectNext();
                        break;
                    case ConsoleKey.Enter:
                        SelectedControl.PerformEnter();
                        break;
                    case ConsoleKey.Escape:
                        if (HideOnEscape)
                            Visible = false;
                        else
                            SelectedControl.PerformKeyPress(kInfo);
                        break;
                    default:
                        SelectedControl.PerformKeyPress(kInfo);
                        break;
                }

                int nextSelectedIndex = SelectedIndex;
                RenderLines(prevSelectedIndex, nextSelectedIndex);
            }

            if (ResetControlsOnHide)
                ResetControls();

            if (ResetSelectedIndexOnHide)
                SelectedIndex = 0;

            needsRerendering = true;
        }

        void RenderTitle()
        {
            System.Console.CursorLeft = System.Console.BufferWidth / 2 - Title.Length / 2;
            System.Console.WriteLine(Title);
            System.Console.WriteLine(new string('─', System.Console.BufferWidth));
        }

        void RenderLines()
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                ControlRenderInfo cri = new ControlRenderInfo(i + 2 + Padding, System.Console.BufferWidth);
                Controls[i].Render(cri);
            }
            RenderLines(SelectedIndex);
        }
        void RenderLines(params int[] indexes)
        {
            foreach (int i in indexes)
            {
                ControlRenderInfo cri = new ControlRenderInfo(i + 2 + Padding, System.Console.BufferWidth);
                Controls[i].Render(cri);
            }
        }
    }
}
