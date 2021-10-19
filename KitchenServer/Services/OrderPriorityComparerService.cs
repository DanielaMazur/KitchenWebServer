using KitchenServer.Entities;
using System.Collections.Generic;

namespace KitchenServer.Services
{
     class OrderPriorityComparerService : IComparer<OrderItem>
     {
          public int Compare(OrderItem x, OrderItem y)
          {
               if (x?.Order?.Priority * x?.Order?.PickUpTime > y?.Order.Priority * y?.Order?.PickUpTime) return 1;
               if (x?.Order?.Priority * x?.Order?.PickUpTime < y?.Order?.Priority * y?.Order?.PickUpTime) return -1;
               return 0;
          }
     }
}
