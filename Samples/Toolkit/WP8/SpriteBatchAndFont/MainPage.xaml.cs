using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SpriteBatchAndFont.Resources;

namespace SpriteBatchAndFont
{
    public partial class MainPage : PhoneApplicationPage
    {
        private SpriteBatchAndFontGame helloWorldGame;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            helloWorldGame = new SpriteBatchAndFontGame();
            helloWorldGame.Run(DrawingSurface);
        }
    }
}