using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoodleDigits.Core;
using DoodleDigits.Core.Execution;
using DoodleDigits.Core.Execution.Results;
using DoodleDigits.Core.Execution.ValueTypes;

namespace DoodleDigits
{
    public partial class MainForm : Form {
        private readonly Executor executor;

        private readonly List<Label> labels;

        public MainForm() {
            this.labels = new List<Label>();
            executor = new Executor(FunctionLibrary.Functions, ConstantLibrary.Constants);
            InitializeComponent();
        }



        private void InputTextBox_TextChanged(object sender, EventArgs e) {
            var results = executor.Calculate(InputTextBox.Text);

            foreach (Label label in labels) {
                this.Controls.Remove(label);
            }

            foreach (Result result in results.Results) {
                MakeLabel(result);
            }

        }

        private void MakeLabel(Result result) {

            Label label = new Label();

            Point offset = new();

            switch (result) {
                case ResultValue resultValue:
                    offset.X += 100;

                    if (resultValue.Value is TooBigValue tooBig) {
                       label.Text = tooBig.ValueSign switch {
                            TooBigValue.Sign.Positive => "Some huge number",
                            TooBigValue.Sign.PositiveInfinity => "Infinity",
                            TooBigValue.Sign.Negative => "Some negative huge number",
                            TooBigValue.Sign.NegativeInfinity => "Negative infinity",
                            _ => throw new ArgumentOutOfRangeException()
                        };
                        break;
                    }

                    label.Text = resultValue.Value.ToString();
                    break;
                case ResultError resultError:
                    label.Text = resultError.Error;
                    break;
                case ResultConversion resultConversion:
                    label.Text = $"Converted {resultConversion.PreviousValue} to {resultConversion.NewValue}";
                    break;
            }

            Point issuePosition = InputTextBox.GetPositionFromCharIndex(result.Position.Start.Value);
            label.Left = issuePosition.X + offset.X;
            label.Top = issuePosition.Y;
            label.BackColor = Color.Transparent;

            this.Controls.Add(label);
            this.Controls.SetChildIndex(label, 0);
            this.labels.Add(label);
        }
    }
}
