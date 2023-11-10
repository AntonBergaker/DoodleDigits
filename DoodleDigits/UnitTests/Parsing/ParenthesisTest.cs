using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoodleDigits.Core;
using DoodleDigits.Core.Parsing;
using DoodleDigits.Core.Parsing.Ast;
using NUnit.Framework;

namespace UnitTests.Parsing; 
class ParenthesisTest {

    [Test]
    public void TestPosition() {

        Assert.AreEqual(0..5, ParsingTestUtils.ParseToAst("(1+1)").Position);

    }

    
}
