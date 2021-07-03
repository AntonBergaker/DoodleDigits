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

namespace DoodleDigits
{
    public partial class MainForm : Form {
        private readonly Executor executor;

        public MainForm() {
            executor = new Executor(FunctionLibrary.Functions);
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            var result = executor.Calculate(textBox1.Text);
            label1.Text = result.Results.Length > 0 ? result.Results[0] : "";
        }
    }
}
