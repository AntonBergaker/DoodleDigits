Remove-Item ./dist -Recurse
New-Item ./dist -ItemType "directory"

cd DoodleDigits.App
npm run package
npx electron-forge package --platform linux
cd ..

$Version = (ls "./DoodleDigits.App/out/Doodle Digits-win32-x64/DoodleDigits.exe" | % versioninfo).ProductVersion

$PathStandaloneWin = ".\dist\DoodleDigits.{0}.Windows.Standalone.zip" -f $Version
$PathStandaloneLinux = ".\dist\DoodleDigits.{0}.Linux.Standalone.zip" -f $Version

Compress-Archive -Path ".\DoodleDigits.App\out\Doodle Digits-win32-x64" -DestinationPath $PathStandaloneWin -CompressionLevel Optimal -Force
Compress-Archive -Path ".\DoodleDigits.App\out\Doodle Digits-linux-x64" -DestinationPath $PathStandaloneLinux -CompressionLevel Optimal -Force

iscc .\Installer\Installer.iss /DVERSION=$Version /O"./dist"
