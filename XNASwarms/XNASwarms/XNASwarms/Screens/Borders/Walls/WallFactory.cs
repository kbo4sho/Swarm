using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNASwarms.Borders.Walls
{
    public static class WallFactory
    {
        public static List<Wall> FourBouncy(int roomwidth, int roomheight, int wallthickness)
        {
            var room = new List<Wall>();
            room.Add(new Wall(WallSideType.Left, WallActionType.Bounce, roomwidth, roomheight, wallthickness));
            room.Add(new Wall(WallSideType.Top, WallActionType.Bounce, roomwidth, roomheight, wallthickness));
            room.Add(new Wall(WallSideType.Right, WallActionType.Bounce, roomwidth, roomheight, wallthickness));
            room.Add(new Wall(WallSideType.Bottom, WallActionType.Bounce, roomwidth, roomheight, wallthickness));
            return room;
        }

        public static List<Wall> FourPortal(int roomwidth, int roomheight, int wallthickness)
        {
            var room = new List<Wall>();
            room.Add(new Wall(WallSideType.Left, WallActionType.Portal, roomwidth, roomheight, wallthickness));
            room.Add(new Wall(WallSideType.Top, WallActionType.Portal, roomwidth, roomheight, wallthickness));
            room.Add(new Wall(WallSideType.Right, WallActionType.Portal, roomwidth, roomheight, wallthickness));
            room.Add(new Wall(WallSideType.Bottom, WallActionType.Portal, roomwidth, roomheight, wallthickness));
            return room;
        }
    }
}
