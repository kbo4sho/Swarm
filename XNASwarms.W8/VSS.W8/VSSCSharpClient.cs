using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace VSS
{
    public class VSSCSharpClient
    {
        //from vssClient.h 
        //DECL int AUDinit(const char *fileName);
        [DllImport("libsnd.dll", EntryPoint = "AUDinit", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int AUDinit(string filename);

        [DllImport("libsnd.dll", EntryPoint = "AUDterminate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AUDterminate(int fileHandle);

        //from vssClient.h 
        //DECL int BeginSoundServer(void);
        [DllImport("libsnd.dll")]
        public static extern int BeginSoundServer();

        [DllImport("libsnd.dll", EntryPoint = "BeginSoundServerAt", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int BeginSoundServerAt(string hostName);

        [DllImport("libsnd.dll", EntryPoint = "UpdateState", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateState(float[] data);

        [DllImport("libsnd.dll", EntryPoint = "AUDupdate", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AUDupdate(int fileHandle, string messageGroupName, int numFloats, float[] floatArray);

        [DllImport("libsnd.dll", EntryPoint = "AUDupdateTwo", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AUDupdateTwo(int theFirst, int theSecond, string messageGroupName, int numFloats, float[] floatArray);

        [DllImport("libsnd.dll", EntryPoint = "AUDupdateMany", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void AUDupdateMany(int numHandles, int[] handleArray, string messageGroupName, int numFloats, float[] floatArray);

        [DllImport("libsnd.dll", EntryPoint = "PingSoundServer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void PingSoundServer();

        [DllImport("libsnd.dll", EntryPoint = "EndSoundServer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern void EndSoundServer();

        [DllImport("libsnd.dll")]
        public static extern void AUDreset();

        /// <summary>
        /// Test Method
        /// </summary>
        /// <returns></returns>
        public static string SayHello()
        {
            return "Hello!";
        }
    }
}
