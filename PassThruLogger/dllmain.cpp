// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include "RegUtils.h"
#include "Loader4.h"
#include "NetworkWriter.h"
#include "WireProtocolConstants.h"

unsigned int refcount = 0;
std::string driverKeyName;
bool loadedFine = FALSE;
std::unique_ptr<NetworkWriter> writer;

bool loadDriver() {

	HKEY LoggerSettingsKey;
	LONG lRes = RegOpenKeyExA(HKEY_CURRENT_USER, "Software\\Passthru Logger", 0, KEY_READ, &LoggerSettingsKey);
	bool bExistsAndSuccess(lRes == ERROR_SUCCESS);
	if (!bExistsAndSuccess) return FALSE;
	//bool bDoesNotExistsSpecifically(lRes == ERROR_FILE_NOT_FOUND);

	GetStringRegKey(LoggerSettingsKey, "DefaultDriverKey", driverKeyName, "");

	//Get the driver file name
	std::string driverKeyFullPath = "SOFTWARE\\PassThruSupport.04.04\\" + driverKeyName;
	HKEY DriverEntryKey;
	lRes = RegOpenKeyExA(HKEY_LOCAL_MACHINE, driverKeyFullPath.c_str(), 0, KEY_READ, &DriverEntryKey);
	bExistsAndSuccess = (lRes == ERROR_SUCCESS);
	if (!bExistsAndSuccess) return FALSE;
	//bDoesNotExistsSpecifically = (lRes == ERROR_FILE_NOT_FOUND);

	std::string driverFilePath;
	GetStringRegKey(DriverEntryKey, "FunctionLibrary", driverFilePath, "");

	long loadResult = LoadJ2534Dll(driverFilePath.c_str());
	if (loadResult != 0) return FALSE;
	return TRUE;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved) {
	switch (ul_reason_for_call) {
	case DLL_PROCESS_ATTACH:
		refcount++;
		if (refcount == 1) {
			WSADATA wsaData;
			int iResult = WSAStartup(MAKEWORD(2, 2), &wsaData);
			if (iResult != 0) {
				printf("WSAStartup failed with error: %d\n", iResult);
				break;
			}
			//Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
			writer = std::make_unique<NetworkWriter>(AF_UNSPEC, SOCK_STREAM, IPPROTO_TCP);
			if (!writer->connect("localhost", "2534"))
				break;

			char filename[MAX_PATH];
			DWORD size = GetModuleFileNameA(NULL, filename, MAX_PATH);
			char sizeb = (char)size;

			writer->writeShort(wireprotover::VER_0_0);//wire proto
			writer->writeShort(j2534protover::VER_4_4);//j2534 proto

			writer->writeByte(msgtype::reportParam);
			writer->writeByte(param::client);
			writer->write(filename);

			if (!writer->isConnected())
				break;

			if (!loadDriver())
				break;

			writer->writeByte(msgtype::reportParam);
			writer->writeByte(param::driver);
			writer->write(driverKeyName.c_str());
			writer->writeInt(0);

			writer->flush();



			loadedFine = TRUE;
		}
		break;
	case DLL_PROCESS_DETACH:
		if (refcount == 0) {
			UnloadJ2534Dll();
			writer = nullptr;
			WSACleanup();
		}
		break;
	}
	return TRUE;
}
