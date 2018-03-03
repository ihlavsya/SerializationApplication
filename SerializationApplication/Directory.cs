using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace SerializationApplication
{
    [Serializable]
    public class Folder
    {
        public string Root;
        public List<Folder> SubFolders { get; set; }
        public List<File> Files { get; set; }
        public string Name { get; set; }

        public Folder() { }

        public Folder(string root)
        {
            Root = root;
        }

        public static IEnumerable<Folder> GetFolders(string root)
        {
            foreach (var dir in Directory.GetDirectories(root))
            {
                var dirInfo = new DirectoryInfo(dir);
                var directory = new Folder
                {
                    Name = dirInfo.Name,
                    Root = dirInfo.FullName,
                    SubFolders = GetFolders(dir).ToList(),
                    Files = GetFiles(dir).ToList()
                };
                yield return directory;
            }
        }

        public static IEnumerable<File> GetFiles(string dir)
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                var fInfo = new FileInfo(file);

                yield return new File
                {
                    Data = System.IO.File.ReadAllBytes(file),
                    Name = fInfo.Name
                };
            }
        }
    }
}