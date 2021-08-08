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

        public Thickness Margin => new Thickness(Position.X, Position.Y, 0, 0);
        

        public ResultViewModel(Result result, TextMeasure measure) {
            switch (result) {
                case ResultValue resultValue:
                    if (resultValue.Value is TooBigValue tooBig) {
                        Content = tooBig.ValueSign switch {
                            TooBigValue.Sign.Positive => " → Some huge number",
                            TooBigValue.Sign.PositiveInfinity => " = ∞",
                            TooBigValue.Sign.Negative => " → Some negative huge number",
                            TooBigValue.Sign.NegativeInfinity => " = -∞",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        break;
                    }

                    if (resultValue.Value.TriviallyAchieved) {
                        break;
                    }

                    if (resultValue.Value is UndefinedValue) {
                        break;
                    }

                    if (resultValue.Value is BooleanValue booleanValue) {
                        Content = " → " + booleanValue.ToString();
                        break;
                    }

                    if (resultValue.Value is RealValue realValue) {
                        Content = " = " + realValue.ToString(75, 30, "ᴇ");
                        break;
                    }

                    break;
                case ResultError resultError:
                    Content = resultError.Error;
                    break;
                case ResultConversion resultConversion:
                    Content = $"Converted {resultConversion.PreviousValue} to {resultConversion.NewValue}";
                    break;
            }

            Position = measure.GetRectForLine(measure.GetLineForIndex(result.Position.End.Value)).BottomRight + new Vector(15, -23);
        }
    }
}
