using System;
using System.Collections.Generic;
using System.Windows;

namespace TrainDispatcher
{
    public class SelectData
    {
        // Список всіх потягів до станції X з інтервалом A-B
        public List<Train> selectedList = new List<Train>();

        // Список потягів за номером
        public List<Train> selectedTicketList = new List<Train>();

        // Змінні для MS Word
        private Microsoft.Office.Interop.Word.Application wordApp;
        private Microsoft.Office.Interop.Word.Document wordDoc;
        private string filePath;

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

        // Метод заміни тексту у документі
        private void ReplaceText(string textToReplace, string replacedText)
        {
            Object missing = Type.Missing;

            Microsoft.Office.Interop.Word.Range selText;
            selText = wordDoc.Range(wordDoc.Content.Start, wordDoc.Content.End);

            Microsoft.Office.Interop.Word.Find find = wordApp.Selection.Find;
            find.Text = replacedText;
            find.Replacement.Text = textToReplace;
            Object wrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            Object replace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;

            find.Execute(FindText: Type.Missing,
                MatchCase: false,
                MatchWholeWord: false,
                MatchWildcards: false,
                MatchSoundsLike: missing,
                MatchAllWordForms: false,
                Forward: true,
                Wrap: wrap,
                Format: false,
                ReplaceWith: missing,
                Replace: replace);
        }

        // Перевизначений метод для запису списку потягів у таблицю документу
        private void ReplaceText(List<Train> selectedList, int numTable)
        {
            for (int i = 0; i < selectedList.Count; i++)
            {
                wordDoc.Tables[numTable].Rows.Add();

                // Номер потяга
                wordDoc.Tables[numTable].Cell(2 + i, 1).Range.Text =
                    selectedList[i].train_number;

                if (numTable == 1)
                {
                    // Таблиця 1: час відправлення, час у дорозі
                    wordDoc.Tables[numTable].Cell(2 + i, 2).Range.Text =
                        selectedList[i].departure_time.ToString();
                    wordDoc.Tables[numTable].Cell(2 + i, 3).Range.Text =
                        selectedList[i].travel_time.ToString();
                }
                else
                {
                    // Таблиця 2: станція, час відправлення, час у дорозі, квитки
                    wordDoc.Tables[numTable].Cell(2 + i, 2).Range.Text =
                        selectedList[i].destination;
                    wordDoc.Tables[numTable].Cell(2 + i, 3).Range.Text =
                        selectedList[i].departure_time.ToString();
                    wordDoc.Tables[numTable].Cell(2 + i, 4).Range.Text =
                        selectedList[i].travel_time.ToString();
                    wordDoc.Tables[numTable].Cell(2 + i, 5).Range.Text =
                        selectedList[i].tickets_available.ToString();
                }
            }
        }

        // Метод збереження у Word
        public void WriteData(List<Train> selXYList, List<Train> selTicketList,
                              string destination, TimeSpan timeA, TimeSpan timeB,
                              string trainNumber)
        {
            filePath = Environment.CurrentDirectory.ToString();

            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                wordDoc = wordApp.Documents.Add(filePath + "\\Шаблон_Пошуку_потягів.dot");
                wordApp.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) +
                    "Помістіть файл Шаблон_Пошуку_потягів.dot" +
                    char.ConvertFromUtf32(13) +
                    "у каталог із exe-файлом програми і повторіть збереження",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Вставка назви станції X
            ReplaceText(destination, "[X]");

            // Вставка часу A
            ReplaceText(timeA.ToString(@"hh\:mm"), "[A]");

            // Вставка часу B
            ReplaceText(timeB.ToString(@"hh\:mm"), "[B]");

            // Вставка номера потяга XXX
            ReplaceText(trainNumber, "[XXX]");

            // Вставка списку потягів UC06 у таблицю 1
            ReplaceText(selXYList, 1);

            // Вставка списку потягів UC07 у таблицю 2
            ReplaceText(selTicketList, 2);

            // Збереження документу
            try
            {
                wordDoc.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + char.ConvertFromUtf32(13) +
                    "Помилка збереження відібраних даних",
                    "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Звільнення ресурсів Word
        ~SelectData()
        {
            if (wordDoc != null)
                wordDoc.Close(Microsoft.Office.Interop.Word.WdSaveOptions.wdPromptToSaveChanges);
            if (wordApp != null)
                wordApp.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdPromptToSaveChanges);
        }
    }
}