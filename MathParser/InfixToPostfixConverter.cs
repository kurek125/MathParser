using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using MathExpressions;

namespace MathExpressions
{
    /// <summary>
    /// InfixToPostfixConverter Infix Notation to Reverse Polish Notation (RPN)
    /// </summary>
    public class InfixToPostfixConverter
    {
        private string _expressionToParse;
        private List<Token> _translatedExpression;

        /// <summary>
        /// Initialize new instance of InfixToPostfixConverter
        /// </summary>
        /// <param name="expression">Math expression (infix/standard notation)</param>
        public InfixToPostfixConverter(string expression)
        {
            _expressionToParse = expression.Replace('.',',');
        }

        private string ReduceSigns(string expression)
        {
            string reducedExpression = expression;

            const string plusMinusPattern = @"(\+-)|(-\+)";
            const string minusPattern = @"(\-){2}"; // 2 '-';
            const string plusPattern = @"(\+){2,}"; // 2 or more '+';

            do
            {
                var reg = new Regex(plusMinusPattern);
                if (reg.Match(reducedExpression).Success)
                {
                    reducedExpression = reg.Replace(reducedExpression, "-");
                    continue;
                }

                reg = new Regex(minusPattern);
                if (reg.Match(reducedExpression).Success)
                {
                    reducedExpression = reg.Replace(reducedExpression, "+");
                    continue;
                }

                reg = new Regex(plusPattern);
                if (reg.Match(reducedExpression).Success)
                {
                    reducedExpression = reg.Replace(reducedExpression, "+");
                    continue;
                }

                break;

            } while (true);

            return reducedExpression;
        }

        private string AddMissingParentheses(string expression)
        {
            string pattern = @"([a-z]+|(\*|\/))(\+|-)([0-9]+(,[0-9]+)?(E(\+|-)?[0-9]+)?|\()";
                            // 4.CG - number or opening parenthesis
                            // 3.CG - sign - or +
                            // 1.CG - function before missing parenthesis

            Regex seeker = new Regex(pattern);
            Match match = seeker.Match(expression);

            while (match.Success)
            {
                if (match.Groups[4].Value != "(")
                {
                    //before the number
                    string newValue = $"{match.Groups[1].Value}({match.Groups[3].Value}{match.Groups[4].Value})";
                    expression = expression.Replace(match.Value, newValue);
                }
                else
                {
                    //before opening parenthesis
                    string expressionBeforeParentheses = expression.Substring(0, match.Index);

                    int openParenthesis = 0;

                    for (int i = match.Groups[4].Index; i < expression.Length; i++)
                    {
                        switch (expression[i])
                        {
                            case '(':
                                openParenthesis++;
                                break;
                            case ')':
                                openParenthesis--;
                                break;
                        }

                        if (openParenthesis == 0)
                        {
                            int afterPaenthesesIndex = i + 1; // 'i' parentheses index, needed is expression after parentheses
                            var expressionAfterParentheses = expression.Substring(afterPaenthesesIndex);

                            int inParenthesesIndex = match.Groups[3].Index; // 3.CG is sign 
                            int inParenthesesLenght = afterPaenthesesIndex - inParenthesesIndex;
                            var expressionInParentheses = expression.Substring(inParenthesesIndex, inParenthesesLenght);

                            expression =
                                $"{expressionBeforeParentheses}{match.Groups[1].Value}({expressionInParentheses}){expressionAfterParentheses}";
                        }
                    }
                }
                match = seeker.Match(expression);
            }

            return expression;
        }

        private string TranslateNegativeNumbers(string expression)
        {
            Regex seeker = new Regex(@"\(-");

            return seeker.Replace(expression, "(0-");
        }

        private List<string> Split(string expression)
        {
            expression = ReduceSigns(expression);
            expression = AddMissingParentheses(expression);
            expression = TranslateNegativeNumbers(expression);
            string splitPattern = @"(^(\+|-)[0-9]+(,[0-9]+)?(E(\+|-)?[0-9]+)?)|([a-zA-Z]+|\+|-|\*|\/|\(|\)|%|!|\^)|([0-9]+(,[0-9]+)?(E(\+|-)?[0-9]+)?)";
            Regex spliter = new Regex(splitPattern);

            return (from Match token in spliter.Matches(expression) select token.Value).ToList();
        }

        private List<Token> Parse(List<string> tokensNames)
        {
            Tokens tokens = new Tokens();
            Stack<Token> operands = new Stack<Token>();
            List<Token> rpnExpression = new List<Token>();

            foreach (var tokenName in tokensNames)
            {
                double number;
                bool isNumber = double.TryParse(tokenName, out number);
                var token = tokens.GetFunction(tokenName);

                if (isNumber)
                {
                    rpnExpression.Add(new Token() {Symbol = tokenName, Precedence = -1}); // important -1 mean Number );
                }
                else if (token != null && token.action!=null) // in functions action != null
                {
                    if (operands.Count == 0 || token.Precedence > operands.Peek().Precedence)
                    {
                        operands.Push(token);
                    }else
                    {
                        Token e = operands.Pop();
                        operands.Push(token);
                        rpnExpression.Add(e);
                    }
                }else if (token != null && token.action == null) // in parentheses action == null
                {
                    if (token.Symbol == "(")
                    {
                        operands.Push(token);
                    }
                    else
                    {
                        Token t = operands.Pop();
                        while (t.Symbol != "(")
                        {
                            rpnExpression.Add(t);
                            t = operands.Pop();
                        }
                    }
                }else
                {
                    throw new UnknowTokenException(tokenName);
                }
            }

            rpnExpression.AddRange(operands);

            return rpnExpression;
        } 

        /// <summary>
        /// Convert Infix/standard notation to Reverse Polish Notation(RPN)
        /// </summary>
        /// <returns>Divided expression in RPN</returns>
        public List<Token> Translate()
        {
            if(_translatedExpression!=null)
                return _translatedExpression;

            List<string> tokens = Split(_expressionToParse);
            _translatedExpression = Parse(tokens);

            return _translatedExpression;
        } 
    }
}
