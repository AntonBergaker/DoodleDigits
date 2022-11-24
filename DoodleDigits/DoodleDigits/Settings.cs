using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DoodleDigits {
    public class Settings {
        class SettingsData {
            [JsonPropertyName("dark_mode")]
            public bool DarkMode { get; set; }

            [JsonPropertyName("zoom")]
            public int ZoomTicks { get; set; }

            [JsonPropertyName("force_on_top")]
            public bool ForceOnTop { get; set; }

        }

        private SettingsData data;

        public bool UnsavedChanges { get; private set; }

        public bool DarkMode {
            get => data.DarkMode;
            set {
                if (data.DarkMode != value) {
                    UnsavedChanges = true;
                }
                data.DarkMode = value;
            }
        }

        public int ZoomTicks {
            get => data.ZoomTicks;
            set {
                if (value < -8) {
                    value = -8;
                }
                if (data.ZoomTicks != value) {
                    UnsavedChanges = true;
                }
                data.ZoomTicks = value;
            }
        }


        public bool ForceOnTop { 
            get => data.ForceOnTop;
            set {
                if (data.ForceOnTop != value) {
                    UnsavedChanges = true;
                }
                data.ForceOnTop = value;
            }
        }

        public Settings() {
            data = new SettingsData();
        }


        private static string DirectoryPath => Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Doodle Digits");
        public static string FilePath => Path.Join(DirectoryPath, "settings.json");

        public async Task Save(CancellationToken cancellationToken) {
            if (UnsavedChanges == false) {
                return;
            }

            if (!Directory.Exists(DirectoryPath)) {
                Directory.CreateDirectory(DirectoryPath);
            }

            string serializedData = JsonSerializer.Serialize(data);
            await File.WriteAllTextAsync(FilePath, serializedData, cancellationToken);
        }

        /// <summary>
        /// Returns true if loaded from file, false if populated from defaults
        /// </summary>
        /// <returns></returns>
        public bool LoadOrPopulateDefaults() {
            if (File.Exists(FilePath) == false) {
                object? lightValue = Registry.GetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", "AppsUseLightTheme", "unknown");
                if (lightValue is int @int) {
                    if (@int == 1) {
                        DarkMode = false;
                    }
                    else if (@int == 0) {
                        DarkMode = true;
                    }
                }
                return false;
            }

            string serializedData = File.ReadAllText(FilePath);
            data = JsonSerializer.Deserialize<SettingsData>(serializedData) ?? data;
            return true;
        }
    }
}