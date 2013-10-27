using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSystem.ScreenSystem.Debuging
{
    public interface IDebugComponent
    {
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime, ScreenManager screenManger);
    }
}
