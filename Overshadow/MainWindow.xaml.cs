namespace Overshadow
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Encryption_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Main.Content = new Encryption();
            Encryption.IsChecked = false;
        }

        private void Home_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Main.Content = new Primary();
            Home.IsChecked = false;
        }

        private void Settings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Main.Content = new Settings();
            Settings.IsChecked = false;
        }

        private void Compile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Main.Content = new Compile();
            Compile.IsChecked = false;
        }

        private void Close_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

        private void Mini_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = System.Windows.WindowState.Minimized;
        }

        private void Help_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Main.Content = new Help();
            Help.IsChecked = false;
        }
    }
}
