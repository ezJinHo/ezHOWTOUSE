using System;
using System.IO;
using System.Text.Json;

namespace HOWTOUSE
{
    public sealed class AppSettings
    {
        private static readonly Lazy<AppSettings> LazyInstance = new Lazy<AppSettings>(Load);

        public static AppSettings Current => LazyInstance.Value;

        public DatabaseSettings Database { get; set; } = new DatabaseSettings();

        private static AppSettings Load()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AppSettings.json");

            if (!File.Exists(filePath))
            {
                return new AppSettings();
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new AppSettings();
        }
    }

    public sealed class DatabaseSettings
    {
        public string Server { get; set; } = "localhost";

        public int Port { get; set; } = 3306;

        public string Name { get; set; } = "ezhowtouse";

        public string User { get; set; } = "root";

        public string Password { get; set; } = string.Empty;

        public bool AllowPublicKeyRetrieval { get; set; } = true;

        public string SslMode { get; set; } = "none";

        public string ConnectionString =>
            $"Server={Server};Port={Port};Database={Name};Uid={User};Pwd={Password};SslMode={SslMode};AllowPublicKeyRetrieval={AllowPublicKeyRetrieval};";
    }
}
