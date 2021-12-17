#include "CodeDependencies.iss"

#define AppSourceFolder "C:\Users\under\Documents\GitHub\Illumine"
#define AppName "Illumine"
#define AppExeName "Illumine.exe"
#define AppPublisher "Luke Pflibsen-Jones"
#define AppURL "https://github.com/luketimothyjones/Illumine"
#define AppVersion GetVersionNumbersString(AppSourceFolder + "\Illumine\Illumine\bin\x64\Release\Illumine.exe")
#define AppIcon AppSourceFolder + "\Illumine\Illumine\Magnifying-glass.ico"

[Setup]
AppId={{A404BEE0-5B55-45CC-AD0E-8198F6B56307}
AppName={#AppName}
AppVersion={#AppVersion}
AppVerName={#AppName} {#AppVersion}
AppPublisher={#AppPublisher}
AppPublisherURL={#AppURL}
AppSupportURL={#AppURL}
AppUpdatesURL={#AppURL}
DefaultGroupName={#AppName}
DefaultDirName={autopf64}\{#AppName}
DisableProgramGroupPage=yes
LicenseFile={#AppSourceFolder}\LICENSE
InfoBeforeFile={#AppSourceFolder}\Installer\Resources\BeforeInstallMessage.txt
OutputDir={#AppSourceFolder}\Installer\Compiled Output
OutputBaseFilename=Illumine Setup
SetupIconFile={#AppSourceFolder}\Installer\Resources\Setup-icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "startmenuentry" ; Description: "Start Illumine when you log in" ; GroupDescription: "Autorun"

[Files]
Source: "{#AppSourceFolder}\Illumine\Illumine\bin\x64\Release\{#AppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#AppSourceFolder}\Illumine\Illumine\bin\x64\Release\GlobalHotkeys.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#AppSourceFolder}\Illumine\Illumine\bin\x64\Release\Updater.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#AppSourceFolder}\Illumine\Illumine\bin\x64\Release\{#AppExeName}.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#AppSourceFolder}\Illumine\Illumine\bin\x64\Release\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#AppSourceFolder}\Illumine\Illumine\bin\x64\Release\Everything64.dll"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autopf64}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{#AppIcon}"
Name: "{autodesktop}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{#AppIcon}"; Tasks: desktopicon
Name: "{group}\{#AppName}"; Filename: "{app}\{#AppExeName}"; IconFilename: "{#AppIcon}"

Name: "{commonstartup}\{#AppName}"; Filename: "{app}\{#AppExeName}"; Tasks: startmenuentry

[Run]
Filename: "{app}\{#AppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(AppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
procedure InitializeWizard;
begin
  Dependency_InitializeWizard;
end;

function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  Result := Dependency_PrepareToInstall(NeedsRestart);
end;

function NeedRestart: Boolean;
begin
  Result := Dependency_NeedRestart;
end;

function UpdateReadyMemo(const Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo: String): String;
begin
  Result := Dependency_UpdateReadyMemo(Space, NewLine, MemoUserInfoInfo, MemoDirInfo, MemoTypeInfo, MemoComponentsInfo, MemoGroupInfo, MemoTasksInfo);
end;

function InitializeSetup: Boolean;
begin
  Dependency_AddDotNet47;
  Dependency_AddEverything141;
  Result := True;
end;
