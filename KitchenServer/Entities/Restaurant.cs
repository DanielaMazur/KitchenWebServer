using System;
using System.Collections.Generic;

namespace KitchenServer.Entities
{
     class Restaurant
     {
          public readonly int Id;
          public string Name { get; set; }
          public string Address { get; set; }
          public int MenuItems { get; set; }
          public List<MenuItem> Menu { get; set; }
          public int Rating { get; set; }

          public Restaurant()
          {
               Id = Guid.NewGuid().GetHashCode();
          }
     }
}
