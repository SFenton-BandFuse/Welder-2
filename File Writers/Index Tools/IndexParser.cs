using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chunk_Tools.ZOBJ_Tools;
using Chunk_Tools.STbl_Tools;
using System.IO;

namespace File_Writers
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
    /// Struct to hold information about index JBOZ object.
    /// </summary>
    public struct IndexObject
    {
        public ulong indexKey;
        public ulong objectKey;
        public ulong typeStringKey;
        public int dlcNum;
        public int numEntries;
        public List<IndexEntry> entryList;
    }

    /// <summary>
    /// Holds info about each entry in the object.
    /// </summary>
    public struct IndexEntry
    {
        public ulong tagKey;
        public string tagKey_string;
        public ulong regularKey;
        public string regularKey_string;
        public int value;
        public int offset;
        public ulong stringKey;
        public string stringKey_string;
        public string filepath;
    }

    /// <summary>
    /// This class parses the index2.rif file found in DLC.
    /// </summary>
    public class IndexParser
    {
        // All objects found in the index2.rif file
        public IndexObject zobject;
        public StringTable engParser;
        public StringTable japParser;
        public StringTable freParser;
        public StringTable gerParser;
        public StringTable espParser;
        public StringTable itaParser;
        public StringTableParser parser;
       
        /// <summary>
        /// Constructor that will take in an array of bytes representing an index file.
        /// </summary>
        /// <param name="indexBytes">Array of bytes representing the index file.</param>
        public IndexParser(byte[] indexBytes)
        {
            parser = new StringTableParser();
            stblParser(indexBytes);
            zobjParser(indexBytes);
        }

        /// <summary>
        /// Parses the object table.
        /// </summary>
        /// <param name="indexBytes">The index file to read.</param>
        private void zobjParser(byte[] indexBytes)
        {
            int index = 8;
            while (index < indexBytes.Length)
            {
                string chunkType = "";
                byte[] chunkType_byte = new byte[4];
                byte[] chunkSize = new byte[4];
                int chunkSize_int;

                // Get the chunk type
                int i = index;
                for (; index < i + 4; index++)
                {
                    chunkType_byte[index - i] = indexBytes[index];
                }

                chunkType = System.Text.Encoding.Default.GetString(chunkType_byte.Reverse().ToArray());

                // Get the chunk size
                i = index;
                for (; index < i + 4; index++)
                {
                    chunkSize[index - i] = indexBytes[index];
                }

                chunkSize_int = BitConverter.ToInt32(chunkSize.Reverse().ToArray(), 0);

                // Get the raw data
                byte[] chunkRawData = new byte[chunkSize_int];
                i = index;

                for (; index < i + chunkSize_int; index++)
                {
                    chunkRawData[index - i] = indexBytes[index];
                }

                switch (chunkType)
                {
                    case "ZOBJ":
                        zobject = new IndexObject();
                        zobjHelper(chunkRawData);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Helps generate object information
        /// </summary>
        /// <param name="chunkRawData">Raw object data to read</param>
        private void zobjHelper(byte[] chunkRawData)
        {
            // Get the index key
            int index = 0;
            ulong indexKey;
            byte[] indexKey_byte = new byte[8];

            int i = index;
            for (; index < i + 8; index++)
            {
                indexKey_byte[index - i] = chunkRawData[index];
            }

            indexKey = BitConverter.ToUInt64(indexKey_byte.Reverse().ToArray(), 0);

            // Get the object key
            ulong objectKey;
            byte[] objectKey_byte = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                objectKey_byte[index - i] = chunkRawData[index];
            }

            objectKey = BitConverter.ToUInt64(objectKey_byte.Reverse().ToArray(), 0);

            // Get the type string key
            ulong typeStringKey;
            byte[] typeStringKey_byte = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                typeStringKey_byte[index - i] = chunkRawData[index];
            }

            typeStringKey = BitConverter.ToUInt64(typeStringKey_byte.Reverse().ToArray(), 0);

            zobject.indexKey = indexKey;
            zobject.objectKey = objectKey;
            zobject.typeStringKey = typeStringKey;

            // Skip zeroed data
            index += 8;

            // Get the dlcNum
            int dlcNum;
            byte[] dlcNum_byte = new byte[4];
            i = index;

            for (; index < i + 4; index++)
            {
                dlcNum_byte[index - i] = chunkRawData[index];
            }

            dlcNum = BitConverter.ToInt32(dlcNum_byte.Reverse().ToArray(), 0);

            zobject.dlcNum = dlcNum;

            // Get the number of entries to read
            int numEntries;
            byte[] numEntries_byte = new byte[4];
            i = index;

            for (; index < i + 4; index++)
            {
                numEntries_byte[index - i] = chunkRawData[index];
            }

            numEntries = BitConverter.ToInt32(numEntries_byte.Reverse().ToArray(), 0);
            zobject.numEntries = numEntries;

            // Skip constant 0x00000004
            index += 4;

            zobject.entryList = generateEntries(chunkRawData, index, numEntries);
        }

        /// <summary>
        /// Helper to generate a list of entries for the object.
        /// </summary>
        /// <param name="chunkRawData">Raw data to read from</param>
        /// <param name="index_original">Original index</param>
        /// <param name="numEntries">Number of entries to read</param>
        /// <returns></returns>
        private List<IndexEntry> generateEntries(byte[] chunkRawData, int index_original, int numEntries)
        {
            List<IndexEntry> entries = new List<IndexEntry>();
            int index = index_original;

            for (int q = 0; q < numEntries; q++ )
            {
                IndexEntry entry = new IndexEntry();

                // Read the tag key
                ulong tagKey;
                byte[] tagKey_byte = new byte[8];
                int i = index;

                for (; index < i + 8; index++)
                {
                    tagKey_byte[index - i] = chunkRawData[index];
                }

                tagKey = BitConverter.ToUInt64(tagKey_byte.Reverse().ToArray(), 0);
                entry.tagKey = tagKey;
                entry.tagKey_string = engParser.stringTable[tagKey];

                // Read the second key
                ulong regularKey;
                byte[] regularKey_byte = new byte[8];
                i = index;

                for (; index < i + 8; index++)
                {
                    regularKey_byte[index - i] = chunkRawData[index];
                }

                regularKey = BitConverter.ToUInt64(regularKey_byte.Reverse().ToArray(), 0);
                entry.regularKey = regularKey;
                entry.regularKey_string = engParser.stringTable[regularKey];

                // Ignore the decimal?
                index += 4;

                // Get the offset
                int offsetData;
                byte[] offsetData_byte = new byte[4];
                i = index;

                for (; index < i + 4; index++)
                {
                    offsetData_byte[index - i] = chunkRawData[index];
                }

                offsetData = BitConverter.ToInt32(offsetData_byte.Reverse().ToArray(), 0);
                entry.offset = offsetData;

                // Get the entry string key
                int tempIndex = index + offsetData - 4;
                ulong entryKey;
                byte[] entryKey_byte = new byte[8];
                i = tempIndex;

                for (; tempIndex < i + 8; tempIndex++)
                {
                    entryKey_byte[tempIndex - i] = chunkRawData[tempIndex];
                }

                entryKey = BitConverter.ToUInt64(entryKey_byte.Reverse().ToArray(), 0);
                entry.stringKey = entryKey;
                entry.stringKey_string = engParser.stringTable[entryKey];

                // Get the filepath
                string filepath = "";
                List<byte> filepath_byte = new List<byte>();

                while (chunkRawData[tempIndex] != 0x00)
                {
                    filepath_byte.Add(chunkRawData[tempIndex]);
                    tempIndex++;
                }

                filepath = System.Text.Encoding.Default.GetString(filepath_byte.ToArray());
                entry.filepath = filepath;

                entries.Add(entry);
            }


            return entries;
        }

        /// <summary>
        /// Parses all the string tables first.
        /// </summary>
        /// <param name="indexBytes">File to read.</param>
        private void stblParser(byte[] indexBytes)
        {
            int index = 8;
            while (index < indexBytes.Length)
            {
                string chunkType = "";
                byte[] chunkType_byte = new byte[4];
                byte[] chunkSize = new byte[4];
                int chunkSize_int;

                // Get the chunk type
                int i = index;
                for (; index < i + 4; index++)
                {
                    chunkType_byte[index - i] = indexBytes[index];
                }

                chunkType = System.Text.Encoding.Default.GetString(chunkType_byte.Reverse().ToArray());

                // Get the chunk size
                i = index;
                for (; index < i + 4; index++)
                {
                    chunkSize[index - i] = indexBytes[index];
                }

                chunkSize_int = BitConverter.ToInt32(chunkSize.Reverse().ToArray(), 0);

                // Get the raw data
                byte[] chunkRawData = new byte[chunkSize_int];
                i = index;

                for (; index < i + chunkSize_int; index++)
                {
                    chunkRawData[index - i] = indexBytes[index];
                }

                // TODO: Individual action per chunk
                switch (chunkType)
                {
                    case "STbl":
                        stblHelper(chunkRawData);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Helper class to take care of string table chunks
        /// </summary>
        /// <param name="chunkRawData">Raw data to analyze</param>
        private void stblHelper(byte[] chunkRawData)
        {
            // Get the index key
            int index = 0;
            ulong indexKey;
            byte[] indexKey_byte = new byte[8];

            int i = index;
            for (; index < i + 8; index++)
            {
                indexKey_byte[index - i] = chunkRawData[index];
            }

            indexKey = BitConverter.ToUInt64(indexKey_byte.Reverse().ToArray(), 0);

            // Get the folder key
            ulong folderKey;
            byte[] folderKey_byte = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                folderKey_byte[index - i] = chunkRawData[index];
            }

            folderKey = BitConverter.ToUInt64(folderKey_byte.Reverse().ToArray(), 0);

            // Get the language identifier
            long language;
            byte[] stblIdentifier = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                stblIdentifier[index - i] = chunkRawData[index];
            }

            language = BitConverter.ToInt64(stblIdentifier.Reverse().ToArray(), 0);

            // Skip zeroed data
            index += 8;

            // Get the count of entries
            int countEntries;
            byte[] countEntries_byte = new byte[4];
            i = index;

            for (; index < i + 4; index++ )
            {
                countEntries_byte[index - i] = chunkRawData[index];
            }

            countEntries = BitConverter.ToInt32(countEntries_byte.Reverse().ToArray(), 0);

            // Skip over data that is always 12
            index += 4;

            // Get the size of the string table
            int tableSize;
            byte[] tableSize_byte = new byte[4];
            i = index;

            for (; index < i + 4; index++ )
            {
                tableSize_byte[index - i] = chunkRawData[index];
            }

            tableSize = BitConverter.ToInt32(tableSize_byte.Reverse().ToArray(), 0);

            // Get the size of data leading up to the table
            int sizeKeys;
            byte[] sizeKeys_byte = new byte[4];
            i = index;

            for (; index < i + 4; index++ )
            {
                sizeKeys_byte[index - i] = chunkRawData[index];
            }

            sizeKeys = BitConverter.ToInt32(sizeKeys_byte.Reverse().ToArray(), 0);

            // Switch based on language
            switch (language)
            {
                case 5412069155413958780:
                    engParser = new StringTable();
                    engParser.folderKey = folderKey;
                    engParser.indexKey = indexKey;
                    engParser.languageIdentifier = language;
                    engParser.sizeTable = tableSize;
                    engParser.sizeLeadingUpToTable = sizeKeys;
                    engParser.stringTable = parser.generateDictionary(chunkRawData, index, countEntries);
                    engParser.numEntries = engParser.stringTable.Count;
                    break;
                case -4916594395764136780:
                    japParser = new StringTable();
                    japParser.folderKey = folderKey;
                    japParser.indexKey = indexKey;
                    japParser.languageIdentifier = language;
                    japParser.sizeTable = tableSize;
                    japParser.sizeLeadingUpToTable = sizeKeys;
                    japParser.stringTable = parser.generateDictionary(chunkRawData, index, countEntries);
                    japParser.numEntries = japParser.stringTable.Count;
                    break;
                case 8434362063832740322:
                    gerParser = new StringTable();
                    gerParser.folderKey = folderKey;
                    gerParser.indexKey = indexKey;
                    gerParser.languageIdentifier = language;
                    gerParser.sizeTable = tableSize;
                    gerParser.sizeLeadingUpToTable = sizeKeys;
                    gerParser.stringTable = parser.generateDictionary(chunkRawData, index, countEntries);
                    gerParser.numEntries = gerParser.stringTable.Count;
                    break;
                case 4181558474080832064:
                    itaParser = new StringTable();
                    itaParser.folderKey = folderKey;
                    itaParser.indexKey = indexKey;
                    itaParser.languageIdentifier = language;
                    itaParser.sizeTable = tableSize;
                    itaParser.sizeLeadingUpToTable = sizeKeys;
                    itaParser.stringTable = parser.generateDictionary(chunkRawData, index, countEntries);
                    itaParser.numEntries = itaParser.stringTable.Count;
                    break;
                case -1868168087102288302:
                    espParser = new StringTable();
                    espParser.folderKey = folderKey;
                    espParser.indexKey = indexKey;
                    espParser.languageIdentifier = language;
                    espParser.sizeTable = tableSize;
                    espParser.sizeLeadingUpToTable = sizeKeys;
                    espParser.stringTable = parser.generateDictionary(chunkRawData, index, countEntries);
                    espParser.numEntries = espParser.stringTable.Count;
                    break;
                case 6388165613802289312:
                    freParser = new StringTable();
                    freParser.folderKey = folderKey;
                    freParser.indexKey = indexKey;
                    freParser.languageIdentifier = language;
                    freParser.sizeTable = tableSize;
                    freParser.sizeLeadingUpToTable = sizeKeys;
                    freParser.stringTable = parser.generateDictionary(chunkRawData, index, countEntries);
                    freParser.numEntries = freParser.stringTable.Count;
                    break;
                default:
                    break;
            }
        }
    }
}
