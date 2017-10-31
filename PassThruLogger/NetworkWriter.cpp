#include "stdafx.h"
#include "NetworkWriter.h"

// Need to link with Ws2_32.lib, Mswsock.lib, and Advapi32.lib
#pragma comment (lib, "Ws2_32.lib")
#pragma comment (lib, "Mswsock.lib")
#pragma comment (lib, "AdvApi32.lib")

NetworkWriter::NetworkWriter(int ai_family, int ai_socktype, IPPROTO ai_proto) {
	this->ai_family = ai_family;
	this->ai_socktype = ai_socktype;
	this->ai_proto = ai_proto;
}


NetworkWriter::~NetworkWriter() {
	close();
}

bool NetworkWriter::connect(PCSTR addr_p, PCSTR port_p) {
	struct addrinfo *result = NULL, *ptr = NULL, hints;

	ZeroMemory(&hints, sizeof(hints));
	hints.ai_family = ai_family;
	hints.ai_socktype = ai_socktype;
	hints.ai_protocol = ai_proto;

	// Resolve the server address and port
	int iResult = getaddrinfo(addr_p, port_p, &hints, &result);
	if (iResult != 0) {
		printf("getaddrinfo failed with error: %d\n", iResult);
		return FALSE;
	}

	//Shamelessly mostly copied from https://msdn.microsoft.com/en-us/library/windows/desktop/ms737591%28v=vs.85%29.aspx?f=255&MSPPError=-2147217396
	// Attempt to connect to an address until one succeeds
	for (ptr = result; ptr != NULL; ptr = ptr->ai_next) {

		// Create a SOCKET for connecting to server
		s = socket(ptr->ai_family, ptr->ai_socktype,
			ptr->ai_protocol);
		if (s == INVALID_SOCKET) {
			printf("socket failed with error: %ld\n", WSAGetLastError());
			return FALSE;
		}

		// Connect to server.
		iResult = ::connect(s, ptr->ai_addr, (int)ptr->ai_addrlen);
		if (iResult == SOCKET_ERROR) {
			close();
			s = INVALID_SOCKET;
			continue;
		}
		break;
	}

	freeaddrinfo(result);

	if (s == INVALID_SOCKET) {
		printf("Unable to connect to server!\n");
		return FALSE;
	}

	connected = true;
	return TRUE;
}

int NetworkWriter::send(_In_reads_bytes_(len) const char FAR * buf,
	_In_ int len,
	_In_ int flags) {
	int iResult = ::send(s, buf, len, flags);

	if (iResult == SOCKET_ERROR) {
		auto err = WSAGetLastError();
		printf("send failed with error: %d\n", err);
		close();
	}

	return iResult;
}

void NetworkWriter::writeByte(char n) {
	*(buffptr++) = n;
	if (buffptr - buff == sizeof(buff))
		flush();
}
void NetworkWriter::writeShort(short n) {
	writeByte(n & 0xFF);
	writeByte((n >> 8) & 0xFF);
}
void NetworkWriter::writeInt(int n) {
	writeByte(n & 0xFF);
	writeByte((n >> 8) & 0xFF);
	writeByte((n >> 16) & 0xFF);
	writeByte((n >> 24) & 0xFF);
}
void NetworkWriter::write(const char* s, size_t len) {
	write7BitEncodedInt(len);
	for(unsigned int i = 0; i < len; i++)
		writeByte(s[i]);
}
void NetworkWriter::write(const char* s) {
	if (s == NULL) {
		writeByte(0);
		return;
	}
	write7BitEncodedInt(strlen(s));
	while (*s != 0)
		writeByte(*(s++));
}

void NetworkWriter::write7BitEncodedInt(int value) {
	unsigned int num = (unsigned int)value;

	while (num >= 128U)
	{
		writeByte((uint8_t)(num | 128U));
		num >>= 7;
	}

	writeByte((uint8_t)num);
}

void NetworkWriter::flush() {
	if (connected) {
		int res = ::send(s, buff, buffptr - buff, 0);
		if (res == SOCKET_ERROR)
			close();
	}
	buffptr = buff;
}

void NetworkWriter::close() {
	connected = false;
	if (s != INVALID_SOCKET) {
		closesocket(s);
		s = INVALID_SOCKET;
	}
}
