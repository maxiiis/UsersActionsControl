using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BP_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new BPs().ShowDialog();
            Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new Systems().ShowDialog();
            Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Hide();
            new Logs().ShowDialog();
            Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Hide();
            new Alerts().ShowDialog();
            Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Hide();
            new Users().ShowDialog();
            Show();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Hide();
            new Parametres().ShowDialog();
            Show();
        }
    }
}
