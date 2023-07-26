Remove-Item ./dist -Recurse
New-Item ./dist -ItemType "directory"

$Version = (ls "./DoodleDigits.App/out/Doodle Digits-win32-x64/DoodleDigits.exe" | % versioninfo).ProductVersion

$PathStandalone = ".\dist\DoodleDigits.{0}.Standalone.zip" -f $Version

Compress-Archive -Path ".\DoodleDigits.App\out\Doodle Digits-win32-x64" -DestinationPath $PathStandalone -CompressionLevel Optimal -Force

iscc .\Installer\Installer.iss /DVERSION=$Version /O"./dist"

PAUSE