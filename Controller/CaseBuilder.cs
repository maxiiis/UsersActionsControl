using EFModels;
using EFModels.LogsDB;
using EFModels.MainDB;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Controller
{
    public class CaseBuilder
    {
        public Case CreateCase(long CaseId)
        {
            LogDBContext logDB = new LogDBContext();
            MainDBContext mainDB = new MainDBContext();
            var Events = logDB.EventLogs.Include(s => s.Resource).Include(s => s.Activity).Where(b => b.CaseId == CaseId).OrderBy(b => b.TimeStamp).ToList();

            Case newCase = new Case(CaseId);
            newCase.BPId = mainDB.BPCases.FirstOrDefault(s => s.CaseId == CaseId).BPId;
            foreach (var e in Events)
            {
                Event @event = new Event(e.Activity.ActivityText2) { TimeStamp = e.TimeStamp };
                if (e.Resource.Department != null)
                    @event.ResourceId = e.ResourceId;
                newCase.Add(@event);
            }

            return newCase;
        }

        public Case CreateGeneralCase(string BpName = "")
        {
            //create only 1 BP
            LogDBContext logDB = new LogDBContext();
            var s = logDB.EventLogs.Include(s => s.Activity).Include(s => s.Resource).OrderBy(s => s.CaseId).ThenBy(s => s.TimeStamp).ToList();

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
                    bP.Add(new Event(e.Activity.ActivityText2) { TimeStamp = e.TimeStamp });
                    general.UpdateHierarchy(bP.Events.Last());
                }
            }
            return general;
        }


        public StandartCase CreateStandartCase(long BPId)
        {
            MainDBContext mainDB = new MainDBContext();

            BP bP = mainDB.BPs.FirstOrDefault(s => s.Id == BPId);

            if (bP == null)
                return null;

            StandartCase standart = bP.StandartCase;

            return standart;
        }

        private void UpdateCases()
        {
            LogDBContext logDB = new LogDBContext();
            var s = logDB.EventLogs.Include(s => s.Activity).Include(s => s.Resource).OrderBy(s => s.CaseId).ThenBy(s => s.TimeStamp).ToList();

            var groups = s.GroupBy(s => s.CaseId);

            MainDBContext mainDBContext = new MainDBContext();
            mainDBContext.BPCases.Load();

            foreach (var g in groups)
            {
                if (mainDBContext.BPCases.FirstOrDefault(s => s.CaseId == g.Key) == null)
                {
                    //пока только для 1 BP
                    mainDBContext.BPCases.Local.Add(new BPCase { BPId = 1, CaseId = g.Key });
                }
            }
            mainDBContext.SaveChanges();
        }
    }
}
