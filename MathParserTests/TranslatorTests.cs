using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MathParser;

namespace MathParserTests
{
    [TestClass]
    public class TranslatorTests
    {
        [TestMethod]
        public void ReduceSignsTest()
        {
            string[] inputArr = new[] {"1--2+---+--3++(--4)","1---------+--2"};
            string[] expctedArr = new[] {"1+2-3+(+4)","1-2"};

            var actualArr = ExecuteMethod("ReduceSigns", inputArr);

            CollectionAssert.AreEqual(expctedArr, actualArr);
        }

        [TestMethod]
        public void AddMissingPatenthesesTest()
        {
            string[] inputArr = new[] { "1*-2-sin-4+cos-(-4*(6-7))" };
            string[] expctedArr = new[] { "1*(-2)-sin(-4)+cos(-(-4*(6-7)))" };

            var actualArr = ExecuteMethod("AddMissingParentheses",inputArr);

            CollectionAssert.AreEqual(expctedArr,actualArr);
        }

        [TestMethod]
        public void TranslateNegativeNumbersTest()
        {
            string[] inputArr = new[] {"2*(-3)"};
            string[] expectedArr = new[] {"2*(0-3)"};

            var actualArr = ExecuteMethod("TranslateNegativeNumbers", inputArr);

            CollectionAssert.AreEqual(expectedArr,actualArr);
        }

        [TestMethod]
        public void SplitTest()
        {
            string input = "-1,2E-3*4+sin(5)";
            List<string> expected = (new string[] {"-1,2E-3","*","4","+","sin","(","5",")"}).ToList();

            List<string> actual = (List<string>)ExecuteMethod("Split",input);

            CollectionAssert.AreEqual(expected,actual);
        }

        private string[] ExecuteMethod(string method, string[] expressions)
        {
            PrivateObject translation = new PrivateObject(new Translator(""));
            string[] actual = new string[expressions.Length];
            for(int i=0;i<expressions.Length;i++)
            {
                actual[i] = (string)translation.Invoke(method, expressions[i]);
            }
            return actual;
        }

        private object ExecuteMethod(string method, string expression)
        {
            PrivateObject translation = new PrivateObject(new Translator(""));
            return translation.Invoke(method, expression);
        }

        [TestMethod]
        public void ParseTest()
        {
            string input = "-2+-4*-5-sin-6";
            List<string> expected = (new string[] {"-2","4","0", "5", "-", "*", "0", "6", "-", "sin", "-", "-"}).ToList();

            Translator translator = new Translator(input);

            List<string> actual = translator.Translate();
            CollectionAssert.AreEqual(expected,actual);

            actual = translator.Translate();
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
