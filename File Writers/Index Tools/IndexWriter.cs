using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Writers
{
    /// <summary>
    /// Class to write out the index2.rif file.
    /// </summary>
    public class IndexWriter
    {
        // Declaration of tables and index object
        private StringTable engTable;
        private StringTable japTable;
        private StringTable itaTable;
        private StringTable freTable;
        private StringTable espTable;
        private StringTable gerTable;
        private IndexObject indexObject;

        // Declare byte lists to store keys and offset representation
        private List<byte> engTable_keys_byte;
        private List<byte> japTable_keys_byte;
        private List<byte> itaTable_keys_byte;
        private List<byte> freTable_keys_byte;
        private List<byte> espTable_keys_byte;
        private List<byte> gerTable_keys_byte;
        
        // Declare byte lists to store string representation
        private List<byte> engTable_strings_byte;
        private List<byte> japTable_strings_byte;
        private List<byte> itaTable_strings_byte;
        private List<byte> freTable_strings_byte;
        private List<byte> espTable_strings_byte;
        private List<byte> gerTable_strings_byte;

        // List to store sections
        private List<byte> engTable_byte;
        private List<byte> japTable_byte;
        private List<byte> itaTable_byte;
        private List<byte> freTable_byte;
        private List<byte> espTable_byte;
        private List<byte> gerTable_byte;

        // List to store object
        private List<byte> zobject_final;

        // List to store index
        private List<byte> index_byte;

        /// <summary>
        /// Constructor for the class.  Sets the in-class parser information.
        /// </summary>
        /// <param name="parser"></param>
        public IndexWriter(IndexParser parser)
        {
            engTable = parser.engParser;
            freTable = parser.freParser;
            japTable = parser.japParser;
            gerTable = parser.gerParser;
            itaTable = parser.itaParser;
            espTable = parser.espParser;
            indexObject = parser.zobject;

            // Initialize all lists.
            engTable_keys_byte = new List<byte>();
            japTable_keys_byte = new List<byte>();
            itaTable_keys_byte = new List<byte>();
            freTable_keys_byte = new List<byte>();
            espTable_keys_byte = new List<byte>();
            gerTable_keys_byte = new List<byte>();

            engTable_strings_byte = new List<byte>();
            japTable_strings_byte = new List<byte>();
            itaTable_strings_byte = new List<byte>();
            freTable_strings_byte = new List<byte>();
            espTable_strings_byte = new List<byte>();
            gerTable_strings_byte = new List<byte>();

            index_byte = new List<byte>();

            // Write out all string tables.
            engTable_byte = WriteTable(engTable, 0);
            freTable_byte = WriteTable(freTable, 1);
            japTable_byte = WriteTable(japTable, 2);
            gerTable_byte = WriteTable(gerTable, 3);
            itaTable_byte = WriteTable(itaTable, 4);
            espTable_byte = WriteTable(espTable, 5);

            // Write out the ZOBJ section.
            zobject_final = WriteObject(indexObject);

            // Write the index section of the file.
            WriteIndex();

            // Write the final goddamn file
            WriteFile();
        }

        /// <summary>
        /// Write the final goddamn index file holy balls
        /// </summary>
        private void WriteFile()
        {
            List<byte> finalFile = new List<byte>();

            // Write out RIFF chunk.
            finalFile.Add(BitConverter.GetBytes('F')[0]);
            finalFile.Add(BitConverter.GetBytes('F')[0]);
            finalFile.Add(BitConverter.GetBytes('I')[0]);
            finalFile.Add(BitConverter.GetBytes('R')[0]);

            // Get the size of all elements
            int totalSize = index_byte.Count + engTable_byte.Count + freTable_byte.Count +
                japTable_byte.Count + itaTable_byte.Count + gerTable_byte.Count +
                espTable_byte.Count + zobject_final.Count;
            byte[] totalSize_byte = BitConverter.GetBytes(totalSize).Reverse().ToArray();
            for (int i = 0; i < totalSize_byte.Length; i++)
            {
                finalFile.Add(totalSize_byte[i]);
            }

            // Add the index chunk
            foreach (byte entry in index_byte)
            {
                finalFile.Add(entry);
            }

            // Add the english entry
            foreach (byte entry in engTable_byte)
            {
                finalFile.Add(entry);
            }

            // Add the french entry
            foreach (byte entry in freTable_byte)
            {
                finalFile.Add(entry);
            }

            // Add the Japanese entry
            foreach (byte entry in japTable_byte)
            {
                finalFile.Add(entry);
            }

            // Add the Italian entry
            foreach (byte entry in itaTable_byte)
            {
                finalFile.Add(entry);
            }

            // Add the German entry
            foreach (byte entry in gerTable_byte)
            {
                finalFile.Add(entry);
            }

            // Add the Spanish entry
            foreach (byte entry in espTable_byte)
            {
                finalFile.Add(entry);
            }

            // Add the object
            foreach (byte entry in zobject_final)
            {
                finalFile.Add(entry);
            }

            File.WriteAllBytes(/* Path to write index2.rif to */, finalFile.ToArray());
        }

        /// <summary>
        /// Writes out the index section of the file.
        /// </summary>
        private void WriteIndex()
        {
            // Write the header
            index_byte.Add(BitConverter.GetBytes('X')[0]);
            index_byte.Add(BitConverter.GetBytes('D')[0]);
            index_byte.Add(BitConverter.GetBytes('N')[0]);
            index_byte.Add(BitConverter.GetBytes('I')[0]);

            // Write the size for this index file
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x78);

            // Add the number of entries
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x7);

            // Add the constant
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x4);

            // Add information about the english string table
            byte[] indexKey_byte = BitConverter.GetBytes(engTable.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            byte[] offset_byte = BitConverter.GetBytes(136).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }

            // Add information about the french string table
            indexKey_byte = BitConverter.GetBytes(freTable.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            offset_byte = BitConverter.GetBytes(136 + engTable_byte.Count).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }

            // Add information about the Japanese string table
            indexKey_byte = BitConverter.GetBytes(japTable.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            offset_byte = BitConverter.GetBytes(136 + engTable_byte.Count +
                freTable_byte.Count).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }

            // Add information about the Italian string table
            indexKey_byte = BitConverter.GetBytes(itaTable.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            offset_byte = BitConverter.GetBytes(136 + engTable_byte.Count +
                freTable_byte.Count + japTable_byte.Count).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }

            // Add information about the German string table
            indexKey_byte = BitConverter.GetBytes(gerTable.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            offset_byte = BitConverter.GetBytes(136 + engTable_byte.Count +
                freTable_byte.Count + japTable_byte.Count + itaTable_byte.Count).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }

            // Add information about the Spanish string table
            indexKey_byte = BitConverter.GetBytes(espTable.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            offset_byte = BitConverter.GetBytes(136 + engTable_byte.Count +
                freTable_byte.Count + japTable_byte.Count + itaTable_byte.Count +
                gerTable_byte.Count).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }

            // Add information about the object
            indexKey_byte = BitConverter.GetBytes(indexObject.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                index_byte.Add(indexKey_byte[i]);
            }

            offset_byte = BitConverter.GetBytes(136 + engTable_byte.Count +
                freTable_byte.Count + japTable_byte.Count + itaTable_byte.Count +
                gerTable_byte.Count + espTable_byte.Count).Reverse().ToArray();
            for (int i = 0; i < offset_byte.Length; i++)
            {
                index_byte.Add(offset_byte[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                index_byte.Add(0x0);
            }
        }

        /// <summary>
        /// Writes out the object section of the index file.
        /// </summary>
        /// <param name="indexObject">The struct representation of the object.</param>
        private List<byte> WriteObject(IndexObject indexObject)
        {
            List<byte> zobject_byte = new List<byte>();
            List<byte> zobject_24entry = new List<byte>();
            List<byte> zobject_filepath = new List<byte>();
            
            // Add header bytes
            zobject_byte.Add(BitConverter.GetBytes('J')[0]);
            zobject_byte.Add(BitConverter.GetBytes('B')[0]);
            zobject_byte.Add(BitConverter.GetBytes('O')[0]);
            zobject_byte.Add(BitConverter.GetBytes('Z')[0]);

            // Add zeroed data, fill it later
            for (int i = 0; i < 4; i++)
            {
                zobject_byte.Add(0x0);
            }

            // Add the index key
            byte[] indexKey_byte = BitConverter.GetBytes(indexObject.indexKey).Reverse().ToArray();
            for (int i = 0; i < indexKey_byte.Length; i++)
            {
                zobject_byte.Add(indexKey_byte[i]);
            }

            // Add the object key
            byte[] objectKey_byte = BitConverter.GetBytes(indexObject.objectKey).Reverse().ToArray();
            for (int i = 0; i < objectKey_byte.Length; i++)
            {
                zobject_byte.Add(objectKey_byte[i]);
            }

            // Add the type string key
            byte[] typeStringKey_byte = BitConverter.GetBytes(indexObject.typeStringKey).Reverse().ToArray();
            for (int i = 0; i < typeStringKey_byte.Length; i++)
            {
                zobject_byte.Add(typeStringKey_byte[i]);
            }

            // Add zeroed data
            for (int i = 0; i < 8; i++)
            {
                zobject_byte.Add(0x0);
            }

            // Add the DLC Number
            byte[] dlcNum_byte = BitConverter.GetBytes(indexObject.dlcNum).Reverse().ToArray();
            for (int i = 0; i < dlcNum_byte.Length; i++)
            {
                zobject_byte.Add(dlcNum_byte[i]);
            }

            // Add the number of entries to write
            byte[] numEntries_byte = BitConverter.GetBytes(indexObject.numEntries).Reverse().ToArray();
            for (int i = 0; i < numEntries_byte.Length; i++)
            {
                zobject_byte.Add(numEntries_byte[i]);
            }

            // Add the constant value
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 3:
                        zobject_byte.Add(0x4);
                        break;
                    default:
                        zobject_byte.Add(0x0);
                        break;
                }
            }

            // Add all entries.
            int counter = indexObject.numEntries - 1;
            int entryNum = 0;
            foreach (IndexEntry entry in indexObject.entryList)
            {
                // Write tag
                byte[] tagKey_byte = BitConverter.GetBytes(entry.tagKey).Reverse().ToArray();
                for (int i = 0; i < tagKey_byte.Length; i++)
                {
                    zobject_24entry.Add(tagKey_byte[i]);
                }

                // Write second key
                byte[] regularKey_byte = BitConverter.GetBytes(entry.regularKey).Reverse().ToArray();
                for (int i = 0; i < regularKey_byte.Length; i++)
                {
                    zobject_24entry.Add(regularKey_byte[i]);
                }

                // Add decimal
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 3:
                            if (counter == 0)
                                zobject_24entry.Add(0x2);
                            else
                                zobject_24entry.Add(0x1);
                            break;
                        default:
                            zobject_24entry.Add(0x0);
                            break;
                    }
                }

                // Calculate offset
                int offset = (counter * 24) + (entryNum * 248) + 4;

                // Write offset
                byte[] offset_byte = BitConverter.GetBytes(offset).Reverse().ToArray();
                for (int i = 0; i < offset_byte.Length; i++)
                {
                    zobject_24entry.Add(offset_byte[i]);
                }

                // Write data to entry list
                byte[] stringKey_byte = BitConverter.GetBytes(entry.stringKey).Reverse().ToArray();
                for (int i = 0; i < stringKey_byte.Length; i++)
                {
                    zobject_filepath.Add(stringKey_byte[i]);
                }

                // Write filepath to entry list
                byte[] filePath_byte = System.Text.Encoding.Unicode.GetBytes(entry.filepath);
                for (int i = 0; i < filePath_byte.Length; i += 2)
                {
                    zobject_filepath.Add(filePath_byte[i]);
                }

                // Write zeroed data
                for (int i = 0; i < (248 - entry.filepath.Length - 8); i++)
                {
                    zobject_filepath.Add(0x0);
                }

                counter--;
            }

            // Concatenate the lists
            foreach (byte entry in zobject_24entry)
            {
                zobject_byte.Add(entry);
            }

            foreach (byte entry in zobject_filepath)
            {
                zobject_byte.Add(entry);
            }

            byte[] realSize = BitConverter.GetBytes(zobject_byte.Count - 8).Reverse().ToArray();
            zobject_byte[4] = realSize[0];
            zobject_byte[5] = realSize[1];
            zobject_byte[6] = realSize[2];
            zobject_byte[7] = realSize[3];

            return zobject_byte;
        }

        /// <summary>
        /// Writes out a string table object.
        /// </summary>
        /// <param name="table">The table to write out.</param>
        /// <param name="language">The language we are writing.</param>
        private List<byte> WriteTable(StringTable table, int language)
        {
            int size = 40;
            List<byte> tableOut = new List<byte>();
            byte[] indexKey = BitConverter.GetBytes(table.indexKey).Reverse().ToArray();
            byte[] folderKey = BitConverter.GetBytes(table.folderKey).Reverse().ToArray();
            byte[] languageIdentifier;
            switch (language)
            {
                case 0:
                    languageIdentifier = BitConverter.GetBytes(5412069155413958780)
                        .Reverse().ToArray();
                    break;
                case 1:
                    languageIdentifier = BitConverter.GetBytes(6388165613802289312)
                        .Reverse().ToArray();
                    break;
                case 2:
                    languageIdentifier = BitConverter.GetBytes(-4916594395764136780)
                        .Reverse().ToArray();
                    break;
                case 3:
                    languageIdentifier = BitConverter.GetBytes(8434362063832740322)
                        .Reverse().ToArray();
                    break;
                case 4:
                    languageIdentifier = BitConverter.GetBytes(4181558474080832064)
                        .Reverse().ToArray();
                    break;
                case 5:
                    languageIdentifier = BitConverter.GetBytes(-1868168087102288302)
                        .Reverse().ToArray();
                    break;
                default:
                    languageIdentifier = new byte[0];
                    break;
            }

            byte[] countEntries = BitConverter.GetBytes(table.numEntries).Reverse().ToArray();

            // Add chunk title
            tableOut.Add(BitConverter.GetBytes('l')[0]);
            tableOut.Add(BitConverter.GetBytes('b')[0]);
            tableOut.Add(BitConverter.GetBytes('T')[0]);
            tableOut.Add(BitConverter.GetBytes('S')[0]);

            // Add size
            byte[] sizeConvert = BitConverter.GetBytes(size).Reverse().ToArray();
            for (int i = 0; i < sizeConvert.Length; i++)
            {
                tableOut.Add(sizeConvert[i]);
            }

            // Add index key
            for (int i = 0; i < indexKey.Length; i++)
            {
                tableOut.Add(indexKey[i]);
            }

            // Add folder key
            for (int i = 0; i < folderKey.Length; i++)
            {
                tableOut.Add(folderKey[i]);
            }

            // Add language identifier
            for (int i = 0; i < languageIdentifier.Length; i++)
            {
                tableOut.Add(languageIdentifier[i]);
            }

            // Add zeroed data
            for (int i = 0; i < 8; i++)
            {
                tableOut.Add(0x0);
            }

            // Add count of entries
            for (int i = 0; i < countEntries.Length; i++)
            {
                tableOut.Add(countEntries[i]);
            }

            // Add 12
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 3:
                        tableOut.Add(0xc);
                        break;
                    default:
                        tableOut.Add(0x0);
                        break;
                }
            }

            // Calculate the number of entries
            int dataLeadingToTable = (table.numEntries * 16) + 4;

            // Begin putting data into key and string tables
            int tableOffset = 0;
             
            foreach(KeyValuePair<ulong, string> pair in table.stringTable)
            {
                byte[] stringKey = BitConverter.GetBytes(pair.Key).Reverse().ToArray();

                AddKey(tableOffset, stringKey, language);
                tableOffset = AddValue(tableOffset, pair.Value, language);
            }

            // Get the size of the string table
            int sizeStringTable = 0;
            switch (language)
            {
                case 0:
                    sizeStringTable = engTable_strings_byte.Count;
                    dataLeadingToTable = engTable_keys_byte.Count + 4;
                    break;
                case 1:
                    sizeStringTable = freTable_strings_byte.Count;
                    dataLeadingToTable = freTable_keys_byte.Count + 4;
                    break;
                case 2:
                    sizeStringTable = japTable_strings_byte.Count;
                    dataLeadingToTable = japTable_keys_byte.Count + 4;
                    break;
                case 3:
                    sizeStringTable = gerTable_strings_byte.Count;
                    dataLeadingToTable = gerTable_keys_byte.Count + 4;
                    break;
                case 4:
                    sizeStringTable = itaTable_strings_byte.Count;
                    dataLeadingToTable = itaTable_keys_byte.Count + 4;
                    break;
                case 5:
                    sizeStringTable = espTable_strings_byte.Count;
                    dataLeadingToTable = espTable_keys_byte.Count + 4;
                    break;
                default:
                    sizeStringTable = 0;
                    break;
            }

            // Add the size of the string table
            byte[] sizeStringTable_byte = BitConverter.GetBytes(sizeStringTable).Reverse().ToArray();
            for (int i = 0; i < sizeStringTable_byte.Length; i++)
            {
                tableOut.Add(sizeStringTable_byte[i]);
            }

            // Add the size of the data leading to the table
            byte[] dataLeadingToTable_byte = BitConverter.GetBytes(dataLeadingToTable).Reverse().ToArray();
            for (int i = 0; i < dataLeadingToTable_byte.Length; i++)
            {
                tableOut.Add(dataLeadingToTable_byte[i]);
            }

            // Add the keys/values
            switch (language)
            {
                case 0:
                    for (int i = 0; i < engTable_keys_byte.Count; i++)
                    {
                        tableOut.Add(engTable_keys_byte[i]);
                    }
                    break;
                case 1:
                    for (int i = 0; i < freTable_keys_byte.Count; i++)
                    {
                        tableOut.Add(freTable_keys_byte[i]);
                    }
                    break;
                case 2:
                    for (int i = 0; i < japTable_keys_byte.Count; i++)
                    {
                        tableOut.Add(japTable_keys_byte[i]);
                    }
                    break;
                case 3:
                    for (int i = 0; i < gerTable_keys_byte.Count; i++)
                    {
                        tableOut.Add(gerTable_keys_byte[i]);
                    }
                    break;
                case 4:
                    for (int i = 0; i < itaTable_keys_byte.Count; i++)
                    {
                        tableOut.Add(itaTable_keys_byte[i]);
                    }
                    break;
                case 5:
                    for (int i = 0; i < espTable_keys_byte.Count; i++)
                    {
                        tableOut.Add(espTable_keys_byte[i]);
                    }
                    break;
                default:
                    break;
            }

            switch (language)
            {
                case 0:
                    for (int i = 0; i < engTable_strings_byte.Count; i++)
                    {
                        tableOut.Add(engTable_strings_byte[i]);
                    }
                    break;
                case 1:
                    for (int i = 0; i < freTable_strings_byte.Count; i++)
                    {
                        tableOut.Add(freTable_strings_byte[i]);
                    }
                    break;
                case 2:
                    for (int i = 0; i < japTable_strings_byte.Count; i++)
                    {
                        tableOut.Add(japTable_strings_byte[i]);
                    }
                    break;
                case 3:
                    for (int i = 0; i < gerTable_strings_byte.Count; i++)
                    {
                        tableOut.Add(gerTable_strings_byte[i]);
                    }
                    break;
                case 4:
                    for (int i = 0; i < itaTable_strings_byte.Count; i++)
                    {
                        tableOut.Add(itaTable_strings_byte[i]);
                    }
                    break;
                case 5:
                    for (int i = 0; i < espTable_strings_byte.Count; i++)
                    {
                        tableOut.Add(espTable_strings_byte[i]);
                    }
                    break;
                default:
                    break;
            }

            int totalSize = tableOut.Count - 8;
            byte[] totalSize_byte = BitConverter.GetBytes(totalSize).Reverse().ToArray();

            // Change size
            tableOut[4] = totalSize_byte[0];
            tableOut[5] = totalSize_byte[1];
            tableOut[6] = totalSize_byte[2];
            tableOut[7] = totalSize_byte[3];

            return tableOut;
        }

        /// <summary>
        /// Adds a value to the actual string table.
        /// </summary>
        /// <param name="tableOffset">The offset we are adding to.</param>
        /// <param name="p">The string to write.</param>
        /// <param name="language">The language to add to.</param>
        /// <returns></returns>
        private int AddValue(int tableOffset, string p, int language)
        {
            int newOffset = tableOffset;
            byte[] string_byte = System.Text.Encoding.Unicode.GetBytes(p);

            switch (language)
            {
                case 0:
                    for (int i = 0; i < string_byte.Length; i++)
                    {
                        switch (string_byte[i])
                        {
                            case 0x0:
                                break;
                            default:
                                engTable_strings_byte.Add(string_byte[i]);
                                newOffset++;
                                break;
                        }
                    }
                    engTable_strings_byte.Add(0x0);
                    newOffset++;
                    break;
                case 1:
                    for (int i = 0; i < string_byte.Length; i++)
                    {
                        switch (string_byte[i])
                        {
                            case 0x0:
                                break;
                            default:
                                freTable_strings_byte.Add(string_byte[i]);
                                newOffset++;
                                break;
                        }
                    }
                    freTable_strings_byte.Add(0x0);
                    newOffset++;
                    break;
                case 2:
                    for (int i = 0; i < string_byte.Length; i++)
                    {
                        switch (string_byte[i])
                        {
                            case 0x0:
                                break;
                            default:
                                japTable_strings_byte.Add(string_byte[i]);
                                newOffset++;
                                break;
                        }
                    }
                    japTable_strings_byte.Add(0x0);
                    newOffset++;
                    break;
                case 3:
                    for (int i = 0; i < string_byte.Length; i++)
                    {
                        switch (string_byte[i])
                        {
                            case 0x0:
                                break;
                            default:
                                gerTable_strings_byte.Add(string_byte[i]);
                                newOffset++;
                                break;
                        }
                    }
                    gerTable_strings_byte.Add(0x0);
                    newOffset++;
                    break;
                case 4:
                    for (int i = 0; i < string_byte.Length; i++)
                    {
                        switch (string_byte[i])
                        {
                            case 0x0:
                                break;
                            default:
                                itaTable_strings_byte.Add(string_byte[i]);
                                newOffset++;
                                break;
                        }
                    }
                    itaTable_strings_byte.Add(0x0);
                    newOffset++;
                    break;
                case 5:
                    for (int i = 0; i < string_byte.Length; i++)
                    {
                        switch (string_byte[i])
                        {
                            case 0x0:
                                break;
                            default:
                                espTable_strings_byte.Add(string_byte[i]);
                                newOffset++;
                                break;
                        }
                    }
                    espTable_strings_byte.Add(0x0);
                    newOffset++;
                    break;
                default:
                    break;
            }

            return newOffset;
        }

        /// <summary>
        /// Adds a key and offset to the appropriate byte list.
        /// </summary>
        /// <param name="tableOffset">The offset to the data we are adding.</param>
        /// <param name="stringKey">The key to add.</param>
        /// <param name="language">Language to determine what list to add to.</param>
        private void AddKey(int tableOffset, byte[] stringKey, int language)
        {
            byte[] tableOffset_key = BitConverter.GetBytes(tableOffset).Reverse().ToArray();

            switch (language)
            {
                case 0:
                    for (int i = 0; i < stringKey.Length; i++)
                    {
                        engTable_keys_byte.Add(stringKey[i]);
                    }
                    for (int i = 0; i < tableOffset_key.Length; i++)
                    {
                        engTable_keys_byte.Add(tableOffset_key[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        engTable_keys_byte.Add(0x0);
                    }
                    break;
                case 1:
                    for (int i = 0; i < stringKey.Length; i++)
                    {
                        freTable_keys_byte.Add(stringKey[i]);
                    }
                    for (int i = 0; i < tableOffset_key.Length; i++)
                    {
                        freTable_keys_byte.Add(tableOffset_key[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        freTable_keys_byte.Add(0x0);
                    }
                    break;
                case 2:
                    for (int i = 0; i < stringKey.Length; i++)
                    {
                        japTable_keys_byte.Add(stringKey[i]);
                    }
                    for (int i = 0; i < tableOffset_key.Length; i++)
                    {
                        japTable_keys_byte.Add(tableOffset_key[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        japTable_keys_byte.Add(0x0);
                    }
                    break;
                case 3:
                    for (int i = 0; i < stringKey.Length; i++)
                    {
                        gerTable_keys_byte.Add(stringKey[i]);
                    }
                    for (int i = 0; i < tableOffset_key.Length; i++)
                    {
                        gerTable_keys_byte.Add(tableOffset_key[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        gerTable_keys_byte.Add(0x0);
                    }
                    break;
                case 4:
                    for (int i = 0; i < stringKey.Length; i++)
                    {
                        itaTable_keys_byte.Add(stringKey[i]);
                    }
                    for (int i = 0; i < tableOffset_key.Length; i++)
                    {
                        itaTable_keys_byte.Add(tableOffset_key[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        itaTable_keys_byte.Add(0x0);
                    }
                    break;
                case 5:
                    for (int i = 0; i < stringKey.Length; i++)
                    {
                        espTable_keys_byte.Add(stringKey[i]);
                    }
                    for (int i = 0; i < tableOffset_key.Length; i++)
                    {
                        espTable_keys_byte.Add(tableOffset_key[i]);
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        espTable_keys_byte.Add(0x0);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
