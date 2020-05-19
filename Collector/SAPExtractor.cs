using EFModels.LogsDB;
using ERPConnect;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text.Json;
using System.Xml.XPath;

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
            var data = ExtractFromFile(10);

            foreach (var d in data)
            {
                Queue.Enqueue(JsonSerializer.Serialize(d));
            }

            SendData();
#endif
        }


        private List<EventLog> ExtractFromFile(int count = 0, int start = 0)
        {
            List<EventLog> results = new List<EventLog>();

            XPathDocument document = new XPathDocument("tp.xml");
            XPathNavigator navigator = document.CreateNavigator();

            //навигация по xml к строкам
            const string URI = "urn:schemas-microsoft-com:office:spreadsheet";
            navigator.MoveToFollowing("Table", URI);
            navigator.MoveToFirstAttribute();
            int columnsCount = Convert.ToInt32(navigator.Value);
            navigator.MoveToNextAttribute();
            int RowsCount = Convert.ToInt32(navigator.Value);

            navigator.MoveToParent();

            XPathNodeIterator rowIterator = navigator.SelectChildren("Row", URI);
            rowIterator.MoveNext();

            //получаем столбцы из 1 строки xml
            XPathNodeIterator cellIterator = rowIterator.Current.SelectChildren("Cell", URI);
            cellIterator.MoveNext();

            List<string> columnsName = new List<string>();

            for (int i = 0; i < columnsCount; i++)
            {
                columnsName.Add(cellIterator.Current.Value);
                cellIterator.MoveNext();
            }

            //заполняем логи
            int MaxRows;
            MaxRows = count + start == 0 ? RowsCount - 1 : count + start;

            for (int i = 0; i < MaxRows; i++)
            {
                rowIterator.MoveNext();
                if (i < start) continue;

                EventLog log = new EventLog();

                EventLogData logData = new EventLogData();

                cellIterator = rowIterator.Current.SelectChildren("Cell", URI);
                cellIterator.MoveNext();
                for (int j = 0; j < columnsCount; j++)
                {
                    if (cellIterator.Current.MoveToFirstAttribute())
                    {
                        if (cellIterator.Current.Name == "ss:Index")
                        {
                            int current = Convert.ToInt32(cellIterator.Current.Value) - 1;
                            cellIterator.Current.MoveToParent();

                            string colName = columnsName[current];
                            string currentValue = cellIterator.Current.Value;
                            FillLogField(currentValue, colName,ref log,ref logData);

                            j = current;
                        }
                        else 
                        {
                            cellIterator.Current.MoveToParent();
                            string colName = columnsName[j];
                            string currentValue = cellIterator.Current.Value;
                            FillLogField(currentValue, colName, ref log, ref logData);
                        }
                        
                    }
                    else
                    {
                        string colName = columnsName[j];
                        string currentValue = cellIterator.Current.Value;
                        FillLogField(currentValue, colName, ref log, ref logData);
                    }
                    cellIterator.MoveNext();
                }

                log.EventLogData = logData;
                results.Add(log);
            }

            return results;
        }

        private void FillLogField(string currentValue, string colName,ref  EventLog log,ref  EventLogData logData)
        {
            switch (colName)
            {
                case "CaseId": log.CaseId = Convert.ToInt64(currentValue); break;
                case "TimeStamp": log.TimeStamp = Convert.ToDateTime(currentValue); break;
                case "Resourse": log.Resourse = currentValue; break;
                case "Activity": log.Activity = currentValue; break;
                case "ActivityText": logData.ActivityText = currentValue; break;
                case "ActivityText2": logData.ActivityText2 = currentValue; break;
                case "ResourseDepartment": logData.ResourseDepartment = currentValue; break;
                case "ResourseFilial": logData.ResourseFilial = currentValue; break;
                case "ResourseFIO": logData.ResourseFIO = currentValue; break;
            }
        }

        public override void SendData()
        {
            LogDBContext context = new LogDBContext();
            foreach (var d in Queue)
            {
                context.Add(JsonSerializer.Deserialize<EventLog>(d));
            }
            context.SaveChanges();
        }


    }
}
