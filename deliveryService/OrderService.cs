using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace deliveryService
{
   public class OrderService
   {
      private readonly LogService _logService;

      public OrderService()
      {
         _logService = null;
      }
      public OrderService(LogService logService)
      {
         _logService = logService;
      }

      public List<Order> LoadOrders(string filePath)
      {
         List<Order> orders = new List<Order>();
         try
         {
            var jsonData = File.ReadAllText(filePath);
            var desOrders = JsonConvert.DeserializeObject<List<Order>>(jsonData);

            foreach (var order in desOrders)
            {
               bool isValidateOrder = ValidateOrder(order);
               if (isValidateOrder)
               {
                  orders.Add(order);
               }
               else
               {
                  _logService.Log($"Заказ с ID {order.OrderId} не прошел валидацию.");
               }
            }
         }
         catch (Exception ex)
         {
            _logService.Log($"Ошибка при загрузке заказов: {ex.Message}");
            return new List<Order>();
         }

         _logService.Log("Заказы успешно загружены.");
         return orders;
      }

      // Валидация данных
      public bool ValidateOrder(Order order)
      {
         if (string.IsNullOrWhiteSpace(order.OrderId))
         {
            _logService.Log("Неверный OrderId.");
            return false;
         }

         if (order.Weight <= 0)
         {
            _logService.Log("Вес заказа должен быть больше 0");
            return false;
         }

         if (string.IsNullOrWhiteSpace(order.District))
         {
            _logService.Log("Район не указан");
            return false;
         } 

         if (order.DeliveryTime == default)
         {
            _logService.Log("Время доставки указано неверно.");
            return false;
         }

         return true;
      }

      // Фильтрация заказов по району и времени
      public List<Order> FilterOrders(List<Order> orders, string district, DateTime firstDeliveryTime)
      {
         DateTime maxDeliveryTime = firstDeliveryTime.AddMinutes(30);
         var filteredOrders = orders.Where(order =>
             order.District!.Equals(district, StringComparison.OrdinalIgnoreCase) &&
             order.DeliveryTime >= firstDeliveryTime &&
             order.DeliveryTime <= maxDeliveryTime)
             .ToList();

         if (_logService != null)
            _logService.Log($"Отфильтровано {filteredOrders.Count} заказов.");

         return filteredOrders;
      }

      // Сохранение результатов фильтрации
      public void SaveFilteredOrders(List<Order> orders, string filePath)
      {
         try
         {
            var jsonData = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(filePath, jsonData);
            _logService.Log("Результаты фильтрации успешно сохранены.");
         }
         catch (IOException ex)
         {
            _logService.Log($"Ошибка ввода-вывода при сохранении результатов: {ex.Message}");
         }
         catch (Exception ex)
         {
            _logService.Log($"Ошибка при сохранении результатов: {ex.Message}");
         }
      }
   }
}
