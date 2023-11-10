using NUnit.Framework;

namespace UnitTests.Parsing;
class ParenthesisTest {

    [Test]
    public void TestPosition() {

        Assert.AreEqual(0..5, ParsingTestUtils.ParseToAst("(1+1)").Position);

    }

    
}
