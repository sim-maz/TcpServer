using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace TcpServer
{
    class FileExplorer
    {
        public List<string> Files { get; set; }
        public List<string> Subdirectories { get; set; }
        public byte[] FilesSerialized { get; set; }

        public FileExplorer(string path)
        {
            GetFolderContents(path);
        }

        private void GetFolderContents(string path)
        {
            var files = Directory.GetFiles(path);
            if (files.Length > 0)
                Files = files.ToList();

            FilesSerialized = SerializeData(Files);
        }

        private byte[] SerializeData(List<string> list)
        {
            var binFormatter = new BinaryFormatter();
            var mStream = new MemoryStream();
            binFormatter.Serialize(mStream, list);
            return mStream.ToArray();
        }
    }
}
