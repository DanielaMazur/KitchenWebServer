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
          private static List<CookingAparatus> _cookingAparatus;
          private List<MenuItem> _menuItems = Menu.Instance.MenuItems;
          private static Mutex _mut = new();

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
                    foreach (var order in OrderList.Instance.Orders.ToList())
                    {
                         if (order == null) continue;
                         if (order.CookingDetails.Count == order.Items.Length)
                         {
                              var isOrderReady = order.CookingDetails.ToList().All(c => c.Status == Enums.CookingStatusEnum.Ready);
                              if (isOrderReady)
                              {
                                   order.CoockingTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - order.OrderArriveTime;
                                   Console.WriteLine($"Order {order.OrderId} is ready in {order.CoockingTime}");
                                   SendRequestService.SendPostRequest("http://dining-hall-server-container:3000/distribution", JsonConvert.SerializeObject(order));
                                   OrderList.Instance.Orders.Remove(order);
                              }
                         }
                    }
               }
          }

          public static void PickUpOrderItem(Cook cook)
          {
               _mut.WaitOne();
               if (cook.ProficiencySemaphore.WaitOne(0))
               {
                    var order = OrderList.Instance.Orders.FirstOrDefault();
                    if (order == null)
                    {
                         cook.ProficiencySemaphore.Release(1);
                         _mut.ReleaseMutex();
                         return;
                    }
                    var availableCookingAparatus = _cookingAparatus.Where(a => a.State == Enums.CookingAparatusStateEnum.Free).Select(a => a.Name).ToList();
                    var unpreparedOrderIds = order.Items.Except(order.CookingDetails.Select(d => d.FoodId)).ToList();
                    var test = unpreparedOrderIds
                         .Select(itemId => Menu.Instance.MenuItems.Single(menuItem => menuItem.Id == itemId))
                         .Where(item => item.Complexity <= (int)cook.Rank && (item.CookingAparatus == null || availableCookingAparatus.Contains(item.CookingAparatus)))
                         .OrderBy(item => item.Complexity).ToList();

                    var menuOrder = test
                              .FirstOrDefault();
                    if (menuOrder == null)
                    {
                         cook.ProficiencySemaphore.Release(1);
                         _mut.ReleaseMutex();
                         return;
                    }

                    cook.CookItemHandler(order, menuOrder, menuOrder.CookingAparatus != null ? _cookingAparatus.Single(a => a.Name == menuOrder.CookingAparatus) : null);
               }
               _mut.ReleaseMutex();
          }
     }
}
