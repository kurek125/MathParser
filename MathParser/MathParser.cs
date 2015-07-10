using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public class MathParser
    {
        private string expression;

        public MathParser(string expression)
        {
            this.expression = expression;
        }

        private List<IToken> Parse()
        {
            List<IToken> tokenList = new List<IToken>();

            InfixToPostfixConverter infixToPostfixConverter = new InfixToPostfixConverter(expression);
            var tokens = infixToPostfixConverter.Translate();

            foreach (string token in tokens.Where(token => !string.IsNullOrEmpty(token)))
            {
                IToken e = Tokens.GetToken(token);
                if (e != null)
                {
                    tokenList.Add(e);
                }
            }

            return tokenList;
        }

        public double Calculate()
        {
            List<IToken> tokens = Parse();
            Stack<double> operands = new Stack<double>();
            foreach (var op in tokens)
            {
                op.Parse(operands);
            }
            return operands.Pop();
        }
    }
}
