using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmEngine
{
    public static class StaticEditModeParameters
    {
        public static EditType editType = EditType.World;

        public static bool IsBrushMode()
        {
            return editType == EditType.Brush;
        }

        public static bool IsWorldMode()
        {
            return editType == EditType.World;
        }

        public static bool IsEraseMode()
        {
            return editType == EditType.Erase;
        }

        public static bool IsHandMode()
        {
            return editType == EditType.Game;
        }

        public static void SetType(string name)
        {
            if(name == "Brush")
            {
                editType = EditType.Brush;
            }
            else if (name == "World")
            {
                editType = EditType.World;
            }
            else if (name == "Erase")
            {
                editType = EditType.Erase;
            }
            else if (name == "Game")
            {
                editType = EditType.Game;
            }
            else
            {
                editType = EditType.None;
            }
        }
    }

    public enum EditType
    {
        None,
        Brush,
        World,
        Erase,
        Game
    }
}
