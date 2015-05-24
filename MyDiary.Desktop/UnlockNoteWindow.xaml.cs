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
    /// Interaction logic for UnlockNoteWindow.xaml
    /// </summary>
    public partial class UnlockNoteWindow : Window
    {
        public UnlockNoteWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void CloseDialog(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.TextBoxPassword.Password))
            {
                this.ErrorMessage.Visibility = Visibility.Visible;
            }
            else
            {
                this.Close();
            }
        }
    }
}
