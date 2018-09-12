using System;
using System.IO;

namespace AddressProcessing.CSV
{
    public interface ICsvReaderWriter
    {
        void Open(string fileName, Mode mode);

        void Write(params string[] columns);

        [Obsolete("Please use Read(out string column1, out string column2) instead")]
        bool Read(string column1, string column2);

        bool Read(out string column1, out string column2);
        
        bool TryRead(out string[] columns);

        void Close();
    }

    // enum moved to interface declaration file
    [Flags]
    public enum Mode
    {
        Read, Write
    }
}