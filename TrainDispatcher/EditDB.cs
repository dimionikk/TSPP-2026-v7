using System;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TrainDispatcher
{
    public class EditDB
    {
        public int trainNum { get; set; }
        public bool trainAdd { get; set; }

        public EditDB()
        {
        }

        public void ChangeDBRow()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MainWindow.DataConnection.connStr))
                using (MySqlCommand cmd = new MySqlCommand(
                    "UPDATE rozklad SET train_number = ?, destination = ?, " +
                    "departure_time = ?, travel_time = ?, tickets_available = ? WHERE id = ?", conn))
                {
                    cmd.Parameters.Add("@train_number", MySqlDbType.VarChar, 6).Value = MainWindow.editedTrain.train_number;
                    cmd.Parameters.Add("@destination", MySqlDbType.VarChar, 25).Value = MainWindow.editedTrain.destination;
                    cmd.Parameters.Add("@departure_time", MySqlDbType.Time).Value = MainWindow.editedTrain.departure_time;
                    cmd.Parameters.Add("@travel_time", MySqlDbType.Time).Value = MainWindow.editedTrain.travel_time;
                    cmd.Parameters.Add("@tickets_available", MySqlDbType.Int32, 4).Value = MainWindow.editedTrain.tickets_available;
                    cmd.Parameters.Add("@id", MySqlDbType.Int32, 11).Value = MainWindow.editedTrain.id;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) +
                    char.ConvertFromUtf32(13) + "Помилка з'єднання з БД",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}