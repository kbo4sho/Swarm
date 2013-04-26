using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSS;

namespace SwarmAudio
{
    public class SoundEngine
    {

        static int handle;

        public static void Init()
        {
            string audFile = "Soundfile Segment and Localize.aud";

            audFile = "PoemGraspThrow_11.22.12.aud";
            audFile = "Cluster_datatest.aud";

            //if (VSSCSharpClient.BeginSoundServerAt("127.0.0.1") != 1)
            if (VSSCSharpClient.BeginSoundServer() != 1)
            {
                Console.WriteLine("Could Not Connect to VSS...");
                Console.WriteLine("Please make sure VSS is running on localhost. also make sure the SOUNDS folder with the audio files for demo is in the same folder as VSS.exe");
                Console.WriteLine("If BeginSoundServerAt is called VSS must be running on the specified ip address.");
                Console.ReadKey();
                return;
            }

            handle = VSSCSharpClient.AUDinit(audFile);

            if (handle < 0)
            {
                Console.WriteLine(string.Format("Failed to load audfile {0}\n", audFile));
                Console.ReadKey();
                return;
            }
        }

        public static void TestData()
        {
            //VSSCSharpClient.AUDupdate(handle, "test", 0, new float[] { 0.0f });
            VSSCSharpClient.AUDupdate(handle, "playSeq", 0, new float[] { 0.0f });
        }
    }
}
