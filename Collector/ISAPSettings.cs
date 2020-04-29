using ERPConnect; 

namespace Collector
{
    /// <summary>
    /// Интерфейс настройки параметров сборщика для SAP
    /// </summary>
    internal interface ISAPSettings
    {
        /// <summary>
        /// Свойство подключения к системе SAP
        /// </summary>
        public R3Connection Connection { get; set; }

        /// <summary>
        /// Название функции SAP
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Создание подключения к SAP
        /// </summary>
        public void CreateConnection(string connectionString);
    }
}