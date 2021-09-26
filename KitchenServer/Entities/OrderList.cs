using System;
using System.Collections.Generic;

namespace KitchenServer.Entities
{
     class OrderList
     {
          public List<Distribution> Orders = new();
          private static readonly Lazy<OrderList> orderList = new(() => new OrderList());

          private OrderList()
          {
          }
          public static OrderList Instance
          {
               get
               {
                    return orderList.Value;
               }
          }
     }
}
