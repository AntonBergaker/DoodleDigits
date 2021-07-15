using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<ResultViewModel> Results { get; } = new();
        

        public MainWindow() {
            InitializeComponent();
        }

        private async void RichTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            string text = RichTextBox.Text;
            var executionResult = await RunExecution(text);
            LineMeasure measure = new LineMeasure(text, RichTextBox);
            Results.Clear();
            foreach (Result result in executionResult.Results) {
                Results.Add(new ResultViewModel(result, measure));
            }
            
        }

        private Task<ExecutionResult> RunExecution(string input) {
            var task = new Task<ExecutionResult>(() => {
                Executor executor = new Executor(FunctionLibrary.Functions, ConstantLibrary.Constants);
                return executor.Calculate(input);
            });
            task.Start();
            return task;
        }

    }
}
