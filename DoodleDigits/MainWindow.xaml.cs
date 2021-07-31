using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using DoodleDigits.Core.Execution.Results;
using Path = System.IO.Path;

namespace DoodleDigits {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public ObservableCollection<ResultViewModel> Results { get; } = new();
        
        private readonly string saveDirectoryPath;

        private readonly string saveStatePath;

        private bool initialized = false;

        private async Task SetCaretIndexDelayed(int index, int milliseconds) {
            if (milliseconds > 0) {
                await Task.Delay(milliseconds);
            }

            this.RichTextBox.CaretIndex = index;
            this.RichTextBox.Select(index, 0);
            this.RichTextBox.Focus();
        }

        public MainWindow() {
            saveDirectoryPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Doodle Digits");
            saveStatePath = Path.Join(saveDirectoryPath, "state.json");

            InitializeComponent();

            try {
                if (File.Exists(saveStatePath)) {
                    string stateContent = File.ReadAllText(saveStatePath);
                    var state = JsonSerializer.Deserialize<SerializedState>(stateContent);

                    if (state != null) {
                        this.Width = state.WindowDimensions.X;
                        this.Height = state.WindowDimensions.Y;
                        this.RichTextBox.Text = state.Content;
                        _ = SetCaretIndexDelayed(state.CursorIndex, 0);
                        _ = SetCaretIndexDelayed(state.CursorIndex, 100);
                    }
                }
            }
            catch {
                // ignored
            }

            initialized = true;
        }

        private DateTime lastSaveTime = DateTime.Now;
        private bool startedSave = false;

        private void AutoSave() {
            if (startedSave) {
                return;
            }

            TimeSpan diff = lastSaveTime + new TimeSpan(0, 0, 5) - DateTime.Now;

            if (diff < new TimeSpan(0)) {
                lastSaveTime = DateTime.Now;
                _ = Save();
            }
            else {
                startedSave = true;
                Task.Delay(diff).ContinueWith(async o => {
                    lastSaveTime = DateTime.Now;
                    await Save();
                    startedSave = false;
                });
            }
        }

        private async Task Save() {
            if (initialized == false) {
                return;
            }

            try {
                await this.Dispatcher.Invoke(async () => {
                    string text = JsonSerializer.Serialize(new SerializedState(
                        this.RichTextBox.Text,
                        this.RichTextBox.CaretIndex,
                        new() {X = this.Width, Y = this.Height}));

                    if (!Directory.Exists(saveDirectoryPath)) {
                        Directory.CreateDirectory(saveDirectoryPath);
                    }

                    await File.WriteAllTextAsync(saveStatePath, text);
                });
            }
            catch (Exception ex) {
                MessageBox.Show("Error saving the calculator state!\n" + ex.ToString());
            }

            Debug.WriteLine("Saved");
        }

        private async void RichTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            AutoSave();

            string text = RichTextBox.Text;
            var calculationResult = await RunExecution(text);
            LineMeasure measure = new LineMeasure(text, RichTextBox);
            Results.Clear();
            foreach (Result result in calculationResult.Results) {
                Results.Add(new ResultViewModel(result, measure));
            }
        }

        private Task<CalculationResult> RunExecution(string input) {
            var task = new Task<CalculationResult>(() => {
                Calculator calculator = new Calculator(FunctionLibrary.Functions, ConstantLibrary.Constants);
                return calculator.Calculate(input);
            });
            task.Start();
            return task;
        }

        private async void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            await Save();
        }
    }
}
