using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using GameFormatReader.Common;
using System.Collections.ObjectModel;

namespace WindWakerTextEditor
{
    public class BMG : INotifyPropertyChanged
    {
        private ObservableCollection<Message> m_messageList;
        int m_fileSize;
        int m_sectionCount;
        Encoding m_Encoding;
        short m_numMessages;

        public ObservableCollection<Message> MessageList
        {
            get { return m_messageList; }

            set
            {
                if (value != m_messageList)
                {
                    m_messageList = value;
                    NotifyPropertyChanged("MessageList");
                }
            }
        }

        public Encoding Encoding
        {
            get { return m_Encoding; }
        }

        public BMG(EndianBinaryReader reader)
        {
            MessageList = new ObservableCollection<Message>();

            string bmgMagic = reader.ReadString(8);
            if (bmgMagic != "MESGbmg1")
                throw new FormatException($"BMG magic was \"{ bmgMagic }\" instead of \"MESGbmg1\"!");

            m_fileSize = reader.ReadInt32();
            m_sectionCount = reader.ReadInt32();

            m_Encoding = Encoding.ASCII;
            byte encoding = reader.ReadByte();
            switch (encoding)
            {
                case 1:
                    m_Encoding = Encoding.GetEncoding("windows-1252"); // ANSI Latin 1; Western European (Windows)
                    break;
                case 2:
                    m_Encoding = Encoding.BigEndianUnicode; // UTF-16
                    break;
                case 3:
                    m_Encoding = Encoding.GetEncoding("shift_jis"); // ANSI/OEM Japanese; Japanese (Shift-JIS)
                    break;
                case 4:
                    m_Encoding = Encoding.UTF8;
                    break;
            }

            reader.Skip(0x17);

            m_numMessages = reader.ReadInt16();

            reader.Skip(6);

            for (int i = 0; i < m_numMessages; i++)
            {
                Message mes = new Message(reader);
                mes.Index = i;
                MessageList.Add(mes);
            }

            reader.Skip((reader.BaseStream.Position % 16) + 8);

            int m_textDataOrigin = (int)reader.BaseStream.Position;
            foreach (Message mes in MessageList)
            {
                int m_textDataOffset = mes.TextDataOffset + m_textDataOrigin;

                if (m_textDataOffset > reader.BaseStream.Length)
                {
                    Console.WriteLine($"Message { mes.MessageId } specified an invalid index. Could not be read.");
                    continue;
                }

                reader.BaseStream.Position = m_textDataOffset;

                mes.ReadTextData(reader, m_Encoding);
            }
        }

        public void Export(EndianBinaryWriter writer)
        {
            WriteHeader(writer);
            WriteInf1(writer);
        }

        private void WriteHeader(EndianBinaryWriter writer)
        {
            writer.Write("MESGbmg1".ToCharArray()); // Magic
            writer.Write((int)0); // File size placeholder
            writer.Write((int)2); // Number of sections

            switch (m_Encoding.WebName)
            {
                case "Windows-1252":
                    writer.Write((byte)1);
                    break;
                case "BigEndianUnicode":
                    writer.Write((byte)2);
                    break;
                case "shift_jis":
                    writer.Write((byte)3);
                    break;
                case "utf-8":
                    writer.Write((byte)4);
                    break;
            }

            ExtensionMethods.Pad32(writer); // Pad header
        }

        private void WriteInf1(EndianBinaryWriter writer)
        {
            writer.Write("INF1".ToCharArray()); // Magic
            writer.Write((int)0); // File size placeholder
            writer.Write((ushort)m_messageList.Count);
            writer.Write((ushort)0x18); // INF1 entry size (for TWW)
            writer.Write((int)0); // Padding

            List<byte[]> messageDataList = new List<byte[]>(); // This will hold the text data as we write the message data

            foreach (Message mes in m_messageList)
            {
                messageDataList.Add(mes.WriteMessage(writer, m_Encoding));
            }

            ExtensionMethods.Pad32(writer);

            writer.BaseStream.Seek(0x24, SeekOrigin.Begin); // Seek to INF1 size field
            writer.Write((int)writer.BaseStream.Length - 0x20); // Write INF1 size, (streamLength - file header size)
            writer.BaseStream.Seek(0, SeekOrigin.End); // Seek to end of stream

            writer.Write("DAT1".ToCharArray()); // DAT1 magic
            int dat1SizeOffset = (int)writer.BaseStream.Length;
            writer.Write((int)0); // Section size placeholder
            writer.Write((byte)0); // Dummy string entry. If any messages had offset 0, it would be this empty string

            int textOffset = 1;

            for (int i = 0; i < messageDataList.Count; i++)
            {
                if (m_messageList[i].MessageId == 0)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        writer.Write((int)0);
                    }

                    continue;
                }

                writer.BaseStream.Seek((0x30 + (i * 0x18)), 0); // Seek to the text data offset field at (size of file header and INF1 header + (currentEntry * entryLength))
                writer.Write(textOffset); // Write text data offset
                textOffset += messageDataList[i].Length; // Add current data length to text offset
                writer.BaseStream.Seek(0, SeekOrigin.End); //  Seek to end of stream
                writer.Write(messageDataList[i]); // Write text data
            }

            writer.BaseStream.Seek(dat1SizeOffset, 0);
            writer.Write((int)textOffset + 8);

            writer.BaseStream.Seek(8, SeekOrigin.Begin); // Seek to file size field
            writer.Write((int)writer.BaseStream.Length); // Write file size
        }

        #region NotifyPropertyChanged Stuff

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion
    }

    public static class ExtensionMethods
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            
            string name = Enum.GetName(type, value);
            
            if (name != null)
            {
                var field = type.GetField(name);
                
                if (field != null)
                {
                    DescriptionAttribute attr = 
                       Attribute.GetCustomAttribute(field, 
                         typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
        public static void Pad32(EndianBinaryWriter writer)
        {
            // Pad up to a 32 byte alignment
            // Formula: (x + (n-1)) & ~(n-1)
            long nextAligned = (writer.BaseStream.Length + 0x1F) & ~0x1F;

            long delta = nextAligned - writer.BaseStream.Length;
            writer.BaseStream.Position = writer.BaseStream.Length;
            for (int i = 0; i < delta; i++)
            {
                writer.Write((byte)0);
            }
        }
    }
}
