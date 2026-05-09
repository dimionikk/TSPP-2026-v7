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

namespace TrainDispatcher
{
    public partial class LogInForm : Window
    {
        public LogInForm()
        {
            InitializeComponent();
        }

        private void AuthCheck()
        {
            if (MainWindow.logedUser.LogCheck(logTextBox.Text, passwordTextBox.Text) == 2)
            {
                Application.Current.MainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Введіть правильні дані авторизації.", 
                                "Помилка!", 
                                MessageBoxButton.OK, 
                                MessageBoxImage.Exclamation);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            AuthCheck();
        }

        private void LogInForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AuthCheck();
        }

        private void LogInForm_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}