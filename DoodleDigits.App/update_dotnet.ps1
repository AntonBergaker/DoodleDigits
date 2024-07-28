dotnet publish ../DoodleDigits/DoodleDigits.JsInterop/ -c Release

Copy-Item -Path "../DoodleDigits/DoodleDigits.JsInterop/bin/Release/net8.0/browser-wasm/AppBundle/*" -Destination "./static/doodledigits/" -Recurse -Force
