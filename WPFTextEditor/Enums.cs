using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTextEditor
{
    public enum BoxTypes
    {
        Normal = 0,
        SpecialWithoutItemImage = 1,
        Wood = 2,
        NoBox = 5,
        Stone = 6,
        Parchment = 7,
        SpecialWithImage = 9,
        Hint = 10,
        NormalWithTextCentered = 14,
        LearningAWindWakerSong = 15

    }

    public enum BoxPositions
    {
        Top1 = 0,
        Top2 = 1,
        Center = 2,
        Bottom1 = 3,
        Bottom2 = 4
    }
}
