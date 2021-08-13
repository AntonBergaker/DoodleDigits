Remove-Item ./dist -Recurse
New-Item ./dist -ItemType "directory"

dotnet publish DoodleDigits\DoodleDigits /p:PublishProfile=Runtime
dotnet publish DoodleDigits\DoodleDigits /p:PublishProfile=Standalone

$Version = (ls .\DoodleDigits\DoodleDigits\bin\publish\standalone\DoodleDigits.exe | % versioninfo).ProductVersion

$PathRuntime = ".\dist\DoodleDigits.{0}.Runtime.Dependent.zip" -f $Version
$PathStandalone = ".\dist\DoodleDigits.{0}.Standalone.zip" -f $Version

Compress-Archive -Path .\DoodleDigits\DoodleDigits\bin\publish\runtime -DestinationPath $PathRuntime -CompressionLevel Optimal -Force
Compress-Archive -Path .\DoodleDigits\DoodleDigits\bin\publish\standalone -DestinationPath $PathStandalone -CompressionLevel Optimal -Force

iscc .\Installer\Installer.iss /DVERSION=$Version /O"./dist"