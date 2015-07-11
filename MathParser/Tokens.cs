using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public class Token
    {
        public Action<Stack<double>> action;
        public int Precedence;
        public string Symbol;

        public void Parse(Stack<double> operands)
        {
            if (action != null)
            {
                action(operands);
            }
            else if (Precedence == -1)
            {
                operands.Push(double.Parse(Symbol));
            }
        }
    }
    
    public class Tokens
    {
        private List<Token> tokens;

        public Tokens()
        {
            tokens = new List<Token>()
            {
                //parentheses
                new Token() {action = null,Precedence = 0,Symbol = "("},
                new Token() {action = null,Precedence = 0,Symbol = ")"},

                //basic
                new Token() {action = Add,Precedence = 1, Symbol = "+"},
                new Token() {action = Sub,Precedence = 1, Symbol = "-"},
                new Token() {action = Mul,Precedence = 2, Symbol = "*"},
                new Token() {action = Div,Precedence = 2, Symbol = "/"},
                new Token() {action = Pow,Precedence = 3, Symbol = "^"},

                //trigonometric
                new Token() {action = Sin,Precedence = 9, Symbol = "sin"},
                new Token() {action = Cos,Precedence = 9, Symbol = "cos"},
                new Token() {action = Tan,Precedence = 9, Symbol = "tan"},
                new Token() {action = Cot,Precedence = 9, Symbol = "cot"},

                //const
                new Token() {action = ConstPi,Precedence = 99,Symbol = "PI"},
                new Token() {action = ConstE,Precedence = 99,Symbol = "e"}
            };
        }

        /// <summary>
        /// This method adding tokens from outside - for example other class
        /// </summary>
        /// <param name="func">Token to add</param>
        /// <returns>Success</returns>
        public bool AddFunction(Token func)
        {
            if (tokens.Any(n => n.Symbol == func.Symbol))
                return false;

            tokens.Add(func);

            return true;
        }

        public Token GetFunction(string symbol)
        {
            return tokens.Find(n=>n.Symbol==symbol);
        }

        private void ConstPi(Stack<double> operands)
        {
            operands.Push(Math.PI);
        }
        private void ConstE(Stack<double> operands)
        {
            operands.Push(Math.E);
        }
        private void Add(Stack<double> operands)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg + secondArg);
        }
        private void Sub(Stack<double> operands)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg - secondArg);
        }
        private void Mul(Stack<double> operands)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg * secondArg);
        }
        private void Div(Stack<double> operands)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg / secondArg);
        }
        private void Pow(Stack<double> operands)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(Math.Pow(firstArg, secondArg));
        }

        private void Sin(Stack<double> operands)
        {
            var arg = operands.Pop();
            operands.Push(Math.Sin(arg));
        }
        private void Cos(Stack<double> operands)
        {
            var arg = operands.Pop();
            operands.Push(Math.Cos(arg));
        }
        private void Tan(Stack<double> operands)
        {
            var arg = operands.Pop();
            operands.Push(Math.Tan(arg));
        }
        private void Cot(Stack<double> operands)
        {
            var arg = operands.Pop();
            operands.Push(1.0 / Math.Tan(arg));
        }
    }
}
