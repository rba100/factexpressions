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
            relationStore.AddSimpleRelation(typeof(SunRise), typeof(BirdsSing));
            relationStore.Declare(typeof(Birthday)).CanAlter<Person>(p => p.Age);

            var robin    = new Person("Robin", age: 35);
            var robinNew = new Person("Robin", age: 36);
            var birthday = new Birthday();

            var eventLogger = new EventLogger();
            eventLogger.LogAsEvent(new SunRise());
            eventLogger.LogAsEvent(birthday);
            eventLogger.LogThat(robin).Became(robinNew);
            eventLogger.LogAsEvent(new BirdsSing());

            var analyser = new EventAnalyser(relationStore, StaticDescribers());

            var exps = analyser.GetExpressions(eventLogger);

            var differences = new ObjectPropertyComparer().Compare(robin, robinNew);

            var expressionConverter = new ObjectExpressionConverter();

            var expression = expressionConverter.FromPropertyDifferences(typeof(Person), differences);

            Console.WriteLine(expression);

            foreach (var exp in exps)
            {
                Console.WriteLine(exp);
            }

            Console.ReadLine();
        }

        static ObjectExpressionConverter StaticDescribers()
        {
            var c = new ObjectExpressionConverter();

            c.AddDescriber<int>(d => $"Age: {d}");
            c.AddDescriber<Double>(d => $"Age: {d}");
            c.AddDescriber<Person>(p => new NounExpression($"{p.Name}"));
            c.AddDescriber<Birthday>(p => new IndefiniteNounExpression("birthday"));
            c.AddDescriber<SunRise>(p => new NounExpression("sunrise"));
            c.AddDescriber<BirdsSing>(p => new VerbExpression(Verbs.ToBecome, Tense.Past, new NounExpression("birds"), new NounExpression("noisy")));

            return c;
        }

    }

    class SunRise
    {
        
    }

    class BirdsSing
    {
        
    }

    class Person
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
