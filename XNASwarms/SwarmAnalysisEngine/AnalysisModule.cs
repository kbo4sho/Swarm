using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SwarmEngine;
using Microsoft.Xna.Framework;
using Windows.Storage;

namespace SwarmAnalysisEngine
{
    public abstract class AnalysisModule
    {
        protected string ModuleName { get; private set; }
        private float TimePerFrame;
        private float TotalElapsed;
        protected Analysis analysis;


        public AnalysisModule(string modulename, float fps)
        {
            ModuleName = modulename;
            TimePerFrame = (float)(1 / fps);
            analysis = new Analysis();
        }

        public Analysis TryAnalysis(List<Individual> indvds, Rectangle viewport, float gametime, bool active)
        {
            if (CanAnalyize(gametime) && active)
            {
                return Analyze(indvds, viewport);
            }
            return analysis;
        }

        private bool CanAnalyize(float gametime)
        {
            TotalElapsed += gametime;
            if (TotalElapsed > TimePerFrame)
            {
                TotalElapsed -= TimePerFrame;
                return true;
            }

            return false;
        }

        protected virtual Analysis Analyze(List<Individual> indvds, Rectangle viewport)
        {
            return analysis;
        }

        protected virtual async void CreateReadOut(string fileName, string message = "")
        {
            try
            {
                string userContent = message;
                if (!String.IsNullOrEmpty(userContent))
                {
                    StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
                    StorageFile file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(file, userContent);
                    analysis.Messages.Add(new AnalysisMessage() { Message = "READ OUT CREATED", Type = this.ModuleName });
                }
                else
                {
                    analysis.Messages.Add(new AnalysisMessage() { Message = "The text box is empty, please write something and then click 'Write' again.", Type = this.ModuleName });
                }
            }
            catch (Exception e)
            {
                analysis.Messages.Add(new AnalysisMessage() { Message = "SAVE FAILED " + e.Message, Type = this.ModuleName });
            }

        }

    }
}
