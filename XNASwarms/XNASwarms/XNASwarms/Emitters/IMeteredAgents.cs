using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Emitters
{
    public interface IMeteredAgents
    {
        Vector2 lastPosition { get; set; }
        bool canDraw { get; set; }
        float Space { get; set; }

        void CheckForSafeDistance(Vector2 position);
    }
}
