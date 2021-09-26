using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KitchenServer.Entities
{
     class Distribution
     {
          public int OrderId { get; set; }
          public int TableId { get; set; }
          public int WaiterId { get; set; }
          public int[] Items { get; set; }
          public int Priority { get; set; }
          public double MaxWait { get; set; }
          public long PickUpTime { get; set; }
          [JsonIgnore]
          public long OrderArriveTime { get; set; }
          public long CoockingTime { get; set; }
          public List<CookingDetails> CookingDetails = new();

          public override string ToString()
          {
               return $"Order-{OrderId} was taken by the waiter-{WaiterId} from the table-{TableId} at {UnixTimeStampToDateTime(PickUpTime)}.";
          }

          private static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
          {
               DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
               dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
               return dateTime;
          }
     }
}
