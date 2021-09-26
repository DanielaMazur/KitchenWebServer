using KitchenServer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KitchenServer.Services
{
     class ControllersService
     {
          public readonly List<Controller> Controllers = new();
          public ControllersService()
          {
               var controllerTypes = Assembly.GetAssembly(typeof(Controller)).GetTypes()
                  .Where(controllerType => controllerType.IsClass && !controllerType.IsAbstract && controllerType.IsSubclassOf(typeof(Controller)));
               foreach (Type controllerType in controllerTypes)
               {
                    Controllers.Add((Controller)controllerType.GetProperty("Instance").GetValue(null));
               }
          }
     }
}
