using KitchenServer.Entities;
using KitchenServer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace KitchenServer
{
     class Kitchen
     {
          private List<Cook> _cooks;
          private List<CookingAparatus> _cookingAparatus;
          private List<MenuItem> _menuItems = Menu.Instance.MenuItems;
          private List<Distribution> _orders = OrderList.Instance.Orders;
          public Kitchen(List<Cook> cooks, List<CookingAparatus> cookingAparatus)
          {
               _cooks = cooks;
               _cookingAparatus = cookingAparatus;
               Thread t = new(new ThreadStart(() => Start()));
               t.Start();
          }

          private void Start()
          {
               while (true)
               {
                    var copyOrders = _orders.ToArray();
                    foreach (var order in copyOrders)
                    {
                         if (order.CookingDetails.Count == order.Items.Length)
                         {
                              var isOrderReady = order.CookingDetails.All(c => c.Status == Enums.CookingStatusEnum.Ready);
                              if (isOrderReady)
                              {
                                   order.CoockingTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - order.OrderArriveTime;
                                   Console.WriteLine($"Order {order.OrderId} is ready in {order.CoockingTime}");
                                   SendRequestService.SendPostRequest("http://dining-hall-server-container:3000/distribution", JsonConvert.SerializeObject(order));
                                   _orders.Remove(order);
                              }
                         }
                    }
               }
          }
     }
}
