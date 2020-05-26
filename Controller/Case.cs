﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Controller
{
    public class Case
    {
        public Event Begin { get; set; }
        public List<Event> Events { get; set; }
        public long CaseId { get; set; }


        public Case(long CaseId)
        {
            this.CaseId = CaseId;
            Events = new List<Event>();
        }

        /// <summary>
        /// Добавление события в цепочку
        /// </summary>
        public void Add(Event @event)
        {
            @event = new Event(@event);

            if (Events.Count == 0)
            {
                Begin = @event;
                Events.Add(@event);
            }
            else
            {
                @event.Previous.Add(Events.Last());
                Events.Last().Next.Add(@event,1);
                Events.Add(@event);
            }

        }

        /// <summary>
        /// Обновление иерархии бизнес-процесса
        /// </summary>
        public void UpdateHierarchy(Event @event)
        {
            @event = new Event(@event);
            @event.TimeStamp = default;

            var Event = Events.FirstOrDefault(s => s.Name == @event.Name);
            if (Event != null)
            {
                Event.Count++;

                if (@event.Previous.Count != 0)
                {
                    var PrevEvent = Events.FirstOrDefault(s => s.Name == @event.Previous[0].Name);

                    if (PrevEvent != null)
                    {
                        var NextEvent = PrevEvent.Next.FirstOrDefault(s => s.Key.Name == Event.Name);
                        if (NextEvent.Equals(default(KeyValuePair<Event, int>)))
                        {
                            PrevEvent.Next.Add(Event, 1);
                        }
                        else
                        {
                            PrevEvent.Next[Event]++;
                        }
                    }
                }
            }
            else
            {
                if (Events.Count == 0)
                {
                    Begin = @event;
                    Events.Add(@event);
                }
                else
                {
                    var PrevEvent = Events.FirstOrDefault(s => s.Name == @event.Previous[0].Name);

                    var NextEvent = PrevEvent.Next.FirstOrDefault(s => s.Key.Name == @event.Name);
                    if (NextEvent.Equals(default(KeyValuePair<Event, int>)))
                    {
                        PrevEvent.Next.Add(@event,1);
                        Events.Add(@event);
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{CaseId}:{Events.Count}";
        }
    }

    public class Event
    {
        public Dictionary<Event, int> Next { get; set; }
        public List<Event> Previous { get; set; }
        public string Name { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Count { get; set; }

        public Event(string name)
        {
            Name = name;
            Next = new Dictionary<Event, int>();
            Previous = new List<Event>();
            Count = 1;
        }

        public Event(Event @event)
        {
            Name = @event.Name;
            Next = new Dictionary<Event, int>();

            foreach (var v in @event.Next)
            {
                Next.Add(v.Key, v.Value);
            }

            Previous = new List<Event>();
            Previous.AddRange(@event.Previous);
            TimeStamp = @event.TimeStamp;
            Count = 1;
        }

        public override string ToString()
        {
            return $"{Name}:{Count}";
        }
    }
}
