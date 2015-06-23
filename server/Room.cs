using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kontroler_budynku
{
    class Room
    {
        private List<Sensor> Sensors=new List<Sensor>();
        private List<Panel> Observers = new List<Panel>();
        private string name;
        private Double TemperatureRoom;
        private Double Tmin;
        private Double Tmax;
        private string[] dane = new string[1024];

        public Room(string name)
        {
            this.name = name;
            this.Tmin = 17;
            this.Tmax = 27;
        }

        public string GetName() 
        {
            return this.name;
        }
        public void SetTempBreadth(Double Tmin, Double Tmax) 
        {
            this.Tmin = Tmin;
            this.Tmax = Tmax;
        }

        public void AddSensor(Sensor czujnik)
        {
            Sensors.Add(czujnik);
        }

        public void DeleteSensor(Sensor czujnik)
        {
            Sensors.Remove(czujnik);
        }

        public void SensorList()
        {
            foreach (var item in Sensors)
                System.Console.WriteLine(item.GetID());
        }

        public void Monitor() 
        {
           // while (true)
           // {
                if (this.TemperatureRoom < this.Tmin)
                    dane[4] = "Wlaczono ogrzewanie";
                //System.Console.WriteLine("Włączono ogrzewanie");
                else if (this.TemperatureRoom > this.Tmax)
                    dane[4] = "Wlaczono klime";
                    //System.Console.WriteLine("Włączono klimatyzacje");
                else if (TemperatureRoom >= this.Tmin && TemperatureRoom <= this.Tmax)
                    dane[4] = "Temperatura optymalna";
                    // System.Console.WriteLine("Grzenie/Klima off");
               // System.Threading.Thread.Sleep(5000);
           // }
        }

        public void UpdateTemp(Double value)
        {

            this.TemperatureRoom=value;
            this.InformObservers();
            //System.Console.Write("\nZmierzona temperatura wynosi: ");
           // System.Console.WriteLine(this.TemperatureRoom);
         }

        public void AddObserver(Panel panel)
        {
            this.Observers.Add(panel);
        }
        public void RemoveObserver(Panel panel)
        {
            this.Observers.Remove(panel);
        }
        public void InformObservers()
        {
            dane[0]=this.TemperatureRoom.ToString();
            dane[1]=this.Tmin.ToString();
            dane[2]=this.Tmax.ToString();
            foreach (var item in Observers)
                item.RefreshView(dane);
        }

    }
    
}

