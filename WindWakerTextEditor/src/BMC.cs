using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GameFormatReader.Common;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WindEditor;

namespace WindWakerTextEditor
{
    public class BMC : INotifyPropertyChanged
    {
        private ObservableCollection<WLinearColor> m_colorList;

        public ObservableCollection<WLinearColor> ColorList
        {
            get { return m_colorList; }
            set
            {
                if (m_colorList != value)
                {
                    m_colorList = value;
                    NotifyPropertyChanged("ColorList");
                }
            }
        }

        public BMC(EndianBinaryReader reader)
        {
            m_colorList = new ObservableCollection<WLinearColor>();

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
                WLinearColor newCol = new WLinearColor(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), 255);
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

            foreach (WLinearColor col in m_colorList)
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
}
