namespace KitchenServer
{
     class Program
     {
          static void Main(string[] args)
          {
               HTTPServer server = new();
               server.Start();

               new Kitchen();
          }
     }
}
