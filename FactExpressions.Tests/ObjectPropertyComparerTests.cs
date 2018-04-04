using System;
using System.Linq;

using FactExpressions.Conversion;

using NUnit.Framework;

namespace FactExpressions.Tests
{
    [TestFixture]
    public class ObjectPropertyComparerTests
    {
        [Test]
        public void Comparer_obtains_difference()
        {
            var comparer = new ObjectPropertyComparer();
            var object1 = new Person("Robin", 35);
            var object2 = new Person("Robin", 36);

            var difference = comparer.Compare(object1, object2).Single();

            Assert.AreEqual("Age", difference.Property.Name);
            Assert.AreEqual(typeof(Double), difference.Property.PropertyType);
            Assert.AreEqual(35d, difference.Previous);
            Assert.AreEqual(36d, difference.Current);
        }

        [Test]
        public void Comparer_ignores_identical_values()
        {
            var comparer = new ObjectPropertyComparer();
            var object1 = new Person("Robin", 35);
            var object2 = new Person("Robin", 35);

            Assert.False(comparer.Compare(object1, object2).Any());
        }
    }
}
