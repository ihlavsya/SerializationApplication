using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationApplication
{
    class DeserializationFileReading
    {
        public string FilePath { get; set; }
        private BinaryReader _Reader { get; set; }
        public string NewPath { get; set; }
        private string CurrentPath { get; set; }
        private string CurrentFilePath { get; set; }

        public DeserializationFileReading(string filePath,string folderPath)
        {
            NewPath = folderPath;
            FilePath = filePath;
            _Reader = new BinaryReader(System.IO.File.Open(FilePath, FileMode.Open));
            CurrentPath = NewPath;
            CurrentFilePath = null;
        }

        public void ReadFile()
        {
            while (_Reader.PeekChar() > -1)
            {
                int size = _Reader.ReadInt32();
                TypeData typeData = (TypeData)_Reader.ReadInt32();
                if ((int)typeData > 5)
                {
                    return;
                }
                string path = null;
                byte[] data = null;
                if (typeData != TypeData.FileData)
                {
                    byte[] bytePath = _Reader.ReadBytes(size);
                    path = Encoding.Default.GetString(bytePath);
                    if (typeData == TypeData.RootFolderPath)
                    {
                        CreateRootFolder(path);
                    }
                    else if (typeData == TypeData.FolderPath)
                    {
                        CreateFolder(path);
                    }
                    else
                    {
                        CreateFile(path);
                    }
                }
                else
                {
                    data = _Reader.ReadBytes(size);
                    FillFileWithData(data);
                }
            }
            _Reader.Close();
        }

        private void CreateRootFolder(string path)
        {
            var dir = new DirectoryInfo(path);
            Directory.CreateDirectory(System.IO.Path.Combine(NewPath, dir.Name));
            CurrentPath = System.IO.Path.Combine(CurrentPath, dir.Name);
        }

        private void CreateFolder(string folderName)
        {
            Directory.CreateDirectory(System.IO.Path.Combine(NewPath, folderName));
            CurrentPath = System.IO.Path.Combine(NewPath, folderName);
        }

        private void CreateFile(string path)
        {
            var fileInfo = new FileInfo(path);
            string pathString = System.IO.Path.Combine(CurrentPath, fileInfo.Name);
            if (CurrentFilePath == null)
            {
                CurrentFilePath = pathString;
            }
        }

        private void FillFileWithData(byte[] data)
        {
            using (System.IO.FileStream fs = System.IO.File.Create(CurrentFilePath))
            {
                foreach (var valueByte in data)
                {
                    fs.WriteByte(valueByte);
                }
            }
            CurrentFilePath = null;
        }
    }
}
