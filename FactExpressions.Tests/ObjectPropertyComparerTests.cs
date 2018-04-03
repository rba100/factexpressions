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
            var object1 = new TestClass { Val = 1 };
            var object2 = new TestClass { Val = 2 };

            var difference = comparer.Compare(object1, object2).Single();

            Assert.AreEqual("Val", difference.Property.Name);
            Assert.AreEqual(typeof(int), difference.Property.PropertyType);
            Assert.AreEqual(1, difference.Previous);
            Assert.AreEqual(2, difference.Current);
        }

        [Test]
        public void Comparer_ignores_identical_values()
        {
            var comparer = new ObjectPropertyComparer();
            var object1 = new TestClass { Val = 1 };
            var object2 = new TestClass { Val = 1 };

            Assert.False(comparer.Compare(object1, object2).Any());
        }
    }
}
