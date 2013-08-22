using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Analysis.Components
{
    public interface IAnalysisComponent
    {
        void Update(List<Individual> swarmInXOrder, GameTime gameTime);
        void SetVisiblity();
    }
}
