using System;
using System.Collections.Generic;
namespace Conmenu
{
    public class Menu
    {
        private static bool needsRerendering = false;

        public static int DefaultWidth { get; set; } = 80;
        public static int DefaultHeight { get; set; } = 25;
        public static ushort DefaultPadding { get; set; } = 1;
        public static ushort DefaultControlPadding { get; set; } = 1;

        public delegate void OnShowingDelegate();
        public delegate void OnHidingDelegate();

        public event OnShowingDelegate OnShowing;
        public event OnHidingDelegate OnHiding;

        public ConsoleColor BackColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor ForeColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor SelectedBackColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor SelectedForeColor { get; set; } = ConsoleColor.Black;

        public ushort ControlPadding { get; set; }
        public bool HideOnEscape { get; set; } = true;
        public ushort Padding { get; set; }
        public bool ResetControlsOnHide { get; set; } = false;
        public bool ResetSelectedIndexOnHide { get; set; } = true;

        public List<Control> Controls { get; }
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
        public Control SelectedControl
        {
            get => Controls[_selectedIndex];
            set
            {
                if (Controls.Count <= 0) return;

                SelectedIndex = Controls.IndexOf(value);
            }
        }
        public string Title { get; set; }
        public bool Visible { get; private set; }

        public int Width { get; private set; }
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

        public void Clear()
        {
            System.Console.BackgroundColor = BackColor;
            System.Console.ForegroundColor = ForeColor;

            System.Console.Clear();
            RenderTitle();
            RenderLines();
        }
        public void Hide()
        {
            Visible = false;
            OnHiding?.Invoke();
        }
        public void Show()
        {
            Visible = true;
            System.Console.SetBufferSize(Width, Height);
            System.Console.SetWindowSize(Width, Height);
            OnShowing?.Invoke();
            DoMenu();
        }

        public void ResetControls()
        {
            foreach (Control c in Controls)
            {
                if (c is TextField tf)
                    tf.Text = "";
                if (c is Checkbox chk)
                    chk.Checked = false;
            }
        }

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
