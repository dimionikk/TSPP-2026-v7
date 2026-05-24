using System;
using System.Windows;

namespace TrainDispatcher
{
    public partial class EditForm : Window
    {
        public bool isAdd = false;

        public EditForm()
        {
            InitializeComponent();
        }

        public void LoadTrain(Train t)
        {
            numTrainTextBox.Text = t.train_number;
            destTrainTextBox.Text = t.destination;
            depTimeTextBox.Text = t.departure_time.ToString(@"hh\:mm");
            travTimeTextBox.Text = t.travel_time.ToString(@"hh\:mm");
            ticketsTextBox.Text = t.tickets_available.ToString();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
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

            TimeSpan depTime, travTime;
            int tickets;

            if (!TimeSpan.TryParse(depTimeTextBox.Text, out depTime))
            {
                MessageBox.Show("Невірний формат часу відправлення. Використовуйте формат гг:хх",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!TimeSpan.TryParse(travTimeTextBox.Text, out travTime))
            {
                MessageBox.Show("Невірний формат часу у дорозі. Використовуйте формат гг:хх",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            if (!int.TryParse(ticketsTextBox.Text, out tickets))
            {
                MessageBox.Show("Невірний формат кількості квитків. Введіть ціле число",
                                "Помилка!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MainWindow.editedTrain.train_number = numTrainTextBox.Text;
            MainWindow.editedTrain.destination = destTrainTextBox.Text;
            MainWindow.editedTrain.departure_time = depTime;
            MainWindow.editedTrain.travel_time = travTime;
            MainWindow.editedTrain.tickets_available = tickets;

            if (isAdd)
            {
                MainWindow.editedRow.ChangeDBRow();
                MainWindow.DataConnection.fList.Clear();
                DataAccess newData = new DataAccess();
                MainWindow.DataConnection.fList = newData.fList;
            }
            else
            {
                MainWindow.DataConnection.fList[MainWindow.editedRow.trainNum] = MainWindow.editedTrain;
                MainWindow.editedRow.ChangeDBRow();
            }

            MainWindow.needRefresh = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void EditForm_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }
    }
}