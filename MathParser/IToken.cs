using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MathExpressions
{
    public static class Tokens
    {
        private static List<IToken> _tokensList;
        private static List<IToken> _parentheses;

        private static void Generate()
        {
            _tokensList = new List<IToken>
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
            _parentheses = new List<IToken>
            {
                new PHOpen(),
                new PHClose()
            };
        }

        public static IToken GetToken(string tokenSymbol)
        {
            if (_tokensList == null)
            {
                Generate();
            }

            foreach (IToken token in _tokensList)
            {
                if (tokenSymbol == token.Symbol)
                {
                    return token.New();
                }

                double number;
                if (double.TryParse(tokenSymbol, out number))
                {
                    return new Number(number);
                }
            }
            return null;
        }

        public static IToken GetParenthesis(string parenthesisSymbol)
        {
            if (_parentheses == null)
            {
                GenerateParentheses();
            }
            foreach (IToken p in _parentheses)
            {
                if (p.Symbol == parenthesisSymbol)
                    return p.New();
            }
            return null;
        }
    }

    // parentheses
    public class PHOpen : IToken
    {
        public string Symbol => "(";
        public int Precedence => 0;

        public void Parse(Stack<double> s, bool useRadians)
        { }

        public IToken New()
        {
            return new PHOpen();
        }
    }
    public class PHClose : IToken
    {
        public string Symbol => ")";
        public int Precedence => 0;

        public void Parse(Stack<double> s, bool useRadians)
        { }

        public IToken New()
        {
            return new PHClose();
        }
    }

    /// <summary>
    ///  precedence "-1" mean: no precedence like number or constants
    /// </summary>
    public interface IToken
    {
        string Symbol { get; }
        int Precedence { get; }
        void Parse(Stack<double> s, bool useRadians);

        IToken New();
    }

    public class ConstPI : IToken
    {
        public string Symbol => "PI";

        public int Precedence => -1;

        public void Parse(Stack<double> s, bool useRadians)
        { }

        public IToken New()
        {
            return new Number(Math.PI);
        }
    }

    public class ConstE : IToken
    {
        public string Symbol => "e";

        public int Precedence => -1;

        public void Parse(Stack<double> s, bool useRadians)
        { }

        public IToken New()
        {
            return new Number(Math.E);
        }
    }

    // precedence: -1
    public class Number : IToken
    {
        private double _number;

        public string Symbol => _number.ToString();

        public int Precedence => -1;

        public Number(double number)
        {
            _number = number;
        }

        public void Parse(Stack<double> s, bool useRadians)
        {
            s.Push(_number);
        }

        public IToken New()
        {
            return null;
        }
    }

    // precedence: 1
    public class Add : IToken
    {
        public string Symbol => "+";

        public int Precedence => 1;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber + secondNumber);
        }

        public IToken New()
        {
            return new Add();
        }
    }

    public class Sub : IToken
    {
        public string Symbol => "-";

        public int Precedence => 1;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber - secondNumber);
        }

        public IToken New()
        {
            return new Sub();
        }
    }

    // precedence: 2
    public class Mul : IToken
    {
        public string Symbol => "*";

        public int Precedence => 2;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber*secondNumber);
        }

        public IToken New()
        {
            return new Mul();
        }
    }

    public class Div : IToken
    {
        public string Symbol => "/";

        public int Precedence => 2;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(firstNumber/secondNumber);
        }

        public IToken New()
        {
            return new Div();
        }
    }

    // precedence: 3
    public class Pow : IToken
    {
        public string Symbol => "^";

        public int Precedence => 3;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double secondNumber = s.Pop();
            double firstNumber = s.Pop();

            s.Push(Math.Pow(firstNumber, secondNumber));
        }

        public IToken New()
        {
            return new Pow();
        }
    }

    // precedence: int.MaxValue   -- all 1 arguments functions
    public class Sin : IToken
    {
        public string Symbol => "sin";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Sin(
                useRadians ? firstNumber : firstNumber*Math.PI/180.0
                ));
        }

        public IToken New()
        {
            return new Sin();
        }
    }

    public class Cos : IToken
    {
        public string Symbol => "cos";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Cos(
                useRadians ? firstNumber : firstNumber * Math.PI / 180.0
                ));
        }

        public IToken New()
        {
            return new Cos();
        }
    }

    public class Tan : IToken
    {
        public string Symbol => "tan";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double firstNumber = s.Pop();

            s.Push(Math.Tan(
                useRadians ? firstNumber : firstNumber * Math.PI / 180.0
                ));
        }

        public IToken New()
        {
            return new Tan();
        }
    }

    public class Cot : IToken
    {
        public string Symbol => "cot";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double firstNumber = s.Pop();

            s.Push(1.0/Math.Tan(
                useRadians ? firstNumber : firstNumber * Math.PI / 180.0
                ));
        }

        public IToken New()
        {
            return new Cot();
        }
    }

    public class Factorial : IToken
    {
        public string Symbol => "!";

        public int Precedence => int.MaxValue;

        public void Parse(Stack<double> s, bool useRadians)
        {
            double firstnumber = s.Pop();
            double returnNumber = 1;
            for (int i = 1; i <= firstnumber; i++)
            {
                returnNumber *= i;
            }
            s.Push(returnNumber);
        }

        public IToken New()
        {
            return new Factorial();
        }
    }
}
