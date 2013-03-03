using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScreenSystem.Debug
{
    public enum DebugFlagType
    {
        
        Normal = 0,
        Odd = 1,
        Important = 2,
    }

    public class DebugItem
    {
        private string Message;
        private string Label;
        private DebugFlagType DebugFlagType;

        public DebugItem(string label, string message) :this(label,message,DebugFlagType.Normal)
        {
        }
        
        public DebugItem(string label, string message, DebugFlagType flagtype)
        {
            DebugFlagType = flagtype;
            Message = message;
            Label = label;

        }

        public string GetMessage()
        {
            return Message;
        }

        public string GetLabel()
        {
            return Label;
        }

        public DebugFlagType GetFlagType()
        {
            return DebugFlagType;
        }


        public string GetFormatedMessage()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Label);
            sb.Append(":  ");
            sb.Append(Message);
            return sb.ToString();
        }

        public Color GetColor()
        {
            switch (DebugFlagType)
            {
                case Debug.DebugFlagType.Normal:
                    return Color.LightCyan;                    
                case Debug.DebugFlagType.Odd:
                    return Color.LightBlue;
                case Debug.DebugFlagType.Important:
                    return Color.LightCoral;
            }
            return Color.Blue;
            
        }

        internal void ResetFlag()
        {
            this.DebugFlagType = Debug.DebugFlagType.Normal;
        }
    }
}
