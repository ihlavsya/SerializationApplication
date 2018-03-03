using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using SerializationApplication;

namespace SerializationApplication
{
    class DeserializeProccess
    {
        public string FilePath { get; set;}
        public string FolderPath { get; set; }

        public DeserializeProccess(string filePath, string folderPath)
        {
            FilePath = filePath;
            FolderPath = folderPath;
        }

        public void ForDeserializingDirectory()
        {
            var newFolder = DeserializeFile();
            SaveCompleteFolder(newFolder, FolderPath);
        }

        private void WriteFile(string folderPath, File file)
        {
            string pathString = System.IO.Path.Combine(folderPath, file.Name);
            if (!System.IO.File.Exists(pathString))
            {
                using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                {
                    foreach (var data in file.Data)
                    {
                        fs.WriteByte(data);
                    }
                }
            }
            else
            {
                throw new Exception();
            }
        }

        private void CreateAndFillFolder(Folder directory, string path)
        {
            Directory.CreateDirectory(path);
            foreach (var file in directory.Files)
            {
                WriteFile(path, file);
            }

            foreach (var dir in directory.SubFolders)
            {
                CreateAndFillFolder(dir, System.IO.Path.Combine(path, dir.Name));
            }
        }

        private void SaveCompleteFolder(List<Folder> arr, string newPath)
        {
            foreach (var dir in arr)
            {
                string folderPath = System.IO.Path.Combine(newPath, dir.Name);
                CreateAndFillFolder(dir, folderPath);
            }
        }
        private List<Folder> DeserializeFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            List<Folder> newFolder;
            System.IO.File.Exists(FilePath);
            using (System.IO.FileStream fs = new System.IO.FileStream("defaultserialization.dat", System.IO.FileMode.OpenOrCreate))
            {
                newFolder = (List<Folder>)formatter.Deserialize(fs);
            }
            return newFolder;
        }
    }
}
