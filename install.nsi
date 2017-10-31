!define Logger_Reg_Path "Software\PassThruSupport.04.04\Passthru Logger"
!define LoggerControl_Reg_Path "Software\Passthru Logger"
!define Install_Name "J2534 PassThru Logger"

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
!include "x64.nsh"

!define MUI_ICON "PassThruLoggerControl/logger.ico"
;NSIS is ignoring the unicon unless it is the same as the normal icon
;!define MUI_UNICON "logger_remove.ico"

;Properly display all languages (Installer will not work on Windows 95, 98 or ME!)
Unicode true

# Set the installer display name
Name "J2534 PassThru Logger"

# set the name of the installer
Outfile "J2534 PassThru Logger install.exe"

; The default installation directory
InstallDir $PROGRAMFILES\J2534PassThruLogger

; Request application privileges for UAC
RequestExecutionLevel admin

; Registry key to check for directory (so if you install again, it will
; overwrite the old one automatically)
InstallDirRegKey HKLM "SOFTWARE\${Install_Name}" "Install_Dir"

;--------------------------------
; Pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE "LICENSE"
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_INSTFILES

!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

!insertmacro MUI_LANGUAGE "English" ;first language is the default language

; -------------------------------------------------------------------------------------------------
; Additional info (will appear in the "details" tab of the properties window for the installer)

VIAddVersionKey /LANG=${LANG_ENGLISH} "ProductName"      "J2534 PassThru Logger"
VIAddVersionKey /LANG=${LANG_ENGLISH} "Comments"         ""
VIAddVersionKey /LANG=${LANG_ENGLISH} "CompanyName"      "Jessy Diamond Exum"
VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalTrademarks"  "Application released under the MIT license"
;VIAddVersionKey /LANG=${LANG_ENGLISH} "LegalCopyright"   "© ${PRODUCT_NAME} Team"
;VIAddVersionKey /LANG=${LANG_ENGLISH} "FileDescription"  "Jessy Exum"
;VIAddVersionKey /LANG=${LANG_ENGLISH} "FileVersion"      "${PRODUCT_VERSION}"
VIProductVersion "1.0.0.0"

;--------------------------------
; Install Sections
Section "J2534 Passthru Logger (required)"

  SectionIn RO
  
  SetOutPath "$TEMP"

  ;If the visual studio version this project is compiled with changes, this section
  ;must be revisited. The registry key must be changed, and the VS redistributable
  ;binary must be updated to the VS version used.
  ClearErrors
  ReadRegStr $0 ${VCRuntimeRegHive} ${VCRuntimeRegKey} "Version"
  ${If} ${Errors}
    DetailPrint "Installing Visual Studio C Runtime..."
    File "${VCRuntimeSetupPath}\${VCRuntimeSetupFile}"
    ExecWait '"$TEMP\${VCRuntimeSetupFile}" /passive /norestart'
  ${Else}
    DetailPrint "Visual Studio C Runtime already installed."
  ${EndIf}

  ;Remove the now unnecessary runtime installer.
  Delete "$TEMP\${VCRuntimeSetupFile}"

  ;.NET framework 4.0 (Required for UI  
  DetailPrint "Installing .NET framework 4.0 Runtime..."
  File "${DotNetRuntimeSetupPath}\${DotNetRuntimeSetupFile}"
  ExecWait '"$TEMP\${DotNetRuntimeSetupFile}" /passive /norestart'

  ;Remove the now unnecessary runtime installer.
  Delete "$TEMP\${DotNetRuntimeSetupFile}"
  
  
  ;Install the actual J2534 logger files
  SetOutPath $INSTDIR
  
  File PassThruLoggerControl\bin\Release\PassThruLoggerControl.exe
  File Release_x86\PassThruLogger.dll

  SetRegView 32
  WriteRegStr    HKLM "${Logger_Reg_Path}" "FunctionLibrary"   "$INSTDIR\PassThruLogger.dll"
  
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "CAN"               00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "CAN_PS"            00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "SW_CAN_PS"         00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "ISO14230"          00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "ISO15765"          00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "ISO15765_PS"       00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "SW_ISO15765_PS"    00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "J1850VPW"          00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "J1850PWM"          00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "KW82_PS"           00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "ISO9141"           00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "ISO9141_PS"        00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "ISO14230_PS"       00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "UART_ECHO_BYTE_PS" 00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "SCI_A_ENGINE"      00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "SCI_A_TRANS"       00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "SCI_B_ENGINE"      00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "SCI_B_TRANS"       00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "FT_CAN_PS"         00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "FT_ISO15675_PS"    00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "GM_UART_PS"        00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "HONDA_DIAG_PS"     00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "J2610_PS"          00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "J1850PWM_PS"       00000001
  WriteRegDWORD  HKLM "${Logger_Reg_Path}" "J1850VPW_PS"       00000001
  
  WriteRegStr    HKLM "${Logger_Reg_Path}" "Name"              "PassThru Logger"
  WriteRegStr    HKLM "${Logger_Reg_Path}" "Vendor"            "Jessy Diamond Exum"
  WriteRegStr    HKLM "${Logger_Reg_Path}" "ConfigApplication" ""
  DetailPrint "Registered J2534 Logger"
  
  
  ; Write the installation path into the registry
  WriteRegStr HKLM "SOFTWARE\${Install_Name}" "Install_Dir" "$INSTDIR"

  ; Write the uninstall keys for Windows
  ;WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "DisplayVersion" ""
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "DisplayIcon" '"$INSTDIR\PassThruLoggerControl.exe",0'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "DisplayName" "${Install_Name}"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "Publisher" "Jessy Diamond Exum"
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "UninstallString" '"$INSTDIR\uninstall.exe"'
  WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "URLInfoAbout" "https://github.com/diamondman/J2534PassThruLogger"
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "NoModify" 1
  WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}" "NoRepair" 1

  SetOutPath $INSTDIR
  WriteUninstaller "uninstall.exe"

SectionEnd

;--------------------------------
; Uninstaller
Section "Uninstall"

  ; Remove registry keys
  DeleteRegKey HKLM "${Logger_Reg_Path}"
  DeleteRegKey HKCU "${LoggerControl_Reg_Path}"
  DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\${Install_Name}"
  DeleteRegKey HKLM "SOFTWARE\${Install_Name}"

  ; Remove files and uninstaller
  Delete "$INSTDIR\uninstall.exe"
  Delete "$INSTDIR\PassThruLoggerControl.exe"
  Delete "$INSTDIR\PassThruLogger.dll"

  ; Remove directories used
  RMDir "$INSTDIR"

SectionEnd
