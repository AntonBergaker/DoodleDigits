using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Rationals;

namespace UnitTests.Execution {
    class ParsingTest {

        [Test]
        public void TestNumberParse() {
            ExecutionUtils.AssertEqual((Rational)5.123, "5.123");
            ExecutionUtils.AssertEqual(1_000_000, "1_000_000");

            string bigNumber = "100000000000000000000000";
            ExecutionUtils.AssertEqual(new Rational(BigInteger.Parse(bigNumber)), bigNumber);
        }

    }
}
