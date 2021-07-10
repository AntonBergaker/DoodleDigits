using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DoodleDigits.Core;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            Executor executor = new Executor(FunctionLibrary.Functions, ConstantLibrary.Constants);
            var executionResult = executor.Calculate(new TextRange(RichTextBox.Document.ContentStart, RichTextBox.Document.ContentEnd).Text);

            foreach (Result result in executionResult.Results) {
                MakeLabel(result);
            }
        }

        private void MakeLabel(Result result) {
            if (ResultLabel == null) return; // todo remove 
            Label label = ResultLabel;

            switch (result) {
                case ResultValue resultValue:
                    if (resultValue.Value is TooBigValue tooBig) {
                        label.Content = tooBig.ValueSign switch {
                            TooBigValue.Sign.Positive => "Some huge number",
                            TooBigValue.Sign.PositiveInfinity => "Infinity",
                            TooBigValue.Sign.Negative => "Some negative huge number",
                            TooBigValue.Sign.NegativeInfinity => "Negative infinity",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        break;
                    }

                    label.Content = resultValue.Value.ToString();
                    break;
                case ResultError resultError:
                    label.Content = resultError.Error;
                    break;
                case ResultConversion resultConversion:
                    label.Content = $"Converted {resultConversion.PreviousValue} to {resultConversion.NewValue}";
                    break;
            }
        }
    }
}
