using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chunk_Tools.STbl_Tools
{
    /// <summary>
    /// Struct to hold information about string table.
    /// </summary>
    public struct StringTable
    {
        public ulong indexKey;
        public ulong folderKey;
        public long languageIdentifier;
        public int numEntries;
        public int sizeTable;
        public int sizeLeadingUpToTable;
        public Dictionary<ulong, string> stringTable;
    }

    /// <summary>
    /// Class for parsing string tables.
    /// </summary>
    public class StringTableParser
    {
        /// <summary>
        /// Generates a key-value map of string keys to string values.
        /// </summary>
        /// <param name="chunkRawData">Raw string table.</param>
        /// <param name="index_original">Current index in the string table.</param>
        /// <param name="countEntries">Number of entries in the table.</param>
        /// <returns></returns>
        public Dictionary<ulong, string> generateDictionary(byte[] chunkRawData, int index_original, int countEntries)
        {
            int index = index_original;
            int offset = index + (countEntries * 16);
            Dictionary<ulong, string> stringTable_local = new Dictionary<ulong, string>();

            for (int q = 0; q < countEntries; q++)
            {
                // Get the string key
                ulong stringKey;
                byte[] stringKey_byte = new byte[8];
                int i = index;

                for (; index < i + 8; index++)
                {
                    stringKey_byte[index - i] = chunkRawData[index];
                }

                stringKey = BitConverter.ToUInt64(stringKey_byte.Reverse().ToArray(), 0);

                // Get the offset
                int tableOffset;
                byte[] tableOffset_byte = new byte[4];
                i = index;

                for (; index < i + 4; index++)
                {
                    tableOffset_byte[index - i] = chunkRawData[index];
                }

                tableOffset = BitConverter.ToInt32(tableOffset_byte.Reverse().ToArray(), 0);

                // Skip the next four bytes
                index += 4;

                // Get the string from the table
                int tempOffset = offset + tableOffset;
                string result = "";
                List<byte> result_byte = new List<byte>();

                while (chunkRawData[tempOffset] != 0x0)
                {
                    result_byte.Add(chunkRawData[tempOffset]);
                    tempOffset++;
                }

                result = System.Text.Encoding.Default.GetString(result_byte.ToArray());
                try
                {
                    stringTable_local.Add(stringKey, result);
                }
                catch (Exception e)
                {

                }
            }

            return stringTable_local;
        }
    }
}
