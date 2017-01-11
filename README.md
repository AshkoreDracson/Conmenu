# Conmenu
Console simple menu &amp; controls API

## Basic usage

Recommended initial setup (Somewhere in your program initialization)
```cs
using Conmenu;
...
Console.DisableConsoleResize();
...
```
This disables the console resizing and maximizing methods, this'll avoid breaking the UI completly due to user resizing.


Creating a menu
```cs
Menu menu = new Menu("Title");
```

Showing/hiding a menu
```cs
menu.Show();
...
menu.Hide();
```

Adding controls ("Control()" is whatever class inherits from `Control` (`Button`, `Checkbox`, `Separator`, `TextField`))
```cs
menu.Controls.Add(new Control());
```

See example project and the wiki/documentation for more information.
