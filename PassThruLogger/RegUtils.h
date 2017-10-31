#pragma once
#include <windows.h>
#include <string>

LONG GetDWORDRegKey(HKEY hKey, const std::string &strValueName, DWORD &nValue, DWORD nDefaultValue);

LONG GetBoolRegKey(HKEY hKey, const std::string &strValueName, bool &bValue, bool bDefaultValue);

LONG GetStringRegKey(HKEY hKey, const std::string &strValueName, std::string &strValue, const std::string &strDefaultValue);
