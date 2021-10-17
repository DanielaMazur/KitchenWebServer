namespace KitchenServer.Entities
{
     class MenuItem
     {
          public readonly int Id;
          public string Name { get; set; }
          public int PreparationTime { get; set; }
          public int Complexity { get; set; }
          public string? CookingAparatus { get; set; }

          public MenuItem(int id, string name, int preparationTime, int complexity, string cookingAparatus = null)
          {
               Id = id;
               Name = name;
               PreparationTime = preparationTime;
               Complexity = complexity;
               CookingAparatus = cookingAparatus;
          }
     }
}
