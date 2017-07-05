using Conmenu;
using System;
using Console = Conmenu.Console;

namespace Conmenu_Test
{
    internal static class Program
    {
        static Menu mainMenu = new Menu("Main menu");
        static Menu createAccountMenu = new Menu("Create account");

        static void Main()
        {
            Console.DisableConsoleResize();
            Console.Title = "Conmenu Test";

            InitializeMenus();

            mainMenu.Show();
        }

        static void InitializeMenus()
        {
            mainMenu.BackColor = mainMenu.SelectedForeColor = ConsoleColor.DarkBlue;
            createAccountMenu.BackColor = createAccountMenu.SelectedForeColor = ConsoleColor.DarkGreen;

            mainMenu.Controls.AddRange(new Control[]
            {
                new Button(mainMenu, "Create account", createAccountMenu.Show)

            });
            createAccountMenu.Controls.AddRange(new Control[]
            {
                new TextField(createAccountMenu, "Username"),
                new TextField(createAccountMenu, "Email"),
                new TextField(createAccountMenu, "Password") { Masked = true },
                new Separator(createAccountMenu),
                new Label(createAccountMenu, "Gender"),
                new SelectionBox(createAccountMenu, "Male", "Female", "Other"),
                new Label(createAccountMenu, "Age"),
                new NumericBox(createAccountMenu) { Maximum = 99, Value = 18, Step = 1.0 / 12.0, Format = "N2" },
                new Separator(createAccountMenu),
                new Checkbox(createAccountMenu, "I agree to the terms, etc..."),
                new Button(createAccountMenu, "Create", System.Console.Beep),
                new Button(createAccountMenu, "Cancel", createAccountMenu.Hide)
            });
        }
    }
}
