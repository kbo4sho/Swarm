﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmEngine.W8
{
    public static class StaticEditModeParameters
    {
        public static EditType editType;

        public static bool IsBrushMode()
        {
            return editType == EditType.Brush;
        }

        public static bool IsWorldMode()
        {
            return editType == EditType.World;
        }

        public static bool IsHandMode()
        {
            return editType == EditType.Hand;
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
            else if (name == "Hand")
            {
                editType = EditType.Hand;
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
        Hand
    }
}
