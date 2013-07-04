using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XNASwarmsXAML.W8;

namespace XNASwarms.Emitters
{
    public class EmitterManager
    {
        private IAudio audioScreen;
        public List<EmitterBase> Emitters
        {
            get;
            private set;
        }
        private PopulationSimulator populationSimulator;

        //TODO Should have an overload for loading emitters from saves
        public EmitterManager(PopulationSimulator simulator, IAudio audio)
        {
            audioScreen = audio;
            populationSimulator = simulator;
            Emitters = new List<EmitterBase>();
            Emitters.Add(new BrushEmitter(new Vector2(0, 0)));

        }

        //TODO this will be a collection of positons 
        public void Update(Vector2 position)
        {
            foreach(EmitterBase emitter in Emitters)
            {
                if (emitter is IGuideable)
                {
                    ((IGuideable)emitter).UpdatePosition(position);
                }

                if (audioScreen.IsPlaying())
                {
                    if (emitter is IAudioInfluenced)
                    {
                        ((IAudioInfluenced)emitter).UpdateByAudio(audioScreen.GetFFTData());
                    }
                }

                if (emitter.IsActive)
                {
                    populationSimulator.EmitIndividual(emitter.Update());
                }
            }
        }
    }
}
