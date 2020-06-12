using Collector;
using EFModels;
using EFModels.LogsDB;
using EFModels.MainDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Controller
{
    class Program
    {
        static void Main(string[] args)
        {

            //StandartCase standartCase = new StandartCase();

            //string sssss = JsonSerializer.Serialize(standartCase);

            //MainDBContext mainDBContext = new MainDBContext();
            //mainDBContext.BPs.First().StandartCase = standartCase;
            //mainDBContext.SaveChanges();

            LogDBContext context = new LogDBContext();
            var s = context.EventLogs.OrderBy(s => s.CaseId).ThenBy(s => s.TimeStamp).ToList();

            var groups = s.GroupBy(s => s.CaseId);

            List<Case> cases = new List<Case>();

            Case etalon = new Case(-1);

            foreach (var g in groups)
            {
                Case bP = cases.FirstOrDefault(f => f.CaseId == g.Key);

                if (bP == null)
                {
                    bP = new Case(g.Key);
                    cases.Add(bP);
                }

                foreach (var e in g)
                {
                    bP.Add(new Event(e.ActivityId) { TimeStamp = e.TimeStamp });
                    etalon.UpdateHierarchy(bP.Events.Last());
                }
            }
        }
    }
}
