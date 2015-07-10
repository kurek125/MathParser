using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public static class Functions
    {
        private static List<IExpression> _functions;
        private static List<IExpression> _parentheses;

        private static void Generate()
        {
            _functions = new List<IExpression>
            {
                new Sin(),
                new Cos(),
                new Tan(),
                new Cot(),
                new Add(),
                new Sub(),
                new Mul(),
                new Div(),
                new Pow(),
                new Factorial(),
                new ConstPI(),
                new ConstE()
            };

        }
        private static void GenerateParentheses()
        {
            _parentheses = new List<IExpression>
            {
                new PHOpen(),
                new PHClose()
            };
        }

        public static IExpression GetFunction(string token)
        {
            if (_functions == null)
            {
                Generate();
            }

            foreach (IExpression e in _functions)
            {
                if (token == e.Symbol)
                {
                    return e.New();
                }

                double number;
                if (double.TryParse(token, out number))
                {
                    return new Number(number);
                }
            }
            return null;
        }

        public static IExpression GetParentheses(string token)
        {
            if (_parentheses == null)
            {
                GenerateParentheses();
            }
            foreach (IExpression p in _parentheses)
            {
                if (p.Symbol == token)
                    return p.New();
            }
            return null;
        }
    }

    /// <summary>
    ///  precedence "-1" mean: no precedence like number or sin, cos...
    /// </summary>
    public interface IExpression
    {
        string Symbol { get; }
        int Precedence { get; }
        void Parse(Stack<double> s);
        IExpression New();
    }

    public class ConstPI : IExpression
    {
        public string Symbol => "PI";

        public int Precedence => -1;

        public void Parse(Stack<double> s)
        {
            s.Push(Math.PI);
        }

        public IExpression New()
        {
            return new Number(Math.PI);
        }
    }

    public class ConstE : IExpression
    {
        public string Symbol => "e";

        public int Precedence => -1;

        public void Parse(Stack<double> s)
        {
            s.Push(Math.E);
        }

        public IExpression New()
        {
            return new Number(Math.E);
        }
    }

    // precedence: -1
    public class Number : IExpression
    {
        private double number;

        public string Symbol => number.ToString();

        public int Precedence => -1;

        public Number(double n)
        {
            this.number = n;
        }

        public void Parse(Stack<double> s)
        {
            s.Push(number);
        }

        public IExpression New()
        {
            return null;
        }
    }

    // precedence: 1
    public class Add : IExpression
    {
        public string Symbol => "+";

        public int Precedence => 1;

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber + secondNumber);
        }

        public IExpression New()
        {
            return new Add();
        }
    }

    public class Sub : IExpression
    {
        public string Symbol => "-";

        public int Precedence => 1;

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber - secondNumber);
        }

        public IExpression New()
        {
            return new Sub();
        }
    }

    // precedence: 2
    public class Mul : IExpression
    {
        public string Symbol => "*";

        public int Precedence => 2;

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber*secondNumber);
        }

        public IExpression New()
        {
            return new Mul();
        }
    }

    public class Div : IExpression
    {
        public string Symbol => "/";

        public int Precedence => 2;

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber/secondNumber);
        }

        public IExpression New()
        {
            return new Div();
        }
    }

    // precedence: 3
    public class Pow : IExpression
    {
        public string Symbol => "^";

        public int Precedence => 3;

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(Math.Pow(firstNumber, secondNumber));
        }

        public IExpression New()
        {
            return new Pow();
        }
    }

    // precedence: int.MaxValue   -- all 1 arguments functions
    public class Sin : IExpression
    {
        public string Symbol => "sin";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Sin(firstNumber));
        }

        public IExpression New()
        {
            return new Sin();
        }
    }

    public class Cos : IExpression
    {
        public string Symbol => "cos";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Cos(firstNumber));
        }

        public IExpression New()
        {
            return new Cos();
        }
    }

    public class Tan : IExpression
    {
        public string Symbol => "tan";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Tan(firstNumber));
        }

        public IExpression New()
        {
            return new Tan();
        }
    }

    public class Cot : IExpression
    {
        public string Symbol => "cot";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(1.0/Math.Tan(firstNumber));
        }

        public IExpression New()
        {
            return new Cot();
        }
    }

    public class Factorial : IExpression
    {
        public string Symbol => "!";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s)
        {
            double firstnumber = s.Pop();
            double returnNumber = 1;
            for (int i = 1; i <= firstnumber; i++)
            {
                returnNumber *= i;
            }
            s.Push(returnNumber);
        }

        public IExpression New()
        {
            return new Factorial();
        }
    }

    // parentheses
    public class PHOpen : IExpression
    {
        public string Symbol => "(";
        public int Precedence => 0;

        public void Parse(Stack<double> s)
        {
        }

        public IExpression New()
        {
            return new PHOpen();
        }
    }
    public class PHClose : IExpression
    {
        public string Symbol => ")";
        public int Precedence => 0;

        public void Parse(Stack<double> s)
        { }

        public IExpression New()
        {
            return new PHClose();
        }
    }
}
