using System;
using System.Collections.Generic;
using System.Linq;

namespace FactExpressions.Relations
{
    public class RelationExperiment
    {
        public readonly IDictionary<object, IDictionary<object, double>> LongTerm 
            = new Dictionary<object, IDictionary<object, double>>();

        public readonly IDictionary<object, IDictionary<object, double>> ShortTerm
            = new Dictionary<object, IDictionary<object, double>>();


        public void Sleep()
        {
            Reinforce();
            DegradeAndPrune();
        }

        public void Observe(params object[] objects)
        {

        }

        private void Reinforce()
        {
            LongTerm.Clear();
            foreach (var kvp in ShortTerm)
            {
                LongTerm[kvp.Key] = new Dictionary<object, double>(kvp.Value);
            }
        }

        private void DegradeAndPrune()
        {
            foreach (var key in LongTerm.Keys)
            { 
                foreach(var subKey in LongTerm[key])
                LongTerm[key][subKey] = LongTerm[key][subKey] * c_FadeRate;

                var toRemove = LongTerm[key].Where(kvp => Math.Abs(kvp.Value) < c_ForgetThreshold);

                foreach (var entry in toRemove)
                {
                    LongTerm[key].Remove(entry);
                }
            }

            var deadRoots = LongTerm.Where(kvp => !kvp.Value.Keys.Any());

            foreach (var root in deadRoots) LongTerm.Remove(root);
        }

        private const double c_FadeRate = 0.99;
        private const double c_ForgetThreshold = 0.01;
    }
}