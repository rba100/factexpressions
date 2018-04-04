using System;
using FactExpressions.Conversion;
using FactExpressions.Events;
using FactExpressions.Relations;

namespace FactExpressions
{
    class Program
    {
        static void Main(string[] args)
        {
            var relationStore = new RelationStore();
            relationStore.AddSimpleRelation(typeof(Birthday), typeof(Person));
            relationStore.DeclareThat<Birthday>().CanAlter<Person>(p => p.Age);

            var robin    = new Person("Robin", age: 35);
            var robinNew = new Person("Robin", age: 36);
            var birthday = new Birthday();

            var eventLogger = new EventLogger();
            eventLogger.LogAsEvent(birthday);
            eventLogger.LogThat(robin).Became(robinNew);

            var analyser = new EventAnalyser(relationStore, StaticDescribers());

            var exps = analyser.GetExpressions(eventLogger);

            foreach (var exp in exps)
            {
                Console.WriteLine(exp);
            }

            Console.ReadLine();
        }

        static ObjectExpressionConverter StaticDescribers()
        {
            var c = new ObjectExpressionConverter();

            c.AddDescriber<Person>(p => new NounExpression($"{p.Name}"));
            c.AddDescriber<Birthday>(p => new NounExpression("birthday"));

            return c;
        }

    }

    public class Person
    {
        public string Name { get; }
        public Double Age { get; }

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

    class Birthday
    {
        
    }
}
