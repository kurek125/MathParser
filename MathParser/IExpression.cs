using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MathParser
{
    public static class Functions
    {
        private static List<IExpression> functions;
        private static List<IExpression> parentheses; 

        private static void Generate()
        {
            functions = new List<IExpression>();
            functions.Add(new Sin());
            functions.Add(new Cos());
            functions.Add(new Tan());
            functions.Add(new Cot());
            functions.Add(new Add());
            functions.Add(new Sub());
            functions.Add(new Mul());
            functions.Add(new Div());
            functions.Add(new Pow());
            functions.Add(new Factorial());
        }
        private static void GenerateParentheses()
        {
            parentheses = new List<IExpression>();
            parentheses.Add(new PHOpen());
            parentheses.Add(new PHClose());
        }

        public static IExpression GetFunction(string token)
        {
            if (functions == null)
            {
                Generate();
            }

            foreach (IExpression e in functions)
            {
                if (token == e.Symbol)
                {
                    return e.Create();
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
            if (parentheses == null)
            {
                GenerateParentheses();
            }
            foreach (IExpression p in parentheses)
            {
                if (p.Symbol == token)
                    return p.Create();
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
        IExpression Create();
    }

    // precedence: -1
    public class Number : IExpression
    {
        private double number;

        public string Symbol
        {
            get { return number.ToString(); }
        }

        public int Precedence
        {
            get { return -1; }
        }

        public Number(double n)
        {
            this.number = n;
        }

        public void Parse(Stack<double> s)
        {
            s.Push(number);
        }

        public IExpression Create()
        {
            return null;
        }
    }

    // precedence: 1
    public class Add : IExpression
    {
        public string Symbol
        {
            get { return "+"; }
        }

        public int Precedence
        {
            get { return 1; }
        }

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber + secondNumber);
        }

        public IExpression Create()
        {
            return new Add();
        }
    }

    public class Sub : IExpression
    {
        public string Symbol
        {
            get { return "-"; }
        }

        public int Precedence
        {
            get { return 1; }
        }

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber - secondNumber);
        }

        public IExpression Create()
        {
            return new Sub();
        }
    }

    // precedence: 2
    public class Mul : IExpression
    {
        public string Symbol
        {
            get { return "*"; }
        }

        public int Precedence
        {
            get { return 2; }
        }

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber*secondNumber);
        }

        public IExpression Create()
        {
            return new Mul();
        }
    }

    public class Div : IExpression
    {
        public string Symbol
        {
            get { return "/"; }
        }

        public int Precedence
        {
            get { return 2; }
        }

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber/secondNumber);
        }

        public IExpression Create()
        {
            return new Div();
        }
    }

    // precedence: 3
    public class Pow : IExpression
    {
        public string Symbol
        {
            get { return "^"; }
        }

        public int Precedence
        {
            get { return 3; }
        }

        public void Parse(Stack<double> s)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(Math.Pow(firstNumber, secondNumber));
        }

        public IExpression Create()
        {
            return new Pow();
        }
    }

    // precedence: int.MaxValue   -- all 1 arguments functions
    public class Sin : IExpression
    {
        public string Symbol
        {
            get { return "sin"; }
        }

        public int Precedence
        {
            get { return int.MaxValue; }
        }

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Sin(firstNumber));
        }

        public IExpression Create()
        {
            return new Sin();
        }
    }

    public class Cos : IExpression
    {
        public string Symbol
        {
            get { return "cos"; }
        }

        public int Precedence
        {
            get { return int.MaxValue; }
        }

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Cos(firstNumber));
        }

        public IExpression Create()
        {
            return new Cos();
        }
    }

    public class Tan : IExpression
    {
        public string Symbol
        {
            get { return "tan"; }
        }

        public int Precedence
        {
            get { return int.MaxValue; }
        }

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Tan(firstNumber));
        }

        public IExpression Create()
        {
            return new Tan();
        }
    }

    public class Cot : IExpression
    {
        public string Symbol
        {
            get { return "cot"; }
        }

        public int Precedence
        {
            get { return int.MaxValue; }
        }

        public void Parse(Stack<double> s)
        {
            double firstNumber = s.Pop();

            s.Push(1.0/Math.Tan(firstNumber));
        }

        public IExpression Create()
        {
            return new Cot();
        }
    }

    public class Factorial : IExpression
    {
        public string Symbol
        {
            get { return "!"; }
        }

        public int Precedence
        {
            get { return int.MaxValue; }
        }

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

        public IExpression Create()
        {
            return new Factorial();
        }
    }

    // parentheses
    public class PHOpen : IExpression
    {
        public string Symbol { get { return "("; } }
        public int Precedence { get { return 0; } }

        public void Parse(Stack<double> s)
        {
        }

        public IExpression Create()
        {
            return new PHOpen();
        }
    }
    public class PHClose : IExpression
    {
        public string Symbol { get { return ")"; } }
        public int Precedence { get { return 0; } }
        public void Parse(Stack<double> s)
        { }

        public IExpression Create()
        {
            return new PHClose();
        }
    }
}
