using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using GameFormatReader.Common;
using System.Collections.ObjectModel;

namespace WPFTextEditor
{
    public class BmgTextFile : INotifyPropertyChanged
    {
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

        private ObservableCollection<Message> m_messageList;

        string m_bmgFileMagic;

        int m_fileSize;

        int m_unknownInt1;

        byte m_unknownBool1;

        short m_numMessages;

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

        public BmgTextFile(EndianBinaryReader reader)
        {
            MessageList = new ObservableCollection<Message>();

            m_bmgFileMagic = reader.ReadStringUntil('\0').Trim('\0');

            reader.BaseStream.Position -= 1;

            m_fileSize = reader.ReadInt32();

            m_unknownInt1 = reader.ReadInt32();

            m_unknownBool1 = reader.ReadByte();

            reader.Skip(0x17);

            m_numMessages = reader.ReadInt16();

            reader.Skip(6);

            for (int i = 0; i < m_numMessages; i++)
            {
                Message mes = new Message(reader);

                if (mes.TextDataOffset != 0)
                    MessageList.Add(mes);
            }

            reader.Skip((reader.BaseStream.Position % 16) + 8);

            int m_textDataOrigin = (int)reader.BaseStream.Position;

            foreach (Message mes in MessageList)
            {
                int m_textDataOffset = mes.TextDataOffset + m_textDataOrigin;

                reader.BaseStream.Position = m_textDataOffset;

                mes.ReadTextData(reader);
            }

            /*using (FileStream stream = new FileStream("C:\\Program Files (x86)\\SZS Tools\\textoutput.txt", FileMode.Create))
            {
                EndianBinaryWriter writer = new EndianBinaryWriter(stream, Endian.Big);

                foreach (Message mes in MessageList)
                {
                    writer.WriteFixedString(mes.TextData + Environment.NewLine + Environment.NewLine, mes.TextData.Length + 2);
                }
            }*/
        }
    }

    public class Message : INotifyPropertyChanged
    {
        public int TextDataOffset
        {
            get { return m_textDataOffset; }
        }

        private int m_textDataOffset;

        #region short MessageID

        public short MessageId
        {
            get { return m_messageId; }
            set 
            { 
                if (value != m_messageId)
                {
                    m_messageId = value;

                    NotifyPropertyChanged("MessageId");
                }
            }
        }

        private short m_messageId;

        #endregion

        #region short UnknownField1

        public short UnknownField1
        {
            get { return m_unknownField1; }

            set
            {
                if (value != m_unknownField1)
                {
                    m_unknownField1 = value;

                    NotifyPropertyChanged("UnknownField1");
                }
            }
        }

        private short m_unknownField1;

        #endregion

        #region short UnknownField2

        public short UnknownField2
        {
            get { return m_unknownField2; }

            set
            {
                if (value != m_unknownField2)
                {
                    m_unknownField2 = value;

                    NotifyPropertyChanged("UnknownField2");
                }
            }
        }

        private short m_unknownField2;

        #endregion

        #region short UnknownField3

        public short UnknownField3
        {
            get { return m_unknownField3; }

            set
            {
                if (value != m_unknownField3)
                {
                    m_unknownField3 = value;

                    NotifyPropertyChanged("UnknownField3");
                }
            }
        }

        private short m_unknownField3;

        #endregion

        #region BoxTypes TextBoxType

        public BoxTypes TextBoxType
        {
            get { return m_textBoxType; }

            set
            {
                if (value != m_textBoxType)
                {
                    m_textBoxType = value;

                    NotifyPropertyChanged("TextBoxType");
                }
            }
        }

        private BoxTypes m_textBoxType;

        #endregion

        #region byte InitialDrawType

        public byte InitialDrawType
        {
            get { return m_initialDrawType; }

            set
            {
                if (value != m_initialDrawType)
                {
                    m_initialDrawType = value;

                    NotifyPropertyChanged("InitialDrawType");
                }
            }
        }

        private byte m_initialDrawType;

        #endregion

        #region BoxPositions TextBoxPosition

        public BoxPositions TextBoxPosition
        {
            get { return m_textBoxPosition; }
            set
            {
                if (value != m_textBoxPosition)
                {
                    m_textBoxPosition = value;

                    NotifyPropertyChanged("TextBoxPosition");
                }
            }
        }

        private BoxPositions m_textBoxPosition;

        #endregion

        #region ItemIdValue DisplayItemId

        public ItemIDValue DisplayItemId
        {
            get { return m_displayItemId; }

            set
            {
                if (value != m_displayItemId)
                {
                    m_displayItemId = value;

                    NotifyPropertyChanged("DisplayItemId");
                }
            }
        }

        private ItemIDValue m_displayItemId;

        #endregion

        #region bool UnknownBool1

        public bool UnknownBool1
        {
            get { return m_unknownBool1; }

            set
            {
                if (value != m_unknownBool1)
                {
                    m_unknownBool1 = value;

                    NotifyPropertyChanged("UnknownBool1");
                }
            }
        }

        private bool m_unknownBool1;

        #endregion

        #region byte InitialSound

        public byte InitialSound
        {
            get { return m_initialSound; }

            set
            {
                if (value != m_initialSound)
                {
                    m_initialSound = value;

                    NotifyPropertyChanged("InitialSound");
                }
            }
        }

        private byte m_initialSound;

        #endregion

        #region byte InitialCameraBehavior

        public byte InitialCameraBehavior
        {
            get { return m_initialCameraBehavior; }

            set
            {
                if (value != m_initialCameraBehavior)
                {
                    m_initialCameraBehavior = value;

                    NotifyPropertyChanged("InitialCameraBehavior");
                }
            }
        }

        private byte m_initialCameraBehavior;

        #endregion

        #region byte InitialSpeakerAnim

        public byte InitialSpeakerAnim
        {
            get { return m_initialSpeakerAnim; }

            set
            {
                if (value != m_initialSpeakerAnim)
                {
                    m_initialSpeakerAnim = value;

                    NotifyPropertyChanged("InitialSpeakerAnim");
                }
            }
        }

        private byte m_initialSpeakerAnim;

        #endregion

        #region short NumLinesPerBox

        public short NumLinesPerBox
        {
            get { return m_numLinesPerBox; }

            set
            {
                if (value != m_numLinesPerBox)
                {
                    m_numLinesPerBox = value;

                    NotifyPropertyChanged("NumLinesPerBox");
                }
            }
        }

        private short m_numLinesPerBox;

        #endregion

        #region string TextData

        public string TextData
        {
            get { return m_textData; }

            set
            {
                if (value != m_textData)
                {
                    m_textData = value;

                    NotifyPropertyChanged("TextData");
                }
            }
        }

        private string m_textData;

        #endregion

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

        public Message(EndianBinaryReader reader)
        {
            m_textDataOffset = reader.ReadInt32();

            m_messageId = reader.ReadInt16();

            m_unknownField1 = reader.ReadInt16();

            m_unknownField2 = reader.ReadInt16();

            m_unknownField3 = reader.ReadInt16();

            m_textBoxType = (BoxTypes)reader.ReadByte();

            m_initialDrawType = reader.ReadByte();

            m_textBoxPosition = (BoxPositions)reader.ReadByte();

            m_displayItemId = (ItemIDValue)reader.ReadByte();

            m_unknownBool1 = reader.ReadBoolean();

            m_initialSound = reader.ReadByte();

            m_initialCameraBehavior = reader.ReadByte();

            m_initialSpeakerAnim = reader.ReadByte();

            reader.SkipByte();

            m_numLinesPerBox = reader.ReadInt16();

            reader.SkipByte();

            m_textData = "";
        }

        public void ReadTextData(EndianBinaryReader reader)
        {
            List<char> m_charList = new List<char>();

            char m_testChar = reader.ReadChar();

            while (m_testChar != '\0')
            {
                if (char.IsLetterOrDigit(m_testChar) || char.IsWhiteSpace(m_testChar) || char.IsPunctuation(m_testChar))
                {
                    m_charList.Add(m_testChar);
                }

                if ((byte)m_testChar == 0x1A)
                {
                    byte m_controlCodeSizeTest = reader.ReadByte();

                    switch (m_controlCodeSizeTest)
                    {
                        case 5:
                            string m_controlCodeFive = GetFiveByteControlTag(reader);
                            m_charList.AddRange(m_controlCodeFive.ToCharArray(0, m_controlCodeFive.Length));
                            break;
                        case 6:
                            string m_controlCodeColor = GetColorControlTag(reader);
                            m_charList.AddRange(m_controlCodeColor.ToCharArray(0, m_controlCodeColor.Length));
                            break;
                        case 7:
                            string m_controlCodeSeven = GetSevenByteControlTag(reader);
                            m_charList.AddRange(m_controlCodeSeven.ToCharArray(0, m_controlCodeSeven.Length));
                            break;
                    }
                }

                m_testChar = reader.ReadChar();
            }

            m_textData = new string(m_charList.ToArray());
        }

        private string GetFiveByteControlTag(EndianBinaryReader reader)
        {
            string m_controlCodeString = "";

            byte typeTestByte = reader.ReadByte();

            switch (typeTestByte)
            {
                case 0:
                    FiveByteTypes fiveControlCode = (FiveByteTypes)reader.ReadInt16();
                    m_controlCodeString = "<" + ExtensionMethods.GetDescription(fiveControlCode) + ">";
                    break;
                case 1:
                    m_controlCodeString = "<sound:" + Convert.ToString(reader.ReadInt16()) + ">";
                    break;
                case 2:
                    m_controlCodeString = "<camera:" + Convert.ToString(reader.ReadInt16()) + ">";
                    break;
                case 3:
                    m_controlCodeString = "<animation:" + Convert.ToString(reader.ReadInt16()) + ">";
                    break;
            }

            return m_controlCodeString;
        }

        private string GetColorControlTag(EndianBinaryReader reader)
        {
            string codeInvariable = "<color:";

            reader.Skip(2);

            string m_controlCodeString = codeInvariable + Convert.ToString(reader.ReadInt16()) + ">";

            return m_controlCodeString;
        }

        private string GetSevenByteControlTag(EndianBinaryReader reader)
        {
            string initialCode = "";

            reader.SkipByte();

            short codeType = reader.ReadInt16();

            short codeData = reader.ReadInt16();

            switch (codeType)
            {
                case (short)SevenByteTypes.SetTextSize:
                    initialCode = "<scale:";
                    break;
                case (short)SevenByteTypes.WaitAndDismissWithPrompt:
                    initialCode = "<wait+dismiss (prompt):";
                    break;
                case (short)SevenByteTypes.WaitAndDismiss:
                    initialCode = "<wait+dismiss:";
                    break;
                case (short)SevenByteTypes.Dismiss:
                    initialCode = "<dismiss:";
                    break;
                case (short)SevenByteTypes.Dummy:
                    initialCode = "<dummy:";
                    break;
                case (short)SevenByteTypes.Wait:
                    initialCode = "<wait:";
                    break;
            }

            string m_controlCodeString = initialCode + Convert.ToString(codeData) + ">";

            return m_controlCodeString;
        }
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
    }
}
