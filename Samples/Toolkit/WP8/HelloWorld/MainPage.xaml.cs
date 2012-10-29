using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using HelloWorld.Resources;

namespace HelloWorld
{
    public partial class MainPage : PhoneApplicationPage
    {
        private HelloWorldGame helloWorldGame;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            helloWorldGame = new HelloWorldGame();
            helloWorldGame.Run(DrawingSurface);
        }
    }
}