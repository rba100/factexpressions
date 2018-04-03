using FactExpressions.Conversion;
using NUnit.Framework;

namespace FactExpressions.Tests
{
    [TestFixture]
    public class ObjectExpressionConverterTests
    {
        [Test]
        public void Blah()
        {
            var object1 = new TestClass { Val = 20 };
            var object2 = new TestClass { Val = 40 };

            var differences = new ObjectPropertyComparer().Compare(object1, object2);

            var expressionConverter = new ObjectExpressionConverter();

            var expression = expressionConverter.FromPropertyDifferences(typeof(TestClass), differences);
        }
    }
}