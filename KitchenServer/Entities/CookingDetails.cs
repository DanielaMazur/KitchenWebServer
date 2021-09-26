using KitchenServer.Enums;
using Newtonsoft.Json;

namespace KitchenServer.Entities
{
     class CookingDetails
     {
          public readonly int FoodId;
          public readonly int CookId;
          [JsonIgnore]
          public CookingStatusEnum Status { get; set; }

          public CookingDetails(int foodId, int cookId)
          {
               FoodId = foodId;
               CookId = cookId;
          }
     }
}
