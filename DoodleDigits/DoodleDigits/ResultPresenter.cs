using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DoodleDigits.Core;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits {

    public class ResultPresenter : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Dictionary<int, List<TempResult>> resultsPerLine = new();


        private record TempResult(Range Position, string Content, Result Result);

        public List<ResultViewModel> Results { get; set; } = new();

        public void ParseResults(TextMeasure measure, CalculationResult calculationResult) {
            resultsPerLine.Clear();
            foreach (Result result in calculationResult.Results) {
                string? text = GetResultString(result);
                if (text == null) {
                    continue;
                }

                int line = measure.GetLineForIndex(result.Position.End.Value);
                
                if (resultsPerLine.TryGetValue(line, out var list) == false) {
                    resultsPerLine[line] = list = new List<TempResult>();
                }

                list.Add(new TempResult(result.Position, text, result));
            }

            List<ResultViewModel> resultViewModels = new();

            measure.ApplyNewTextBoxDimensions();

            foreach (int line in resultsPerLine.Keys) {
                var results = resultsPerLine[line];
                results.Sort(CompareResults);
                string content = string.Join(", ", results.Select(x => x.Content));

                Point position = measure.GetFinalRectOfLine(line).BottomRight + new Vector(15, -23);
                resultViewModels.Add(new ResultViewModel(content, position));
            }

            Results = resultViewModels;
            OnPropertyChanged(nameof(Results));
        }

        private int CompareResults(TempResult resultA, TempResult resultB) {
            int GetValue(Result result) {
                int value = result.Position.End.Value;
                if (result is ResultValue) {
                    value -= 1000;
                }

                return value;
            }

            int a = GetValue(resultA.Result);
            int b = GetValue(resultB.Result);

            return a - b;
        }

        private string? GetResultString(Result result) {
            switch (result) {
                case ResultValue resultValue:
                    if (resultValue.Value is TooBigValue tooBig) {
                        return tooBig.ValueSign switch {
                            TooBigValue.Sign.Positive => " → Some huge number",
                            TooBigValue.Sign.PositiveInfinity => " = ∞",
                            TooBigValue.Sign.Negative => " → Some negative huge number",
                            TooBigValue.Sign.NegativeInfinity => " = -∞",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }

                    if (resultValue.Value.TriviallyAchieved) {
                        return null;
                    }

                    if (resultValue.Value is UndefinedValue) {
                        return null;
                    }

                    if (resultValue.Value is BooleanValue booleanValue) {
                        return " → " + booleanValue.ToString();
                    }

                    if (resultValue.Value is RealValue realValue) {
                        return " = " + realValue.ToString(75, 30, "ᴇ");
                    }

                    break;
                case ResultError resultError:
                    return resultError.Error;
                case ResultConversion resultConversion:
                    return $"converted {resultConversion.PreviousValue} to {resultConversion.NewValue}";
            }

            return null;
        }
    }
}
