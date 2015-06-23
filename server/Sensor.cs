using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace kontroler_budynku
{
    abstract class Sensor:Device
    {
        //protected string name;
       // protected string TransferProtocol;
       // protected Int16 Value;
        protected Thread RecvThread;


        public abstract void AddObserver(Room room);
        public abstract void RemoveObserver(Room room);
        public abstract void InformObservers();

        public abstract void Assign(object client);
        public abstract void UpdateValue(object client);

    }
    

    class TemperatureSensor : Sensor
    {
        private Double Value;
        private string name = "TempSensor";
        private List<Room> Observers = new List<Room>();

        public TemperatureSensor() { }
       /* public TemperatureSensor(Room room) 
        {
            AddObserver(room);

        } */

        public override string GetID() { return "TempSensor"; }

        public override void AddObserver(Room room)
        {
            this.Observers.Add(room);
        }

        public override void RemoveObserver(Room room)
        {
            this.Observers.Remove(room);
        }

        public override void InformObservers()
        {
            foreach (var item in Observers)
                item.UpdateTemp(this.Value);
        }

        public Double TempSend()
        {
            return this.Value;
        }

        public override void Assign(object client) 
        {
            System.Console.WriteLine("\ntworze watek czujnika");
            this.RecvThread = new Thread(new ParameterizedThreadStart(UpdateValue));
            this.RecvThread.Start(client);
        }

        public override void UpdateValue(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            //System.Console.WriteLine(tcpClient.Client.LocalEndPoint.ToString());
            //this.connectedClients.Add(tcpClient.Client.LocalEndPoint.ToString());

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    break;

                }

                if (bytesRead == 0)
                {
                    foreach (var item in Observers)
                        item.DeleteSensor(this);
                    System.Console.WriteLine("utracono czujnik");
                    break;

                }

                try
                {
                    ASCIIEncoding encoder = new ASCIIEncoding();
                   // System.Console.Write(encoder.GetString(message, 0, bytesRead));
                    this.Value = Convert.ToDouble(encoder.GetString(message, 0, bytesRead));
                    this.InformObservers();
                }
                catch
                {

                }
            }
            tcpClient.Close();
        }
    }
}


