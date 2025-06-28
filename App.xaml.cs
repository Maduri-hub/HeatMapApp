namespace HeatMapApp
{
    public partial class App : Application
    {
        public App(MainPage mainPage)
        {
            InitializeComponent(); // This is generated from App.xaml

            MainPage = new AppShell(); // or mainPage directly if you are not using Shell
        }
    }
}
