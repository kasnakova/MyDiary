using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyDiary.Desktop
{
    /// <summary>
    /// Interaction logic for MessageBoxWindow.xaml
    /// </summary>
    public partial class MessageBoxWindow : Window
    {
        public MessageBoxWindow(string title, string content)
        {
            InitializeComponent();
            this.Title = title;
            this.Content.Text = content;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
