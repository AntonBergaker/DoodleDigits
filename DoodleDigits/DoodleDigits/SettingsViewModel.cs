using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DoodleDigits {
    public class SettingsViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Settings settings;

        public SettingsViewModel(Settings settings) {
            this.settings = settings;
        }

        public bool UnsavedChanges => settings.UnsavedChanges;

        public bool DarkMode {
            get => settings.DarkMode;
            set {
                settings.DarkMode = value;
                OnPropertyChanged();
            }
        }

        public async Task Save() {
            await settings.Save();
        }

        public async Task<bool> Load() {
            return await settings.LoadOrPopulateDefaults();
        }
    }
}
