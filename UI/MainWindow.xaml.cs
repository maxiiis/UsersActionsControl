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
            new BPs().Show();
        }
    }
}
