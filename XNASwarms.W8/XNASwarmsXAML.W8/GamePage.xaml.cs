using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MonoGame.Framework;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;
using System.Collections.Generic;
using XNASwarms;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using XNASwarmsXAML.W8.Authoring.ViewModels;
using Windows.Foundation;
using Windows.UI.Xaml.Media;


namespace XNASwarmsXAML.W8
{
    /// <summary>
    /// The root page used to display the game.
    /// </summary>
    public sealed partial class GamePage : SwapChainBackgroundPanel
    {
        readonly Game1 _game;

        public GamePage(string launchArguments)
        {
            DataContext = App.ViewModel;
            _game = XamlGame<Game1>.Create(launchArguments, Window.Current.CoreWindow, this);
            this.InitializeComponent();
        }
    }

    public static class MyHelper
    {
        //Get the rotation angle from the value
        public static double GetAngle(double value, double maximum, double minimum)
        {
            double current = (value / (maximum - minimum)) * 360;
            if (current == 360)
                current = 359.999;

            return current;
        }

        //Get the rotation angle from the position of the mouse
        public static double GetAngleR(Point pos, double radius)
        {
            //Calculate out the distance(r) between the center and the position
            Point center = new Point(radius, radius);
            double xDiff = center.X - pos.X;
            double yDiff = center.Y - pos.Y;
            double r = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

            //Calculate the angle
            double angle = Math.Acos((center.Y - pos.Y) / r);
            //Console.WriteLine("r:{0},y:{1},angle:{2}.", r, pos.Y, angle);
            if (pos.X < radius)
                angle = 2 * Math.PI - angle;
            if (Double.IsNaN(angle))
                return 0.0;
            else
                return angle;
        }
    }
}


