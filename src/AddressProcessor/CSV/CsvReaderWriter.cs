using System;
using System.Text;
using AddressProcessing.FileSystem;

namespace AddressProcessing.CSV
{
    /*
        2) Refactor this class into clean, elegant, rock-SOLID & well performing code, without over-engineering.
           Assume this code is in production and backwards compatibility must be maintained.
    */

    public class CsvReaderWriter: ICsvReaderWriter, IDisposable
    {
        // Ensure dispose and close actions are idempotent
        private bool _disposed;
        // Abstract filesystem handling with IFile
        private IFile _file;

        // Move constants to field declarations
        private const int FirstColumn = 0;
        private const int SecondColumn = 1;
        private const char Separator = '\t';

        // Add constructor with stream injections for easy testing
        public CsvReaderWriter(IFile file)
        {
            _file = file;
        }

        // Empty constructor to be non-breaking
        public CsvReaderWriter()
        {
        }

        // Move implementation detail to TextFile
        public void Open(string fileName, Mode mode)
        {
            _file = new TextFile(fileName, mode);
        }

        public void Write(params string[] columns)
        {
            // Use string builder for slight optimisation
            var strBuilder = new StringBuilder();
            for (var i = 0; i < columns.Length; i++)
            {
                strBuilder.Append(columns[i]);
                if ((columns.Length - 1) != i)
                {
                    strBuilder.Append(Separator);
                }
            }

            if (strBuilder.Length > 0)
            {
                WriteLine(strBuilder.ToString());
            }
        }

        public bool Read(string column1, string column2)
        {
            // This method should not do anything as the input params are wrong
            // Putting ReadLine here to ensure exact behaviour as before. Can be removed if this behaviour is redundant
            return ReadLine() != null;
        }

        public bool Read(out string column1, out string column2)
        {
            if (TryRead(out var columns))
            {
                column1 = columns[FirstColumn];
                column2 = columns[SecondColumn];
                return true;
            }

            column1 = null;
            column2 = null;
            return false;
        }

        public bool TryRead(out string[] columns)
        {
            var line = ReadLine();

            // Also return false if file is empty to avoid potentially confusing index out of bound exception
            if (string.IsNullOrEmpty(line))
            {
                columns = null;
                return false;
            }
            
            // Removed redundant empty line checking logic

            // Removed redundant nesting
            columns = line.Split(Separator);
            
            return true;
        }

        private void WriteLine(string line)
        {
            _file.WriteLine(line);
        }

        private string ReadLine()
        {
            return _file.ReadLine();
        }

        // Dispose on close to ensure resource release
        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _file?.Dispose();
        }
    }
}
