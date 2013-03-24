using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScreenSystem.ScreenSystem;
using XNASwarms.Borders.Walls;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SwarmEngine;
using ScreenSystem.Debug;
using System.Threading.Tasks;

namespace XNASwarms.Borders
{
    public class Border
    {

        private GameScreen gameScreen;
        private List<Wall> borderWalls;
        Texture2D borderTexture;
        int rightBound, bottomBound;
        private IDebugScreen debugScreen;
        Individual currentInd;

        public Border(GameScreen gamescreen, List<Wall> borderwalls, ScreenManager screenmanger)
        {
            debugScreen = gamescreen.ScreenManager.Game.Services.GetService(typeof(IDebugScreen)) as IDebugScreen;

            gameScreen = gamescreen;
            borderWalls = borderwalls;
            borderTexture = screenmanger.Content.Load<Texture2D>("Backgrounds/gray");

            rightBound = borderWalls.Where(s => s.GetWallOrientation() == WallOrientationType.Horizontal).First().GetLength();
            bottomBound = borderWalls.Where(s => s.GetWallOrientation() == WallOrientationType.Vertical).First().GetLength();
        }


        public void Update(List<Individual> individuals)
        {
            int numberOfSwarm = individuals.Count;
            int currentX, currentY;

            for (int i = 0; i < numberOfSwarm; i++)
            {
                currentInd = individuals[i];
                currentX = (int)currentInd.X;
                currentY = (int)currentInd.Y;

                if (currentX > rightBound)
                {
                    //Right
                    HandleWallAction(borderWalls[2].GetWallActionType(), borderWalls[2].GetWallOrientation(), currentInd);
                    //debugScreen.AddDebugItem("BORDER RIGHT", currentInd.getX().ToString(), ScreenSystem.Debug.DebugFlagType.Odd);
                }
                else if (currentX < -rightBound)
                {
                    //Left
                    HandleWallAction(borderWalls[0].GetWallActionType(), borderWalls[0].GetWallOrientation(), currentInd);
                    //debugScreen.AddDebugItem("BORDER LEFT", currentInd.getX().ToString());

                }

                if (currentY > bottomBound)
                {
                    //Bottom
                    HandleWallAction(borderWalls[3].GetWallActionType(), borderWalls[3].GetWallOrientation(), currentInd);

                }
                else if (currentY < -bottomBound)
                {
                    //Top
                    HandleWallAction(borderWalls[1].GetWallActionType(), borderWalls[1].GetWallOrientation(), currentInd);
                }
            }
        }

        private void HandleWallAction(WallActionType wallactiontype, WallOrientationType wallorientationtype, IContainable currentind)
        {
            switch (wallactiontype)
            {
                case Walls.WallActionType.Bounce:
                    if(wallorientationtype == WallOrientationType.Vertical)
                    {
                        currentind.BounceXWall();
                    }
                    else
                    {
                        currentind.BounceYWall();
                    }
                    break;
                case Walls.WallActionType.Portal:
                    if(wallorientationtype == WallOrientationType.Vertical)
                    {
                        currentind.TravelThroughXWall();
                    }
                    else
                    {
                        currentind.TravelThroughYWall();
                    }
                    break;
            }
        }

        public void Draw(SpriteBatch spritebatch, SwarmsCamera camera)
        {
            for (int i = 0; i < borderWalls.Count; i++)
            {
                //Vector2 position = camera.ConvertScreenToWorldAndDisplayUnits(new Vector2(0, 0));

                spritebatch.Draw(borderTexture, new Rectangle(
                    borderWalls[i].GetX(),
                    borderWalls[i].GetY(), (int)borderWalls[i].GetWidth(), (int)borderWalls[i].GetHeight()),
                    null,
                    Color.Gray,
                    0f,
                    new Vector2(0,0),
                    SpriteEffects.None, 0);
            }
        }

        public string GetWallTypeAsText()
        {
            if (borderWalls.Where(w => w.GetWallActionType() == WallActionType.Portal).Count() == 2
                && borderWalls.Where(w => w.GetWallActionType() == WallActionType.Bounce).Count() == 2)
            {
                return "Two Bouncy Two Portal";
            }
            else if(borderWalls.Where(w => w.GetWallActionType() == WallActionType.Portal).Count() == 4)
            {
                return "Portal";
            }
            else if (borderWalls.Where(w => w.GetWallActionType() == WallActionType.Bounce).Count() == 4)
            {
               return "Bouncy";
            }
            else
            {
                return "";
            }

            return borderWalls[0].GetWallActionType().ToString();
        }
    }
}
