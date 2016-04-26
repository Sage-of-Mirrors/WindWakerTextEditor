using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFormatReader.Common;
using System.Collections.ObjectModel;

namespace WPFTextEditor
{
    public class BmcColorFile
    {
        ObservableCollection<Color> m_colorList;

        public BmcColorFile(EndianBinaryReader reader)
        {
            m_colorList = new ObservableCollection<Color>();

            string magic = new string(reader.ReadChars(8));

            if (magic != "MGCLbmc1")
                throw new FormatException("BMC magic was incorrect.");

            reader.BaseStream.Seek(0x20, 0);

            string clt1Magic = new string(reader.ReadChars(4));
            uint clt1Size = reader.ReadUInt32();

            ushort numColors = reader.ReadUInt16();

            ushort padding = reader.ReadUInt16();

            if (padding != 0)
                Console.WriteLine("Padding in CLT1 was NOT 0!");

            for (ushort i = 0; i < numColors; i++)
            {
                Color newCol = Color.FromArgb(255, reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
                reader.SkipByte(); // Colors are stored in RGBA format, so we skip the A component

                m_colorList.Add(newCol);
            }
        }

        public void Export(EndianBinaryWriter writer)
        {
            writer.Write("MGCLbmc1".ToCharArray());
            writer.Write((int)0); // Placeholder for file size
            writer.Write((int)1); // Unknown
            ExtensionMethods.Pad32(writer);

            writer.Write("CLT1".ToCharArray());
            writer.Write((int)0); // Placeholder for CLT1 size
            writer.Write((ushort)m_colorList.Count);
            writer.Write((ushort)0);

            foreach (Color col in m_colorList)
            {
                writer.Write((byte)col.R);
                writer.Write((byte)col.G);
                writer.Write((byte)col.B);
                writer.Write((byte)255);
            }

            ExtensionMethods.Pad32(writer);

            writer.BaseStream.Seek(0x24, 0);
            writer.Write((int)writer.BaseStream.Length - 0x20);

            writer.BaseStream.Seek(8, 0);
            writer.Write((int)writer.BaseStream.Length);
        }
    }
}
