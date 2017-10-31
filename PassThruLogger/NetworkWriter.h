#pragma once
#include <winsock2.h>
#include <ws2tcpip.h>

#define DEFAULT_BUFLEN 512

class NetworkWriter
{
public:
	NetworkWriter(int ai_family, int ai_socktype, IPPROTO ai_proto);
	~NetworkWriter();

	bool connect(PCSTR addr, PCSTR port);
	int send(_In_reads_bytes_(len) const char FAR * buf,
		_In_ int len,
		_In_ int flags);

	void writeByte(char n);
	void writeShort(short n);
	void writeInt(int n);
	void write(const char* s);
	void write(const char* s, size_t len);
	void write(const wchar_t* s);
	void write7BitEncodedInt(int valud);

	void flush();

	void close();

	bool isConnected() {
		return connected;
	}

private:
	int ai_family;
	int ai_socktype;
	IPPROTO ai_proto;

	SOCKET s = INVALID_SOCKET;
	bool connected = false;

	int recvbuflen = DEFAULT_BUFLEN;

	char buff[DEFAULT_BUFLEN];
	char* buffptr = buff;
};
