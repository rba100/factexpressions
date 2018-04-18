
using System;
using System.Linq;

using FactExpressions.Conversion;
using FactExpressions.Events;
using FactExpressions.Language;

namespace FactExpressions
{
    class Program
    {
        static void Main()
        {
            var daybreakEventMessage = new BusMessage("daybreak", null);
            var birthdayEventMessage = new BusMessage("birthday", null);
            var robinPrevious = new Person("Robin", age: 35) { HairColour = "brown" };
            var robinCurrent = new Person("Robin", age: 36) { HairColour = "grey" };

            IEventLogger eventLogger = new EventLogger();

            eventLogger.LogEvent(daybreakEventMessage);

            eventLogger.LogThat(birthdayEventMessage).WasReceived()
                       .AndThus(robinPrevious).Became(robinCurrent);

            var objectDescriber = new ObjectDescriber();
            var eventDescriber = new EventDescriber(objectDescriber);

            objectDescriber.AddDescriber<Person>(p => new Noun($"{p.Name}"));
            objectDescriber.AddPronoun<Person>(p => Pronouns.Male);
            objectDescriber.AddDescriber<BusMessage>(m => new Noun($"Message of type {m.Type}"));
            eventDescriber.Describe(eventLogger.EventItems()).ToList().ForEach(e => e.PrintToConsole());

            Console.ReadLine();
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
