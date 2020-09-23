using EFModels;
using EFModels.MainDB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Controller
{
    public class Analyzer
    {
        public List<Alert> FindAlerts()
        {
            MainDBContext mainDB = new MainDBContext();
            var BPs = mainDB.BPCases.Include(s => s.BP).ToList();

            CaseBuilder caseBuilder = new CaseBuilder();

            List<Alert> alerts = new List<Alert>();

            foreach (var bp in BPs)
            {
                Case @case = caseBuilder.CreateCase(bp.CaseId);
                
                alerts.AddRange(SearchRouteErrors(@case, bp.BP.StandartCase));
                alerts.AddRange(SearchTimeErrors(@case, bp.BP.StandartCase));
                alerts.AddRange(SearchAccessErrors(@case, bp.BP.AccessMatrix));
            }
            return alerts;
        }

        public List<Alert> SearchRouteErrors(Case @case, StandartCase standartCase)
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
                    NextEventsNames.Add(standartCase.Events[id].Name);

                if (NextEventsNames.Contains(next.Name))
                {
                    i = NextEventsId[NextEventsNames.IndexOf(next.Name)];
                }
                else
                {
                    i = standartCase.Events.IndexOf(standartCase.GetEvent(next.Name));

                    alerts.Add(new Alert
                    {
                        Type = "Ошибка маршрута",
                        CaseId = @case.CaseId,
                        Text = $"{current.Name} -> {next.Name}",
                        BPId = @case.BPId
                    });
                }
                current = next;
                if (current.Next.Count != 0)
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
                    NextEventsNames.Add(standartCase.Events[id].Name);

                if (NextEventsNames.Contains(next.Name))
                {
                    int j = i;
                    i = NextEventsId[NextEventsNames.IndexOf(next.Name)];

                    DateTime timeCurrent = current.TimeStamp;
                    DateTime timeNext = next.TimeStamp;

                    double deltaTime = standartCase.Events[j].Edges[i].deltaTime;

                    if (deltaTime != 0 && (timeNext - timeCurrent).TotalSeconds > deltaTime)
                    {
                        alerts.Add(new Alert
                        {
                            Type = "Ошибка времени выполнения",
                            CaseId = @case.CaseId,
                            Text = $"{current.Name} -> {next.Name} = {timeNext - timeCurrent} > Нормированного {TimeSpan.FromSeconds(deltaTime)} ",
                            BPId = @case.BPId
                        });
                    }
                }
                else
                {
                    i = standartCase.Events.IndexOf(standartCase.GetEvent(next.Name));
                }

                current = next;
                if (current.Next.Count != 0)
                    next = current.Next[0].Key;
            }
            return alerts;
        }

        public List<Alert> SearchAccessErrors(Case @case, AccessMatrix accessMatrix)
        {
            List<Alert> alerts = new List<Alert>();
            var matrix = accessMatrix.Matrix;
            Event current = @case.Begin;
            if (current == null)
                return alerts;

            while (current.Next.Count != 0)
            {
                if (current.ResourceId!=null && !matrix[current.ResourceId][current.Name])
                {
                    alerts.Add(new Alert
                    {
                        Type = "Ошибка уровня доступа",
                        CaseId = @case.CaseId,
                        Text = $"Отсутствует доступ у {current.ResourceId} к {current.Name}",
                        BPId = @case.BPId
                    });
                }
                current = current.Next[0].Key;
            }
            return alerts;
        }
    }
}
