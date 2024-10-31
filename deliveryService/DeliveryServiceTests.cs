using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using deliveryService;
using Xunit;

namespace deliveryService
{
   public class DeliveryServiceTests
   {

      // Проверяем, возвращается ли правильное значение для существующего ключа в конфигурации
      [Fact]
      public void TestGetConfigValue_ValidKey_ReturnsValue()
      {
         var config = new Dictionary<string, string>
         {
            { "CityDistrict", "Downtown" },
            { "FirstDeliveryDateTime", "2023-10-28 12:00:00" },
            { "DeliveryLog", "log.txt" },
            { "DeliveryOrder", "result.txt" }
         };

         string result = ConfigHelper.GetConfigValue("CityDistrict", config, "DefaultDistrict");
         Assert.Equal("Downtown", result);
      }

      // Проверяет, возвращается ли значение по умолчанию для несуествующего ключа
      [Fact]
      public void TestGetConfigValue_InvalidKey_ReturnsDefaultValue()
      {
         var config = new Dictionary<string, string>();
         string result = ConfigHelper.GetConfigValue("NonExistentKey", config, "DefaultDistrict");
         Assert.Equal("DefaultDistrict", result);
      }


      // Проверяет, правильно ли фильтруются заказы по району и времени
      [Fact]
      public void TestFilterOrders_ValidInput_ReturnsFilteredOrders()
      {
         // Пример входных данных
         var orders = new List<Order>
        {
            new Order { OrderId = "1", Weight = 5, District = "Downtown", DeliveryTime = new DateTime(2023, 10, 28, 12, 15, 0) },
            new Order { OrderId = "2", Weight = 10, District = "Uptown", DeliveryTime = new DateTime(2023, 10, 28, 12, 30, 0) },
            new Order { OrderId = "3", Weight = 3, District = "Downtown", DeliveryTime = new DateTime(2023, 10, 28, 12, 30, 0) }
        };

         var orderService = new OrderService();
         var filteredOrders = orderService.FilterOrders(orders, "Downtown", new DateTime(2023, 10, 28, 12, 00, 0));

         Assert.Equal(2, filteredOrders.Count);
         Assert.Contains(filteredOrders, o => o.OrderId == "1");
         Assert.Contains(filteredOrders, o => o.OrderId == "3");
      }

   }

}
