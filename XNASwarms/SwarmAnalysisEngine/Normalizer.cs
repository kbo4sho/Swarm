using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SwarmAnalysisEngine
{
    public static class Normalizer
    {
        public static float Width;
        public static float Height;

       

        #region 0 - 1
        //NOT WORKING
        //public static float NormalizeWidth(float f)
        //{
        //    return f / Width;
        //}

        //public static float NormalizeHeight(float f)
        //{
        //    return f / Height;
        //}

        #endregion

        #region -1 - 1

        public static float NormalizeWidthCentered(float f)
        {
            return (f * 2 / (Width));
        }

        public static float NormalizeHeightCentered(float f)
        {
            return (f * 2 / (Height));
        }

        #endregion

        //public Vector2 ConvertWorldToScreen(Vector2 location)
        //{
        //    Vector3 t = new Vector3(location, 0);

        //    t = Graphics.Viewport.Project(t, _projection, _view, Matrix.Identity);

        //    return new Vector2(t.X, t.Y);
        //}

    }
}
