using EFModels.MainDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFModels
{
    public class Analyzer
    {
        public List<Alert> FindAlerts(long BPId = 0, long CaseId=0, long EventId =0)
        {
            MainDBContext mainDB = new MainDBContext();
            var BPs = mainDB.BPCases.Include(s=>s.BP).ToList();

            CaseBuilder caseBuilder = new CaseBuilder();

            List<Alert> alerts = new List<Alert>();

            foreach (var bp in BPs)
            {
                alerts.AddRange(SearchRouteErrors(caseBuilder.CreateCase(bp.CaseId), new StandartCase()));
                alerts.AddRange(SearchTimeErrors(caseBuilder.CreateCase(bp.CaseId), new StandartCase()));
            }
            return alerts;
        }

        public List<Alert> SearchRouteErrors(Case @case, StandartCase standartCase)
        {
            List<Alert> alerts = new List<Alert>();

            Event current = @case.Begin;
            Event next;
            if (current==null || current.Next.Count == 0)
                return alerts;
            else
                next =  @case.Begin.Next[0].Key;

            int i = 0;
            while (current.Next.Count!=0)
            {
                var NextEventsId = standartCase.Events[i].GetNextEventNumbers();
                List<string> NextEventsNames = new List<string>();

                foreach (var id in NextEventsId)
                {
                    NextEventsNames.Add(standartCase.Events[id].Name);
                }

                if (NextEventsNames.Contains(next.Name))
                {
                    i = NextEventsId[NextEventsNames.IndexOf(next.Name)];
                }
                else
                {
                    i = standartCase.Events.IndexOf(standartCase.GetEvent(next.Name));

                    alerts.Add(new Alert {
                        CaseId = @case.CaseId,
                        Text = $"{current.Name} -> {next.Name}",
                        BPId = 1,
                        Id=1
                    });
                }
                current = next;
                if (current.Next.Count == 0)
                {
                    return alerts;
                }
                else
                    next = current.Next[0].Key;
            }
            return alerts;
        }

        public List<Alert> SearchTimeErrors(Case @case, StandartCase standartCase)
        {
            List<Alert> alerts = new List<Alert>();

            Event current = @case.Begin;
            Event next;
            if (current == null || current.Next.Count == 0)
                return alerts;
            else
                next = @case.Begin.Next[0].Key;

            int i = 0;
            while (current.Next.Count != 0)
            {
                var NextEventsId = standartCase.Events[i].GetNextEventNumbers();

                List<string> NextEventsNames = new List<string>();

                foreach (var id in NextEventsId)
                {
                    NextEventsNames.Add(standartCase.Events[id].Name);
                }

                if (NextEventsNames.Contains(next.Name))
                {
                    int j = i;
                    i = NextEventsId[NextEventsNames.IndexOf(next.Name)];

                    DateTime timeCurrent = current.TimeStamp;
                    DateTime timeNext = next.TimeStamp;

                    TimeSpan deltaTime = standartCase.Events[j].Edges[i].deltaTime;

                    if (deltaTime!=TimeSpan.Zero && timeNext - timeCurrent > deltaTime)
                    {
                        alerts.Add(new Alert
                        {
                            CaseId = @case.CaseId,
                            Text = $"{current.Name} -> {next.Name} = {timeNext - timeCurrent} > Нормированного {deltaTime} ",
                            BPId = 1,
                            Id = 2
                        });
                    }
                }
                else
                {
                    i = standartCase.Events.IndexOf(standartCase.GetEvent(next.Name));
                }

                current = next;
                if (current.Next.Count == 0)
                {
                    return alerts;
                }
                else
                    next = current.Next[0].Key;
            }
            return alerts;
        }
    }
}
