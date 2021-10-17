using KitchenServer.Entities;
using System.Collections.Generic;

namespace KitchenServer
{
     class Program
     {
          static void Main(string[] args)
          {
               List<Cook> cooks = new() { new Cook(1, "Alin", Enums.CookRankEnum.ExecutiveChef, 2, "Hi, I hope you like spicy food!"),
                                          new Cook(2, "Ana", Enums.CookRankEnum.Saucier, 1, "Hi, I cook the most delicious food ever!") };

               List<CookingAparatus> cookingAparatus = new() { new CookingAparatus("oven"), new CookingAparatus("stove") };

               var kitchen = new Kitchen(cooks, cookingAparatus);

               HTTPServer server = new(8000);
               server.Start();
          }
     }
}
