namespace KitchenServer.Entities
{
     class OrderItem
     {
          public Distribution Order { get; private set; }
          public MenuItem OrderMenuItem { get; private set; }

          public OrderItem(Distribution order, MenuItem orderMenuItem)
          {
               Order = order;
               OrderMenuItem = orderMenuItem;
          }
     }
}
