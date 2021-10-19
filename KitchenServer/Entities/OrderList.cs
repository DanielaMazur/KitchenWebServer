using DotNetty.Common.Utilities;
using KitchenServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KitchenServer.Entities
{
     class OrderList
     {
          public List<Distribution> Orders = new();
          public PriorityQueue<OrderItem> OrderedItems = new(new OrderPriorityComparerService());

          private static readonly Lazy<OrderList> orderList = new(() => new OrderList());
          private Mutex _addOrderMutex = new();

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

          public void AddNewOrder(Distribution order)
          {
               _addOrderMutex.WaitOne();
               Orders.Add(order);
               foreach (var menuItemId in order.Items)
               {
                    var menuItem = Menu.Instance.MenuItems.Single(item => item.Id == menuItemId);
                    OrderedItems.Enqueue(new OrderItem(order, menuItem));
               }
               _addOrderMutex.ReleaseMutex();
          }
     }
}
