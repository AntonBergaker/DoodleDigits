using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DoodleDigits.Core;

namespace DoodleDigits {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public ResultPresenter ResultPresenter { get; } = new();


        private readonly string saveDirectoryPath;

        private readonly string saveStatePath;

        private bool initialized = false;
        // If over 0, will block saving
        private int blockSaving = 0;

        private int failedSaves = 0;

        private void SetCaretIndex(int index) {
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
                        blockSaving++;
                        this.Width = state.WindowDimensions.X;
                        this.Height = state.WindowDimensions.Y;
                        this.RichTextBox.Text = state.Content;
                        SetCaretIndex(state.CursorIndex);
                        blockSaving--;
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

            Task InvokeSave() {
                return Dispatcher.Invoke(async () => {
                    startedSave = true;
                    lastSaveTime = DateTime.Now;
                    await Save();
                    startedSave = false;
                });
            }

            TimeSpan diff = lastSaveTime + new TimeSpan(0, 0, 5) - DateTime.Now;

            if (diff < new TimeSpan(0)) {
                _ = InvokeSave();
            }
            else {
                startedSave = true;
                Task.Delay(diff).ContinueWith(o => InvokeSave());
            }
        }

        private async Task Save() {
            if (initialized == false || blockSaving > 0) {
                return;
            }

            try {
                string text = JsonSerializer.Serialize(new SerializedState(
                    this.RichTextBox.Text,
                    this.RichTextBox.CaretIndex,
                    new() {X = this.Width, Y = this.Height}));

                if (!Directory.Exists(saveDirectoryPath)) {
                    Directory.CreateDirectory(saveDirectoryPath);
                }

                await File.WriteAllTextAsync(saveStatePath, text);
                failedSaves = 0;
            }
            catch (Exception ex) {
                failedSaves++;
                if (failedSaves >= 2) {
                    MessageBox.Show("Error saving the calculator state!\n" + ex.ToString());
                }
            }

            Debug.WriteLine("Saved");
        }
        

        private async void RichTextBoxTextChanged(object sender, TextChangedEventArgs e) {
            AutoSave();
            if (initialized == false) {
                // Delay for a little bit because of wpf wonkyness
                await Task.Delay(100);
            }

            string text = RichTextBox.Text;
            TextMeasure measure = new TextMeasure(text, RichTextBox);
            var calculationResult = await RunExecution(text);

            ResultPresenter.ParseResults(measure, calculationResult);
        }

        private Task<CalculationResult> RunExecution(string input) {
            var task = Task.Run(() => {
                Calculator calculator = new Calculator(FunctionLibrary.Functions, ConstantLibrary.Constants);
                return calculator.Calculate(input);
            });
            return task;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) {
            AutoSave();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (startedSave) {
                Save().Wait();
            }
        }


        void ClickClear(object sender, RoutedEventArgs args) { RichTextBox.Clear(); }

    }
}
