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
          private static List<CookingAparatus> _cookingAparatus;
          private static Mutex _mut = new();

          public Kitchen()
          {
               new Cook(1, "Alin", Enums.CookRankEnum.ExecutiveChef, 8, "Hi, I hope you like spicy food!");
               new Cook(2, "Ana", Enums.CookRankEnum.Saucier, 5, "Hi, I cook the most delicious food ever!");
               new Cook(3, "Cristi", Enums.CookRankEnum.LineCook, 5, "Hi, I hope you like spicy food!");
               new Cook(4, "Ioana", Enums.CookRankEnum.LineCook, 2, "Hi, I cook the most delicious food ever!");
               //new Cook(5, "Erik", Enums.CookRankEnum.LineCook, 1, "Hi, I cook the most delicious food ever!");

               _cookingAparatus = new() { new CookingAparatus("oven"), new CookingAparatus("stove") };

               RegisterRestaurant();

               new Thread(new ThreadStart(() => this.Start())).Start();
          }

          private void Start()
          {
               while (true)
               {
                    foreach (var order in OrderList.Instance.Orders.ToArray())
                    {
                         if (order == null) continue;
                         if (order.CookingDetails.Count == order.Items.Length && order.CookingDetails.All(c => c.Status == Enums.CookingStatusEnum.Ready))
                         {
                              order.CoockingTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - order.OrderArriveTime;
                              Console.WriteLine($"ORDER {order.OrderId} is ready in {order.CoockingTime} TIME");
                              SendRequestService.SendPostRequest($"{Constants.DINING_HALL_ADDRESS}/distribution", JsonConvert.SerializeObject(order));
                              OrderList.Instance.Orders.Remove(order);
                         }
                    }
               }
          }

          public static void PickUpOrderItem(Cook cook)
          {
               if (GetPerfectOrderForCook(cook) == null) return;
               _mut.WaitOne();
               if (cook.ProficiencySemaphore.WaitOne(0))
               {
                    var orderItem = GetPerfectOrderForCook(cook);
                    if (orderItem == null)
                    {
                         cook.ProficiencySemaphore.Release(1);
                         _mut.ReleaseMutex();
                         return;
                    }
                    OrderList.Instance.OrderedItems.Remove(orderItem);
                    var order = OrderList.Instance.Orders.Single(order => order.OrderId == orderItem.Order.OrderId);
                    var requiredCookingAparatus = orderItem.OrderMenuItem.CookingAparatus != null ? _cookingAparatus.Single(a => a.Name == orderItem.OrderMenuItem.CookingAparatus) : null;
                    _mut.ReleaseMutex();
                    cook.CookItemHandler(order, orderItem.OrderMenuItem, requiredCookingAparatus);
               }
               else
               {
                    _mut.ReleaseMutex();
               }
          }

          private static OrderItem GetPerfectOrderForCook(Cook cook)
          {
               var availableCookingAparatus = _cookingAparatus.Where(a => a.State == Enums.CookingAparatusStateEnum.Free).Select(a => a.Name).ToList();
               return OrderList.Instance.OrderedItems
                         .Where(item => item != null && item.OrderMenuItem.Complexity <= (int)cook.Rank && (item.OrderMenuItem.CookingAparatus == null || availableCookingAparatus.Contains(item.OrderMenuItem.CookingAparatus)))
                         .OrderBy(item => item.OrderMenuItem.Complexity)
                         .LastOrDefault();
          }

          private void RegisterRestaurant()
          {
               var restaurant = new Restaurant()
               {
                    Menu = Menu.Instance.MenuItems,
                    Name = "Dragon",
                    Address = Constants.RESTAURANT_ADDRESS,
                    Rating = 0, 
                    MenuItems = Menu.Instance.MenuItems.Count,
               };
               SendRequestService.SendPostRequest($"{Constants.FOOD_ORDERING_SERVICE_ADDRESS}/register", JsonConvert.SerializeObject(restaurant));
          }
     }
}
