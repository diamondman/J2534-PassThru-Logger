// Loader4.cpp
// (c) 2005 National Control Systems, Inc.
// Portions (c) 2004 Drew Technologies, Inc.
// Dynamic J2534 v04.04 dll loader for VB

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to:
// the Free Software Foundation, Inc.
// 51 Franklin Street, Fifth Floor
// Boston, MA  02110-1301, USA

// National Control Systems, Inc.
// 10737 Hamburg Rd
// Hamburg, MI 48139
// 810-231-2901

// Drew Technologies, Inc.
// 7012  E.M -36, Suite 3B
// Whitmore Lake, MI 48189
// 810-231-3171

#include "stdafx.h"
#include <windows.h>
#include "Loader4.h"

PTOPEN LocalOpen;
PTCLOSE LocalClose;
PTCONNECT LocalConnect;
PTDISCONNECT LocalDisconnect;
PTREADMSGS LocalReadMsgs;
PTWRITEMSGS LocalWriteMsgs;
PTSTARTPERIODICMSG LocalStartPeriodicMsg;
PTSTOPPERIODICMSG LocalStopPeriodicMsg;
PTSTARTMSGFILTER LocalStartMsgFilter;
PTSTOPMSGFILTER LocalStopMsgFilter;
PTSETPROGRAMMINGVOLTAGE LocalSetProgrammingVoltage;
PTREADVERSION LocalReadVersion;
PTGETLASTERROR LocalGetLastError;
PTIOCTL LocalIoctl;

static HINSTANCE hDLL = NULL;

static long loadAPIFunctionsFromDLL() {
	long lFuncList = 0;

	LocalOpen = (PTOPEN)(GetProcAddress(hDLL, "PassThruOpen"));
	if (LocalOpen == NULL) lFuncList = lFuncList | ERR_NO_PTOPEN;

	LocalClose = (PTCLOSE)(GetProcAddress(hDLL, "PassThruClose"));
	if (LocalClose == NULL) lFuncList = lFuncList | ERR_NO_PTCLOSE;

	LocalConnect = (PTCONNECT)(GetProcAddress(hDLL, "PassThruConnect"));
	if (LocalConnect == NULL) lFuncList = lFuncList | ERR_NO_PTCONNECT;

	LocalDisconnect = (PTDISCONNECT)(GetProcAddress(hDLL, "PassThruDisconnect"));
	if (LocalDisconnect == NULL) lFuncList = lFuncList | ERR_NO_PTDISCONNECT;

	LocalReadMsgs = (PTREADMSGS)(GetProcAddress(hDLL, "PassThruReadMsgs"));
	if (LocalReadMsgs == NULL) lFuncList = lFuncList | ERR_NO_PTREADMSGS;

	LocalWriteMsgs = (PTWRITEMSGS)(GetProcAddress(hDLL, "PassThruWriteMsgs"));
	if (LocalWriteMsgs == NULL) lFuncList = lFuncList | ERR_NO_PTWRITEMSGS;

	LocalStartPeriodicMsg = (PTSTARTPERIODICMSG)(GetProcAddress(hDLL, "PassThruStartPeriodicMsg"));
	if (LocalStartPeriodicMsg == NULL) lFuncList = lFuncList | ERR_NO_PTSTARTPERIODICMSG;

	LocalStopPeriodicMsg = (PTSTOPPERIODICMSG)(GetProcAddress(hDLL, "PassThruStopPeriodicMsg"));
	if (LocalStopPeriodicMsg == NULL) lFuncList = lFuncList | ERR_NO_PTSTOPPERIODICMSG;

	LocalStartMsgFilter = (PTSTARTMSGFILTER)(GetProcAddress(hDLL, "PassThruStartMsgFilter"));
	if (LocalStartPeriodicMsg == NULL) lFuncList = lFuncList | ERR_NO_PTSTARTMSGFILTER;

	LocalStopMsgFilter = (PTSTOPMSGFILTER)(GetProcAddress(hDLL, "PassThruStopMsgFilter"));
	if (LocalStopMsgFilter == NULL) lFuncList = lFuncList | ERR_NO_PTSTOPMSGFILTER;

	LocalSetProgrammingVoltage = (PTSETPROGRAMMINGVOLTAGE)(GetProcAddress(hDLL, "PassThruSetProgrammingVoltage"));
	if (LocalSetProgrammingVoltage == NULL) lFuncList = lFuncList | ERR_NO_PTSETPROGRAMMINGVOLTAGE;

	LocalReadVersion = (PTREADVERSION)(GetProcAddress(hDLL, "PassThruReadVersion"));
	if (LocalReadVersion == NULL) lFuncList = lFuncList | ERR_NO_PTREADVERSION;

	LocalGetLastError = (PTGETLASTERROR)(GetProcAddress(hDLL, "PassThruGetLastError"));
	if (LocalGetLastError == NULL) lFuncList = lFuncList | ERR_NO_PTGETLASTERROR;

	LocalIoctl = (PTIOCTL)(GetProcAddress(hDLL, "PassThruIoctl"));
	if (LocalIoctl == NULL) lFuncList = lFuncList | ERR_NO_PTIOCTL;

	//WRONG_DLL is a vague and less useful error.
	//if (lFuncList == ERR_NO_FUNCTIONS) return ERR_WRONG_DLL_VER;

	return lFuncList;
}

long WINAPI LoadJ2534Dll(const wchar_t *sLib) {
	if (hDLL != NULL) UnloadJ2534Dll();
	hDLL = LoadLibraryW(sLib);
	if (hDLL == NULL) return ERR_NO_DLL;
	return loadAPIFunctionsFromDLL();
}
long WINAPI LoadJ2534Dll(const char *sLib) {
	if (hDLL != NULL) UnloadJ2534Dll();
	hDLL = LoadLibraryA (sLib);
	if (hDLL == NULL) return ERR_NO_DLL;
	return loadAPIFunctionsFromDLL();
}

long WINAPI UnloadJ2534Dll()
{
	if (FreeLibrary(hDLL))
	{
		hDLL = NULL;
		LocalOpen = NULL;
		LocalClose = NULL;
		LocalConnect = NULL;
		LocalDisconnect = NULL;
		LocalReadMsgs = NULL;
		LocalWriteMsgs = NULL;
		LocalStartPeriodicMsg = NULL;
		LocalStopPeriodicMsg = NULL;
		LocalStartMsgFilter = NULL;
		LocalStopMsgFilter = NULL;
		LocalSetProgrammingVoltage = NULL;
		LocalReadVersion = NULL;
		LocalGetLastError = NULL;
		LocalIoctl = NULL;
		return 0;
	}
	return ERR_NO_DLL;
}
