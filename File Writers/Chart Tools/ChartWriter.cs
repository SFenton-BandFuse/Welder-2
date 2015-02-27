using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace File_Writers.Chart_Tools
{

    public class ChartWriter
    {
        List<byte> zobj_bytes;
        List<byte> stbl_bytes;
        List<byte> index_byte;

        List<byte> tempKeys;
        List<byte> tempValues;
        List<List<byte>> rawTables;

        /// <summary>
        /// Writes out a chart with a provided parser.
        /// </summary>
        /// <param name="parser"></param>
        public ChartWriter(ChartParser parser)
        {
            zobj_bytes = new List<byte>();
            stbl_bytes = new List<byte>();
            index_byte = new List<byte>();
            rawTables = new List<List<byte>>();

            generateZOBJ(parser.tempList);
            generateSTBL(parser.list_stringtable);
            generateINDX(parser);

            WriteFile();
        }

        private void WriteFile()
        {
            List<byte> finalFile = new List<byte>();

            // Write out RIFF chunk.
            finalFile.Add(BitConverter.GetBytes('F')[0]);
            finalFile.Add(BitConverter.GetBytes('F')[0]);
            finalFile.Add(BitConverter.GetBytes('I')[0]);
            finalFile.Add(BitConverter.GetBytes('R')[0]);

            // Get the size of all elements
            int totalSize = index_byte.Count + stbl_bytes.Count + zobj_bytes.Count;
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

            // Add the string table
            foreach (byte entry in stbl_bytes)
            {
                finalFile.Add(entry);
            }

            // Add the object
            foreach (byte entry in zobj_bytes)
            {
                finalFile.Add(entry);
            }

            File.WriteAllBytes(/* Path to write fused.rif to */, finalFile.ToArray());
        }

        private void generateINDX(ChartParser parser)
        {
            // Write the header
            index_byte.Add(BitConverter.GetBytes('X')[0]);
            index_byte.Add(BitConverter.GetBytes('D')[0]);
            index_byte.Add(BitConverter.GetBytes('N')[0]);
            index_byte.Add(BitConverter.GetBytes('I')[0]);

            // Write the size
            int indx_size = (parser.tempList.Count + parser.list_stringtable.Count) * 16 + 8;
            byte[] indx_size_byte = BitConverter.GetBytes(indx_size).Reverse().ToArray();
            for (int i = 0; i < indx_size_byte.Length; i++)
            {
                index_byte.Add(indx_size_byte[i]);
            }

            // Write the number of entries
            int numEntries = parser.tempList.Count + parser.list_stringtable.Count;
            byte[] numEntries_byte = BitConverter.GetBytes(numEntries).Reverse().ToArray();
            for (int i = 0; i < numEntries_byte.Length; i++)
            {
                index_byte.Add(numEntries_byte[i]);
            }

            // Add the constant
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x0);
            index_byte.Add(0x4);

            // Initial offset
            int offset = 16 * (numEntries) + 24;

            // Add information about all string tables first
            for (int k = 0; k < rawTables.Count; k++)
            {
                byte[] indexKey_byte = BitConverter.GetBytes(parser.list_stringtable[k].indexKey).Reverse().ToArray();
                for (int i = 0; i < indexKey_byte.Length; i++)
                {
                    index_byte.Add(indexKey_byte[i]);
                }

                byte[] offset_byte = BitConverter.GetBytes(offset).Reverse().ToArray();
                for (int i = 0; i < offset_byte.Length; i++)
                {
                    index_byte.Add(offset_byte[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    index_byte.Add(0x0);
                }

                offset += rawTables[k].Count;
            }

            // Add information about ZOBJ items
            for (int k = 0; k < parser.tempList.Count; k++)
            {
                byte[] indexKey_byte = BitConverter.GetBytes(parser.tempList[k].indexKey).Reverse().ToArray();
                for (int i = 0; i < indexKey_byte.Length; i++)
                {
                    index_byte.Add(indexKey_byte[i]);
                }

                byte[] offset_byte = BitConverter.GetBytes(offset).Reverse().ToArray();
                for (int i = 0; i < offset_byte.Length; i++)
                {
                    index_byte.Add(offset_byte[i]);
                }

                for (int i = 0; i < 4; i++)
                {
                    index_byte.Add(0x0);
                }

                offset += (parser.tempList[k].rawData.Length + 40);
            }
        }

        private void generateSTBL(List<StringTable> list)
        {
            foreach (StringTable table in list)
            {
                tempKeys = new List<byte>();
                tempValues = new List<byte>();

                int size = 40;
                List<byte> tableOut = new List<byte>();
                byte[] indexKey = BitConverter.GetBytes(table.indexKey).Reverse().ToArray();
                byte[] folderKey = BitConverter.GetBytes(table.folderKey).Reverse().ToArray();
                byte[] languageIdentifier = BitConverter.GetBytes(table.languageIdentifier)
                            .Reverse().ToArray();

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

                foreach (KeyValuePair<ulong, string> pair in table.stringTable)
                {
                    byte[] stringKey = BitConverter.GetBytes(pair.Key).Reverse().ToArray();

                    AddKey(tableOffset, stringKey);
                    tableOffset = AddValue(tableOffset, pair.Value);
                }

                // Get the size of the string table
                int sizeStringTable = 0;
                sizeStringTable = tempValues.Count;
                dataLeadingToTable = tempKeys.Count + 4;

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
                for (int i = 0; i < tempKeys.Count; i++)
                {
                    tableOut.Add(tempKeys[i]);
                }

                for (int i = 0; i < tempValues.Count; i++)
                {
                    tableOut.Add(tempValues[i]);
                }


                int totalSize = tableOut.Count - 8;
                byte[] totalSize_byte = BitConverter.GetBytes(totalSize).Reverse().ToArray();

                // Change size
                tableOut[4] = totalSize_byte[0];
                tableOut[5] = totalSize_byte[1];
                tableOut[6] = totalSize_byte[2];
                tableOut[7] = totalSize_byte[3];

                for (int i = 0; i < tableOut.Count; i++)
                {
                    stbl_bytes.Add(tableOut[i]);
                }

                rawTables.Add(tableOut);
            }
        }

        /// <summary>
        /// Writes out our shit
        /// </summary>
        /// <param name="list"></param>
        private void generateZOBJ(List<ChartParser.tempZOBJ> list)
        {
            foreach (ChartParser.tempZOBJ temp in list)
            {
                zobj_bytes.Add(BitConverter.GetBytes('J')[0]);
                zobj_bytes.Add(BitConverter.GetBytes('B')[0]);
                zobj_bytes.Add(BitConverter.GetBytes('O')[0]);
                zobj_bytes.Add(BitConverter.GetBytes('Z')[0]);

                byte[] size = BitConverter.GetBytes(temp.rawData.Length + 32).Reverse().ToArray();
                for (int i = 0; i < size.Length; i++)
                {
                    zobj_bytes.Add(size[i]);
                }

                byte[] index_byte = BitConverter.GetBytes(temp.indexKey).Reverse().ToArray();
                for (int i = 0; i < index_byte.Length; i++)
                {
                    zobj_bytes.Add(index_byte[i]);
                }

                byte[] object_byte = BitConverter.GetBytes(temp.objectKey).Reverse().ToArray();
                for (int i = 0; i < object_byte.Length; i++)
                {
                    zobj_bytes.Add(object_byte[i]);
                }

                byte[] string_byte = BitConverter.GetBytes(temp.typeStringKey).Reverse().ToArray();
                for (int i = 0; i < string_byte.Length; i++)
                {
                    zobj_bytes.Add(string_byte[i]);
                }

                for (int i = 0; i < index_byte.Length; i++)
                {
                    zobj_bytes.Add(0x0);
                }

                for (int i = 0; i < temp.rawData.Length; i++)
                {
                    zobj_bytes.Add(temp.rawData[i]);
                }
            }
        }

        /// <summary>
        /// Adds a key and offset to the appropriate byte list.
        /// </summary>
        /// <param name="tableOffset">The offset to the data we are adding.</param>
        /// <param name="stringKey">The key to add.</param>
        private void AddKey(int tableOffset, byte[] stringKey)
        {
            byte[] tableOffset_key = BitConverter.GetBytes(tableOffset).Reverse().ToArray();

            for (int i = 0; i < stringKey.Length; i++)
            {
                tempKeys.Add(stringKey[i]);
            }

            for (int i = 0; i < tableOffset_key.Length; i++)
            {
                tempKeys.Add(tableOffset_key[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                tempKeys.Add(0x0);
            }
        }

        /// <summary>
        /// Adds a value to the actual string table.
        /// </summary>
        /// <param name="tableOffset">The offset we are adding to.</param>
        /// <param name="p">The string to write.</param>
        /// <param name="language">The language to add to.</param>
        /// <returns></returns>
        private int AddValue(int tableOffset, string p)
        {
            int newOffset = tableOffset;
            byte[] string_byte = System.Text.Encoding.Unicode.GetBytes(p);

            for (int i = 0; i < string_byte.Length; i++)
            {
                switch (string_byte[i])
                {
                    case 0x0:
                        break;
                    default:
                        tempValues.Add(string_byte[i]);
                        newOffset++;
                        break;
                }
            }
            tempValues.Add(0x0);
            newOffset++;

            return newOffset;
        }
    }
}
