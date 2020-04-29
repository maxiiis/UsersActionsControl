using System;
using System.Collections.Generic;


namespace Collector
{
    /// <summary>
    /// Класс-родитель для сборщиков
    /// </summary>
    public abstract class Extractor :IDisposable
    {
        /// <summary>
        /// Очередь на отправку 
        /// <para>TODO: заменить на файл, как вариант</para>
        /// </summary>
        protected Queue<string> Queue = new Queue<string>();

        /// <summary>
        /// Открыть подключение
        /// </summary>
        public abstract void OpenConnect();

        /// <summary>
        /// Закрыть подключение
        /// </summary>
        public abstract void CloseConnect();

        /// <summary>
        /// Извлечь данные
        /// </summary>
        public abstract void ExtractData();

        /// <summary>
        /// Отправить данные
        /// </summary>
        public abstract void SendData();

        public virtual void Dispose()
        {
            CloseConnect();
        }
    }
}
