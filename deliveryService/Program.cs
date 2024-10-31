// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using deliveryService;
using Newtonsoft.Json.Linq;
using System.Text.Json;

//string GetConfigValue(string key, Dictionary<string, string> config, string defaultValue)
//{
//   if (config.ContainsKey(key)) return config[key];
//   return defaultValue;
//}



// Настройка значений по умолчанию
string district = null;
DateTime firstDeliveryTime = DateTime.MinValue;
string logFilePath = "log.txt";
string resultFilePath = "result.txt";

// Чтение конфигурацинного файла
Dictionary<string, string> config = ConfigHelper.LoadConfigFile("AppSettings.json");

// Получение значений с помощью метода
district = ConfigHelper.GetConfigValue("CityDistrict", config, district);
firstDeliveryTime = ConfigHelper.GetDateTimeConfigValue("FirstDeliveryDateTime", config, firstDeliveryTime);
logFilePath = ConfigHelper.GetConfigValue("DeliveryLog", config, logFilePath);
resultFilePath = ConfigHelper.GetConfigValue("DeliveryOrder", config, resultFilePath);

// Проверка обязательных параметров
if (string.IsNullOrEmpty(district) || firstDeliveryTime == DateTime.MinValue)
{
   Console.WriteLine("Ошибка: Не указаны обязательные параметры (район доставки или время первой доставки).");
   return;
}

// Инициализация сервисов
var logService = new LogService(logFilePath);
var orderService = new OrderService(logService);

// Чтение исходных данных из файла orders.json
var orders = orderService.LoadOrders("orders.json");

try
{
   Console.WriteLine($"Логирование операций в: {logFilePath}");
   Console.WriteLine($"Запись результатов в: {resultFilePath}");
   File.AppendAllText(logFilePath, $"Запуск приложения: {DateTime.Now}\n");
   File.AppendAllText(logFilePath, $"Фильтрация для района: {district}, с первой доставкой после: {firstDeliveryTime}\n");

   // Вызов метода фильтрации заказов (заглушка)
   var filteredOrders = orderService.FilterOrders(orders, district, firstDeliveryTime);

   // Сохранение результатов
   orderService.SaveFilteredOrders(filteredOrders, resultFilePath);
   File.AppendAllText(logFilePath, $"Завершение фильтрации: {DateTime.Now}\n");
   Console.WriteLine("Фильтрация завершена. Результаты сохранены.");
}
catch (Exception ex)
{
   Console.WriteLine($"Ошибка при логировании или записи: {ex.Message}");
}






