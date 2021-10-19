using KitchenServer.Entities;
using System.Collections.Generic;

namespace KitchenServer
{
     class Program
     {
          static void Main(string[] args)
          {
               HTTPServer server = new(8000);
               server.Start();

               new Kitchen();
          }
     }
}
