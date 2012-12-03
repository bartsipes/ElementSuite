using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementSuite.Core.Service
{
    public class FileService : IFileService
    {
        IsolatedStorageFile _isoFile;

        public FileService()
        {
            _isoFile = IsolatedStorageFile.GetUserStoreForDomain();
        }

        public FileStream OpenFile(string path)
        {
            return new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, _isoFile);
        }

        public void DeleteFile(string path)
        {
            if (_isoFile.FileExists(path))
                _isoFile.DeleteFile(path);
        }

        public void CreateDirectory(string dir)
        {
            _isoFile.CreateDirectory(dir);
        }

        public void DeleteDirectory(string dir)
        {
            if (_isoFile.DirectoryExists(dir))
                _isoFile.DeleteDirectory(dir);
        }
    }
}
