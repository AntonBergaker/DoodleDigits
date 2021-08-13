using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using ControlzEx.Theming;

namespace DoodleDigits {
    public class PresentationProperties : INotifyPropertyChanged {
        private readonly MainWindow window;
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Uri imageSourceDark = new Uri("/Resources/grid_dark.png", UriKind.Relative);
        private readonly Brush inputTextColorDark = new SolidColorBrush(Color.FromRgb(0xEE, 0xEE, 0xEE));
        private readonly Brush labelTextColorDark = new SolidColorBrush(Color.FromRgb(0x2E, 0xA0, 0xFF));

        private readonly Uri imageSourceLight = new Uri("/Resources/grid.png", UriKind.Relative);
        private readonly Brush inputTextColorLight = new SolidColorBrush(Color.FromRgb(0x11, 0x11, 0x11));
        private readonly Brush labelTextColorLight = new SolidColorBrush(Color.FromRgb(0x1E, 0x90, 0xFF));


        public PresentationProperties(MainWindow window, SettingsViewModel settings) {
            this.window = window;
            imageSourceField = imageSourceLight;
            inputTextColorField = inputTextColorLight;
            labelTextColorField = labelTextColorLight;

            DarkMode = settings.DarkMode;
            settings.PropertyChanged += (s, e) => {
                switch (e.PropertyName) {
                    case nameof(settings.DarkMode):
                        DarkMode = settings.DarkMode;
                        break;
                }
            };
        }

        private Uri imageSourceField;
        public Uri ImageSource {
            get => imageSourceField;
            private set {
                imageSourceField = value;
                OnPropertyChanged();
            }
        }

        private bool darkModeField;
        public bool DarkMode {
            get => darkModeField;
            set {
                darkModeField = value;
                string theme;
                if (darkModeField) {
                    ImageSource = imageSourceDark;
                    InputTextColor = inputTextColorDark;
                    LabelTextColor = labelTextColorDark;
                    theme = "Dark.Blue";
                }
                else {
                    ImageSource = imageSourceLight;
                    InputTextColor = inputTextColorLight;
                    LabelTextColor = labelTextColorLight;
                    theme = "Light.Blue";
                }
                ThemeManager.Current.ChangeTheme(window, theme);
                OnPropertyChanged();
            }
        }

        private Brush inputTextColorField;
        public Brush InputTextColor {
            get => inputTextColorField;
            set {
                inputTextColorField = value;
                OnPropertyChanged();
            }
        }

        private Brush labelTextColorField;
        public Brush LabelTextColor {
            get => labelTextColorField;
            set {
                labelTextColorField = value;
                OnPropertyChanged();
            }
        }
    }
}
