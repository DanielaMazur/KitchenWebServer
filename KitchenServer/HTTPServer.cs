using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KitchenServer
{
     class HTTPServer
     {
          private bool _isRunning = false;
          private TcpListener _tcpListener;
          public const string VERSION = "HTTP/1.1";
          public const string NAME = "Kitchen";

          public HTTPServer(int port)
          {
               _tcpListener = new TcpListener(IPAddress.Any, port);
          }

          public void Start()
          {
               Thread serverThread = new(Run);
               serverThread.Start();
          }

          private void Run()
          {
               _isRunning = true;
               _tcpListener.Start();

               while (_isRunning)
               {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();
                    HandleClient(tcpClient);
                    tcpClient.Close();
               }

               _tcpListener.Stop();
          }

          private static void HandleClient(TcpClient client)
          {
               StreamReader reader = new(client.GetStream());

               string msg = "";

               while (reader.Peek() != -1)
               {
                    msg += reader.ReadLine() + "\n";
               }

               System.Console.WriteLine("Request: \n" + msg);

               Request req = Request.GetRequest(msg);
               Response resp = Response.From(req);
               resp.Post(client.GetStream());
          }
     }
}
