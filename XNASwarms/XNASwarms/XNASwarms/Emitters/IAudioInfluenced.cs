using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarms.Emitters
{
    public interface IAudioInfluenced
    {
        void UpdateByAudio(double[] fftData);
    }
}
