using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;

namespace EFModels
{
    public class StandartCase
    {
        public List<StandartEvent> Events { get; set; }

        public StandartCase()
        {
            Events = new List<StandartEvent>();

            Events.Add(new StandartEvent("ZTP-Z1(Заявка подана)"));
            Events.Add(new StandartEvent("ZTP-Z3(Заявка принята)"));
            Events.Add(new StandartEvent("ZTY-Y1(В работе)"));
            Events.Add(new StandartEvent("ZTY-YY(На согласов. SAP)"));
            Events.Add(new StandartEvent("ZTY-Y3(Согласован РСК)"));
            Events.Add(new StandartEvent("ZDT-1(Cоздан)"));
            Events.Add(new StandartEvent("ZDT-ZA(На согласовании)"));
            Events.Add(new StandartEvent("ZDT-Z7(Соглас-е/подп-е.)"));
            Events.Add(new StandartEvent("ZTY-Y6(Зарегистрирован)"));
            Events.Add(new StandartEvent("ZDT-ZB(Зарегистрирован)"));
            Events.Add(new StandartEvent("ZDT-ZO(Направлен заявит)"));
            Events.Add(new StandartEvent("ZDT-Z8(Подписан/заключ.)"));
            Events.Add(new StandartEvent("ZTY-Y7(На исполнении)"));
            Events.Add(new StandartEvent("ZTY-YA(Проверка вып ТУ)"));
            Events.Add(new StandartEvent("ZTY-YB(ТУ выполнено)"));
            Events.Add(new StandartEvent("ZDT-ZT(Техническое закр)"));
            Events.Add(new StandartEvent("ZTP-ZP(Отказ в присоед.)"));
            Events.Add(new StandartEvent("ZTP-Z4(Заявка аннулиров)"));
            Events.Add(new StandartEvent("ZTY-YC(Аннулировано)"));
            Events.Add(new StandartEvent("ZDT-ZE(Аннулирован)"));
            Events.Add(new StandartEvent("ZTP-Z2(Недостат. сведен)"));

            foreach (var e in Events)
            {
                e.CreateEdges(Events.Count);
            }

            Events[0].Edges[1].Trans = true;
            Events[0].Edges[1].deltaTime = new TimeSpan(15, 0, 0, 0).TotalSeconds;
            Events[0].Edges[20].Trans = true;
            Events[0].Edges[20].deltaTime = new TimeSpan(15, 0, 0, 0).TotalSeconds;

            Events[1].Edges[2].Trans = true;
            Events[1].Edges[2].deltaTime = new TimeSpan(0, 0, 1,0).TotalSeconds;
            Events[2].Edges[3].Trans = true;
            Events[2].Edges[3].deltaTime = new TimeSpan(0, 0, 2, 0).TotalSeconds;
            Events[3].Edges[4].Trans = true;
            Events[3].Edges[4].deltaTime = new TimeSpan(0, 0, 2, 0).TotalSeconds;
            Events[4].Edges[5].Trans = true;

            Events[5].Edges[6].Trans = true;
            Events[5].Edges[6].deltaTime = new TimeSpan(0, 0, 2, 0).TotalSeconds;
            Events[6].Edges[7].Trans = true;
            Events[6].Edges[7].deltaTime = new TimeSpan(0, 0, 3, 0).TotalSeconds;
            Events[7].Edges[8].Trans = true;
            Events[8].Edges[9].Trans = true;
            Events[8].Edges[9].deltaTime = new TimeSpan(0, 0, 3, 0).TotalSeconds;
            Events[9].Edges[10].Trans = true;
            Events[10].Edges[11].Trans = true;
            Events[11].Edges[12].Trans = true;
            Events[12].Edges[13].Trans = true;
            Events[13].Edges[14].Trans = true;
            Events[13].Edges[14].deltaTime = new TimeSpan(3, 0, 3, 0).TotalSeconds;
            Events[14].Edges[15].Trans = true;
            Events[16].Edges[17].Trans = true;
            Events[17].Edges[18].Trans = true;
            Events[17].Edges[19].Trans = true;
            Events[20].Edges[16].Trans = true;

        }

        public StandartEvent GetEvent(string Name)
        {
            return Events.FirstOrDefault(s => s.Name == Name);
        }
    }

    public class StandartEvent
    {
        public string Name { get; set; }
        public StandartEdge[] Edges { get; set; }

        public StandartEvent()
        {
        }

        public StandartEvent(string Name)
        {
            this.Name = Name;
        }

        public void CreateEdges(int Count)
        {
            Edges = new StandartEdge[Count];
            for (int i = 0; i < Count; i++)
            {
                Edges[i] = new StandartEdge();
            }
        }

        public List<int> GetNextEventNumbers()
        {
            List<int> NextEvents = new List<int>();
            for (int i = 0; i < Edges.Count(); i++)
            {
                if (Edges[i].Trans)
                    NextEvents.Add(i);
            }
            return NextEvents;
        }
    }

    //Можно создать абстракцию для множества типов связей (условия проверки)
    public class StandartEdge
    {
        public bool Trans { get; set; }
        public double deltaTime { get; set; }

    }
}
