; -- Example1.iss --
; Demonstrates copying 3 files and creating an icon.

; SEE THE DOCUMENTATION FOR DETAILS ON CREATING .ISS SCRIPT FILES!

#define Dependency_NoExampleSetup
#include "InnoDependencyInstaller/CodeDependencies.iss"

#define MyAppName "Doodle Digits"
#define MyAppExeName "DoodleDigits.exe"

#ifndef VERSION
    #define VERSION "1.69.69"
#endif

[Setup]
AppName={#MyAppName}
AppVersion={#VERSION}
WizardStyle=modern
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
AppPublisher="Anton Bergåker"
Compression=lzma2
SolidCompression=yes
; "ArchitecturesAllowed=x64" specifies that Setup cannot run on
; anything but x64.
ArchitecturesAllowed=x64
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
ArchitecturesInstallIn64BitMode=x64
OutputBaseFilename="DoodleDigits.{#Version}.Installer"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; \
    GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\DoodleDigits\DoodleDigits\bin\publish\runtime\*"; DestDir: "{app}"; Flags: recursesubdirs ignoreversion
Source: ".\InnoDependencyInstaller\src\netcorecheck_x64.exe"; Flags: dontcopy noencryption

[Icons]
Name: "{group}\Doodle Digits"; Filename: "{app}\{#MyAppExeName}"
Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup: Boolean;
begin
  // add the dependencies you need
  Dependency_AddDotNet60Desktop;
  // ...
  Result := True;
end;
