using System;

namespace KitchenServer
{
     class Program
     {
          static void Main(string[] args)
          {
               HTTPServer server = new(8000);
               server.Start();
          }
     }
}
