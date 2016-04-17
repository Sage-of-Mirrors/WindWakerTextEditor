using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WPFTextEditor
{
    //Control codes in TWW are signaled by a byte value
    //of 0x1A/26 dec. This is followed by the size of the code.
    public enum ControlCodeSizes : byte
    {
        //Control codes have three possible sizes.

        //Most common, used mostly for inserting icons or variable char data
        //Also includes dynamic drawing mode switching
        FiveBytes = 0x5,

        //Used for changing text color
        SixBytes = 0x6,

        //Used for things that affect the text, like pausing drawing
        //or setting text size
        SevenBytes = 0x7
    }

    //Control code types determine what the control code does.
    //For FiveByte types, the last byte of the code determines
    //what it does. But for some it does change. An example is the FiveByte type
    //PlaySound, which has a variable in the spot that the other
    //FiveByte types use for their identifier; thus the type byte
    //for PlaySound is actually right after the size, instead of
    //in the last byte of the code.

    public enum FiveByteTypes : byte
    {
        [Description("player")]
        PlayerName = 0x00,

        [Description("draw:instant")]
        CharDrawInstant = 0x01, //PlaySound = 0x01, but in a different field within the code

        [Description("draw:char")]
        CharDrawByChar = 0x02,

        //0x03 through 0x07 aren't used

        [Description("two choices")]
        TwoChoices = 0x08,

        [Description("three choices")]
        ThreeChoices = 0x09,

        [Description("icon:A button")]
        AButtonIcon = 0x0A,

        [Description("icon:B button")]
        BButtonIcon = 0x0B,

        [Description("icon:C stick")]
        CStickIcon = 0x0C,

        [Description("icon:L trigger")]
        LTriggerIcon = 0x0D,

        [Description("icon:R trigger")]
        RTriggerIcon = 0x0E,

        [Description("icon:X button")]
        XButtonIcon = 0x0F,

        [Description("icon:Y button")]
        YButtonIcon = 0x10,

        [Description("icon:Z button")]
        ZButtonIcon = 0x11,

        [Description("icon:D pad")]
        DPadIcon = 0x12,

        [Description("icon:control stick")]
        StaticControlStickIcon = 0x13,

        [Description("icon:left arrow")]
        LeftArrowIcon = 0x14,

        [Description("icon:right arrow")]
        RightArrowIcon = 0x15,

        [Description("icon:up arrow")]
        UpArrowIcon = 0x16,

        [Description("icon:down arrow")]
        DownArrowIcon = 0x17,

        [Description("icon:control stick (moving up)")]
        ControlStickMovingUp = 0x18,

        [Description("icon:control stick (moving down)")]
        ControlStickMovingDown = 0x19,

        [Description("icon:control stick (moving left")]
        ControlStickMovingLeft = 0x1A,

        [Description("icon:control stick (moving right)")]
        ControlStickMovingRight = 0x1B,

        [Description("icon:control stick (moving up+down)")]
        ControlStickMovingUpAndDown = 0x1C,

        [Description("icon:control stick (moving left+right)")]
        ControlStickMovingLeftAndRight = 0x1D,

        [Description("first choice")]
        ChoiceOne = 0x1E,

        [Description("second choice")]
        ChoiceTwo = 0x1F,

        [Description("canon game balls")]
        CanonGameBalls = 0x20,

        [Description("broken vase payment")]
        BrokenVasePayment = 0x21,

        [Description("auction character")]
        AuctionCharacter = 0x22,

        [Description("auction item")]
        AuctionItemName = 0x23,

        [Description("auction bid")]
        AuctionPersonBid = 0x24,

        [Description("starting auction bid")]
        AuctionStartingBid = 0x25,

        [Description("player bid selector")]
        PlayerAuctionBidSelector = 0x26,

        [Description("icon:starburst A button")]
        StarburstAIcon = 0x27,

        [Description("blows")]
        OrcaBlowCount = 0x28,

        [Description("pirate ship password")]
        PirateShipPassword = 0x29,

        [Description("icon:target starburst")]
        TargetStarburstIcon = 0x2A,

        [Description("letters")]
        PostOfficeGamePlayerLetterCount = 0x2B,

        [Description("letter sorting minigame rupees")]
        PostOfficeGameRupeeReward = 0x2C,

        [Description("post box letter count")]
        PostBoxLetterCount = 0x2D,

        [Description("Korok count")]
        RemainingKoroks = 0x2E,

        [Description("forest water time")]
        RemainingForestWaterTime = 0x2F,

        [Description("Flight Platform time")]
        FlightPlatformGameTime = 0x30,

        [Description("Flight Platform record")]
        FlightPlatformGameRecord = 0x31,

        [Description("Beedle points")]
        BeedlePointCount = 0x32,

        [Description("Ms. Marie Joy Pendants")]
        JoyPendantCountMsMarie = 0x33,

        [Description("Ms. Marie Pendant Total")]
        MsMariePendantTotal = 0x34,

        [Description("pig game time")]
        PigGameTime = 0x35,

        [Description("Boating Course rupees")]
        SailingGameRupeeReward = 0x36,

        [Description("current bomb capacity")]
        CurrentBombCapacity = 0x37,

        [Description("current arrow capacity")]
        CurrentArrowCapacity = 0x38,

        [Description("icon:heart")]
        HeartIcon = 0x39,

        [Description("icon:musical note")]
        MusicNoteIcon = 0x3A,

        [Description("target letter count")]
        TargetLetterCount = 0x3B,

        [Description("fishman hit count")]
        FishmanHitCount = 0x3C,

        [Description("fishman rupees")]
        FishmanRupeeReward = 0x3D,

        [Description("Boko Baba Seed count")]
        BokoBabaSeedCount = 0x3E,

        [Description("Skull Necklace count")]
        SkullNecklaceCount = 0x3F,

        [Description("Chu Jelly count")]
        ChuJellyCount = 0x40,

        [Description("Joy Pendant count")]
        JoyPendantCountBeedle = 0x41,

        [Description("Golden Feather count")]
        GoldenFeatherCount = 0x42,

        [Description("Knight's Crest count")]
        KnightsCrestCount = 0x43,

        [Description("Beedle price offer")]
        BeedlePriceOffer = 0x44,

        [Description("Boko Baba Seed selector")]
        BokoBabaSeedSellSelector = 0x45,

        [Description("Skull Necklace selector")]
        SkullNecklaceSellSelector = 0x46,

        [Description("Chu Jelly selector")]
        ChuJellySellSelector = 0x47,

        [Description("Joy Pendant selector")]
        JoyPendantSellSelector = 0x48,

        [Description("Golden Feather selector")]
        GoldenFeatherSellSelector = 0x49,

        [Description("Knight's Crest selector")]
        KnightsCrestSellSelector = 0x4A
    }

    //There is only one SixByte Type - SetTextColor.
    public enum SixByteTypes : byte
    {
        SetTextColor = 0x0 //Includes 0xFF in the byte after Size
    }

    //SevenByte types are like SixByte types in structure,
    //except SevenBytes can store variables in a short
    //rather than a byte.
    public enum SevenByteTypes : byte
    {
        SetTextSize = 0x01, //Includes 0xFF in the byte after Size

        WaitAndDismissWithPrompt = 0x03,

        WaitAndDismiss = 0x04,

        Dismiss = 0x05,

        Dummy = 0x06,

        Wait = 0x07
    }
}
