using DotNetty.Common.Utilities;
using KitchenServer.Services;
using System;

namespace KitchenServer.Entities
{
     class OrderList
     {
          public PriorityQueue<Distribution> Orders = new(new OrderPriorityComparerService());
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
