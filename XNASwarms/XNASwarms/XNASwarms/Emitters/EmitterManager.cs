using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if NETFX_CORE
using XNASwarmsXAML.W8;
#endif

namespace XNASwarms.Emitters
{
    public class EmitterManager
    {
#if NETFX_CORE
        private IAudio audioScreen;
#endif
        public List<EmitterBase> Emitters
        {
            get;
            private set;
        }
        private PopulationSimulator populationSimulator;

#if NETFX_CORE
        //TODO Should have an overload for loading emitters from saves
        public EmitterManager(PopulationSimulator simulator, IAudio audio)
        {
            audioScreen = audio;
            populationSimulator = simulator;
            Emitters = new List<EmitterBase>();
            Emitters.Add(new BrushEmitter(new Vector2(0, 0)));

        }
#else
        public EmitterManager(PopulationSimulator simulator)
        {
#if NETFX_CORE
            audioScreen = audio;
#endif
            populationSimulator = simulator;
            Emitters = new List<EmitterBase>();
            Emitters.Add(new BrushEmitter(new Vector2(0, 0)));

        }
#endif

        //TODO this will be a collection of positons 
        public void Update(Vector2 position)
        {
            foreach(EmitterBase emitter in Emitters)
            {
                if (emitter is IGuideable)
                {
                    ((IGuideable)emitter).UpdatePosition(position);
                }
#if NETFX_CORE
                if (audioScreen!=null && audioScreen.IsPlaying())
                {
                    if (emitter is IAudioInfluenced)
                    {
                        ((IAudioInfluenced)emitter).UpdateByAudio(audioScreen.GetFFTData());
                    }
                }
#endif

                if (emitter is IMeteredAgents)
                {
                    ((IMeteredAgents)emitter).CheckForSafeDistance(position);
                }

                if (emitter.IsActive)
                {
                    populationSimulator.EmitIndividual(emitter.Update());
                }
            }
        }
    }
}
