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
        public static Train editedTrain;
        public static EditDB editedRow = new EditDB();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InfoTrainForm_Loaded(object sender, RoutedEventArgs e)
        {
            TrainsMenuItem.Visibility = Visibility.Hidden;
            TrainsMenuItem.Width = 0;

            trainGroupBox.Visibility = Visibility.Hidden;

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

        private void EditDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            trainGroupBox.Visibility = Visibility.Visible;
            TrainListDG.SelectedIndex = -1;
            editedTrain = null;

            MessageBox.Show("Оберіть у списку запис для редагування подвійним кліком",
                            "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void TrainListDG_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            editedTrain = TrainListDG.SelectedItem as Train;
            try
            {
                numTrainTextBox.Text = editedTrain.train_number;
                destTrainTextBox.Text = editedTrain.destination;
                depTimeTextBox.Text = editedTrain.departure_time.ToString(@"hh\:mm");
                travTimeTextBox.Text = editedTrain.travel_time.ToString(@"hh\:mm");
                ticketsTextBox.Text = editedTrain.tickets_available.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            editedRow = new EditDB();
            editedRow.trainNum = TrainListDG.SelectedIndex;
            editedRow.trainAdd = false;
        }

        private void ChangeTrainListData(int num)
        {
            TimeSpan depTime;
            TimeSpan travTime;
            int tickets;

            editedTrain.train_number = numTrainTextBox.Text;
            editedTrain.destination = destTrainTextBox.Text;

            if (!TimeSpan.TryParse(depTimeTextBox.Text, out depTime))
            {
                MessageBox.Show("Невірний формат часу відправлення. Використовуйте формат гг:хх",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            editedTrain.departure_time = depTime;

            if (!TimeSpan.TryParse(travTimeTextBox.Text, out travTime))
            {
                MessageBox.Show("Невірний формат часу у дорозі. Використовуйте формат гг:хх",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            editedTrain.travel_time = travTime;

            if (!int.TryParse(ticketsTextBox.Text, out tickets))
            {
                MessageBox.Show("Невірний формат кількості квитків. Введіть ціле число",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            editedTrain.tickets_available = tickets;

            DataConnection.fList[num] = editedTrain;

            TrainListDG.ItemsSource = null;
            TrainListDG.ItemsSource = DataConnection.fList;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (editedTrain == null || TrainListDG.SelectedIndex < 0)
            {
                MessageBox.Show("Оберіть у списку запис для редагування подвійним кліком",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                ChangeTrainListData(TrainListDG.SelectedIndex);
                editedRow.ChangeDBRow();
            }
        }
    }
}