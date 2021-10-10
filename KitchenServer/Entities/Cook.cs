using KitchenServer.Enums;
using System;
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

          public Semaphore ProficiencySemaphore;

          public Cook(int id, string name, CookRankEnum rank, int proficiency, string catchPhrase)
          {
               Id = id;
               Name = name;
               Rank = rank;
               Proficiency = proficiency;
               CatchPhrase = catchPhrase;
               ProficiencySemaphore = new Semaphore(proficiency, proficiency);
               Work();
          }

          public void Work()
          {
               Thread t = new(new ThreadStart(() =>
               {
                    while (true)
                    {
                         if (OrderList.Instance.Orders.Count > 0)
                         {
                              Kitchen.PickUpOrderItem(this, CookItemHandler);
                         }
                    }
               }));

               t.Start();
          }

          private void CookItemHandler(Distribution order, MenuItem menuOrder)
          {
               Console.WriteLine($"Cook {Name} took the {menuOrder.Name} from the order {order.OrderId}");
               var cookingDetails = new CookingDetails(menuOrder.Id, Id)
               {
                    Status = CookingStatusEnum.Cooking
               };
               order.CookingDetails.Add(cookingDetails);
               Task.Delay(menuOrder.PreparationTime).ContinueWith((task) =>
                    {
                         cookingDetails.Status = CookingStatusEnum.Ready;
                         Console.WriteLine($"Food {menuOrder.Name} from the order {order.OrderId} is Ready!");
                         ProficiencySemaphore.Release(1);
                    });
          }

     }
}
