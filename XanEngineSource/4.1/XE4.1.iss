; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Xan Engine"
#define MyAppVersion "4.1"
#define MyAppPublisher "TechSide"
#define MyAppURL "https://github.com/techsideofficial/XanEngine"
#define MyAppExeName "Xan.exe"
#define MyAppAssocName "Xan Cache File"
#define MyAppAssocExt ".xcache"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

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
LicenseFile=C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\LICENSE.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\Compiler
OutputBaseFilename=XanEngine-4.1-win64-shipping
SetupIconFile=C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\assets\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\assets\*"; DestDir: "{app}\assets"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\base\*"; DestDir: "{app}\base"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\binaries\*"; DestDir: "{app}\binaries"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\plugins\*"; DestDir: "{app}\plugins"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\LICENSE.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\README.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\OneDrive\Documents\GitHub\XanEngine\XanEngineSource\4.1\PF4.1\Xan.exe"; DestDir: "{app}"; Flags: ignoreversion
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
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

