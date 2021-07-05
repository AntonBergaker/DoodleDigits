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

namespace DoodleDigits
{
    public partial class MainForm : Form {
        private readonly Executor executor;

        public MainForm() {
            executor = new Executor(FunctionLibrary.Functions, ConstantLibrary.Constants);
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            var results = executor.Calculate(textBox1.Text);
            foreach (var result in results.Results) {
                if (result is ResultValue value) {
                    label1.Text = value.Value.ToString();
                }
            }
        }
    }
}
