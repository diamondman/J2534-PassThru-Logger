!define Install_Name "J2534 PassThru Logger"
!define Author_Name "Jessy Diamond Exum"
!define Product_Version ""

!define Logger_Reg_Path        'HKLM "Software\PassThruSupport.04.04\${Install_Name}"'
!define LoggerControl_Reg_Path 'HKCU "Software\${Install_Name}"'
!define InstallRecord_Reg_Path 'HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}"'

;NOTE! The driver requires a VC runtime to be installed in order to work.
;The Logger UI requires the .net framework 4.0 runtime to work.
;This installer must be bundled with the appropriate runtime installers, and have
;the installation registry key set so the installer can tell if the runtime is
;already installed. Copy runtimeinfo.nsh.sample to runtimeinfo.nsh and edit
;it for your version of Visual Studio.
!include "redist\runtimeinfo.nsh"

;--------------------------------
;Include Modern UI
!include "MUI2.nsh"
!include "FileFunc.nsh"

!define MUI_ICON "PassThruLoggerControl/logger.ico"
;NSIS is ignoring the unicon unless it is the same as the normal icon
;!define MUI_UNICON "logger_remove.ico"

;Properly display all languages (Installer will not work on Windows 95, 98 or ME!)
Unicode true

# Set the installer display name
Name "${Install_Name}"

# set the name of the installer
Outfile "${Install_Name} install.exe"

; The default installation directory
InstallDir $PROGRAMFILES\J2534PassThruLogger

; Request application privileges for UAC
RequestExecutionLevel admin

; Registry key to check for directory (so if you install again, it will
; overwrite the old one automatically)
InstallDirRegKey ${InstallRecord_Reg_Path} "InstallLocation"

;--------------------------------
; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "LICENSE"
;!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English" ;first language is the default language

; -------------------------------------------------------------------------------------------------
; Additional info (will appear in the "details" tab of the properties window for the installer)

VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName"      "J2534 PassThru Logger"
VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments"         ""
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName"      "${Author_Name}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks"  "Application released under the MIT license"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright"   "© ${Author_Name}"
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription"  "Logging Utility/Driver to monitor and debug J2534 implementations."
VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion"      "${Product_Version}"
VIProductVersion "1.0.0.0"

;--------------------------------
; Install Sections
Section "J2534 Passthru Logger (required)"

  SectionIn RO
  
  SetOutPath "$TEMP"

  ;If the visual studio version this project is compiled with changes, this section
  ;must be revisited. The registry key must be changed, and the VS redistributable
  ;binary must be updated to the VS version used.
  ;ClearErrors
  ;ReadRegStr $0 ${VCRuntimeRegValPath}
  ;${If} ${Errors}
    DetailPrint "Installing Visual Studio C Runtime..."
    File "${VCRuntimeSetupPath}\${VCRuntimeSetupFile}"
    ExecWait '"$TEMP\${VCRuntimeSetupFile}" /passive /norestart'
  ;${Else}
  ;  DetailPrint "Visual Studio C Runtime already installed."
  ;${EndIf}

  ;Remove the now unnecessary runtime installer.
  Delete "$TEMP\${VCRuntimeSetupFile}"

  ;.NET framework 4.0 (Required for UI)
  ClearErrors
  ;ReadRegStr $0 ${DotNetRuntimeRegValPath}
  ;${If} ${Errors}
    DetailPrint "Installing .NET framework 4.0 Runtime..."
    File "${DotNetRuntimeSetupPath}\${DotNetRuntimeSetupFile}"
    ExecWait '"$TEMP\${DotNetRuntimeSetupFile}" /passive /norestart'
  ;${Else}
  ;  DetailPrint ".NET framework 4.0 Runtime already installed."
  ;${EndIf}

  ;Remove the now unnecessary runtime installer.
  Delete "$TEMP\${DotNetRuntimeSetupFile}"
  
  
  ;Install the actual J2534 logger files
  SetOutPath $INSTDIR
  
  File PassThruLoggerControl\bin\Release\PassThruLoggerControl.exe
  File Release_x86\PassThruLogger.dll

  SetRegView 32
  WriteRegStr    ${Logger_Reg_Path} "FunctionLibrary"   "$INSTDIR\PassThruLogger.dll"
  WriteRegStr    ${Logger_Reg_Path} "Name"              "${Install_Name}"
  WriteRegStr    ${Logger_Reg_Path} "Vendor"            "${Author_Name}"
  WriteRegStr    ${Logger_Reg_Path} "ConfigApplication" "$INSTDIR\PassThruLoggerControl.exe"

  WriteRegDWORD  ${Logger_Reg_Path} "CAN"               00000001
  WriteRegDWORD  ${Logger_Reg_Path} "CAN_PS"            00000001
  WriteRegDWORD  ${Logger_Reg_Path} "SW_CAN_PS"         00000001
  WriteRegDWORD  ${Logger_Reg_Path} "ISO14230"          00000001
  WriteRegDWORD  ${Logger_Reg_Path} "ISO15765"          00000001
  WriteRegDWORD  ${Logger_Reg_Path} "ISO15765_PS"       00000001
  WriteRegDWORD  ${Logger_Reg_Path} "SW_ISO15765_PS"    00000001
  WriteRegDWORD  ${Logger_Reg_Path} "J1850VPW"          00000001
  WriteRegDWORD  ${Logger_Reg_Path} "J1850PWM"          00000001
  WriteRegDWORD  ${Logger_Reg_Path} "KW82_PS"           00000001
  WriteRegDWORD  ${Logger_Reg_Path} "ISO9141"           00000001
  WriteRegDWORD  ${Logger_Reg_Path} "ISO9141_PS"        00000001
  WriteRegDWORD  ${Logger_Reg_Path} "ISO14230_PS"       00000001
  WriteRegDWORD  ${Logger_Reg_Path} "UART_ECHO_BYTE_PS" 00000001
  WriteRegDWORD  ${Logger_Reg_Path} "SCI_A_ENGINE"      00000001
  WriteRegDWORD  ${Logger_Reg_Path} "SCI_A_TRANS"       00000001
  WriteRegDWORD  ${Logger_Reg_Path} "SCI_B_ENGINE"      00000001
  WriteRegDWORD  ${Logger_Reg_Path} "SCI_B_TRANS"       00000001
  WriteRegDWORD  ${Logger_Reg_Path} "FT_CAN_PS"         00000001
  WriteRegDWORD  ${Logger_Reg_Path} "FT_ISO15675_PS"    00000001
  WriteRegDWORD  ${Logger_Reg_Path} "GM_UART_PS"        00000001
  WriteRegDWORD  ${Logger_Reg_Path} "HONDA_DIAG_PS"     00000001
  WriteRegDWORD  ${Logger_Reg_Path} "J2610_PS"          00000001
  WriteRegDWORD  ${Logger_Reg_Path} "J1850PWM_PS"       00000001
  WriteRegDWORD  ${Logger_Reg_Path} "J1850VPW_PS"       00000001
  DetailPrint "Registered J2534 Logger"

  ; Write the uninstall keys for Windows
  WriteRegStr   ${InstallRecord_Reg_Path} "DisplayVersion"       "${Product_Version}"
  WriteRegStr   ${InstallRecord_Reg_Path} "DisplayIcon"          '"$INSTDIR\PassThruLoggerControl.exe",0'
  WriteRegStr   ${InstallRecord_Reg_Path} "DisplayName"          "${Install_Name}"
  WriteRegStr   ${InstallRecord_Reg_Path} "Publisher"            "${Author_Name}"
  WriteRegStr   ${InstallRecord_Reg_Path} "UninstallString"      "$\"$INSTDIR\uninstall.exe$\""
  WriteRegStr   ${InstallRecord_Reg_Path} "QuietUninstallString" "$\"$INSTDIR\uninstall.exe$\" /S"
  WriteRegStr   ${InstallRecord_Reg_Path} "InstallLocation"      "$\"$INSTDIR$\""
  WriteRegStr   ${InstallRecord_Reg_Path} "URLInfoAbout"         "https://github.com/diamondman/J2534PassThruLogger"
  WriteRegDWORD ${InstallRecord_Reg_Path} "NoModify"             1
  WriteRegDWORD ${InstallRecord_Reg_Path} "NoRepair"             1
  ${GetSize} "$INSTDIR" "/S=0K" $0 $1 $2
  IntFmt $0 "0x%08X" $0
  WriteRegDWORD ${InstallRecord_Reg_Path} "EstimatedSize"        "$0"

  SetOutPath $INSTDIR
  WriteUninstaller "uninstall.exe"
 
  # Start Menu
  createShortCut "$SMPROGRAMS\PassThruLoggerControl.lnk" "$INSTDIR\PassThruLoggerControl.exe"

SectionEnd

;--------------------------------
; Uninstaller
Section "Uninstall"

  # Remove Start Menu launcher
  delete "$SMPROGRAMS\PassThruLoggerControl.lnk"

  ; Remove registry keys
  DeleteRegKey ${Logger_Reg_Path}
  DeleteRegKey ${LoggerControl_Reg_Path}
  DeleteRegKey ${InstallRecord_Reg_Path}

  ; Remove files and uninstaller
  Delete "$INSTDIR\uninstall.exe"
  Delete "$INSTDIR\PassThruLoggerControl.exe"
  Delete "$INSTDIR\PassThruLogger.dll"

  ; Remove directories used
  RMDir "$INSTDIR"

SectionEnd
