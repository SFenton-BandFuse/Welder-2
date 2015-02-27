using Chunk_Tools.STbl_Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MID2RIF;

namespace File_Writers.Chart_Tools
{
    public class ChartParser
    {

        // String table parser.
        public StringTableParser parser;

        // List of all StringTable objects.  Not modifying for now.
        public List<StringTable> list_stringtable;

        // Temporary struct to hold ZOBJ elements.
        public struct tempZOBJ
        {
            public ulong indexKey;
            public ulong objectKey;
            public ulong typeStringKey;
            public byte[] rawData;
        }

        // List of all ZOBJ elements.  Not modifying for now.
        public List<tempZOBJ> tempList;

        /*// Structures to hold data about song metadata
        public StringTable freSong;
        public StringTable engSong;
        public StringTable japSong;
        public StringTable espSong;
        public StringTable gerSong;
        public StringTable itaSong;
        // public SongObject zobjSong;

        // Structure to hold information about master track
        // public MasterObject zobjMaster;

        // Structure to hold information about measure track
        // public MeasureObject zobjMeasure;

        // Structure to hold information about sections
        // public SectionObject zobjSection;

        // Structure to hold information about events
        // public EventObject zobjEvent;

        // Structure to hold information about tempo track
        // public TempoObject zobjTempo;

        // Structure to hold information about time signature
        // public TimeSigObject zobjTimeSig;

        // Structures to hold information about gtr_jam
        public StringTable freJam;
        public StringTable engJam;
        public StringTable japJam;
        public StringTable espJam;
        public StringTable gerJam;
        public StringTable itaJam;
        // public JamObject zobjJam;

        // Structure to hold chord info for gtr_jam
        // public ChordObject jamChord;

        // Structure to hold tab for gtr_jam
        // public NoteObject jamNotes;

        // Structure to hold audio effect for gtr_jam
        // public AudioEffectObject jamEffect;

        // Structures to hold information about video
        public StringTable freVideo;
        public StringTable engVideo;
        public StringTable japVideo;
        public StringTable espVideo;
        public StringTable gerVideo;
        public StringTable itaVideo;

        // Structures to hold information about gtr_beg
        public StringTable freBeg;
        public StringTable engBeg;
        public StringTable japBeg;
        public StringTable espBeg;
        public StringTable gerBeg;
        public StringTable itaBeg;
        // public JamObject zobjBeg;

        // Structure to hold chord info for gtr_beg
        // public ChordObject begChord;

        // Structure to hold tab for gtr_beg
        // public NoteObject begNotes;

        // Structure to hold audio effect for gtr_jam
        // public AudioEffectObject begEffect;

        // Structures to hold information about bss_nov
        public StringTable fre_bssNov;
        public StringTable eng_bssNov;
        public StringTable jap_bssNov;
        public StringTable esp_bssNov;
        public StringTable ger_bssNov;
        public StringTable ita_bssNov;
        // public JamObject zobj_bssNov;

        // Structure to hold bss)nov instrument info
        // public InstrumentInfo instrument_bssNov;

        // Structure to hold tab for bss_nov;
        // public NoteObject bss_novNotes;

        // Structures to hold lyrics
        public StringTable freLyrics;
        public StringTable engLyrics;
        public StringTable japLyrics;
        public StringTable espLyrics;
        public StringTable gerLyrics;
        public StringTable itaLyrics;

        // Structure to hold vox instrument information
        // public InstrumentInfo instrument_vox;

        // Structure to hold vox push phrase info
        // public VoxPushPhrase pushPhrase;

        // Structure to hold vocals notes
        // public VocalsNoteObject voxNotes;

        // Structure to hold bss_beg instrument info
        // public InstrumentInfo instrument_bssBeg;

        // Structures to hold information about bss_beg
        public StringTable fre_bssBeg;
        public StringTable eng_bssBeg;
        public StringTable jap_bssBeg;
        public StringTable esp_bssBeg;
        public StringTable ger_bssBeg;
        public StringTable ita_bssBeg;
        // public JamObject zobj_bssBeg;

        // Structure to hold audio effect for bss_beg
        // public AudioEffectObject bss_begEffect;

        // Structure to hold tab for bss_beg
        // public NoteObject bss_begNotes;

        // Structures to hold information about gtr_nov
        public StringTable fre_gtrNov;
        public StringTable eng_gtrNov;
        public StringTable jap_gtrNov;
        public StringTable esp_gtrNov;
        public StringTable ger_gtrNov;
        public StringTable ita_gtrNov;
        // public JamObject zobj_gtrNov;

        // Structure to hold instrument info about gtr_nov
        // public InstrumentInfo instrument_gtrNov;

        // Structure to hold notes for gtr_nov
        // public NoteObject gtr_novNotes;

        // Structure to hold audio effect for gtr_nov
        // public AudioEffectObject gtr_novEffect;

        // Structure to hold texture information
        // public TextureObject texture;

        // Structures to hold preview information
        public StringTable frePreview;
        public StringTable engPreview;
        public StringTable japPreview;
        public StringTable espPreview;
        public StringTable gerPreview;
        public StringTable itaPreview;
        // public PreviewObject preview;

        // Structure to hold gtr_adv information
        // public InstrumentInfo instrument_gtrAdv;

        // Structures to hold gtr_adv chords
        public StringTable fre_gtrAdv_chords;
        public StringTable eng_gtrAdv_chords;
        public StringTable jap_gtrAdv_chords;
        public StringTable esp_gtrAdv_chords;
        public StringTable ger_gtrAdv_chords;
        public StringTable ita_gtrAdv_chords;
        // public ChordObject gtrAdv_chord;

        // Structure to hold gtr_adv AudioEffect
        // public AudioEffectObject gtr_advEffect;

        // Structure to hold gtr_adv events
        // public EventObject gtr_advEvents;

        // Structure to hold gtr_adv tab
        // public NoteObject gtr_advNotes;

        // Structures to hold info for GTR2
        public StringTable freGtr2;
        public StringTable engGtr2;
        public StringTable japGtr2;
        public StringTable espGtr2;
        public StringTable gerGtr2;
        public StringTable itaGtr2;
        // public StemObject gtr2_stem;

        // Structure to hold bss_jam information
        // public InstrumentInfo instrument_bssJam;

        // Structures to hold info for bss_jam
        public StringTable fre_bssJam;
        public StringTable eng_bssJam;
        public StringTable jap_bssJam;
        public StringTable esp_bssJam;
        public StringTable ger_bssJam;
        public StringTable ita_bssJam;

        // Structure to hold bss_jam AudioEffect
        // public AudioEffectObject bss_jamEffect;

        // Structure to hold tab for bss_jam
        // public NoteObject bss_jamNotes;

        // Structures to hold info for gtr_int chords
        public StringTable fre_gtrInt_chords;
        public StringTable eng_gtrInt_chords;
        public StringTable jap_gtrInt_chords;
        public StringTable esp_gtrInt_chords;
        public StringTable ger_gtrInt_chords;
        public StringTable ita_gtrInt_chords;
        // public ChordObject gtrInt_chord;

        // Structure to hold gtr_int information
        // public InstrumentInfo instrument_gtrInt;

        // Structure to hold gtr_int AudioEffect
        // public AudioEffectObject gtr_intEffect;

        // Structure to hold tab for gtr_int
        // public NoteObject gtr_intNotes;

        // Structures to hold info for gtr_rhy chords
        public StringTable fre_gtrRhy_chords;
        public StringTable eng_gtrRhy_chords;
        public StringTable jap_gtrRhy_chords;
        public StringTable esp_gtrRhy_chords;
        public StringTable ger_gtrRhy_chords;
        public StringTable ita_gtrRhy_chords;
        // public ChordObject gtrRhy_chord;

        // Structure to hold gtr_rhy information
        // public InstrumentInfo instrument_gtrRhy;

        // Structure to hold gtr_rhy AudioEffect
        // public AudioEffectObject gtr_rhyEffect;

        // Structure to hold tab for gtr_rhy
        // public NoteObject gtr_rhyNotes;

        // Structures to hold info for bass stems
        public StringTable fre_stems_bass;
        public StringTable eng_stems_bass;
        public StringTable jap_stems_bass;
        public StringTable esp_stems_bass;
        public StringTable ger_stems_bass;
        public StringTable ita_stems_bass;
        // public StemObject bass_stem;

        // Structures to hold info for GTR1 stems
        public StringTable fre_stems_GTR1;
        public StringTable eng_stems_GTR1;
        public StringTable jap_stems_GTR1;
        public StringTable esp_stems_GTR1;
        public StringTable ger_stems_GTR1;
        public StringTable ita_stems_GTR1;
        // public StemObject GTR1_stem;

        // Structures to hold info for drum stems
        public StringTable fre_stems_drums;
        public StringTable eng_stems_drums;
        public StringTable jap_stems_drums;
        public StringTable esp_stems_drums;
        public StringTable ger_stems_drums;
        public StringTable ita_stems_drums;
        // public StemObject drums_stem;

        // Structures to hold info for bss_adv
        public StringTable fre_bssAdv;
        public StringTable eng_bssAdv;
        public StringTable jap_bssAdv;
        public StringTable esp_bssAdv;
        public StringTable ger_bssAdv;
        public StringTable ita_bssAdv;

        // Structure to hold bss_adv information
        // public InstrumentInfo instrument_bssAdv;

        // Structure to hold bss_adv AudioEffect
        // public AudioEffectObject bss_advEffect;

        // Structure to hold tab for bss_adv
        // public NoteObject bss_advNotes;

        // Structure to hold bss_adv events
        // public EventObject bss_advEvents;*/

        /// <summary>
        /// Parses out the fused.rif file.
        /// </summary>
        /// <param name="file_bytes">Byte representation of the fused.rif file we are analyzing.</param>
        /// <param name="indexDictionary">Dictionary to check keys against.</param>
        public ChartParser(string filepath, StringTable indexDictionary)
        {
            string fused_filepath = filepath;
            FileStream file = System.IO.File.OpenRead(fused_filepath);
            byte[] file_bytes = new byte[file.Length];

            file.Read(file_bytes, 0, file_bytes.Length);
            file.Close();

            parser = new StringTableParser();
            list_stringtable = new List<StringTable>();
            tempList = new List<tempZOBJ>();

            // Skip the first twelve bytes, unnecessary for parsing.  We will
            // skip the index section of the file, as it is useless to us.
            int index = 12;
            int indx_length = 0;
            byte[] indx_length_byte = new byte[4];
            int temp = index;

            for (; index < temp + 4; index++)
            {
                indx_length_byte[index - temp] = file_bytes[index];
            }

            indx_length = BitConverter.ToInt32(indx_length_byte.Reverse().ToArray(), 0);
            index += indx_length;

            while (index < file_bytes.Length)
            {
                // Test to print out a section name
                string sectionName = "";
                byte[] sectionName_byte = new byte[4];
                temp = index;
                for (; index < temp + 4; index++)
                {
                    sectionName_byte[index - temp] = file_bytes[index];
                }

                sectionName = System.Text.Encoding.Default.GetString(sectionName_byte.Reverse().ToArray());

                // Get the section size, skip it
                int section_length = 0;
                byte[] section_length_byte = new byte[4];
                temp = index;

                for (; index < temp + 4; index++)
                {
                    section_length_byte[index - temp] = file_bytes[index];
                }

                section_length = BitConverter.ToInt32(section_length_byte.Reverse().ToArray(), 0);

                // Copy data into a byte array
                temp = index;
                byte[] section_byte = new byte[section_length];

                for (; index < temp + section_length; index++)
                {
                    section_byte[index - temp] = file_bytes[index];
                }

                switch (sectionName)
                {
                    case "ZOBJ":
                        zobjHelper(section_byte);
                        break;
                    case "STbl":
                        stblHelper(section_byte);
                        break;
                    default:
                        break;
                }

                // Fix any lingering issues
                if (file_bytes.Length - index < 4)
                {
                    index = file_bytes.Length;
                }
            }

            int i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("gtr_adv.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("gtr_rhy.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("gtr_nov.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("gtr_beg.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("gtr_int.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("gtr_jam.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("bss_adv.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("bss_nov.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("bss_beg.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("bss_int.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("bss_jam.tab"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateRawData();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("tempo"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateTempo();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("measure"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateMeasure();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("timesignature"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateTimeSignature();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }

            i = 0;
            foreach (tempZOBJ elem in tempList)
            {
                if (indexDictionary.stringTable[elem.indexKey].Contains("vox.vox") && !indexDictionary.stringTable[elem.indexKey].Contains("vox.voxpushphrase"))
                {
                    Console.WriteLine(indexDictionary.stringTable[elem.indexKey]);

                    tempZOBJ newElem = tempList[i];
                    newElem.rawData = generateVocals();
                    tempList[i] = newElem;
                    break;
                }

                i++;
            }
        }

        private byte[] generateVocals()
        {
            VocalsConverter converter = new VocalsConverter();
            List<MID2RIF.VocalsConverter.VoxNote> notes = converter.notes;

            List<byte> outByte = new List<byte>();

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x08);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x20);

            int numEntries = notes.Count;
            byte[] numEntries_byte = BitConverter.GetBytes(numEntries).Reverse().ToArray();
            outByte.Add(numEntries_byte[0]);
            outByte.Add(numEntries_byte[1]);
            outByte.Add(numEntries_byte[2]);
            outByte.Add(numEntries_byte[3]);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x04);

            foreach (MID2RIF.VocalsConverter.VoxNote note in notes)
            {
                byte[] start = BitConverter.GetBytes((float)note.start).Reverse().ToArray();
                for (int i = 0; i < start.Length; i++)
                {
                    outByte.Add(start[i]);
                }

                byte[] end = BitConverter.GetBytes((float)note.end).Reverse().ToArray();
                for (int i = 0; i < end.Length; i++)
                {
                    outByte.Add(end[i]);
                }

                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(note.pitch);
                outByte.Add(note.unknownValue);

                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(0x00);

                byte[] key = BitConverter.GetBytes((ulong)note.key).Reverse().ToArray();
                for (int i = 0; i < key.Length; i++)
                {
                    outByte.Add(key[i]);
                }


                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(note.noteType);

                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(0x00);
                outByte.Add(0x00);
            }

            return outByte.ToArray();
        }

        private byte[] generateTimeSignature()
        {
            TimeSignatureConverter converter = new TimeSignatureConverter();
            List<TimeSignature> sig = converter.sig;

            List<byte> outByte = new List<byte>();
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x10);

            int numEntries = sig.Count;
            byte[] numEntries_byte = BitConverter.GetBytes(numEntries).Reverse().ToArray();
            outByte.Add(numEntries_byte[0]);
            outByte.Add(numEntries_byte[1]);
            outByte.Add(numEntries_byte[2]);
            outByte.Add(numEntries_byte[3]);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x04);

            foreach (TimeSignature sign in sig)
            {
                byte[] start = BitConverter.GetBytes((float)sign.time).Reverse().ToArray();
                for (int i = 0; i < start.Length; i++)
                {
                    outByte.Add(start[i]);
                }

                byte[] end = BitConverter.GetBytes((float)sign.end).Reverse().ToArray();
                for (int i = 0; i < end.Length; i++)
                {
                    outByte.Add(end[i]);
                }

                byte[] num = BitConverter.GetBytes(sign.numerator).Reverse().ToArray();
                for (int i = 0; i < num.Length; i++)
                {
                    outByte.Add(num[i]);
                }

                byte[] denom = BitConverter.GetBytes(sign.denominator).Reverse().ToArray();
                for (int i = 0; i < denom.Length; i++)
                {
                    outByte.Add(denom[i]);
                }
            }

            return outByte.ToArray();
        }

        /// <summary>
        /// Generates the measures for the song to use.
        /// </summary>
        /// <returns>Byte representation of the measure track.</returns>
        private byte[] generateMeasure()
        {
            MeasureConverter converter = new MeasureConverter();
            List<MID2RIF.MeasureConverter.Measure> measures = converter.measures;

            List<byte> outByte = new List<byte>();
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x04);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x0c);

            int numEntries = measures.Count;
            byte[] numEntries_byte = BitConverter.GetBytes(numEntries).Reverse().ToArray();
            outByte.Add(numEntries_byte[0]);
            outByte.Add(numEntries_byte[1]);
            outByte.Add(numEntries_byte[2]);
            outByte.Add(numEntries_byte[3]);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x04);

            foreach (MID2RIF.MeasureConverter.Measure measure in measures)
            {
                byte[] start = BitConverter.GetBytes((float)measure.start).Reverse().ToArray();
                for (int i = 0; i < start.Length; i++)
                {
                    outByte.Add(start[i]);
                }

                for (int i = 0; i < start.Length; i++)
                {
                    outByte.Add(start[i]);
                }

                byte[] upDown = BitConverter.GetBytes((float)measure.upDown).Reverse().ToArray();
                for (int i = 0; i < upDown.Length; i++)
                {
                    outByte.Add(upDown[i]);
                }
            }

            return outByte.ToArray();
        }

        /// <summary>
        /// Generates a tempo track.
        /// </summary>
        /// <returns>Byte representation of the data.</returns>
        private byte[] generateTempo()
        {
            TempoConverter converter = new TempoConverter();
            List<TempoEvent> events = converter.tempos;

            List<byte> outByte = new List<byte>();
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x01);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x0c);

            int numEntries = events.Count;
            byte[] numEntries_byte = BitConverter.GetBytes(numEntries).Reverse().ToArray();
            outByte.Add(numEntries_byte[0]);
            outByte.Add(numEntries_byte[1]);
            outByte.Add(numEntries_byte[2]);
            outByte.Add(numEntries_byte[3]);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x04);

            foreach (TempoEvent tempo in events)
            {
                byte[] start = BitConverter.GetBytes((float)tempo.start).Reverse().ToArray();
                for (int i = 0; i < start.Length; i++)
                {
                    outByte.Add(start[i]);
                }

                byte[] end = BitConverter.GetBytes((float)tempo.end).Reverse().ToArray();
                for (int i = 0; i < end.Length; i++)
                {
                    outByte.Add(end[i]);
                }

                byte[] BPM = BitConverter.GetBytes((float)tempo.BPM).Reverse().ToArray();
                for (int i = 0; i < BPM.Length; i++)
                {
                    outByte.Add(BPM[i]);
                }
            }

            return outByte.ToArray();
        }

        /// <summary>
        /// Generates raw data.
        /// </summary>
        /// <returns></returns>
        private byte[] generateRawData()
        {
            MIDIConverter converter = new MIDIConverter();
            List<Note> notes = converter.notes;

            List<byte> outByte = new List<byte>();
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x0b);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x40);

            int numEntries = notes.Count;
            byte[] numEntries_byte = BitConverter.GetBytes(numEntries).Reverse().ToArray();
            outByte.Add(numEntries_byte[0]);
            outByte.Add(numEntries_byte[1]);
            outByte.Add(numEntries_byte[2]);
            outByte.Add(numEntries_byte[3]);

            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x00);
            outByte.Add(0x04);

            foreach (Note note in notes)
            {
                float offset = (float)note.offset;
                float length = (float)note.length;

                byte[] off_byte = BitConverter.GetBytes(offset).Reverse().ToArray();
                for (int i = 0; i < off_byte.Length; i++)
                {
                    outByte.Add(off_byte[i]);
                }

                byte[] len_byte = BitConverter.GetBytes(length).Reverse().ToArray();
                for (int i = 0; i < len_byte.Length; i++)
                {
                    outByte.Add(len_byte[i]);
                }

                byte[] str = BitConverter.GetBytes(note.stringNum).Reverse().ToArray();
                for (int i = 0; i < str.Length; i++)
                {
                    outByte.Add(str[i]);
                }

                byte[] fret = BitConverter.GetBytes(note.fret).Reverse().ToArray();
                for (int i = 0; i < fret.Length; i++)
                {
                    outByte.Add(fret[i]);
                }

                byte[] finger = BitConverter.GetBytes(note.finger).Reverse().ToArray();
                for (int i = 0; i < finger.Length; i++)
                {
                    outByte.Add(finger[i]);
                }

                byte[] noteOp;
                switch (note.noteOp)
                {
                    case 1:
                        noteOp = BitConverter.GetBytes(4).Reverse().ToArray();
                        break;
                    case 2:
                        noteOp = BitConverter.GetBytes(3).Reverse().ToArray();
                        break;
                    case 3:
                        noteOp = BitConverter.GetBytes(2).Reverse().ToArray();
                        break;
                    case 4:
                        noteOp = BitConverter.GetBytes(1).Reverse().ToArray();
                        break;
                    case 5:
                        noteOp = BitConverter.GetBytes(10).Reverse().ToArray();
                        break;
                    case 6:
                        noteOp = BitConverter.GetBytes(11).Reverse().ToArray();
                        break;
                    case 7:
                        noteOp = BitConverter.GetBytes(9).Reverse().ToArray();
                        break;
                    default:
                        noteOp = BitConverter.GetBytes(0).Reverse().ToArray();
                        break;
                }

                for (int i = 0; i < noteOp.Length; i++)
                {
                    outByte.Add(noteOp[i]);
                }

                byte[] bend = BitConverter.GetBytes(note.bend).Reverse().ToArray();
                for (int i = 0; i < bend.Length; i++)
                {
                    outByte.Add(bend[i]);
                }

                byte[] amount = BitConverter.GetBytes((note.amount / 2.0f)).Reverse().ToArray();
                for (int i = 0; i < amount.Length; i++)
                {
                    outByte.Add(amount[i]);
                }

                byte[] vibrato = BitConverter.GetBytes(note.vibrato).Reverse().ToArray();
                for (int i = 0; i < vibrato.Length; i++)
                {
                    outByte.Add(vibrato[i]);
                }

                byte[] zero = BitConverter.GetBytes(0);
                for (int i = 0; i < zero.Length; i++)
                {
                    outByte.Add(zero[i]);
                }

                int whatisthis = Convert.ToInt32("427c0000", 16);

                byte[] what = BitConverter.GetBytes(whatisthis).Reverse().ToArray();
                for (int i = 0; i < what.Length; i++)
                {
                    outByte.Add(what[i]);
                }
                for (int i = 0; i < what.Length; i++)
                {
                    outByte.Add(what[i]);
                }

                for (int i = 0; i < zero.Length; i++)
                {
                    outByte.Add(zero[i]);
                }

                for (int i = 0; i < zero.Length; i++)
                {
                    outByte.Add(zero[i]);
                }

                byte[] connect = new byte[1];
                if (note.connect != 0)
                    connect[0] = 1;
                else
                    connect[0] = 0;

                for (int i = 0; i < connect.Length; i++)
                {
                    outByte.Add(connect[i]);
                }

                byte[] palm = new byte[1];
                if (note.palmMute != 0)
                    palm[0] = 1;
                else
                    palm[0] = 0;

                for (int i = 0; i < palm.Length; i++)
                {
                    outByte.Add(palm[i]);
                }

                byte[] trem = new byte[1];
                if (note.tremolo != 0)
                    trem[0] = 1;
                else
                    trem[0] = 0;

                for (int i = 0; i < trem.Length; i++)
                {
                    outByte.Add(trem[i]);
                }

                byte[] zeroBit = new byte[1];
                zeroBit[0] = 0;
                for (int i = 0; i < zeroBit.Length; i++)
                {
                    outByte.Add(zeroBit[i]);
                }

                for (int i = 0; i < zero.Length; i++)
                {
                    outByte.Add(zero[i]);
                }
            }

            return outByte.ToArray();
        }

        /// <summary>
        /// Deals with all forms of string tables
        /// </summary>
        /// <param name="section">Object section to analyze</param>
        private void zobjHelper(byte[] section)
        {
            // Get the index key
            int index = 0;
            ulong indexKey;
            byte[] indexKey_byte = new byte[8];

            int i = index;
            for (; index < i + 8; index++)
            {
                indexKey_byte[index - i] = section[index];
            }

            indexKey = BitConverter.ToUInt64(indexKey_byte.Reverse().ToArray(), 0);

            // Get the object key
            ulong objectKey;
            byte[] objectKey_byte = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                objectKey_byte[index - i] = section[index];
            }

            objectKey = BitConverter.ToUInt64(objectKey_byte.Reverse().ToArray(), 0);

            // Get the type string key
            ulong typeStringKey;
            byte[] typeStringKey_byte = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                typeStringKey_byte[index - i] = section[index];
            }

            typeStringKey = BitConverter.ToUInt64(typeStringKey_byte.Reverse().ToArray(), 0);

            // Skip zeroed data
            index += 8;

            int size_raw = section.Length - index;
            byte[] section_raw_byte = new byte[size_raw];
            i = index;

            for (; index < i + size_raw; index++)
            {
                section_raw_byte[index - i] = section[index];
            }

            tempZOBJ temp = new tempZOBJ();
            temp.indexKey = indexKey;
            temp.objectKey = objectKey;
            temp.typeStringKey = typeStringKey;
            temp.rawData = section_raw_byte;

            tempList.Add(temp);
        }

        /// <summary>
        /// Deals with all forms of string tables
        /// </summary>
        /// <param name="section">Section to analyze</param>
        public void stblHelper(byte[] section)
        {
            // Get the index key
            int index = 0;
            ulong indexKey;
            byte[] indexKey_byte = new byte[8];

            int i = index;
            for (; index < i + 8; index++)
            {
                indexKey_byte[index - i] = section[index];
            }

            indexKey = BitConverter.ToUInt64(indexKey_byte.Reverse().ToArray(), 0);

            // Get the folder key
            ulong folderKey;
            byte[] folderKey_byte = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                folderKey_byte[index - i] = section[index];
            }

            folderKey = BitConverter.ToUInt64(folderKey_byte.Reverse().ToArray(), 0);

            // Get the language identifier
            long language;
            byte[] stblIdentifier = new byte[8];

            i = index;
            for (; index < i + 8; index++)
            {
                stblIdentifier[index - i] = section[index];
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
                countEntries_byte[index - i] = section[index];
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
                tableSize_byte[index - i] = section[index];
            }

            tableSize = BitConverter.ToInt32(tableSize_byte.Reverse().ToArray(), 0);

            // Get the size of data leading up to the table
            int sizeKeys;
            byte[] sizeKeys_byte = new byte[4];
            i = index;

            for (; index < i + 4; index++ )
            {
                sizeKeys_byte[index - i] = section[index];
            }

            sizeKeys = BitConverter.ToInt32(sizeKeys_byte.Reverse().ToArray(), 0);
            StringTable tempTable = new StringTable();
            tempTable.stringTable = parser.generateDictionary(section, index, countEntries);
            if (tempTable.stringTable.ContainsValue("whoo!#"))
            {
                tempTable.stringTable = generateLyrics();
                tempTable.folderKey = folderKey;
                tempTable.indexKey = indexKey;
                tempTable.languageIdentifier = language;
                tempTable.sizeLeadingUpToTable = (tempTable.stringTable.Count * 16) + 4;
                VocalsConverter converter = new VocalsConverter();
                tempTable.sizeTable = converter.size;
                tempTable.numEntries = tempTable.stringTable.Count;
            }
            else
            {
                tempTable.folderKey = folderKey;
                tempTable.indexKey = indexKey;
                tempTable.languageIdentifier = language;
                tempTable.sizeTable = tableSize;
                tempTable.sizeLeadingUpToTable = sizeKeys;
                tempTable.numEntries = tempTable.stringTable.Count;
            }

            list_stringtable.Add(tempTable);
        }

        private Dictionary<ulong, string> generateLyrics()
        {
            VocalsConverter converter = new VocalsConverter();
            return converter.lyricTable;
        }
    }
}
