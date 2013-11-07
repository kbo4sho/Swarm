using Microsoft.Xna.Framework;
using SwarmEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmAnalysisEngine
{
    public class SwarmModule : AnalysisModule
    {
        private const string messageHeader = "SendData AgentDataRefresh [";
        private const string messageFooter = "];";
        private const int sampleCount = 60;
        private const string fileName = "60SamplesOf50Agents.dat";

        private int lineCount;
        private StringBuilder builder;
        public SwarmModule()
            : base("Swarm Module", 2)   
        {
        }

        private void BuilderInit(List<Individual> indvds, Rectangle viewport)
        {
            if (builder == null)
            {
                builder = new StringBuilder("File: " + fileName + "  Samples: " + sampleCount + "/2hz  Agent Count: " + indvds.Count + "  Viewport Size: " + viewport.Width + " by " + viewport.Height);
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }
        }
        protected override Analysis Analyze(List<Individual> indvds, Rectangle viewport)
        {
            BuilderInit(indvds, viewport);

            Normalizer.Width = viewport.Width;
            Normalizer.Height = viewport.Height;

            lineCount++;

            builder.Append(messageHeader);
            //BUILD THE MESSAGE
            foreach (Individual indvd in indvds)
            {
                builder.Append(Normalizer.NormalizeWidthCentered((float)indvd.Position.X) + "," + Normalizer.NormalizeHeightCentered((float)indvd.Position.Y) + ",");
            }

            builder.Replace(",", "", builder.Length - 1, 1);

            builder.Append(messageFooter);

            builder.Append(Environment.NewLine);

            analysis.Messages.Add(new AnalysisMessage() { Message = "Just Wrote line " + lineCount, Type = this.ModuleName });

            if (lineCount >= sampleCount)
            {
                if (lineCount == sampleCount)
                {
                    //CREATE READOUT
                    CreateReadOut(fileName, builder.ToString());
                }

                lineCount = 0;

                analysis.Messages.Add(new AnalysisMessage() { Message = "LINE COUNT RESET", Type = this.ModuleName });
            }

            return base.Analyze(indvds, viewport);
        }

        protected override void CreateReadOut(string fileName, string message = "")
        {
            base.CreateReadOut(fileName, message);
        }
    }
}
