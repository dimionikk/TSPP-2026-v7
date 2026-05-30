using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

namespace TrainDispatcher
{
    public class DataAccess
    {
        public string connStr;
        public List<Train> fList = new List<Train>(85);

        private void OpenDbFile()
        {
            connStr = "Server=localhost; Port=3306; Database=trains; Uid=root; Pwd=;";

            MySqlConnection conn = new MySqlConnection(connStr);
            MySqlCommand command = new MySqlCommand();

            string commandString = "SELECT * FROM rozklad;";
            command.CommandText = commandString;
            command.Connection = conn;

            MySqlDataReader reader;

            try
            {
                command.Connection.Open();
                reader = command.ExecuteReader();

                int i = 0;
                while (reader.Read())
                {
                    fList.Add(new Train(
                        (int)reader["id"],
                        (string)reader["train_number"],
                        (string)reader["destination"],
                        (System.TimeSpan)reader["departure_time"],
                        (System.TimeSpan)reader["travel_time"],
                        (int)reader["tickets_available"]
                    ));
                    i += 1;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MainWindow.ErrorShow(ex, "Для завантаження даних виконайте команду Файл-Завантажити",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public DataAccess()
        {
            OpenDbFile();
        }
    }
}