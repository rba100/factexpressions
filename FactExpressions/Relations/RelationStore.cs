using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FactExpressions.Relations
{
    // TODO: Dijkstra's algorithm

    internal class RelationStore : IRelationStore
    {
        private readonly IDictionary<Type, HashSet<Type>> m_SimpleRelations
            = new Dictionary<Type, HashSet<Type>>();

        private readonly IDictionary<object, HashSet<object>> m_SimpleObjectRelations
            = new Dictionary<object, HashSet<object>>();


        private readonly IDictionary<Type, List<CausalRelation>> m_CausalRelations
            = new Dictionary<Type, List<CausalRelation>>();

        public void AddSimpleRelation(Type a, Type b)
        {
            if (a == b) return;

            var setA = GetOrCreate(a);
            var setB = GetOrCreate(b);

            setA.Add(b);
            setB.Add(a);
        }

        public void AddCausalRelation(CausalRelation causalRelation)
        {
            var list = GetOrCreateRelationList(causalRelation.Subject);
            list.Add(causalRelation);
        }

        public IRelationBuilder Declare(Type type)
        {
            return new RelationBuilder(this, type);
        }

        public IReadOnlyCollection<Type> GetSimpleRelations(Type type)
        {
            if (type == null) return new Type[0];
            return GetOrCreate(type).ToArray();
        }

        public IEnumerable<object> GetSimpleRelations(object obj)
        {
            if (obj == null) return new object[0];
            return GetOrCreate(obj).ToArray();
        }

        public int TypeConnectivity(Type type)
        {
            return m_SimpleRelations.Sum(kvp => kvp.Value.Count(t => t == type));
        }

        private HashSet<Type> GetOrCreate(Type type)
        {
            if (!m_SimpleRelations.TryGetValue(type, out HashSet<Type> hashSet))
            {
                hashSet = new HashSet<Type>();
                m_SimpleRelations[type] = hashSet;
            }
            return hashSet;
        }

        private List<CausalRelation> GetOrCreateRelationList(Type type)
        {
            if (!m_CausalRelations.TryGetValue(type, out List<CausalRelation> list))
            {
                list = new List<CausalRelation>();
                m_CausalRelations[type] = list;
            }
            return list;
        }

        private HashSet<object> GetOrCreate(object obj)
        {
            if (!m_SimpleObjectRelations.TryGetValue(obj, out HashSet<object> hashSet))
            {
                hashSet = new HashSet<object>();
                m_SimpleObjectRelations[obj] = hashSet;
            }
            return hashSet;
        }
    }

    public class CausalRelation
    {
        public RelationType RelationType { get; }
        
        public Type Subject { get; }
        public object Object { get; }

        public CausalRelation(RelationType relationType, Type subject, object o)
        {
            RelationType = relationType;
            Subject = subject;
            Object = o;
        }
    }

    public enum RelationType { Create, Remove, Alter }

    internal class RelationBuilder : IRelationBuilder
    {
        private readonly IRelationStore m_RelationStore;
        private readonly Type m_Type;

        internal RelationBuilder(IRelationStore relationStore, Type type)
        {
            m_RelationStore = relationStore;
            m_Type = type;
        }

        public IRelationStore CanAlter<T>(Expression<Func<T, object>> selector)
        {
            selector.Compile();

            var exp = (selector.Body as UnaryExpression);
            var t1 = exp.Operand as MemberExpression;
            var t2 = t1.Member as PropertyInfo;

            m_RelationStore.AddCausalRelation(new CausalRelation(RelationType.Alter, m_Type, t2));
            return m_RelationStore;
        }
    }

    public interface IRelationBuilder
    {
        IRelationStore CanAlter<T>(Expression<Func<T, object>> selector);
    }
}