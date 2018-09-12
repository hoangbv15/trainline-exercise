using System;
using System.IO;
using AddressProcessing.FileSystem;

namespace AddressProcessing.CSV
{
    public class TextFile: IFile
    {
        // Renamed for more readability
        private readonly StreamReader _reader;
        private readonly StreamWriter _writer;
        // Ensure dispose and close actions are idempotent
        private bool _disposed;
        
        public TextFile(string fileName, Mode mode)
        {
            // Change if to switch block for readability
            switch (mode)
            {
                case Mode.Read:
                    _reader = File.OpenText(fileName);
                    break;
                case Mode.Write:
                    var fileInfo = new FileInfo(fileName);
                    _writer = fileInfo.CreateText();
                    break;
                default:
                    throw new Exception("Unknown file mode for " + fileName);
            }
        }
        
        public void WriteLine(string line)
        {
            _writer.WriteLine(line);
        }

        public string ReadLine()
        {
            return _reader.ReadLine();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            _writer?.Flush();
            _reader?.Dispose();
            _writer?.Dispose();
        }
    }
}