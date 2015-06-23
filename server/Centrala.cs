using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace kontroler_budynku
{
    class Centrala
    {
        //private List<Sensor> sensors = new List<Sensor>();
        //private List<string> displays = new List<string>();
        //private List<string> actuators = new List<string>();
        private List<Room> rooms = new List<Room>();
        SensorCreator SensorCreator = new SensorCreator();
        PanelCreator PanelCreator = new PanelCreator();
        Centrala myRef;


        public Centrala(){}
        
        public void Init(ref Centrala ctr)
        {   
            this.myRef=ctr;
            TCPConnect interfejsTCP = new TCPConnect(8051, ref ctr);
            interfejsTCP.Start(8051);
            // wlaczenie pozostalych interfejsow
        }

        public void AddRoom(string nazwa) 
        {
            Room room1 = new Room(nazwa);
            this.rooms.Add(room1);
        }



        private void RoomList()
        {
            rooms.ForEach(System.Console.WriteLine);

        }

        public void run()
        {
            while (true)
            {
                if (this.rooms.Count == 0)
                {
                    System.Console.WriteLine("Brak zdefiniowanych pomieszczen. Podaj nazwe pomieszczenia: ");
                    AddRoom(System.Console.ReadLine());
                }
                else
                    foreach (var item in rooms)
                        item.Monitor();
                System.Threading.Thread.Sleep(5000);
            }

        }

      /* private void AddSensor(Sensor sensor)
        {
            this.sensors.Add(sensor);
        } */

        public void AddSensorToRoom(Room room, Sensor sensor)
        {
            sensor.AddObserver(room);
            room.AddSensor(sensor);
        }

        public void ConnectDevice(string[] id,TcpClient client)
        {
            Sensor czujnik;
            Panel panel;
            if (id[0] == "Sensor")
            {
                czujnik = this.SensorCreator.CreateSensor(id[1]);
                if (czujnik != null)
                {
                    //this.AddSensor(czujnik);                      // Find(x => x.GetName() == "pokoj")
                    this.AddSensorToRoom(this.rooms.ElementAt(0), czujnik);   // dodac wybieranie pokoju do ktorego ma przypisac czujnik
                    czujnik.Assign(client);
                }
                else 
                {
                    System.Console.WriteLine("Nie rozpoznano urządzenia\n");
                    client.Close();
                }
            }
            else if (id[0] == "Panel")
            {
                panel=this.PanelCreator.CreatePanel(id[1]);
                if (panel != null)
                {
                    this.AddRoomToPanel(this.rooms.ElementAt(0), panel);
                    panel.Assign(client);
                }
                else
                {
                    System.Console.WriteLine("Nie rozpoznano urządzenia\n");
                    client.Close();
                }
            }
            else
            {
                System.Console.WriteLine("Nie rozpoznano urządzenia\n");
                client.Close();
            }
        }

        public void AddRoomToPanel(Room room, Panel panel)
        {
            room.AddObserver(panel);
        }
       /* private object RefreshDisplay(object wartosc)
        {
           //return a ;
        } */

    }
}
