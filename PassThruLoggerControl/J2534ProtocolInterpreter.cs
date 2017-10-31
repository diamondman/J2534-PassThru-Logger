using System;
using System.IO;

namespace PassThruLoggerControl
{
    public abstract class J2534ProtocolInterpreter
    {
        public abstract string J2534Version { get; }

        protected void checkEnum(Type type, object thing)
        {
            if (!Enum.IsDefined(type, thing))
            {
                throw new InvalidEnumException(String.Format("Got invalid enum value for {0}: {1}", type.Name, thing));
            }
        }

        public abstract string interpret(FullBinaryReader reader);
    }
}
