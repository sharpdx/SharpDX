using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace MiniCube.SwitchContext.WinRTXaml
{
    using SharpDX.Toolkit;

    sealed partial class App
    {
        private readonly Game game;
        private int pageNumber;

        public App()
        {
            InitializeComponent();

            game = new MiniCubeGame();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            ActivatePage();
            Window.Current.Activate();
        }

        private void ActivatePage()
        {
            var previousPage = Window.Current.Content as MainPage;
            if (previousPage != null)
                previousPage.button.Click -= HandleMainPageButtonClick;

            var page = new MainPage(game);
            page.button.Click += HandleMainPageButtonClick;
            page.text.Text = "Page " + (pageNumber++);

            Window.Current.Content = page;
        }

        private void HandleMainPageButtonClick(object sender, RoutedEventArgs e)
        {
            ActivatePage();
        }
    }
}
