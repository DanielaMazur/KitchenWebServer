using KitchenServer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KitchenServer.Entities
{
     class Cook
     {
          public readonly int Id;
          public CookRankEnum Rank { get; set; }
          public int Proficiency { get; set; }
          public string Name { get; set; }
          public string CatchPhrase { get; set; }

          private List<MenuItem> _menuItems = Menu.Instance.MenuItems;
          private List<Distribution> _orders = OrderList.Instance.Orders;

          public Cook(int id, string name, CookRankEnum rank, int proficiency, string catchPhrase)
          {
               Id = id;
               Name = name;
               Rank = rank;
               Proficiency = proficiency;
               CatchPhrase = catchPhrase;
          }

          public void Work()
          {
               Thread t = new(new ThreadStart(() =>
               {
                    while (true)
                    {
                         foreach(var order in _orders.ToArray())
                         {
                              if (order == null || order.Items.Length == order.CookingDetails.Count ) continue;

                              var unpreparedOrderIds = order.Items.Except(order.CookingDetails.Select(d => d.FoodId));
                              foreach(var unpreparedOrderId in unpreparedOrderIds)
                              {
                                   var menuOrder = _menuItems.Single(menuItem => menuItem.Id == unpreparedOrderId);
                                   if(menuOrder.Complexity <= (int)Rank)
                                   {
                                        Console.WriteLine($"Cook {Name} took the {menuOrder.Name} from the order {order.OrderId}");
                                        var cookingDetails = new CookingDetails(unpreparedOrderId, Id)
                                        {
                                             Status = CookingStatusEnum.Cooking
                                        };
                                        order.CookingDetails.Add(cookingDetails);
                                        Task.Delay(menuOrder.PreparationTime).ContinueWith((task) => {
                                             cookingDetails.Status = CookingStatusEnum.Ready;
                                             Console.WriteLine($"Food {menuOrder.Name} from the order {order.OrderId} is Ready!");
                                        });
                                   }
                              }
                         }
                    }
               }));

               t.Start();
          }
     }
}
