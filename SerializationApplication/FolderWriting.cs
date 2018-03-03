using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerializationApplication
{
    public enum TypeData
    {
        FolderPath=1,
        FilePath=2,
        FileData=3,
        RootFolderPath=4
    }

    class FolderWriting
    {
        public BinaryWriter binaryWriter;
        private string _writePath;
        private string _originPath;

        public FolderWriting(string writePath, string originPath)
        {
            _writePath = System.IO.Path.Combine(writePath, "customserialization.dat");
            binaryWriter = new BinaryWriter(System.IO.File.Open(_writePath, FileMode.OpenOrCreate));
            _originPath = originPath;
        }

        public void StartWriting()
        {
            WriteFolderContent(_originPath);
            binaryWriter.Close();
        }

        private void WriteFolderContent(string path)
        {
            WriteFolderFiles(path);
            string[] subFolders = Directory.GetDirectories(path);
            if (subFolders.Length == 0)
            {
                return;
            }
            foreach (var folder in subFolders)
            {
                var dir = new DirectoryInfo(folder);
                WriteFolderContent(folder);
            }
        }

        private void WriteFolderFiles(string path)
        {
            string namePath = "";
            if (path!=_originPath)
            {
                var dir = new DirectoryInfo(_originPath);
                string diff = dir.Parent.FullName;
                namePath = path.Substring(diff.Length+1,path.Length-diff.Length-1);
                WriteText(namePath, TypeData.FolderPath);
            }
            else
            {
                WriteText(path, TypeData.RootFolderPath);
            }
            
            string[] files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return;
            }
            foreach (var file in files)
            {
                var fInfo = new FileInfo(file);
                WriteText(fInfo.Name, TypeData.FilePath);
                byte[] data = System.IO.File.ReadAllBytes(file);
                WriteData(data, TypeData.FileData);
            }
        }

        private void WriteData(byte[] data, TypeData type)
        {
            int size=Convert.ToInt32(data.Length);
            binaryWriter.Write(size);

            int typeData = Convert.ToInt32(type);
            binaryWriter.Write(typeData);

            binaryWriter.Write(data, 0, data.Length);
        }

        private void WriteText(string text, TypeData type)
        {
            byte[] input = Encoding.Default.GetBytes(text);
            WriteData(input,type);
        }
    }
}
