using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public class Token
    {
        public Action<Stack<double>,bool> action;
        public int Precedence;
        public string Symbol;

        public void Parse(Stack<double> operands, bool useRadians=true)
        {
            if (action != null)
            {
                action(operands,useRadians);
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

        //constants
        private void ConstPi(Stack<double> operands, bool useRadians)
        {
            operands.Push(Math.PI);
        }
        private void ConstE(Stack<double> operands, bool useRadians)
        {
            operands.Push(Math.E);
        }

        //basics
        private void Add(Stack<double> operands, bool useRadians)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg + secondArg);
        }
        private void Sub(Stack<double> operands, bool useRadians)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg - secondArg);
        }
        private void Mul(Stack<double> operands, bool useRadians)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg * secondArg);
        }
        private void Div(Stack<double> operands, bool useRadians)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(firstArg / secondArg);
        }
        private void Pow(Stack<double> operands, bool useRadians)
        {
            var secondArg = operands.Pop();
            var firstArg = operands.Pop();
            operands.Push(Math.Pow(firstArg, secondArg));
        }

        //trigonometric
        private void Sin(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop()*Math.PI/180.0);
            operands.Push(Math.Sin(arg));
        }
        private void Cos(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(Math.Cos(arg));
        }
        private void Tan(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(Math.Tan(arg));
        }
        private void Cot(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(1.0 / Math.Tan(arg));
        }
    }
}
