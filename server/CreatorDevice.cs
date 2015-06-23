using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kontroler_budynku
{
   abstract class CreatorDevice
    {
       //public abstract Device CreateDevice();
    }


   abstract class Device
   {
        public virtual string GetID() { return "UndefinedDevice"; }
   }


   class SensorCreator : CreatorDevice
   {
       private List<Sensor> SensorsType = new List<Sensor>();

       public SensorCreator()
       {
           this.RegisterSensor(new TemperatureSensor());
       }
       
       public void RegisterSensor(Sensor sensor)
       {
           this.SensorsType.Add(sensor);
           System.Console.Write("Zarejestrowano czujnik: ");
           System.Console.WriteLine(sensor.GetID());
       }

       public Sensor CreateSensor(string sensor_id)
       {
            if (this.SensorsType.Exists(x => x.GetID() == sensor_id))
            {
                switch (sensor_id)
                {
                    case "TempSensor": return new TemperatureSensor();
                    default: System.Console.WriteLine("Sensor not defined"); return null;
                }
                // return new (this.SensorsType.Find(x=>x.GetID()==sensor_id).GetType())();
            }
            else
                System.Console.WriteLine("Sensor not defined");
                return null;
       }
   }

   class PanelCreator : CreatorDevice
   {
       private List<Panel> PanelsType =new List<Panel>();

       public PanelCreator()
       {
           this.RegisterPanel(new Display());
       }

       public void RegisterPanel(Panel panel)
       {
           this.PanelsType.Add(panel);
           System.Console.Write("Zarejestrowano panel: ");
           System.Console.WriteLine(panel.GetID());
       }

       public Panel CreatePanel(string panel_id)
       {
           if (this.PanelsType.Exists(x => x.GetID() == panel_id))
           {
               switch (panel_id)
               {
                   case "Display": return new Display();
                   default: System.Console.WriteLine("Panel not defined"); return null;
               }
           }
           else
               System.Console.WriteLine("Panel not defined");
           return null;
       }
   }


}
