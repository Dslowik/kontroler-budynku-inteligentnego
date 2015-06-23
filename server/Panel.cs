using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace kontroler_budynku
{
    abstract class Panel:Device
    {
        protected Thread RecvThread;
        protected string[] Values;

        public abstract void Assign(object client);
        public abstract void RefreshView(string[] odebranedane);
    }


    class Display : Panel
    {
        private List<Room> Rooms = new List<Room>();
        private string ID="Display";

        public Display()
        {
            this.Values = new string[1024]; this.Values.Initialize();
        }

        public override string GetID() { return ID; }
       
        public override void Assign(object client)
        {
            System.Console.WriteLine("\ntworze watek wyswietlcza");
            this.RecvThread = new Thread(new ParameterizedThreadStart(SendData));
            this.RecvThread.Start(client);
        }

        public override void RefreshView(string[] odebranedane)
        {
            //this.Values[0] = odebranedane[0];
            this.Values = odebranedane;
        }

        private void SendData(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
           // int bytesSend=0;
            while (tcpClient.Connected)
            {
                try
                {
                    if (this.Values != null)
                    {
                        message = Encoding.UTF8.GetBytes(this.Values[0]+"\n\r "+this.Values[4]+"\n\r");          //wyswietlam tylko odczytana temperature
                        clientStream.Write(message, 0, message.Length);
                        System.Threading.Thread.Sleep(9000);
                    }
                }
                catch
                {
                }
            }
            System.Console.WriteLine("Odłączono wyświetlacz");
            tcpClient.Close();

        }

    }
}
