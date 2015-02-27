using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chunk_Tools.ZOBJ_Tools
{
    public class ObjectParser
    {
        // For all objects
        ulong objectKey { get; set; }
        ulong indexKey { get; set; }
        ulong typeStringKey { get; set; }

        // For child objects
        int entryIdentifier { get; set; }
        int entrySize { get; set; }
        int entryCount { get; set; }

        // Raw data array
        byte[] rawData { get; set; }

        // Parent or child flag
        bool isParent { get; set; }

        /// <summary>
        /// Default constructor.  This should never be hit.
        /// </summary>
        public ObjectParser()
        {

        }

        /// <summary>
        /// Constructor for passing a file path and parsing from that.
        /// May not actually be necessary.
        /// </summary>
        /// <param name="objectPath">Path to the file to read from.</param>
        public ObjectParser(string objectPath)
        {

        }

        /// <summary>
        /// Constructor for passing raw byte data and parsing from that.
        /// Likely the used constructor.
        /// </summary>
        /// <param name="objectData">Byte array of object data to parse.</param>
        /// <param name="keyList">Dictionary to check keys against to see if object is parent or child</param>
        public ObjectParser(byte[] objectData, Dictionary<ulong, string> keyList)
        {
            int index = 0;
            int temp = index;

            // Retrieve the index key
            byte[] ulongData = new byte[8];
            for (; index < temp + 8; index++)
            {
                ulongData[index - temp] = ulongData[index];
            }

            indexKey = BitConverter.ToUInt64(ulongData.Reverse().ToArray(), 0);

            // Retrieve the object key
            temp = index;
            ulongData = new byte[8];
            for (; index < temp + 8; index++)
            {
                ulongData[index - temp] = ulongData[index];
            }

            objectKey = BitConverter.ToUInt64(ulongData.Reverse().ToArray(), 0);

            // Retrieve the type string key
            temp = index;
            ulongData = new byte[8];
            for (; index < temp + 8; index++)
            {
                ulongData[index - temp] = ulongData[index];
            }

            typeStringKey = BitConverter.ToUInt64(ulongData.Reverse().ToArray(), 0);

            // Check if we are a child object or not
            if (keyList[objectKey] == null)
            {

            }
        }
    }
}
