using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNASwarms
{
    /// <summary>
    /// Used in conjuction with Border
    /// </summary>
    public interface IContainable
    {
        void TravelThroughXWall();
        void TravelThroughYWall();
        void BounceXWall();
        void BounceYWall();

    }
}
