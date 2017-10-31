#pragma once

enum wiredatatype : char
{
	TYPE_END = 0,
	TYPE_INT = 1,
	TYPE_STRING = 2,
	TYPE_ARRAY = 3,
	TYPE_POINTER = 4,
	TYPE_MSG = 5,

	TYPE_J2534_PROTOCOL_ID = 6,
	TYPE_J2534_CONNECT_FLAGS = 7,
	TYPE_J2534_FILTER_TYPE = 8,
	TYPE_J2534_PROG_VOLTAGE_PIN_NUMBER = 9,
	TYPE_J2534_PROG_VOLTAGE = 10,
	TYPE_J2534_IOCTL = 11,
	TYPE_J2534_TXFLAGS = 12,
	TYPE_J2534_RXSTATUS = 13,
	TYPE_J2534_CONFIG_PARAMS = 14,

	TYPE_DATAARRAY = 15,
	TYPE_INOUT_INT = 16,
};

enum pointernulltype : char
{
	POINTER_NULL = 0,
	POINTER_NOTNULL = 1,
};

enum wireprotover : short
{
	VER_0_0 = 0x0000
};

enum j2534protover : short
{
	VER_4_4 = 0x0404
};

enum msgtype : char
{
	reportParam = 0,
	J2534Msg = 1
};

enum param : char
{
	driver = 0,
	client = 1
};

enum J2534_0404func : char
{
	API_PassThruOpen = 0,
	API_PassThruClose,
	API_PassThruConnect,
	API_PassThruDisconnect,
	API_PassThruReadMsgs,
	API_PassThruWriteMsgs,
	API_PassThruStartPeriodicMsg,
	API_PassThruStopPeriodicMsg,
	API_PassThruStartMsgFilter,
	API_PassThruStopMsgFilter,
	API_PassThruSetProgrammingVoltage,
	API_PassThruReadVersion,
	API_PassThruGetLastError,
	API_PassThruIoctl
};
