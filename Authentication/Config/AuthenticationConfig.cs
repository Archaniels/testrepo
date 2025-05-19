using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TUBES_KPL.Authentication.Config
{
    public class AuthenticationConfig
    {
        // properties untuk runtime configuration
        public int MinPasswordLength { get; set; } = 8;
        public int MinUsernameLength { get; set; } = 3;
        public bool RequireEmailValidation { get; set; } = false;

        // lokasi default configuration 
        private static readonly string ConfigFile = "authentication_config.json";

        private static AuthenticationConfig _instance;

        public static AuthenticationConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = LoadConfiguration();
                }
                return _instance;
            }
        }

        private static AuthenticationConfig LoadConfiguration()
        {
            if (File.Exists(ConfigFile))
            {
                try
                {
                    string json = File.ReadAllText(ConfigFile);
                    var config = JsonSerializer.Deserialize<AuthenticationConfig>(json);
                    if (config != null)
                    {
                        return config;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("[ERROR] Error dalam memuat configuration!: ");
                    Console.WriteLine("\n" + e.Message);
                }
            }

            // buat configuration default jika file tidak ada/tidak bisa di load
            var defaultCon = new AuthenticationConfig();
            SaveConfiguration(defaultCon);
            return defaultCon;
        }

        // simpan configuration 
        public static void SaveConfiguration(AuthenticationConfig config)
        {
            try
            {
                string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ConfigFile, json);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Error dalam menyimpan configuration!: ");
                Console.WriteLine("\n" + e.Message);
            }
        }

        // update configuration ketika runtime
        public static void UpdateConfiguration(Action<AuthenticationConfig> update)
        {
            update(Instance);
            SaveConfiguration(Instance);
        }
    }
}
