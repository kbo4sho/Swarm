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
            this.InitializeComponent();

            // Create the game.
            _game = XamlGame<Game1>.Create(launchArguments, Window.Current.CoreWindow, this);
            DataContext = App.ViewModel;
            myappbar.Tapped += myappbar_Tapped;
        }

        void myappbar_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DataContext = new WorldControlsViewModel();
        }


        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Call static class
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
           //Call Static class
        }
    }
}
