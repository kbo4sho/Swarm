using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarms.Screens.Emitters;

namespace XNASwarms.Screens.Emitters
{
    public class EmitterManager
    {
        public List<EmitterBase> Emitters
        {
            get;
            private set;
        }

        //TODO Should have an overload for loading emitters from saves
        public EmitterManager(EmitterBase emitter)
        {
            Emitters = new List<EmitterBase>();
            Emitters.Add(emitter);
        }

        public void Update()
        {
            foreach(EmitterBase emitter in Emitters)
            {
            }
        }
    }
}
