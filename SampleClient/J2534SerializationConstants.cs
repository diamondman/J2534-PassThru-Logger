namespace PassThruLoggerControl
{
    enum msgtype : byte
    {
        reportParam = 0,
        J2534Msg = 1
    }

    enum param : byte
    {
        driver = 0,
        client = 1
    }

    enum J2534_0404func : byte
    {
        PassThruOpen = 0,
        PassThruClose,
        PassThruConnect,
        PassThruDisconnect,
        PassThruReadMsgs,
        PassThruWriteMsgs,
        PassThruStartPeriodicMsg,
        PassThruStopPeriodicMsg,
        PassThruStartMsgFilter,
        PassThruStopMsgFilter,
        PassThruSetProgrammingVoltage,
        PassThruReadVersion,
        PassThruGetLastError,
        PassThruIoctl
    }
}
