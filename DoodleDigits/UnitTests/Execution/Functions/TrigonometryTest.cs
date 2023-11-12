using DoodleDigits.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Execution.Functions;
public class TrigonometryTest {
    [Test]
    public void TestDegrees() {
        var settings = new CalculatorSettings() {
            AngleUnit = AngleUnits.Degrees
        };

        ExecutionTestUtils.AssertEqual(0, "sin(180)", settings);
        ExecutionTestUtils.AssertEqual(0, "sin(0)", settings);
        ExecutionTestUtils.AssertEqual(-1, "sin(270)", settings);
        ExecutionTestUtils.AssertEqual(1, "sin(90)", settings);

        ExecutionTestUtils.AssertEqual(45, "acot(1)", settings);
    }
}
