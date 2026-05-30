using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TrainDispatcher;

namespace TrainDispatcherTests
{
    [TestClass]
    public class MainWindowTests
    {
        // Ініціалізація форми та підключення до БД
        private MainWindow InitTarget()
        {
            var target = new MainWindow();
            object sender = target;
            RoutedEventArgs e = null;
            target.InitializeComponent();
            target.InfoTrainForm_Loaded(sender, e);
            return target;
        }

        // Тест 1: SelectXY — позитивний сценарій (Київ, 07:00-21:00)
        [TestMethod]
        public void SelectXYTest_Found()
        {
            List<Train> expected = new List<Train>();
            expected.Add(new Train(1, "007П", "Київ", TimeSpan.Parse("08:30:00"), TimeSpan.Parse("04:55:00"), 32));
            expected.Add(new Train(12, "088К", "Київ", TimeSpan.Parse("20:00:00"), TimeSpan.Parse("04:20:00"), 60));

            InitTarget();

            SelectData selData = new SelectData();
            selData.SelectXY("Київ", TimeSpan.Parse("07:00"), TimeSpan.Parse("21:00"));

            List<Train> actual = selData.selectedList;

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].id, actual[i].id);
                Assert.AreEqual(expected[i].train_number, actual[i].train_number);
                Assert.AreEqual(expected[i].destination, actual[i].destination);
                Assert.AreEqual(expected[i].departure_time, actual[i].departure_time);
                Assert.AreEqual(expected[i].tickets_available, actual[i].tickets_available);
            }
        }

        // Тест 2: SelectXY — негативний сценарій (нічого не знайдено)
        [TestMethod]
        public void SelectXYTest_NotFound()
        {
            InitTarget();

            SelectData selData = new SelectData();
            selData.SelectXY("Київ", TimeSpan.Parse("01:00"), TimeSpan.Parse("02:00"));

            Assert.AreEqual(0, selData.selectedList.Count);
        }

        // Тест 3: SelectTickets — позитивний сценарій
        [TestMethod]
        public void SelectTicketsTest_Found()
        {
            List<Train> expected = new List<Train>();
            expected.Add(new Train(2, "112К", "Харків", TimeSpan.Parse("14:15:00"), TimeSpan.Parse("02:50:00"), 12));

            InitTarget();

            SelectData selData = new SelectData();
            selData.SelectTickets("112К");

            List<Train> actual = selData.selectedTicketList;

            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected[0].id, actual[0].id);
            Assert.AreEqual(expected[0].train_number, actual[0].train_number);
            Assert.AreEqual(expected[0].destination, actual[0].destination);
            Assert.AreEqual(expected[0].departure_time, actual[0].departure_time);
            Assert.AreEqual(expected[0].tickets_available, actual[0].tickets_available);
        }

        // Тест 4: SelectTickets — негативний сценарій
        [TestMethod]
        public void SelectTicketsTest_NotFound()
        {
            InitTarget();

            SelectData selData = new SelectData();
            selData.SelectTickets("999X");

            Assert.AreEqual(0, selData.selectedTicketList.Count);
        }

        // Тест 5: GetDestinations — перевірка унікальних станцій
        [TestMethod]
        public void GetDestinationsTest()
        {
            List<string> expected = new List<string>();
            expected.Add("Вінниця");
            expected.Add("Дніпро");
            expected.Add("Запоріжжя");
            expected.Add("Івано-Франківськ");
            expected.Add("Київ");
            expected.Add("Львів");
            expected.Add("Маріуполь");
            expected.Add("Одеса");
            expected.Add("Полтава");
            expected.Add("Харків");
            expected.Add("Чернівці");

            InitTarget();

            SelectData selData = new SelectData();
            List<string> actual = selData.GetDestinations();

            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.AreEqual(expected[i], actual[i]);
        }
    }
}