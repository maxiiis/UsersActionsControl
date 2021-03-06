﻿using EFModels.LogsDB;
using ERPConnect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            LoadFromLocalQueue();
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
            SaveInLocalQueue();
            Connection.Close();
        }
        public override void ExtractData()
        {
            //throw new NotImplementedException();
        }
        public void ExtractData(int count = 0,int start=0)
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

            LoadFromLocalQueue();

            //загрузка из tp.csv
            var data = ExtractFromFile(count,start);

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

            XPathDocument document = new XPathDocument(@"Files/tp.xml");
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

                Activity logData = new Activity();

                User user = new User();

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
                            FillLogField(currentValue, colName, ref log, ref logData, ref user);

                            j = current;
                        }
                        else
                        {
                            cellIterator.Current.MoveToParent();
                            string colName = columnsName[j];
                            string currentValue = cellIterator.Current.Value;
                            FillLogField(currentValue, colName, ref log, ref logData, ref user);
                        }

                    }
                    else
                    {
                        string colName = columnsName[j];
                        string currentValue = cellIterator.Current.Value;
                        FillLogField(currentValue, colName, ref log, ref logData, ref user);
                    }
                    cellIterator.MoveNext();
                }

                log.Activity = logData;
                log.Resource = user;
                results.Add(log);
            }

            return results;
        }

        private void FillLogField(string currentValue, string colName, ref EventLog log, ref Activity logData, ref User user)
        {
            switch (colName)
            {
                case "CaseId": log.CaseId = Convert.ToInt64(currentValue); break;
                case "TimeStamp": log.TimeStamp = Convert.ToDateTime(currentValue); break;
                case "Resourse":
                    log.ResourceId = currentValue;
                    user.Id = currentValue;
                    break;
                case "Activity":
                    log.ActivityId = currentValue;
                    logData.Id = currentValue;
                    break;
                case "ActivityText": logData.ActivityText = currentValue; break;
                case "ActivityText2": logData.ActivityText2 = currentValue; break;
                case "ResourseDepartment": user.Department = currentValue; break;
                case "ResourseFilial": user.Filial = currentValue; break;
                case "ResourseFIO":
                    var split = currentValue.Split(' ').ToArray();
                    if (split.Count() > 1)
                    {
                        user.LastName = currentValue.Split(' ').ToArray()[0];
                        user.FirstName = currentValue.Split(' ').ToArray()[1];
                        user.MiddleName = currentValue.Split(' ').ToArray()[2];
                    }
                    else
                    {
                        user.LastName = currentValue;
                    }
                    break;
                case "STATUS_TEXT": logData.StatusText = currentValue; break;
            }
        }

        public override void SendData()
        {
            LoadFromLocalQueue();

            LogDBContext context = new LogDBContext();
            while (Queue.Count != 0)
            {
                EventLog eventLog = JsonSerializer.Deserialize<EventLog>(Queue.Dequeue());

                if (context.Users.FirstOrDefault(s => s.Id == eventLog.ResourceId) == null)
                    context.Users.Add(eventLog.Resource);
                if (context.Activities.FirstOrDefault(s => s.Id == eventLog.ActivityId) == null)
                    context.Activities.Add(eventLog.Activity);

                eventLog.Activity = null;
                eventLog.Resource = null;
                if (context.EventLogs.FirstOrDefault(s => s.CaseId == eventLog.CaseId && s.TimeStamp == eventLog.TimeStamp) == null)
                    context.EventLogs.Add(eventLog);
                try
                {
                    context.SaveChanges();
                }
                catch
                {
                    SaveInLocalQueue();
                }
            }

        }

        //Сохранение результатов в файлы
        private void SaveInLocalQueue()
        {
            Type extractorType = GetType();
            if (!Directory.Exists(extractorType.Name))
                Directory.CreateDirectory(extractorType.Name);

            while (Queue.Count != 0)
            {
                string path = DateTime.Now.ToString("o").Replace(':', '-');
                path = $@"{extractorType.Name}\{path}.json";
                File.WriteAllText(path, Queue.Dequeue());
            }
        }

        //Загрузка результатов из файлов
        private void LoadFromLocalQueue()
        {
            Type extractorType = GetType();

            if (!Directory.Exists(extractorType.Name))
                return;

            IEnumerable<string> files = Directory.EnumerateFiles(extractorType.Name);

            if (files.Count() > 0)
            {
                while (files.Count() != 0)
                {
                    string data = File.ReadAllText(files.First());
                    Queue.Enqueue(data);
                    File.Delete(files.First());
                }
            }
        }
    }
}
