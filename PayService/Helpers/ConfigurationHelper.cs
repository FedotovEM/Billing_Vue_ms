namespace PayService.Helpers
{
    /// <summary>
    /// Получает значения параметра из конфигурации
    /// </summary>
    /// <param name="configKeyName">Наименование параметра</param>
    /// <returns>Значение параметра конфигурации</returns>
    static class ConfigurationHelper
    {
        public static string GetSectionValue(string configKeyName)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IConfigurationSection section = config.GetSection(configKeyName);

            return section.Value;
        }
    }
}
