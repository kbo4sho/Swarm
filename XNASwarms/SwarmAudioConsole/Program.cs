using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSS;

namespace SwarmAudioConsole
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {

            int handle;
            float[] data;

            handle = 0;
            string audFile = "Soundfile Segment and Localize.aud";

            audFile = "PoemGraspThrow_11.22.12.aud";
            audFile = "Cluster_datatest.aud";

            //
            //if (VSSCSharpClient.BeginSoundServerAt("127.0.0.1") != 1)
            if (VSSCSharpClient.BeginSoundServer() != 1)
            {
                Console.WriteLine("Could Not Connect to VSS...");
                Console.WriteLine("Please make sure VSS is running on localhost. also make sure the SOUNDS folder with the audio files for demo is in the same folder as VSS.exe");
                Console.WriteLine("If BeginSoundServerAt is called VSS must be running on the specified ip address.");
                Console.ReadKey();
                return;
            }

            //VSSCSharpClient.SetPrintCommands(1);

            handle = VSSCSharpClient.AUDinit(audFile);
            /*
            if (handle < 0)
            {
                Console.WriteLine(string.Format("Failed to load audfile {0}\n", audFile ));
                Console.ReadKey();
                return;

            }
             * */

            string command = "";
            int index = 0;
            float fdata = 0.0f;
            ListCommands();
            do
            {
                Console.Write("Enter A Command:");

                command = Console.ReadLine();
                //Parse commands for data
                if (command.ToLower().StartsWith("set index"))
                {

                    int.TryParse(command.ToLower().Replace("set index", "").ToString(), out index);
                    Console.WriteLine("index set: " + index);
                }


                if (command.StartsWith("aud"))
                {

                    string AudCommand = command.Replace("aud ", "");
                    VSSCSharpClient.AUDupdate(handle, AudCommand, 0, new float[] { 0.0f });
                }

                if (command.ToLower().StartsWith("set pan"))
                {

                    float.TryParse(command.ToLower().Replace("set pan", "").ToString(), out fdata);
                    Console.WriteLine("data set: " + fdata);
                    command = "set pan";
                }

                if (command.ToLower().StartsWith("set dist"))
                {

                    float.TryParse(command.ToLower().Replace("set dist", "").ToString(), out fdata);
                    Console.WriteLine("data set: " + fdata);
                    command = "set dist";
                }

                switch (command)
                {
                    case "PlaySeries":
                    case "ps":
                        VSSCSharpClient.AUDupdate(handle, "PlaySeries", 0, new float[] { 0.0f });
                        break;

                    case "play":
                    case "po":
                        VSSCSharpClient.AUDupdate(handle, "playSeq", index, new float[] { 0.0f });
                        break;

                    case "numagents":
                        VSSCSharpClient.AUDupdate(handle, "SetNumAgents", index, new float[] { 0.2f, .3f });
                        break;

                    case "JustPause":
                    case "jp":
                        VSSCSharpClient.AUDupdate(handle, "JustPause", 0, new float[] { 0.0f });
                        break;

                    //Reverse

                    case "none":
                        VSSCSharpClient.AUDupdate(handle, "none", 0, new float[] { 0.0f });
                        break;

                    case "1":
                        VSSCSharpClient.AUDupdate(handle, "playEvent1", 0, new float[] { 0.0f });
                        break;


                    case "5":
                        VSSCSharpClient.AUDupdate(handle, "playEvent5", 0, new float[] { 0.0f });
                        break;

                    case "g1":
                        VSSCSharpClient.AUDupdate(handle, "GraspSound1", 0, new float[] { 0.0f });
                        break;
                    case "r1":
                        VSSCSharpClient.AUDupdate(handle, "ReleaseSound1", 0, new float[] { 0.0f });
                        break;

                    case "throw":
                        VSSCSharpClient.AUDupdate(handle, "ThrowSound", 0, new float[] { 0.0f });
                        break;
                    case "ethrow":
                        VSSCSharpClient.AUDupdate(handle, "EndThrow", 0, new float[] { 0.0f });
                        break;

                    //SetMyPanning
                    case "set pan":
                        //VSSCSharpClient.AUDupdate(handle, "SetMyPanning", 0, new float[] { fdata });
                        VSSCSharpClient.AUDupdateMany(1, new int[] { handle }, "SetMyPanning", 1, new float[] { fdata });
                        break;

                    case "set dist":
                        VSSCSharpClient.AUDupdateMany(1, new int[] { handle }, "SetMyDistance", 1, new float[] { fdata });
                        break;

                    //SetMyDistance
                    default:
                        Console.WriteLine(ListCommands());
                        break;

                }
            } while (command.ToLower() != "q");



            //vss.AUDreset();
            //VSSCSharpClient.AUDreset();           //not sure if I need these
            //VSSCSharpClient.AUDterminate(handle); //not sure if I need these
            VSSCSharpClient.EndSoundServer();


            Console.WriteLine("Finished...");
            Console.ReadKey();
        }
        public static string ListCommands()
        {
            string commands = string.Format("Commands:");
            commands += string.Format("\nPlaySeries, ps");
            commands += string.Format("\nPlayOne, po");
            commands += string.Format("\nJustPause, jp");
            commands += string.Format("\nset pan float");
            commands += string.Format("\nset dist float");
            commands += string.Format("\nq quit");

            return commands;
        }
    }
}
