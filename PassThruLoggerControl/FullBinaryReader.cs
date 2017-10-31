using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PassThruLoggerControl
{
    public class FullBinaryReader : BinaryReader
    {
        public FullBinaryReader(Stream stream) : base(stream) { }
        public FullBinaryReader(Stream stream, Encoding encoding) : base(stream, encoding) { }
        public new int Read7BitEncodedInt()
        {
            return base.Read7BitEncodedInt();
        }
    }
}
