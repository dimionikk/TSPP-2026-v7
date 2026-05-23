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

        private SelectData selData = new SelectData();
        private bool isSearchXY = false; 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InfoTrainForm_Loaded(object sender, RoutedEventArgs e)
        {
            TrainsMenuItem.Visibility = Visibility.Hidden;
            TrainsMenuItem.Width = 0;

            trainGroupBox.Visibility = Visibility.Hidden;
            searchGroupBox.Visibility = Visibility.Hidden;

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
            searchGroupBox.Visibility = Visibility.Hidden;
            trainGroupBox.Visibility = Visibility.Visible;
            TrainListDG.SelectedIndex = -1;
            editedTrain = null;

            MessageBox.Show("Оберіть у списку запис для редагування подвійним кліком",
                            "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void AddDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            searchGroupBox.Visibility = Visibility.Hidden;
            trainGroupBox.Visibility = Visibility.Visible;
            TrainListDG.SelectedIndex = -1;

            numTrainTextBox.Text = "";
            destTrainTextBox.Text = "";
            depTimeTextBox.Text = "";
            travTimeTextBox.Text = "";
            ticketsTextBox.Text = "";

            editedRow = new EditDB();
            editedRow.trainAdd = true;
            editedTrain = new Train(0, "", "", TimeSpan.Zero, TimeSpan.Zero, 0);
        }

        private void DeleteDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (editedTrain == null || TrainListDG.SelectedIndex < 0)
            {
                MessageBox.Show("Оберіть у списку запис для видалення подвійним кліком",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені що хочете видалити обраний запис?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                editedRow.DeleteDBRow();
                DataConnection.fList.RemoveAt(TrainListDG.SelectedIndex);
                TrainListDG.ItemsSource = null;
                TrainListDG.ItemsSource = DataConnection.fList;
                editedTrain = null;
                trainGroupBox.Visibility = Visibility.Hidden;
            }
        }

        private void SelectXYMenuItem_Click(object sender, RoutedEventArgs e)
        {
            isSearchXY = true;
            trainGroupBox.Visibility = Visibility.Hidden;

            destSearchLabel.Visibility = Visibility.Visible;
            destComboBox.Visibility = Visibility.Visible;
            timeALabel.Visibility = Visibility.Visible;
            timeATextBox.Visibility = Visibility.Visible;
            timeBLabel.Visibility = Visibility.Visible;
            timeBTextBox.Visibility = Visibility.Visible;
            trainNumSearchLabel.Visibility = Visibility.Hidden;
            trainNumSearchTextBox.Visibility = Visibility.Hidden;

            destComboBox.Items.Clear();
            foreach (string dest in selData.GetDestinations())
                destComboBox.Items.Add(dest);

            searchGroupBox.Visibility = Visibility.Visible;
        }

        private void SelectTicketsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            isSearchXY = false;
            trainGroupBox.Visibility = Visibility.Hidden;

            destSearchLabel.Visibility = Visibility.Hidden;
            destComboBox.Visibility = Visibility.Hidden;
            timeALabel.Visibility = Visibility.Hidden;
            timeATextBox.Visibility = Visibility.Hidden;
            timeBLabel.Visibility = Visibility.Hidden;
            timeBTextBox.Visibility = Visibility.Hidden;
            trainNumSearchLabel.Visibility = Visibility.Visible;
            trainNumSearchTextBox.Visibility = Visibility.Visible;

            searchGroupBox.Visibility = Visibility.Visible;
        }

        private void SelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (isSearchXY)
            {
                if (destComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Оберіть станцію призначення!",
                                    "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                TimeSpan timeA, timeB;
                if (!TimeSpan.TryParse(timeATextBox.Text, out timeA))
                {
                    MessageBox.Show("Невірний формат часу A. Використовуйте формат гг:хх",
                                    "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                if (!TimeSpan.TryParse(timeBTextBox.Text, out timeB))
                {
                    MessageBox.Show("Невірний формат часу B. Використовуйте формат гг:хх",
                                    "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                string dest = destComboBox.SelectedItem.ToString();
                selData.SelectXY(dest, timeA, timeB);

                if (selData.selectedList.Count == 0)
                {
                    MessageBox.Show("Потягів за вказаними критеріями не знайдено.",
                                    "Увага!", MessageBoxButton.OK, MessageBoxImage.Information);
                    TrainListDG.ItemsSource = DataConnection.fList;
                }
                else
                {
                    TrainListDG.ItemsSource = selData.selectedList;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(trainNumSearchTextBox.Text))
                {
                    MessageBox.Show("Введіть номер потяга!",
                                    "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                selData.SelectTickets(trainNumSearchTextBox.Text.Trim());

                if (selData.selectedTicketList.Count == 0)
                {
                    MessageBox.Show("Потяг з таким номером не знайдено.",
                                    "Увага!", MessageBoxButton.OK, MessageBoxImage.Information);
                    TrainListDG.ItemsSource = DataConnection.fList;
                }
                else
                {
                    TrainListDG.ItemsSource = selData.selectedTicketList;
                }
            }
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            TrainListDG.ItemsSource = DataConnection.fList;
            searchGroupBox.Visibility = Visibility.Hidden;
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

            if (string.IsNullOrWhiteSpace(numTrainTextBox.Text) ||
                string.IsNullOrWhiteSpace(destTrainTextBox.Text) ||
                string.IsNullOrWhiteSpace(depTimeTextBox.Text) ||
                string.IsNullOrWhiteSpace(travTimeTextBox.Text) ||
                string.IsNullOrWhiteSpace(ticketsTextBox.Text))
            {
                MessageBox.Show("Заповніть усі поля перед збереженням!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

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

            if (editedRow.trainAdd)
            {
                DataConnection.fList.Add(editedTrain);
            }
            else
            {
                DataConnection.fList[num] = editedTrain;
            }

            TrainListDG.ItemsSource = null;
            TrainListDG.ItemsSource = DataConnection.fList;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (editedTrain == null)
            {
                MessageBox.Show("Оберіть у списку запис для редагування подвійним кліком",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                if (editedRow.trainAdd)
                {
                    ChangeTrainListData(-1);
                    if (!DataConnection.fList.Contains(editedTrain))
                        return;

                    editedRow.ChangeDBRow();
                    DataConnection.fList.Clear();
                    DataAccess newData = new DataAccess();
                    DataConnection.fList = newData.fList;
                    TrainListDG.ItemsSource = null;
                    TrainListDG.ItemsSource = DataConnection.fList;
                }
                else
                {
                    ChangeTrainListData(TrainListDG.SelectedIndex);
                    editedRow.ChangeDBRow();
                }
            }
        }
    }
}