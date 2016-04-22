using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTextEditor
{
    public enum BoxTypes
    {
        Dialog = 0,
        Special = 1,
        Wood = 2,
        None = 5,
        Stone = 6,
        Parchment = 7,
        Item_Get = 9,
        Hint = 10,
        Centered_Text = 13,
        Wind_Waker_Song = 14

    }

    public enum BoxPositions
    {
        Top1 = 0,
        Top2 = 1,
        Center = 2,
        Bottom1 = 3,
        Bottom2 = 4
    }

    public enum DrawTypes
    {
        By_Char_Skippable = 0,
        Instantly = 1,
        By_Char_Slow = 2
    }
}
