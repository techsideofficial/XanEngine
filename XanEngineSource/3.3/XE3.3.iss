; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Xan Engine"
#define MyAppVersion "3.3"
#define MyAppPublisher "TechSide"
#define MyAppURL "https://github.com/techsideofficial/XanEngine"
#define MyAppExeName "xan.exe"
#define MyAppAssocName "Xan Config File"
#define MyAppAssocExt ".xfg"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{B573A8E0-58B6-4CE8-94D2-3DE953D58693}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableDirPage=yes
ChangesAssociations=yes
DisableProgramGroupPage=yes
LicenseFile=C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\LICENSE.txt
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\Compiler
OutputBaseFilename=XanSetup-3.3-win64-shipping
SetupIconFile=C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\assets\icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\binaries\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\README.txt"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\launch.bat"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\xac\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\temp\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\plugins\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\binaries\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\base\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\wiene\Documents\GitHub\XanEngine\XanEngineSource\3.3\PF3.3\assets\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
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

