using ERPConnect;
using System;
using System.Data;
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
            ExtractFromFile(3, 2);

#endif
            //TODO: сохранение в очередь
        }

        
        private DataTable ExtractFromFile(int count, int start = 0)
        {

            DataTable dataTable = new DataTable();

            XPathDocument document = new XPathDocument("tp.xml");

            XPathNavigator navigator = document.CreateNavigator();


            const string URI = "urn:schemas-microsoft-com:office:spreadsheet";

            navigator.MoveToFollowing("Table", URI);
            navigator.MoveToFirstAttribute();
            int columnsCount = Convert.ToInt32(navigator.Value);
            navigator.MoveToParent();

            XPathNodeIterator rowIterator = navigator.SelectChildren("Row", URI);
            rowIterator.MoveNext();

            //заполняем столбцы datatable из 1 строки xml
            XPathNodeIterator cellIterator = rowIterator.Current.SelectChildren("Cell", URI);
            cellIterator.MoveNext();
            for (int i = 0; i < columnsCount; i++)
            {
                dataTable.Columns.Add(cellIterator.Current.Value);
                cellIterator.MoveNext();
            }

            //заполняем строки datatable
            for (int i = 0; i < count + start; i++)
            {
                rowIterator.MoveNext();
                if (i < start) continue;

                string[] rowString = new string[columnsCount];
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
                            rowString[current] = cellIterator.Current.Value;
                            j = current;
                        }
                        else 
                        {
                            cellIterator.Current.MoveToParent();
                            rowString[j] = cellIterator.Current.Value;
                        }
                        
                    }
                    else
                    {
                        rowString[j] = cellIterator.Current.Value;
                    }
                    cellIterator.MoveNext();
                }

                dataTable.Rows.Add(rowString);
            }


            return dataTable;
        }

        public override void SendData()
        {
            //TODO: отправка данных из очереди
            throw new NotImplementedException();
        }


    }
}
