using System.Collections.Generic;
using System.Reflection;

namespace FactExpressions.Conversion
{
    public class ObjectPropertyComparer
    {
        public IEnumerable<PropertyDifference> Compare<T>(T previous, T current)
        {
            var differences = new List<PropertyDifference>();

            var type = previous.GetType();

            var publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in publicProperties)
            {
                if (!property.CanRead) continue;

                var previousValue = property.GetValue(previous);
                var currentValue = property.GetValue(current);

                if (previousValue != null)
                {
                    if (!previousValue.Equals(currentValue))
                    {
                        differences.Add(new PropertyDifference(property,
                            previousValue,
                            currentValue));
                    }
                }
                else if (currentValue != null)
                {
                    differences.Add(new PropertyDifference(property,
                        null,
                        currentValue));
                }
            }

            return differences;
        }
    }

    public class PropertyDifference
    {
        public PropertyDifference(PropertyInfo property,
                                  object previous,
                                  object current)
        {
            Property = property;
            Previous = previous;
            Current = current;
        }

        public PropertyInfo Property { get; }
        public object Previous { get; }
        public object Current { get; }
    }
}