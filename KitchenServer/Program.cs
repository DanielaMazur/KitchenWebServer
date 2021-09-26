using KitchenServer.Entities;
using System.Collections.Generic;

namespace KitchenServer
{
     class Program
     {
          static void Main(string[] args)
          {
               List<Cook> cooks = new() { new Cook(1, "Alin", Enums.CookRankEnum.ExecutiveChef, 2, "Hi, I hope you like spicy food!") };
               List<CookingAparatus> cookingAparatus = new() { };

               var kitchen = new Kitchen(cooks, cookingAparatus);

               foreach(var cook in cooks)
               {
                    cook.Work();
               }

               HTTPServer server = new(8000);
               server.Start();
          }
     }
}
