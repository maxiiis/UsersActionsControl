using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EFModels
{
    public class Analyzer
    {
        public List<string> SearchRouteErrors(Case @case, StandartCase standartCase)
        {
            List<string> alerts = new List<string>();

            Event current = @case.Begin;
            Event next;
            if (current.Next.Count == 0)
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

                    alerts.Add($"Ошибка маршрута {current.Name} -> {next.Name}");
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

        public List<string> SearchTimeErrors(Case @case, StandartCase standartCase)
        {
            List<string> alerts = new List<string>();

            Event current = @case.Begin;
            Event next;
            if (current.Next.Count == 0)
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
                        alerts.Add($"Ошибка времени выполнения {current.Name} -> {next.Name} = {timeNext-timeCurrent} > Нормированного {deltaTime} ");
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
