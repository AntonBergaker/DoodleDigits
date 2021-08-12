using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DoodleDigits {
    class Settings {
        class SettingsData {
            [JsonPropertyName("dark_mode")]
            public bool DarkMode { get; set; }

        }

        private readonly SettingsData data;

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

        public Settings() {
            data = new SettingsData();
        }

        public async Task Save() {


        }
    }
}

class Foo {
    protected Foo() {
        Baz();
    }

    protected virtual void Baz() { }
}

class Bar : Foo {
    protected override void Baz() {
        throw new NotImplementedException("hi!");
    }
}