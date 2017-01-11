using Conmenu;
namespace Conmenu_Test
{
    class Program
    {
        static Menu mainMenu = new Menu("Main menu");
        static Menu createAccMenu = new Menu("Create account");
        static Menu nestedMenu = new Menu("Nested menu (And with colors!)");

        static void Main(string[] args)
        {
            Console.DisableConsoleResize();
            Console.Title = "Conmenu Test";

            InitializeMenus();

            mainMenu.Show();
        }

        static void InitializeMenus()
        {
            mainMenu.Controls.AddRange(new Control[]
            {
                new Button(mainMenu, "Create account or something", createAccMenu.Show),
                new Button(mainMenu, "Nested Menu", nestedMenu.Show),
                new Button(mainMenu, "Exit", mainMenu.Hide)
            });

            createAccMenu.ControlPadding = 3;
            createAccMenu.ResetControlsOnHide = true;
            createAccMenu.Controls.AddRange(new Control[]
            {
                new TextField(createAccMenu, "Name    "),
                new TextField(createAccMenu, "Password") { Masked = true },
                new TextField(createAccMenu, "Email   "),
                new Checkbox(createAccMenu, "Some checkbox"),
                new Separator(createAccMenu),
                new Button(createAccMenu, "Create account"),
                new Button(createAccMenu, "Cancel", createAccMenu.Hide)
            });

            nestedMenu.BackColor = System.ConsoleColor.DarkMagenta;
            nestedMenu.SelectedForeColor = System.ConsoleColor.DarkMagenta;
            nestedMenu.Controls.AddRange(new Control[]
            {
                new Button(nestedMenu, "Button 1"),
                new Button(nestedMenu, "Button 2"),
                new Button(nestedMenu, "Button 3"),
                new Button(nestedMenu, "Exit", nestedMenu.Hide)
            });
        }
    }
}
