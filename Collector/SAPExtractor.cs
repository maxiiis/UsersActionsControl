using ERPConnect;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Collector
{
    /// <summary>
    /// Сборщик для SAP с ERPConnect
    /// </summary>
    public class SAPExtractor : Extractor, ISAPSettings
    {
        public R3Connection Connection { get; set; }
        public string FunctionName { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="connectionString">Строка подключения к SAP</param>
        /// <param name="functionName">Название функции в SAP</param>
        public SAPExtractor(string connectionString, string functionName)
        {
#if !TEST
            try
            {
                CreateConnection(connectionString);
                FunctionName = functionName;
                OpenConnect();
            }
            catch 
            {
                throw new ERPException("Connection string not correct or system not available");
            }
#endif
        }

        public void CreateConnection(string connectionString)
        {
            Connection = new R3Connection(connectionString);
            Connection.ParseConnectionString(connectionString);
        }

        public override void OpenConnect()
        {
            Connection.Open();
        }

        public override void CloseConnect()
        {
            Connection.Close();
        }

        public override void ExtractData()
        {
#if DEBUG
            RFCFunction function = Connection.CreateFunction(FunctionName);
            //TODO: определиться с входными параметрами
            //function.Exports.Add
            function.Execute();

            //TODO: определиться с вызодными параметрами 
            // или
            var data = function.Imports[0].ToTable();
            data.ToADOTable();
            // 
            // или
            function.Tables[0].ToADOTable();
            //
#elif TEST

            //загрузка из tp.csv

            
#endif
            //TODO: сохранение в очередь
        }

        public override void SendData()
        {
            //TODO: отправка данных из очереди
            throw new NotImplementedException();
        }

        
    }
}
