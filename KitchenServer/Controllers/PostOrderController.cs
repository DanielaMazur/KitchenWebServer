using KitchenServer.Entities;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace KitchenServer.Controllers
{
     class PostOrderController : Controller
     {
          private static readonly Lazy<Controller> controllerInstance = new(() => new PostOrderController());

          public static Controller Instance { get { return controllerInstance.Value; } }

          private PostOrderController() : base("POST", "/order")
          {
          }
          public override void HandleRequest(HttpListenerContext httpListenerContext)
          {
               StreamReader stream = new(httpListenerContext.Request.InputStream);
               string order = stream.ReadToEnd();

               var recivedOrder = JsonConvert.DeserializeObject<Distribution>(order);
               recivedOrder.OrderArriveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
               OrderList.Instance.Orders.Add(recivedOrder);

               Console.WriteLine($"Kitchen recived the order with id-{recivedOrder.OrderId}!");

               httpListenerContext.Response.StatusCode = 200;
               httpListenerContext.Response.ContentType = "text/plain";
               byte[] responseBuffer = Encoding.UTF8.GetBytes($"Kitchen recived the order with id-{recivedOrder.OrderId}!");
               httpListenerContext.Response.ContentLength64 = responseBuffer.Length;
               Stream output = httpListenerContext.Response.OutputStream;
               output.Write(responseBuffer, 0, responseBuffer.Length);
               output.Close();
          }
     }
}
