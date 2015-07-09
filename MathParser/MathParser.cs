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

        private List<IExpression> Parse()
        {
            List<IExpression> tokenList = new List<IExpression>();

            Translator translator = new Translator(expression);
            var tokens = translator.Translate();

            foreach (string token in tokens.Where(token => !string.IsNullOrEmpty(token)))
            {
                try
                {
                    IExpression e = Functions.GetFunction(token);
                    if (e != null)
                    {
                        tokenList.Add(e);
                    }
                    else
                    {
                        throw new Exception($"token: \"{token}\" is unknown");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }

            return tokenList;
        }

        public double Calculate()
        {
            List<IExpression> tokens = Parse();
            Stack<double> operands = new Stack<double>();
            foreach (var op in tokens)
            {
                op.Parse(operands);
            }
            return operands.Pop();
        }
    }
}
