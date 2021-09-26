using System.Net;

namespace KitchenServer.Controllers
{
     abstract class Controller
     {
          public string Method { get; private set; }
          public string Route { get; private set; }
          public Controller(string method, string route)
          {
               Method = method;
               Route = route;
          }
          public abstract void HandleRequest(HttpListenerContext httpListenerContext);
     }
}
