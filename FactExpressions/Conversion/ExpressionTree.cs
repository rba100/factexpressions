using System;
using System.Collections.Generic;
using FactExpressions.Language;

namespace FactExpressions.Conversion
{
    public class ExpressionTree
    {
        public IExpression Expression;
        public IEnumerable<ExpressionTree> Children;

        public ExpressionTree(IExpression expression, IEnumerable<ExpressionTree> children)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Children = children ?? throw new ArgumentNullException(nameof(children));
        }

        public void PrintToConsole()
        {
            int indentation = 0;
            void RecursivePrint(ExpressionTree tree)
            {
                Console.Write(new string(' ', indentation * 2));
                Console.WriteLine(tree.Expression);
                ++indentation;
                foreach (var child in tree.Children) RecursivePrint(child);
                --indentation;
            }

            RecursivePrint(this);
        }
    }
}