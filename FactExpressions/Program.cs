
using System;
using System.Collections.Generic;
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

            var mogginsPrevious = new Pet("old moggins", "cat", remainingLimbs: 4);
            var mogginsCurrent  = new Pet("old moggins", "cat", remainingLimbs: 3);

            var robinPrevious = new Person("Robin", age: 35) { HairColour = "brown", Pets = new[] { mogginsPrevious } };
            var robinCurrent  = new Person("Robin", age: 36) { HairColour = "grey",  Pets = new[] { mogginsPrevious } };

            IEventLogger eventLogger = new EventLogger();

            eventLogger.LogEvent(daybreakEventMessage);

            var childLogger = eventLogger.LogThat(birthdayEventMessage).WasReceived().GetChildLogger();
            childLogger.LogThat(robinPrevious).Became(robinCurrent);

            eventLogger.LogEvent("accident")
                       .AndThus(mogginsPrevious)
                       .Became(mogginsCurrent);

            var objectDescriber = new ObjectDescriber();
            var eventDescriber = new EventDescriber(objectDescriber);

            objectDescriber.AddDescriber<Person>(p => new Noun($"{p.Name}"));
            objectDescriber.AddDescriber<Pet>(p => new Noun($"{p.Name} the {p.Species} with {p.RemainingLimbs} leg{(p.RemainingLimbs == 1 ? "" : "s")}"));
            objectDescriber.AddDescriber<IEnumerable<Pet>>(pets => "'" + string.Join(", ", pets.Select(p => objectDescriber.GetNoun(p))) + "'");
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

        public IReadOnlyCollection<Pet> Pets { get; set; }

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

    public class Pet
    {
        public string Name { get; }
        public string Species { get; }
        public int RemainingLimbs { get; }

        public Pet(string name, string species, int remainingLimbs)
        {
            Name = name;
            Species = species;
            RemainingLimbs = remainingLimbs;
        }

        protected bool Equals(Pet other)
        {
            return string.Equals(Name, other.Name) && string.Equals(Species, other.Species) && RemainingLimbs == other.RemainingLimbs;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Pet)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Species != null ? Species.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ RemainingLimbs;
                return hashCode;
            }
        }
    }
}
