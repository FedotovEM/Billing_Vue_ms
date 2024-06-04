namespace RepairRequestsService.Helpers
{
    static class ConfigurationHelper
    {
        /// <summary>
        /// Получает значения параметра из конфигурации
        /// </summary>
        /// <param name="configKeyName">Наименование параметра</param>
        /// <returns>Значение параметра конфигурации</returns>
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
