using EFModels.LogsDB;
using System.Collections.Generic;
using System.Linq;

namespace EFModels
{
    public class CaseBuilder
    {
        public Case CreateCase(long CaseId)
        {
            LogDBContext logDB = new LogDBContext();

            var Events = logDB.EventLogs.Where(b => b.CaseId == CaseId).OrderBy(b => b.TimeStamp).ToList();

            Case newCase = new Case(CaseId);

            foreach (var e in Events)
            {
                newCase.Add(new Event(e.Activity) { TimeStamp = e.TimeStamp });
            }

            return newCase;
        }

        public Case CreateGeneralCase(string BpName="")
        {
            LogDBContext logDB = new LogDBContext();
            var s = logDB.EventLogs.OrderBy(s => s.CaseId).ThenBy(s => s.TimeStamp).ToList();

            var groups = s.GroupBy(s => s.CaseId);

            List<Case> cases = new List<Case>();

            Case general = new Case(-1);

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
                    bP.Add(new Event(e.Activity) { TimeStamp = e.TimeStamp });
                    general.UpdateHierarchy(bP.Events.Last());
                }
            }
            return general;
        }
    }
}
