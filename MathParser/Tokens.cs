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

                //
                new Token() {action = Percentage,Precedence = 9,Symbol = "%"},
                new Token() {action = Factorial,Precedence = 9,Symbol = "!"},
                new Token() {action = Exponential,Precedence = 9,Symbol = "exp"},
                new Token() {action = Round,Precedence = 9,Symbol = "round"},
                new Token() {action = Abs,Precedence = 9,Symbol = "abs"},
                new Token() {action = Sign,Precedence = 9,Symbol = "sign"},

                //logarithm
                new Token() {action = Logn,Precedence = 9,Symbol = "ln"},
                new Token() {action = Log10,Precedence = 9,Symbol = "log10"},

                //trigonometric
                new Token() {action = Sin,Precedence = 9, Symbol = "sin"},
                new Token() {action = Cos,Precedence = 9, Symbol = "cos"},
                new Token() {action = Tan,Precedence = 9, Symbol = "tan"},
                new Token() {action = Cot,Precedence = 9, Symbol = "cot"},
                new Token() {action = Sec,Precedence = 9,Symbol = "sec"},
                new Token() {action = Cosec,Precedence = 9,Symbol = "cosec"},

                //cyclometric
                new Token() {action = Arcsin,Precedence = 9,Symbol = "arcsin"},
                new Token() {action = Arccos,Precedence = 9,Symbol = "arccos"},
                new Token() {action = Arctan,Precedence = 9,Symbol = "arctan"},
                new Token() {action = Arccot,Precedence = 9,Symbol = "arccot"},
                new Token() {action = Arcsec,Precedence = 0,Symbol = "arcsec"},
                new Token() {action = Arccosec,Precedence = 9,Symbol = "arccosec"},
                //cyclometric alias
                new Token() {action = Arcsin,Precedence = 9,Symbol = "asin"},
                new Token() {action = Arccos,Precedence = 9,Symbol = "acos"},
                new Token() {action = Arctan,Precedence = 9,Symbol = "atan"},
                new Token() {action = Arccot,Precedence = 9,Symbol = "acot"},
                new Token() {action = Arcsec,Precedence = 0,Symbol = "asec"},
                new Token() {action = Arccosec,Precedence = 9,Symbol = "acosec"},

                //hyperbolic
                new Token() {action = Sinh,Precedence = 9,Symbol = "sinh"},
                new Token() {action = Cosh,Precedence = 9,Symbol = "cosh"},
                new Token() {action = Tanh,Precedence = 9,Symbol = "tanh"},
                new Token() {action = Coth,Precedence = 9,Symbol = "coth"},
                new Token() {action = Sech,Precedence = 0,Symbol = "sech"},
                new Token() {action = Cosech,Precedence = 9,Symbol = "cosech"},

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

        //
        private void Percentage(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(arg / 100.0);
        }
        private void Factorial(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            double result = 1.0;

            if (arg <= 1)
            {
                operands.Push(result);
            }
            else
            {
                for (int i = 2; i <= arg; i++)
                {
                    result *= i;
                }

                operands.Push(result);
            }
        }
        private void Exponential(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(Math.Exp(arg));
        }
        private void Round(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(Math.Round(arg));
        }
        private void Abs(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(Math.Abs(arg));
        }
        private void Sign(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(Math.Sign(arg));
        }
        //todo: Truncate
        //todo: Floor
        //todo: Ceiling

        //logarithm
        private void Logn(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(Math.Log(arg));
        }
        private void Log10(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(Math.Log10(arg));
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
        private void Sec(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(1.0 / Math.Cos(arg));
        }
        private void Cosec(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(1.0/Math.Sin(arg));
        }
        

        //cyclometric
        private void Arcsin(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(
                useRadians ? Math.Asin(arg) : Math.Asin(arg) *180.0 /Math.PI
                );
        }
        private void Arccos(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(
                useRadians ? Math.Acos(arg) : Math.Acos(arg) * 180.0 / Math.PI
                );
        }
        private void Arctan(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(
                useRadians ? Math.Atan(arg) : Math.Atan(arg) * 180.0 / Math.PI
                );
        }
        private void Arccot(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(
                useRadians ? Math.Atan(arg) : Math.Atan(arg) * 180.0 / Math.PI
                );
        }
        private void Arcsec(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(
                useRadians ? Math.Acos(1.0/arg) : Math.Acos(1.0/arg) * 180.0 / Math.PI
                );
        }
        private void Arccosec(Stack<double> operands, bool useRadians)
        {
            var arg = operands.Pop();
            operands.Push(
                useRadians ? Math.Asin(1.0/arg) : Math.Asin(1.0/arg) * 180.0 / Math.PI
                );
        }

        //hyperbolic
        private void Sinh(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(Math.Sinh(arg));
        }
        private void Cosh(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(Math.Cosh(arg));
        }
        private void Tanh(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(Math.Tanh(arg));
        }
        private void Coth(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(1.0/Math.Tanh(arg));
        }
        private void Sech(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(1.0/Math.Cosh(arg));
        }
        private void Cosech(Stack<double> operands, bool useRadians)
        {
            var arg = useRadians ? operands.Pop() : (operands.Pop() * Math.PI / 180.0);
            operands.Push(1.0 / Math.Sinh(arg));
        }

        //todo: Gudermannian function
        //todo: Error function

    }
}
