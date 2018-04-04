using System;
using FactExpressions.Conversion;
using FactExpressions.Events;
using FactExpressions.Language;
using FactExpressions.Relations;

namespace FactExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            var robin = new Person("Robin", age: 35);
            var robinNew = new Person("Robin", age: 36) { HairColour = "grey" };

            var eventLogger = new EventLogger();

            eventLogger.LogAsEvent(new BusMessage("birthday", null));
            eventLogger.LogThat(robin).Became(robinNew);

            var analyser = new EventAnalyser(GetRelationStore(), GetDescribers());

            foreach (var exp in analyser.GetExpressions(eventLogger)) Console.WriteLine(exp);
            Console.ReadLine();
        }

        static RelationStore GetRelationStore()
        {
            var relationStore = new RelationStore();
            relationStore.DeclareThat<BusMessage>(m => m.Type == "birthday").CanAlter<Person>(p => p.Age);
            return relationStore;
        }

        static ObjectExpressionConverter GetDescribers()
        {
            var c = new ObjectExpressionConverter();
            c.AddDescriber<Person>(p => new Noun($"{p.Name}"));
            c.AddPronoun<Person>(p => Pronouns.Male);
            c.AddDescriber<BusMessage>(m => new Noun($"Message of type {m.Type}"));
            return c;
        }

    }

    public class BusMessage
    {
        public string Type { get; }
        public string Payload { get; }

        public BusMessage(string type, string payload)
        {
            Type = type;
            Payload = payload;
        }
    }

    public class Person
    {
        public string Name { get; }
        public Double Age { get; }

        public string HairColour { get; set; }

        public Person(string name, double age)
        {
            Name = name;
            Age = age;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
