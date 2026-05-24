using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TrainDispatcher
{
    public partial class MainWindow : Window
    {
        public static Authorization logedUser = new Authorization();
        public static DataAccess DataConnection;
        public static Train editedTrain;
        public static EditDB editedRow = new EditDB();
        public static bool needRefresh = false;

        private SelectData selData = new SelectData();

        public static string selectedCity = "";
        public static TimeSpan timeFlightA = TimeSpan.Zero;
        public static TimeSpan timeFlightB = TimeSpan.Zero;
        public static string trainNumSearch = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InfoTrainForm_Loaded(object sender, RoutedEventArgs e)
        {
            DataConnection = new DataAccess();
            TrainListDG.ItemsSource = DataConnection.fList;
        }

        private void InfoTrainForm_Activated(object sender, EventArgs e)
        {
            if (needRefresh)
            {
                TrainListDG.ItemsSource = null;
                TrainListDG.ItemsSource = DataConnection.fList;
                needRefresh = false;
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
            try { TrainListDG.ItemsSource = null; }
            catch { }
            DataConnection = new DataAccess();
            TrainListDG.ItemsSource = DataConnection.fList;
        }

        private void DeleteDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.logUser != 2)
            {
                MessageBox.Show("Для видалення потрібна авторизація як Редактор!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (TrainListDG.SelectedIndex < 0)
            {
                MessageBox.Show("Оберіть запис у таблиці для видалення!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            editedTrain = TrainListDG.SelectedItem as Train;

            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені що хочете видалити обраний запис?",
                "Підтвердження видалення",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                editedRow = new EditDB();
                editedRow.trainNum = TrainListDG.SelectedIndex;
                editedRow.DeleteDBRow();
                DataConnection.fList.RemoveAt(TrainListDG.SelectedIndex);
                TrainListDG.ItemsSource = null;
                TrainListDG.ItemsSource = DataConnection.fList;
            }
        }

        // UC06 — пошук за містом і часом
        private void SearchXYBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(citySearchTextBox.Text))
            {
                MessageBox.Show("Введіть назву міста!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            TimeSpan timeA, timeB;
            if (!TimeSpan.TryParse(timeASearchTextBox.Text, out timeA))
            {
                MessageBox.Show("Невірний формат часу A. Використовуйте формат гг:хх",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            if (!TimeSpan.TryParse(timeBSearchTextBox.Text, out timeB))
            {
                MessageBox.Show("Невірний формат часу B. Використовуйте формат гг:хх",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            selectedCity = citySearchTextBox.Text.Trim();
            timeFlightA = timeA;
            timeFlightB = timeB;

            selData.SelectXY(selectedCity, timeA, timeB);

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

        // UC07 — пошук за номером потяга
        private void SearchTicketsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trainNumSearchTextBox.Text))
            {
                MessageBox.Show("Введіть номер потяга!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            trainNumSearch = trainNumSearchTextBox.Text.Trim();
            selData.SelectTickets(trainNumSearch);

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

        private void ResetSearchBtn_Click(object sender, RoutedEventArgs e)
        {
            TrainListDG.ItemsSource = DataConnection.fList;
            citySearchTextBox.Text = "";
            timeASearchTextBox.Text = "";
            timeBSearchTextBox.Text = "";
            trainNumSearchTextBox.Text = "";
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.logUser != 2)
            {
                MessageBox.Show("Для додавання потрібна авторизація як Редактор!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            editedTrain = new Train(0, "", "", TimeSpan.Zero, TimeSpan.Zero, 0);
            editedRow = new EditDB();
            editedRow.trainAdd = true;

            EditForm editForm = new EditForm();
            editForm.isAdd = true;
            editForm.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Authorization.logUser != 2)
            {
                MessageBox.Show("Для редагування потрібна авторизація як Редактор!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (TrainListDG.SelectedIndex < 0)
            {
                MessageBox.Show("Оберіть запис у таблиці для редагування!",
                                "Увага!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            editedTrain = TrainListDG.SelectedItem as Train;
            editedRow = new EditDB();
            editedRow.trainNum = TrainListDG.SelectedIndex;
            editedRow.trainAdd = false;

            EditForm editForm = new EditForm();
            editForm.isAdd = false;
            editForm.LoadTrain(editedTrain);
            editForm.Show();
            this.Visibility = Visibility.Collapsed;
        }

        private void SaveWordBtn_Click(object sender, RoutedEventArgs e)
        {
            selData.WriteData(
                selData.selectedList,
                selData.selectedTicketList,
                selectedCity,
                timeFlightA,
                timeFlightB,
                trainNumSearch);
        }
    }
}