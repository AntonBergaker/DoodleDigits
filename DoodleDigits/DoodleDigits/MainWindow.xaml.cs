using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DoodleDigits.Core;
using MahApps.Metro.Controls;

namespace DoodleDigits {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow {

        public PresentationProperties PresentationProperties { get; }
        
        public ResultPresenter ResultPresenter { get; } = new();

        private bool initialized = false;
        // If over 0, will block saving
        private int blockSaving = 0;

        private int failedSaves = 0;

        private readonly SettingsViewModel settings;

        private void SetCaretIndex(int index) {
            this.InputTextBox.CaretIndex = index;
            this.InputTextBox.Select(index, 0);
            this.InputTextBox.Focus();
        }

        public MainWindow() {
            settings = new SettingsViewModel(new Settings());
            settings.Load();
            PresentationProperties = new(this, settings);
            InitializeComponent();

            try {
                var state = SerializedState.Load();
                if (state != null) {
                    blockSaving++;
                    this.Width = state.WindowDimensions.X;
                    this.Height = state.WindowDimensions.Y;
                    this.InputTextBox.Text = state.Content;
                    SetCaretIndex(state.CursorIndex);
                    blockSaving--;
                }
            }
            catch {
                // ignored
            }

            initialized = true;
        }

        private DateTime lastSaveTime = DateTime.Now;
        private volatile Task? currentSaveTask;
        private SerializedState? currentState;

        private void AutoSave() {
            Dispatcher.Invoke(() => {
                currentState = new(
                    this.InputTextBox.Text,
                    this.InputTextBox.CaretIndex,
                    new(this.Width, this.Height)
                );
            });

            if (currentSaveTask != null) {
                return;
            }


            async Task InvokeSave() {
                lastSaveTime = DateTime.Now;
                await SaveState();
                currentSaveTask = null;
            }

            TimeSpan diff = lastSaveTime + new TimeSpan(0, 0, 5) - DateTime.Now;

            if (diff < new TimeSpan(0)) {
                currentSaveTask = InvokeSave();
            }
            else {
                currentSaveTask = Task.Delay(diff).ContinueWith(async _ => await InvokeSave());
            }
        }

        
        private async Task SaveState() {
            if (initialized == false || blockSaving > 0) {
                return;
            }

            try {
                if (currentState == null) {
                    return;
                }
                CancellationTokenSource source = new();
                source.CancelAfter(5000);
                await currentState.Save(source.Token);
                failedSaves = 0;
            }
            catch (Exception ex) {
                failedSaves++;
                if (failedSaves >= 2) {
                    MessageBox.Show("Error saving the calculator state!\n" + ex.ToString());
                }
            }
            
        }
        

        private async void RichTextBoxTextChanged(object sender, TextChangedEventArgs e) {
            AutoSave();
            if (initialized == false) {
                // Delay for a little bit because of wpf wonkyness if we just started the app
                await Task.Delay(100);
            }

            string text = InputTextBox.Text;
            TextMeasure measure = new TextMeasure(text, InputTextBox);
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
            if (currentSaveTask != null) {
                currentSaveTask.Wait();
            }
        }


        void ClickClear(object sender, RoutedEventArgs args) { InputTextBox.Clear(); }

        private async void ToggleDarkMode(object sender, RoutedEventArgs e) {
            settings.DarkMode = !settings.DarkMode;
            if (settings.UnsavedChanges) {
                await settings.Save();
            }
        }

        private void TextBoxKeyDownEvent(object sender, KeyEventArgs e) {
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0) {
                if (e.Key is Key.OemPlus or Key.Add) {
                    ZoomIn(sender, e);
                }
                if (e.Key is Key.OemMinus or Key.Subtract) {
                    ZoomOut(sender, e);
                }
            }
        }
        private async void ZoomOut(object sender, RoutedEventArgs e) {
            settings.ZoomTicks --;
            if (settings.UnsavedChanges) {
                await settings.Save();
            }
        }

        private async void ZoomIn(object sender, RoutedEventArgs e) {
            settings.ZoomTicks++;
            if (settings.UnsavedChanges) {
                await settings.Save();
            }
        }
    }
}
