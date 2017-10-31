using Microsoft.Win32;

namespace PassThruLoggerControl
{
    class J2534Driver
    {
        private string name;
        private string path;
        private bool valid;
        private string _key;

        public string key { get { return _key; } }

        public J2534Driver(string keyname, RegistryKey driverentry)
        {
            valid = driverentry.GetValueKind("FunctionLibrary") == RegistryValueKind.String;
            _key = keyname;// driverentry.Name;

            if (valid)
                path = (string)driverentry.GetValue("FunctionLibrary");

            name = keyname;

            if (driverentry.GetValueKind("Vendor") == RegistryValueKind.String &&
                driverentry.GetValueKind("Name") == RegistryValueKind.String)
            {
                string vendorName = (string)driverentry.GetValue("Vendor");
                string productName = (string)driverentry.GetValue("Name");
                if (vendorName != null && productName != null)
                    name = productName + " {" + vendorName + "}";
            }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
