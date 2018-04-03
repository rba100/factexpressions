using System;
using System.Collections.Generic;

namespace FactExpressions.Relations
{
    public interface IRelationStore
    {
        void AddSimpleRelation(Type a, Type b);
        void AddCausalRelation(CausalRelation causalRelation);

        IRelationBuilder Declare(Type type);

        IReadOnlyCollection<Type> GetSimpleRelations(Type type);
        int TypeConnectivity(Type type);
    }
}