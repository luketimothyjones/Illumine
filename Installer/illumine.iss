#include "CodeDependencies.iss"

#define MyAppName "Illumine"
#define MyAppVersion "0.0.5"
#define MyAppPublisher "Luke Pflibsen-Jones"
#define MyAppURL "https://github.com/luketimothyjones/Illumine"
#define MyAppExeName "Illumine.exe"
#define MyAppIcon "C:\Users\under\Documents\GitHub\Illumine\Illumine\Magnifying-glass.ico"

[Setup]
AppId={{A404BEE0-5B55-45CC-AD0E-8198F6B56307}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultGroupName={#MyAppName}
DefaultDirName={autopf64}\{#MyAppName}
DisableProgramGroupPage=yes
LicenseFile=C:\Users\under\Documents\GitHub\Illumine\LICENSE
InfoBeforeFile=C:\Users\under\Documents\GitHub\Illumine\Installer\Resources\BeforeInstallMessage.txt
OutputDir=C:\Users\under\Documents\GitHub\Illumine\Installer\Compiled Output
OutputBaseFilename=Illumine Setup
SetupIconFile=C:\Users\under\Documents\GitHub\Illumine\Installer\Resources\Setup-icon.ico
Compression=lzma
SolidCompression=yes
WizardStyle=modern

ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "C:\Users\under\Documents\GitHub\Illumine\Illumine\Illumine\bin\x64\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\under\Documents\GitHub\Illumine\Illumine\Illumine\bin\x64\Release\Everything64.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\under\Documents\GitHub\Illumine\Illumine\Illumine\bin\x64\Release\GlobalHotkeys.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\under\Documents\GitHub\Illumine\Illumine\Illumine\bin\x64\Release\Illumine.exe.config"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autopf64}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{#MyAppIcon}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; IconFilename: "{#MyAppIcon}"; Tasks: desktopicon
Name: "{group}\{#MyAppName}"; Filename: "{#MyAppExeName}"; IconFilename: "{#MyAppIcon}"

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

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
