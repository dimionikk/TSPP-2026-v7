using System;
using System.Collections.Generic;

namespace TrainDispatcher
{
    public class SelectData
    {
        // Список всіх потягів до станції X з інтервалом A-B
        public List<Train> selectedList = new List<Train>();

        // Список потягів за номером
        public List<Train> selectedTicketList = new List<Train>();

        // Відбір потягів до станції X в інтервалі від A до B годин
        public void SelectXY(string destination, TimeSpan timeA, TimeSpan timeB)
        {
            selectedList.Clear();
            foreach (Train t in MainWindow.DataConnection.fList)
            {
                if (t.destination == destination &&
                    t.departure_time >= timeA &&
                    t.departure_time <= timeB)
                {
                    selectedList.Add(t);
                }
            }
        }

        // Відбір потягів за номером потяга
        public void SelectTickets(string trainNumber)
        {
            selectedTicketList.Clear();
            foreach (Train t in MainWindow.DataConnection.fList)
            {
                if (t.train_number == trainNumber)
                {
                    selectedTicketList.Add(t);
                }
            }
        }

        // Заповнення списку унікальних станцій для ComboBox
        public List<string> GetDestinations()
        {
            List<string> destinations = new List<string>();
            foreach (Train t in MainWindow.DataConnection.fList)
            {
                if (!destinations.Contains(t.destination))
                    destinations.Add(t.destination);
            }
            destinations.Sort();
            return destinations;
        }
    }
}