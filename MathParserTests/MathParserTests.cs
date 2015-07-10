using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MathExpressions;

namespace MathParserTests
{
    [TestClass]
    public class MathParserTests
    {
        [TestMethod]
        public void ParseTest()
        {
            string input = "1,23E-4*5+sin(3/(2-1))*-1";

            PrivateObject parser = new PrivateObject(new MathParser(input));

            List<IToken> expected = (new IToken[]
            {
                new Number(double.Parse("1,23E-4")), new Number(5), new Mul(), new Number(3), new Number(2),
                new Number(1), new Sub(), new Div(), new Sin(), new Number(0),new Number(1), new Sub(), new Mul(), new Add()
            }).ToList();

            var actual = (List<IToken>)parser.Invoke("Parse");

            Assert.IsFalse(expected.Count != actual.Count);
            Assert.IsFalse(expected.Where((t, i) => t.Symbol != actual[i].Symbol).Any());
        }

        [TestMethod]
        [ExpectedException(typeof (UnknowTokenException))]
        public void ParseExceptionTest()
        {
            string input = "2+throwexception(123)";
            PrivateObject parser = new PrivateObject(new MathParser(input));
            parser.Invoke("Parse"); // exception - "token "throwexception" is unknow"
        }

        [TestMethod]
        public void CalculateTest()
        {
            string input = "1,23E+2*5+(3/(4-1))*-1";
            double expected = 614.0;

            var parser = new MathParser(input);
            double actual = parser.Calculate();

            Assert.AreEqual(expected,actual,1e-6);
        }

        [TestMethod]
        public void CalculateConstantsTest()
        {
            string input = "sin(90*PI/180)*e";
            double expected = Math.Sin(90*Math.PI/180.0)*Math.E;

            var parser = new MathParser(input);
            double actual = parser.Calculate();

            Assert.AreEqual(expected, actual, 1e-6);
        }

        [TestMethod]
        public void RadiansTest()
        {
            MathParser parser = new MathParser("sin90");
            double actual = parser.Calculate(useRadians: false);
            double expected = 1.0;
            Assert.AreEqual(expected,actual,1e-6);

            actual = parser.Calculate();
            expected = Math.Sin(90);
            Assert.AreEqual(expected,actual,1e-6);
        }
    }
}
