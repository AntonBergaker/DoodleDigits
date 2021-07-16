using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits {
    public class ResultViewModel {
        public string Content { get; } = "";

        public Point Position { get; }

        public ResultViewModel(Result result, LineMeasure measure) {
            switch (result) {
                case ResultValue resultValue:
                    if (resultValue.Value is TooBigValue tooBig) {
                        Content = tooBig.ValueSign switch {
                            TooBigValue.Sign.Positive => "Some huge number",
                            TooBigValue.Sign.PositiveInfinity => "Infinity",
                            TooBigValue.Sign.Negative => "Some negative huge number",
                            TooBigValue.Sign.NegativeInfinity => "Negative infinity",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        break;
                    }

                    if (resultValue.Value is UndefinedValue) {
                        Content = "";
                        break;
                    }

                    Content = " = " + resultValue.Value.ToString();
                    break;
                case ResultError resultError:
                    Content = resultError.Error;
                    break;
                case ResultConversion resultConversion:
                    Content = $"Converted {resultConversion.PreviousValue} to {resultConversion.NewValue}";
                    break;
            }

            Position = measure.GetLineEndPosition(measure.GetLineIndex(result.Position.End.Value)) + new Vector(15, -11);
        }
    }
}
