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
    }
}
