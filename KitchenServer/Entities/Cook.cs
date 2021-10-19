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

          private bool _isWaitingToOrder;
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
                              if (_isWaitingToOrder) continue;
                              _isWaitingToOrder = true;
                              Kitchen.PickUpOrderItem(this);
                              _isWaitingToOrder = false;
                         }
                    }
               }));

               t.Start();
          }

          public void CookItemHandler(Distribution order, MenuItem menuOrder, CookingAparatus cookingAparatus)
          {
               Console.WriteLine($"Cook {Name} took the {menuOrder.Name} from the order {order.OrderId} at = {DateTimeOffset.Now.ToUnixTimeMilliseconds() - order.OrderArriveTime}");
               if (cookingAparatus != null)
               {
                    cookingAparatus.State = CookingAparatusStateEnum.Busy;
               }
               var cookingDetails = new CookingDetails(menuOrder.Id, Id)
               {
                    Status = CookingStatusEnum.Cooking
               };
               order.CookingDetails.Add(cookingDetails);
               var startTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

               Task.Delay(menuOrder.PreparationTime).ContinueWith((task) =>
               {
                    if (cookingAparatus != null)
                    {
                         cookingAparatus.State = CookingAparatusStateEnum.Free;
                    }
                    cookingDetails.Status = CookingStatusEnum.Ready;
                    Console.WriteLine($"Food {menuOrder.Name} from the order {order.OrderId} is Ready in {DateTimeOffset.Now.ToUnixTimeMilliseconds() - startTime}, expedcted={menuOrder.PreparationTime}, time from order reveived = {DateTimeOffset.Now.ToUnixTimeMilliseconds() - order.OrderArriveTime}!");
                    ProficiencySemaphore.Release(1);
               });
          }
     }
}
