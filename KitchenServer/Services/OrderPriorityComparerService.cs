using KitchenServer.Entities;
using System.Collections.Generic;

namespace KitchenServer.Services
{
     class OrderPriorityComparerService : IComparer<Distribution>
     {
          public int Compare(Distribution x, Distribution y)
          {
               if (x.Priority > y.Priority) return 1;
               if (x.Priority < y.Priority) return -1;
               return 0;
          }
     }
}
