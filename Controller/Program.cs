using System;
using System.Collections.Generic;

namespace Controller
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }

    public class Event
    {
        public List<Event> Next { get; set; }
        public List<Event> Previous { get; set; }
        public string Name { get; set; }
        public double Frequency { get; set; }

        public Event(string name)
        {
            Name = name;
        }
    }

    public class BP
    {
        public Event Begin { get; set; }
        public string Name { get; set; }
        public List<Event> Stages { get; set; }

        public BP(string Name, Event start)
        {
            this.Name = Name;
            Begin = start;
            Stages = new List<Event>();
            Stages.Add(start);
        }

        public void Add()
        { }

        public void Find()
        { }
    }
}
