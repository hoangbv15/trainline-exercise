using System;

namespace AddressProcessing.FileSystem
{
    public interface IFile: IDisposable
    {
        void WriteLine(string line);

        string ReadLine();
    }
}