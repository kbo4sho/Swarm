
using Microsoft.Xna.Framework;

namespace XNASwarms.Borders.Walls
{
    public class Wall
    {
        private int Length;
        private int Width, Height;
        private Vector2 Position;
        private WallActionType WallActionType;
        private WallOrientationType WallOrientationType;
        private WallSideType WallSideType;
        private int Thickness;

        public Wall(WallSideType wallsidetype, WallActionType walltype,int roomwidth, int roomheight, int wallthickness)
        {
            WallActionType = walltype;
            WallSideType = wallsidetype;
            Thickness = wallthickness;

            SetWallLength(wallsidetype, roomwidth, roomheight);
            SetWidthAndHeight(wallsidetype);
            SetOrientation(wallsidetype);
            SetPosition(wallsidetype, roomwidth, roomheight);

        }

        private void SetWallLength(WallSideType wallsidetype, int roomwidth, int roomheight)
        {
            switch (wallsidetype)
            {
                case Walls.WallSideType.Left:
                    Length = roomheight;
                    break;
                case Walls.WallSideType.Top:
                    Length = roomwidth;
                    break;
                case Walls.WallSideType.Right:
                    Length = roomheight;
                    break;
                case Walls.WallSideType.Bottom:
                    Length = roomwidth;
                    break;
            }
        }

        private void SetWidthAndHeight(WallSideType wallsidetype)
        {
            switch (wallsidetype)
            {
                case Walls.WallSideType.Left:
                    Width = Thickness;
                    Height = Length * 2;
                    break;
                case Walls.WallSideType.Top:
                    Width = Length * 2;
                    Height = Thickness;
                    break;
                case Walls.WallSideType.Right:
                    Width = Thickness;
                    Height = Length * 2;
                    break;
                case Walls.WallSideType.Bottom:
                    Width = Length * 2;
                    Height = Thickness;
                    break;
            }
        }

        private void SetPosition(WallSideType wallsidetype, int roomwidth, int roomheight)
        {
            switch (wallsidetype)
            {
                case Walls.WallSideType.Left:
                    Position = new Vector2(-roomwidth, -roomheight);
                    break;
                case Walls.WallSideType.Top:
                    Position = new Vector2(-roomwidth, -roomheight);
                    break;
                case Walls.WallSideType.Right:
                    Position = new Vector2(roomwidth, -roomheight);
                    break;
                case Walls.WallSideType.Bottom:
                    Position = new Vector2(-roomwidth, roomheight);
                    break;
            }
        }

        private void SetOrientation(WallSideType wallsidetype)
        {
            switch (wallsidetype)
            {
                case Walls.WallSideType.Left:
                    this.WallOrientationType = Walls.WallOrientationType.Vertical;
                    break;
                case Walls.WallSideType.Top:
                    this.WallOrientationType = Walls.WallOrientationType.Horizontal;
                    break;
                case Walls.WallSideType.Right:
                    this.WallOrientationType = Walls.WallOrientationType.Vertical;
                    break;
                case Walls.WallSideType.Bottom:
                    this.WallOrientationType = Walls.WallOrientationType.Horizontal;
                    break;
            }
        }

        public WallActionType GetWallActionType()
        {
            return this.WallActionType; ;
        }

        public WallOrientationType GetWallOrientation()
        {
            return this.WallOrientationType;
        }

        public WallSideType GetSideType()
        {
            return this.WallSideType;
        }

        public int GetLength()
        {
            return this.Length;
        }

        public int GetWidth()
        {
            return this.Width;
        }

        public int GetHeight()
        {
            return this.Height;
        }

        public int GetX()
        {
            return (int)this.Position.X;
        }

        public int GetY()
        {
            return (int)this.Position.Y;
        }
    }
}