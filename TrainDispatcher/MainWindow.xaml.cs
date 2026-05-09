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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TrainDispatcher
{
    public partial class MainWindow : Window
    {
        public static Authorization logedUser = new Authorization();
        public static DataAccess DataConnection;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InfoTrainForm_Loaded(object sender, RoutedEventArgs e)
        {
            TrainsMenuItem.Visibility = Visibility.Hidden;
            TrainsMenuItem.Width = 0;

            DataConnection = new DataAccess();
            TrainListDG.ItemsSource = DataConnection.fList;
        }

        private void InfoTrainForm_Activated(object sender, EventArgs e)
        {
            if (Authorization.logUser == 2)
            {
                TrainsMenuItem.Visibility = Visibility.Visible;
                TrainsMenuItem.Width = 50;
            }
            else
            {
                TrainsMenuItem.Visibility = Visibility.Hidden;
                TrainsMenuItem.Width = 0;
            }
        }

        private void AuthMenuItem_Click(object sender, RoutedEventArgs e)
        {
            LogInForm logWnd = new LogInForm();
            logWnd.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void LoadDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TrainListDG.ItemsSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) +
                    char.ConvertFromUtf32(13) +
                    "Для завантаження даних виконайте команду Файл-Завантажити",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            DataConnection = new DataAccess();
            TrainListDG.ItemsSource = DataConnection.fList;
        }
    }
}