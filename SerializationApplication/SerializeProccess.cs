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
    class SerializeProccess
    {
        public string Root;
        public string WritePath;
        public SerializeProccess(string writePath,string root)
        {
            WritePath = System.IO.Path.Combine(writePath, "defaultserialization.dat");
            Root = root;
        }
        private void SerializeDirectory(List<Folder> folder)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (System.IO.FileStream fs = new System.IO.FileStream(WritePath, System.IO.FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, folder);
            }
        }

        public void ForSerializingDirectory()
        {
            var folder = new List<Folder>();
            var dirInfo = new DirectoryInfo(Root);
            var dir = new Folder
            {
                Name = dirInfo.Name,
                SubFolders = Folder.GetFolders(Root).ToList(),
                Files = Folder.GetFiles(Root).ToList()
            };
            folder.Add(dir);
            SerializeDirectory(folder);
        }

    }
}
