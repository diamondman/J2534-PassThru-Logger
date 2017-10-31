using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;

namespace PassThruLoggerControl
{
    class J2534ProtocolInterpreter_0404 : J2534ProtocolInterpreter
    {
        public override string J2534Version { get { return "04.04"; } }

        public override string interpret(FullBinaryReader reader)
        {
            StringBuilder logentry = new StringBuilder();

            J2534_0404func f = (J2534_0404func)reader.ReadByte();
            checkEnum(typeof(J2534_0404func), f);

            logentry.Append(f + "(");

            //Loop over params
            bool firstparam = true;

            wiredatatype datatype = (wiredatatype)reader.ReadByte();
            while (datatype != wiredatatype.END)
            {
                if (!firstparam) logentry.Append(", ");
                parseDataField(reader, logentry, datatype);
                firstparam = false;
                datatype = (wiredatatype)reader.ReadByte();
            }

            logentry.Append(") -> ");
            J2534_errorCodes retcode = (J2534_errorCodes)reader.ReadInt32();
            checkEnum(typeof(J2534_errorCodes), retcode);
            logentry.Append(retcode);

            return logentry.ToString();
        }

        private void parseDataField(FullBinaryReader reader, StringBuilder logentry, wiredatatype datatype)
        {
            checkEnum(typeof(wiredatatype), datatype);

            switch (datatype)
            {
                case wiredatatype.INT:
                    logentry.Append(reader.ReadUInt32().ToString());
                    break;
                case wiredatatype.STRING:
                    logentry.Append(ToLiteral(reader.ReadString()));
                    break;
                case wiredatatype.POINTER:
                    pointernulltype isvalid = (pointernulltype)reader.ReadByte();
                    checkEnum(typeof(pointernulltype), isvalid);
                    switch (isvalid)
                    {
                        case pointernulltype.NULL:
                            logentry.Append("NULL");
                            break;
                        case pointernulltype.NOTNULL:
                            datatype = (wiredatatype)reader.ReadByte();
                            parseDataField(reader, logentry, datatype);
                            break;
                    }
                    break;
                case wiredatatype.MSG:
                    {
                        logentry.Append("{");

                        parseDataField(reader, logentry, wiredatatype.J2534_PROTOCOL_ID);//ProtocolID
                        logentry.Append("; ");

                        logentry.Append("RxStatus: ");
                        parseDataField(reader, logentry, wiredatatype.J2534_RXSTATUS);//RxStatus
                        logentry.Append("; ");

                        logentry.Append("TxFlags: ");
                        parseDataField(reader, logentry, wiredatatype.J2534_TXFLAGS);//TxFlags
                        logentry.Append("; ");

                        var ts = reader.Read7BitEncodedInt();
                        logentry.Append("TS: " + TimeSpan.FromMilliseconds(ts/1000).ToString() + "; ");//Timestamp

                        uint extradata = (uint)reader.Read7BitEncodedInt();
                        int datasize = reader.Read7BitEncodedInt();
                        byte[] data = reader.ReadBytes(datasize);
                        logentry.Append("LEN: " + data.Length + "; ");//DataSize
                        logentry.Append("ExtraIndex: " + extradata + "; ");//ExtraDataIndex
                        logentry.Append("Data: " + System.BitConverter.ToString(data));//data

                        logentry.Append("}");
                    }
                    break;
                case wiredatatype.DATAARRAY:
                    {
                        int datasize = reader.Read7BitEncodedInt();
                        byte[] data = reader.ReadBytes(datasize);
                        logentry.Append(System.BitConverter.ToString(data));
                    }
                    break;
                case wiredatatype.ARRAY:
                    {
                        logentry.Append("[");
                        UInt32 len = (UInt32)reader.Read7BitEncodedInt();
                        for(UInt32 i = 0; i < len; i++)
                        {
                            if (i != 0) logentry.Append(", ");
                            datatype = (wiredatatype)reader.ReadByte();
                            parseDataField(reader, logentry, datatype);
                        }
                        logentry.Append("]");
                    }
                    break;
                case wiredatatype.INOUT_INT:
                    {
                        logentry.Append((UInt32)reader.Read7BitEncodedInt());
                        logentry.Append("=>");
                        logentry.Append((UInt32)reader.Read7BitEncodedInt());
                    }
                    break;

                case wiredatatype.J2534_PROTOCOL_ID:
                    processJ2534Enum(reader, logentry, typeof(J2534_protocolValues));
                    break;
                case wiredatatype.J2534_FILTER_TYPE:
                    processJ2534Enum(reader, logentry, typeof(J2534_filterTypes));
                    break;
                case wiredatatype.J2534_PROG_VOLTAGE_PIN_NUMBER:
                    processJ2534Enum(reader, logentry, typeof(J2534_progVoltagePinNumbers));
                    break;
                case wiredatatype.J2534_PROG_VOLTAGE:
                    processJ2534Enum(reader, logentry, typeof(J2534_programmingVolgate));
                    break;
                case wiredatatype.J2534_IOCTL:
                    processJ2534Enum(reader, logentry, typeof(J2534_IOCTLIDs));
                    break;
                case wiredatatype.J2534_CONFIG_PARAMS:
                    processJ2534Enum(reader, logentry, typeof(J2534_configParams));
                    break;

                case wiredatatype.J2534_CONNECT_FLAGS:
                    processBitfield(reader, logentry, typeof(J2534_connectFlags));
                    break;
                case wiredatatype.J2534_TXFLAGS:
                    processBitfield(reader, logentry, typeof(J2534_txFlags));
                    break;
                case wiredatatype.J2534_RXSTATUS:
                    processBitfield(reader, logentry, typeof(J2534_rxStatus));
                    break;
            }
        }

        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        //TODO: This could use better type safety or genericness
        private void processBitfield(FullBinaryReader reader, StringBuilder logentry, Type type)
        {
            UInt32 flags = (UInt32)reader.Read7BitEncodedInt();
            if(flags == 0)
            {
                logentry.Append("0");
                return;
            }

            foreach (var flag in Enum.GetValues(type))
                if ((flags & (UInt32)flag) == (UInt32)flag)
                {
                    logentry.Append(flag);
                    flags ^= (UInt32)flag;
                    if (flags != 0)
                        logentry.Append(" | ");
                }

            if (flags != 0)
                logentry.Append("0x" + flags.ToString("X"));
        }

        private void processJ2534Enum(FullBinaryReader reader, StringBuilder logentry, Type type)
        {
            UInt32 val = (UInt32)reader.Read7BitEncodedInt();
            if (Enum.IsDefined(type, val))
            {
                object a = Enum.ToObject(type, val);
                logentry.Append(a);
            }
            else
                logentry.Append("UNK(" + val.ToString("X") + "); ");
        }
    }
}
