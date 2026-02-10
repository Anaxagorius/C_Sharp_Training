using System.Windows;

namespace NsccSystem
{
    public partial class SecondWindow : Window
    {
        public SecondWindow()
        {
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}