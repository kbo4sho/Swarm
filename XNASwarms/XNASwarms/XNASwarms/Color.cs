using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNASwarms
{
    public class ColourFake
    {
        private float p;
        private float p_2;
        private float p_3;

        public ColourFake(float p, float p_2, float p_3)
        {
            // TODO: Complete member initialization
            this.p = p;
            this.p_2 = p_2;
            this.p_3 = p_3;
        }
        public static Microsoft.Xna.Framework.Color CornflowerBlue { get; set; }
    }
}
