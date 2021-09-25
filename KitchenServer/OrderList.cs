using System;
using System.Collections.Generic;

namespace KitchenServer
{
     class OrderList
     {
          public List<string> Orders = new() { "order1", "order2" };
          private OrderList()
          {
          }
          private static readonly Lazy<OrderList> lazy = new(() => new OrderList());
          public static OrderList Instance
          {
               get
               {
                    return lazy.Value;
               }
          }
     }
}
