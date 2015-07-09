using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MathParser;

namespace MathParserTests
{
    [TestClass]
    public class PostfixParserTests
    {
        [TestMethod]
        public void ParseTest()
        {
            string input = "1,23E-4*5+sin(3/(2-1))*-1";

            PrivateObject parser = new PrivateObject(new PostfixParser(input));

            List<IExpression> expected = (new IExpression[]
            {
                new Number(Double.Parse("1,23E-4")), new Number(5), new Mul(), new Number(3), new Number(2),
                new Number(1), new Sub(), new Div(), new Sin(), new Number(0),new Number(1), new Sub(), new Mul(), new Add()
            }).ToList();

            var actual = (List<IExpression>)parser.Invoke("Parse");

            Assert.IsFalse(expected.Count != actual.Count);
            Assert.IsFalse(expected.Where((t, i) => t.Symbol != actual[i].Symbol).Any());
            


            input = "2+throwexception(123)";
            parser = new PrivateObject(new PostfixParser(input));
            try
            {
                parser.Invoke("Parser"); // exception - "token "throwexception" is unknow"
                Assert.Fail();
            }
            catch (Exception e)
            {

            }
        }

        [TestMethod]
        public void CalculateTest()
        {
            string input = "1,23E+2*5+(3/(4-1))*-1";
            double expected = 614.0;

            var parser = new PostfixParser(input);
            double actual = parser.Calculate();

            Assert.AreEqual(expected,actual);
        }
    }
}
