using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Writers
{
    public class FileWriterInit
    {
        public IndexParser parser { get; set; }
        public IndexWriter writer { get; set; }

        /// <summary>
        /// Initializer for the file parsers.
        /// </summary>
        /// <param name="args">Filepaths for every file.</param>
        /// Args[0]: index2.rif
        public FileWriterInit(string[] args)
        {
            string index2_filepath = args[0];
            FileStream file = System.IO.File.OpenRead(index2_filepath);
            byte[] fileBytes = new byte[file.Length];

            file.Read(fileBytes, 0, fileBytes.Length);
            file.Close();

            parser = new IndexParser(fileBytes);
            writer = new IndexWriter(parser);
        }
    }
}
