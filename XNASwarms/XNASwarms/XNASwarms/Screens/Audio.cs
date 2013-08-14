using Microsoft.Xna.Framework;
using ScreenSystem.ScreenSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XNASwarmsXAML.W8
{
    public interface IAudio
    {
        void Play();
        void Pause();
        bool IsPlaying();

        double[] GetFFTData();
    }

    /// <summary>
    /// Handles the playing of a in game audio
    /// Also exposes FFT data on the playing sound through IAudio
    /// </summary>
    public class Audio : GameScreen, IAudio
    {
        public const int num_of_fft_bands = 10; //count of  bands returned by GetFFT

        float[] fftRawData;
        double[] fftData;

        MMAudioPlayer.Player player = new MMAudioPlayer.Player();

        public Audio()
            : base()
        {
            InitAudio();
            fftData = new double[num_of_fft_bands];
            player.OnEndOfStream += player_OnEndOfStream;
        }

        void player_OnEndOfStream(MMAudioPlayer.EndOfStreamReason __param0)
        {
           
        }

        async private void InitAudio()
        {
            var file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync("Assets\\Surface-Movement.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            player.SetAudioData(stream);
            //StartAudio();
        }

        private void StartAudio()
        {
            player.Start();
        }

        public override void LoadContent()
        {
            
            //Load Songs
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (player.IsPlaying)
            {
                DoWork();
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void DoWork()
        {
            //Update FFT
            // frequency index for current FFT values returned by GetFFT is approx
            // [0] 60 Hz
            // [1] 110 Hz
            // [2] 150 Hz
            // [3] 220 Hz
            // [4] 360 Hz
            // [5] 440 Hz
            // [10] 880 Hz
            // [20] 1760 Hz
            // [41] 3520 Hz
            fftRawData = player.GetFFT();

            //Update FFT Data
            for (int i = 0; i < num_of_fft_bands; i++)
            {
                double h = fftRawData[i] * 1000;
                if (h < 0)
                {
                    h = 0;
                    fftData[i] = h;
                }
                else
                {
                    fftData[i] = h;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public override void HandleInput(InputHelper input, GameTime gameTime)
        {
            
        }

        public void Play()
        {
            player.Start();
        }

        public void Pause()
        {
            player.Stop();
        }

        public bool IsPlaying()
        {
            return player.IsPlaying;
        }

        public double[] GetFFTData()
        {
            return fftData;
        }
    }
}
