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
    public class Message : INotifyPropertyChanged
    {
        public int TextDataOffset
        {
            get { return m_textDataOffset; }
        }

        private int m_textDataOffset;

        private int m_index;

        public int Index
        {
            get { return m_index; }
            set
            {
                if (value != m_index)
                {
                    m_index = value;
                    NotifyPropertyChanged("Index");
                }
            }
        }

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

        #region short Item Price

        public short ItemPrice
        {
            get { return m_itemPrice; }

            set
            {
                if (value != m_itemPrice)
                {
                    m_itemPrice = value;

                    NotifyPropertyChanged("UnknownField1");
                }
            }
        }

        private short m_itemPrice;

        #endregion

        #region short Next Message ID

        public short NextMessageID
        {
            get { return m_nextMessageID; }

            set
            {
                if (value != m_nextMessageID)
                {
                    m_nextMessageID = value;

                    NotifyPropertyChanged("UnknownField2");
                }
            }
        }

        private short m_nextMessageID;

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

        public DrawTypes InitialDrawType
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

        private DrawTypes m_initialDrawType;

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

        public ItemID DisplayItemId
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

        private ItemID m_displayItemId;

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

        public Message(short id, int index)
        {
            MessageId = id;
            Index = index;
            UnknownField3 = 96;
            TextBoxPosition = BoxPositions.Bottom1;
            DisplayItemId = ItemID.No_item;
            TextData = "";
        }

        public Message(EndianBinaryReader reader)
        {
            m_textDataOffset = reader.ReadInt32();
            m_messageId = reader.ReadInt16();
            m_itemPrice = reader.ReadInt16();
            m_nextMessageID = reader.ReadInt16();

            m_unknownField3 = reader.ReadInt16();

            m_textBoxType = (BoxTypes)reader.ReadByte();
            m_initialDrawType = (DrawTypes)reader.ReadByte();
            m_textBoxPosition = (BoxPositions)reader.ReadByte();
            m_displayItemId = (ItemID)reader.ReadByte();

            m_unknownBool1 = reader.ReadBoolean();

            m_initialSound = reader.ReadByte();
            m_initialCameraBehavior = reader.ReadByte();
            m_initialSpeakerAnim = reader.ReadByte();

            reader.SkipByte();

            m_numLinesPerBox = reader.ReadInt16();

            reader.SkipByte();

            m_textData = "";
        }

        public void ReadTextData(EndianBinaryReader reader, Encoding encoding)
        {
            if (MessageId == 160)
            {

            }

            List<byte> charList = new List<byte>();

            byte nextByte = reader.ReadByte();

            while (nextByte != 0)
            {
                // Not a control code
                if (nextByte != 0x1A)
                {
                    //if (nextByte == '\\')
                        //charList.Add((byte)'\\');

                    charList.Add(nextByte);
                }

                // Control code
                else
                {
                    byte m_controlCodeSizeTest = reader.ReadByte();

                    switch (m_controlCodeSizeTest)
                    {
                        case 5:
                            string controlCode5 = GetFiveByteControlTag(reader);
                            charList.AddRange(encoding.GetBytes(controlCode5.ToCharArray()));
                            break;
                        case 6:
                            string controlCodeCol = GetColorControlTag(reader);
                            charList.AddRange(encoding.GetBytes(controlCodeCol.ToCharArray()));
                            break;
                        case 7:
                            string controlCode7 = GetSevenByteControlTag(reader);
                            charList.AddRange(encoding.GetBytes(controlCode7.ToCharArray()));
                            break;
                        default:
                            string controlCodeFuri = GetFuriganaControlTag(reader, m_controlCodeSizeTest, encoding);
                            charList.AddRange(encoding.GetBytes(controlCodeFuri.ToCharArray()));
                            break;
                    }
                }

                nextByte = reader.ReadByte();
            }

            m_textData = encoding.GetString(charList.ToArray());
        }

        public byte[] WriteMessage(EndianBinaryWriter writer, Encoding encoding)
        {
            writer.Write((int)0); // Text offset placeholder
            writer.Write((ushort)m_messageId);
            writer.Write((ushort)m_itemPrice);
            writer.Write((ushort)m_nextMessageID);
            writer.Write((ushort)m_unknownField3);
            writer.Write((byte)m_textBoxType);
            writer.Write((byte)m_initialDrawType);
            writer.Write((byte)m_textBoxPosition);
            writer.Write((byte)m_displayItemId);
            writer.Write((bool)m_unknownBool1);
            writer.Write((byte)m_initialSound);
            writer.Write((byte)m_initialCameraBehavior);
            writer.Write((byte)m_initialSpeakerAnim);
            writer.Write((byte)0); // Padding?
            writer.Write((ushort)m_numLinesPerBox);
            writer.Write((byte)0); // Padding.

            return TextToByteArray(encoding);
        }

        private byte[] TextToByteArray(Encoding encoding)
        {
            if (MessageId == 7401)
            {

            }

            List<byte> outList = new List<byte>();

            int i = 0;

            while (i < TextData.Length)
            {
                int index = TextData.IndexOf('\\', i);
                int count = ((index < 0 ? TextData.Length : index) - i);

                byte[] bytes = encoding.GetBytes(TextData.Substring(i, count));
                outList.AddRange(bytes);

                i += count;

                if (i == TextData.Length)
                    break;

                i++;

                try
                {
                    switch (TextData[i])
                    {
                        case '<':
                            i++;
                            int end = TextData.IndexOf('>', i);
                            string tag = TextData.Substring(i, end - i);
                            bytes = ProcessControlTag(tag, encoding);
                            outList.AddRange(bytes);
                            i = end + 1;
                            break;
                        case '\\':
                            i++;
                            outList.Add((byte)'\\');
                            break;
                    }
                }
                catch
                {
                    throw new FormatException();
                }
            }

            if (m_messageId != 0)
                outList.Add(0);

            return outList.ToArray();
        }

        /*private byte[] TextToByteArray(Encoding encoding)
        {
            byte[] output = new byte[1];

            List<byte> charData = new List<byte>(encoding.GetBytes(m_textData));
            List<byte> newData = new List<byte>();

            for (int i = 0; i < charData.Count; i++)
            {
                if (charData[i] != '\\')
                {
                    if (charData[i] == (char)0xD)
                    {
                        continue;
                    }

                    newData.Add(charData[i]);
                    continue;
                }

                if (i + 1 < charData.Count)
                {
                    if (charData[i + 1] == '\\')
                    {
                        int index = i + 1;

                        while (charData[index] != '<')
                        {
                            newData.Add(charData[index]);
                            index++;
                        }

                        charData.Remove(charData[i]);
                        //i--;
                    }
                    else if (charData[i + 1] == '<')
                    {
                        charData.Remove(charData[i]);
                        //i++;

                        int tagSize = 0;
                        byte[] code = ProcessControlTag(charData, i, out tagSize);

                        charData.InsertRange((int)(i + tagSize), code.ToArray());
                        charData.RemoveRange(i, (int)tagSize);

                        i += code.Length - 1;
                    }
                }
            }

            if (charData.Count != 0)
                if (charData[charData.Count - 1] != 0)
                    charData.Add(0);

            return charData.ToArray();
        }*/

        private byte[] ProcessControlTag(string tag, Encoding encoding)
        {
            List<byte> tagBuffer = new List<byte>();

            string[] tagArgs = tag.Split(':');

            tagArgs[0] = tagArgs[0].ToLower();

            if (tagArgs.Length > 1)
            {
                tagArgs[1] = tagArgs[1].ToLower();
            }

            List<byte> code = new List<byte>();

            switch (tagArgs[0])
            {
                #region Five-Byte Codes
                case "player":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PlayerName, 0);
                    break;

                case "draw":
                    if (tagArgs.Length > 1)
                    {
                        if (tagArgs[1] == "instant")
                        {
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.CharDrawInstant, 0);
                        }

                        if (tagArgs[1] == "char")
                        {
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.CharDrawByChar, 0);
                        }

                        else
                        {
                            //Error handler
                        }
                    }
                    break;

                case "two choices":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.TwoChoices, 0);
                    break;

                case "three choices":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ThreeChoices, 0);
                    break;

                case "icon":
                    switch (tagArgs[1])
                    {
                        case "a button":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.AButtonIcon, 0);
                            break;
                        case "b button":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.BButtonIcon, 0);
                            break;
                        case "c stick":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.CStickIcon, 0);
                            break;
                        case "d pad":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.DPadIcon, 0);
                            break;
                        case "l trigger":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.LTriggerIcon, 0);
                            break;
                        case "r trigger":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.RTriggerIcon, 0);
                            break;
                        case "x button":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.XButtonIcon, 0);
                            break;
                        case "y button":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.YButtonIcon, 0);
                            break;
                        case "z button":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ZButtonIcon, 0);
                            break;
                        case "control stick (all directions)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.StaticControlStickIcon, 0);
                            break;
                        case "control stick (moving up)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingUp, 0);
                            break;
                        case "control stick (moving down)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingDown, 0);
                            break;
                        case "control stick (moving left)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingLeft, 0);
                            break;
                        case "control stick (moving right)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingRight, 0);
                            break;
                        case "control stick (moving up+down)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingUpAndDown, 0);
                            break;
                        case "control stick (moving left+right)":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingLeftAndRight, 0);
                            break;
                        case "left arrow":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.LeftArrowIcon, 0);
                            break;
                        case "right arrow":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.RightArrowIcon, 0);
                            break;
                        case "up arrow":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.UpArrowIcon, 0);
                            break;
                        case "down arrow":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.DownArrowIcon, 0);
                            break;
                        case "flashing a button":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.StarburstAIcon, 0);
                            break;
                        case "target starburst":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.TargetStarburstIcon, 0);
                            break;
                        case "heart":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.HeartIcon, 0);
                            break;
                        case "music note":
                            code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.HeartIcon, 0);
                            break;
                    }
                    break;

                case "control stick":
                    if (tagArgs.Length == 1)
                    {
                        code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.StaticControlStickIcon, 0);
                    }

                    else
                    {
                        switch (tagArgs[1])
                        {
                            #region Direction Switch
                            case "up":
                                code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingUp, 0);
                                break;

                            case "down":
                                code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingDown, 0);
                                break;

                            case "left":
                                code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingLeft, 0);
                                break;

                            case "right":
                                code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingRight, 0);
                                break;

                            case "up+down":
                                code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingUpAndDown, 0);
                                break;

                            case "left+right":
                                code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ControlStickMovingLeftAndRight, 0);
                                break;

                            default:
                                //Error handling
                                break;
                                #endregion
                        }
                    }
                    break;

                case "first choice":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ChoiceOne, 0);
                    break;

                case "second choice":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ChoiceTwo, 0);
                    break;

                case "canon game balls":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.CanonGameBalls, 0);
                    break;

                case "broken vase payment":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.BrokenVasePayment, 0);
                    break;

                case "auction character":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.AuctionCharacter, 0);
                    break;

                case "auction item":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.AuctionItemName, 0);
                    break;

                case "auction bid":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.AuctionPersonBid, 0);
                    break;

                case "starting auction bid":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.AuctionStartingBid, 0);
                    break;

                case "player bid selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PlayerAuctionBidSelector, 0);
                    break;

                case "blows":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.OrcaBlowCount, 0);
                    break;

                case "pirate ship password":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PirateShipPassword, 0);
                    break;

                case "player letter count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PostOfficeGamePlayerLetterCount, 0);
                    break;

                case "letter rupee reward":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PostOfficeGameRupeeReward, 0);
                    break;

                case "letters":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PostBoxLetterCount, 0);
                    break;

                case "korok count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.RemainingKoroks, 0);
                    break;

                case "forest water time":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.RemainingForestWaterTime, 0);
                    break;

                case "flight platform time":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.FlightPlatformGameTime, 0);
                    break;

                case "flight platform record":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.FlightPlatformGameRecord, 0);
                    break;

                case "beedle points":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.BeedlePointCount, 0);
                    break;

                case "ms. marie joy pendants":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.JoyPendantCountMsMarie, 0);
                    break;

                case "ms. marie pendant total":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.MsMariePendantTotal, 0);
                    break;

                case "pig game time":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.PigGameTime, 0);
                    break;

                case "boating course rupees":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.SailingGameRupeeReward, 0);
                    break;

                case "current bomb capacity":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.CurrentBombCapacity, 0);
                    break;

                case "current arrow capacity":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.CurrentArrowCapacity, 0);
                    break;

                case "target letter count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.TargetLetterCount, 0);
                    break;

                case "fishman hit count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.FishmanHitCount, 0);
                    break;

                case "fishman rupees":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.FishmanRupeeReward, 0);
                    break;

                case "boko baba seed count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.BokoBabaSeedCount, 0);
                    break;

                case "skull necklace count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.SkullNecklaceCount, 0);
                    break;

                case "chu jelly count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ChuJellyCount, 0);
                    break;

                case "joy pendant count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.JoyPendantCountBeedle, 0);
                    break;

                case "golden feather count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.GoldenFeatherCount, 0);
                    break;

                case "knight's crest count":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.KnightsCrestCount, 0);
                    break;

                case "beedle price offer":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.BeedlePriceOffer, 0);
                    break;

                case "boko baba seed selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.BokoBabaSeedSellSelector, 0);
                    break;

                case "skull necklace selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.SkullNecklaceSellSelector, 0);
                    break;

                case "chu jelly selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.ChuJellySellSelector, 0);
                    break;

                case "joy pendant selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.JoyPendantSellSelector, 0);
                    break;

                case "golden feather selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.GoldenFeatherSellSelector, 0);
                    break;

                case "knight's crest selector":
                    code = ConvertTagToFiveByteControlCode(0, (byte)FiveByteTypes.KnightsCrestSellSelector, 0);
                    break;

                case "sound":
                    if (tagArgs.Length > 1)
                    {
                        code = ConvertTagToFiveByteControlCode(1, 1, Convert.ToInt16(tagArgs[1]));
                    }

                    else
                    {
                        //Error handler
                    }
                    break;

                case "camera":
                    if (tagArgs.Length > 1)
                    {
                        code = ConvertTagToFiveByteControlCode(2, 2, Convert.ToInt16(tagArgs[1]));
                    }

                    else
                    {
                        //Error handler
                    }
                    break;

                case "animation":
                    if (tagArgs.Length > 1)
                    {
                        code = ConvertTagToFiveByteControlCode(3, 3, Convert.ToInt16(tagArgs[1]));
                    }

                    else
                    {
                        //Error handler
                    }
                    break;
                #endregion

                #region Six-Byte Code
                case "color":
                    if (tagArgs.Length > 1)
                    {
                        byte[] arg = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));
                        code.Add(0x1A);
                        code.Add(0x06);
                        code.Add(0xFF);
                        code.Add(0x00);
                        code.Add(arg[1]);
                        code.Add(arg[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;
                #endregion

                #region Seven-Byte Codes
                case "scale":
                    if (tagArgs.Length > 1)
                    {
                        code.Add(0x1A);
                        code.Add(0x07);
                        code.Add(0xFF);
                        code.Add(0x00);
                        code.Add((byte)SevenByteTypes.SetTextSize);

                        byte[] tempShort = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));

                        code.Add(tempShort[1]);
                        code.Add(tempShort[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;

                case "wait+dismiss (prompt)":
                    if (tagArgs.Length > 1)
                    {
                        code.Add(0x1A);
                        code.Add(0x07);
                        code.Add(0x00);
                        code.Add(0x00);
                        code.Add((byte)SevenByteTypes.WaitAndDismissWithPrompt);

                        byte[] tempShort = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));

                        code.Add(tempShort[1]);
                        code.Add(tempShort[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;

                case "wait+dismiss":
                    if (tagArgs.Length > 1)
                    {
                        code.Add(0x1A);
                        code.Add(0x07);
                        code.Add(0x00);
                        code.Add(0x00);
                        code.Add((byte)SevenByteTypes.WaitAndDismiss);

                        byte[] tempShort = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));

                        code.Add(tempShort[1]);
                        code.Add(tempShort[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;

                case "dismiss":
                    if (tagArgs.Length > 1)
                    {
                        code.Add(0x1A);
                        code.Add(0x07);
                        code.Add(0x00);
                        code.Add(0x00);
                        code.Add((byte)SevenByteTypes.Dismiss);

                        byte[] tempShort = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));

                        code.Add(tempShort[1]);
                        code.Add(tempShort[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;

                case "dummy":
                    if (tagArgs.Length > 1)
                    {
                        code.Add(0x1A);
                        code.Add(0x07);
                        code.Add(0x00);
                        code.Add(0x00);
                        code.Add((byte)SevenByteTypes.Dummy);

                        byte[] tempShort = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));

                        code.Add(tempShort[1]);
                        code.Add(tempShort[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;

                case "wait":
                    if (tagArgs.Length > 1)
                    {
                        code.Add(0x1A);
                        code.Add(0x07);
                        code.Add(0x00);
                        code.Add(0x00);
                        code.Add((byte)SevenByteTypes.Wait);

                        byte[] tempShort = BitConverter.GetBytes(Convert.ToInt16(tagArgs[1]));

                        code.Add(tempShort[1]);
                        code.Add(tempShort[0]);
                    }

                    else
                    {
                        //error handling
                    }
                    break;
                case "furigana":
                    string[] furArgs = tagArgs[1].Split(',');
                    byte stride = Convert.ToByte(furArgs[0]);

                    int numChars = furArgs[1].Length;
                    int codeLength = 6 + (2 * numChars);

                    code.Add(0x1A);
                    code.Add((byte)codeLength);
                    code.Add(255);
                    code.Add(0);
                    code.Add(2);
                    code.Add(stride);

                    byte[] charBytes = encoding.GetBytes(furArgs[1]);
                    code.AddRange(charBytes);
                    break;
                #endregion
                default:
                    break;
            }

            return code.ToArray();
        }

        private string GetFiveByteControlTag(EndianBinaryReader reader)
        {
            string m_controlCodeString = "";

            byte typeTestByte = reader.ReadByte();

            switch (typeTestByte)
            {
                case 0:
                    FiveByteTypes fiveControlCode = (FiveByteTypes)reader.ReadInt16();
                    m_controlCodeString = "\\<" + ExtensionMethods.GetDescription(fiveControlCode) + ">";
                    break;
                case 1:
                    m_controlCodeString = "\\<sound:" + Convert.ToString(reader.ReadInt16()) + ">";
                    break;
                case 2:
                    m_controlCodeString = "\\<camera:" + Convert.ToString(reader.ReadInt16()) + ">";
                    break;
                case 3:
                    m_controlCodeString = "\\<animation:" + Convert.ToString(reader.ReadInt16()) + ">";
                    break;
            }

            return m_controlCodeString;
        }

        private string GetColorControlTag(EndianBinaryReader reader)
        {
            string codeInvariable = "\\<color:";

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
                    initialCode = "\\<scale:";
                    break;
                case (short)SevenByteTypes.WaitAndDismissWithPrompt:
                    initialCode = "\\<wait+dismiss (prompt):";
                    break;
                case (short)SevenByteTypes.WaitAndDismiss:
                    initialCode = "\\<wait+dismiss:";
                    break;
                case (short)SevenByteTypes.Dismiss:
                    initialCode = "\\<dismiss:";
                    break;
                case (short)SevenByteTypes.Dummy:
                    initialCode = "\\<dummy:";
                    break;
                case (short)SevenByteTypes.Wait:
                    initialCode = "\\<wait:";
                    break;
            }

            string m_controlCodeString = initialCode + Convert.ToString(codeData) + ">";

            return m_controlCodeString;
        }

        private string GetFuriganaControlTag(EndianBinaryReader reader, int codeSize, Encoding encoding)
        {
            List<char> tagList = new List<char>();
            tagList.AddRange("\\<furigana:");
            codeSize -= 6;

            reader.SkipByte();

            short type = reader.ReadInt16();

            if (type != 2)
                throw new Exception("Furigana tag type was not 2!");

            byte characterStride = reader.ReadByte();
            tagList.AddRange($"{ characterStride },");

            while (codeSize > 0)
            {
                byte[] newChar = reader.ReadBytes(2);
                tagList.AddRange(encoding.GetChars(newChar));
                codeSize -= 2;
            }

            tagList.Add('>');
            return new string(tagList.ToArray());
        }

        public List<byte> ConvertTagToFiveByteControlCode(byte field2Type, byte normalType, short arg)
        {
            List<byte> code = new List<byte>();

            byte[] temp;

            code.Add(0x1A);
            code.Add(0x05);

            switch (field2Type)
            {
                case 0:
                    code.Add(0x00);
                    code.Add(0x00);
                    code.Add(normalType);
                    break;
                case 1:
                case 2:
                case 3:
                    code.Add(field2Type);

                    temp = BitConverter.GetBytes(Convert.ToInt16(arg));

                    code.Add(temp[1]);
                    code.Add(temp[0]);
                    break;
            }

            return code;
        }
    }
}
