using KitchenServer.Services;
using System;
using System.Linq;
using System.Net;
using System.Threading;

namespace KitchenServer
{
     class HTTPServer
     {
          private bool _isRunning;
          private HttpListener _httpListener;
          public const string VERSION = "HTTP/1.1";
          public const string NAME = "Kitchen";

          public HTTPServer(int port)
          {
               _httpListener = new HttpListener();
               _httpListener.Prefixes.Add($"http://localhost:{port}/");
          }

          public void Start()
          {
               Thread serverThread = new(Run);
               serverThread.Start();
          }

          private void Run()
          {
               _isRunning = true;
               _httpListener.Start();

               while (_isRunning)
               {
                    HttpListenerContext context = _httpListener.GetContext();
                    HandleRequest(context);
               }

               _httpListener.Stop();
          }

          private void HandleRequest(HttpListenerContext httpContext)
          {
               var controllers = new ControllersService().Controllers;
               var requestedController = controllers.SingleOrDefault(c => c.Method == httpContext.Request.HttpMethod && c.Route == httpContext.Request.RawUrl);

               if (requestedController == null)
               {
                    Console.WriteLine("No controller found for the given request.");
                    return;
               }
               requestedController.HandleRequest(httpContext);
          }
     }
}
