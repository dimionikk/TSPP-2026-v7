using System;
using System.Collections.Generic;

namespace TrainDispatcher
{
    public class Globals
    {
    }

    public class Train
    {
        public Train(int idNum, string nF, string cF,
                     System.TimeSpan tF, System.TimeSpan trF, int tS)
        {
            this.id = idNum;
            this.train_number = nF;
            this.destination = cF;
            this.departure_time = tF;
            this.travel_time = trF;
            this.tickets_available = tS;
        }

        public int id { get; set; }
        public string train_number { get; set; }
        public string destination { get; set; }
        public System.TimeSpan departure_time { get; set; }
        public System.TimeSpan travel_time { get; set; }
        public int tickets_available { get; set; }
    }
}