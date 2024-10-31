using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace deliveryService
{
   public class ConfigHelper
   {
      public static string GetConfigValue(string key, Dictionary<string, string> config, string defaultValue)
      {
         if (config.ContainsKey(key)) return config[key];
         return defaultValue;
      }

      public static DateTime GetDateTimeConfigValue(string key, Dictionary<string, string> config, DateTime defaultValue)
      {
         if (config.ContainsKey(key) && DateTime.TryParse(config[key], out DateTime result))
            return result;

         return defaultValue;
      }
      public static Dictionary<string, string> LoadConfigFile(string path)
      {
         try
         {
            if (File.Exists(path))
            {
               var configJson = File.ReadAllText(path);
               return JsonSerializer.Deserialize<Dictionary<string, string>>(configJson);
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Ошибка при чтении конфигурационного файла: {ex.Message}");
         }

         return new Dictionary<string, string>();
      }
   }
}
