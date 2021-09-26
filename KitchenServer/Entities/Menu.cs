using System;
using System.Collections.Generic;

namespace KitchenServer.Entities
{
     class Menu
     {
          public readonly List<MenuItem> MenuItems = new()
          {
               new MenuItem(1, "pizza", 20, 2, "oven"),
               new MenuItem(2, "salat", 10, 1, null),
               new MenuItem(3, "zeama", 7, 1, "stove"),
               new MenuItem(4, "Scallop Sashimi with Meyer Lemon Confit", 32, 3, null),
               new MenuItem(5, "Island Duck with Mulberry Mustard", 35, 3, "oven"),
               new MenuItem(6, "Waffles", 10, 1, "stove"),
               new MenuItem(7, "Aubergine", 20, 2, null),
               new MenuItem(8, "Lasagna", 30, 2, "oven"),
               new MenuItem(9, "Burger", 15, 1, "oven"),
               new MenuItem(10, "Gyros", 15, 1, null)
          };
          private static readonly Lazy<Menu> menu = new(() => new Menu());

          public static Menu Instance { get { return menu.Value; } }

          private Menu()
          {
          }
     }
}
