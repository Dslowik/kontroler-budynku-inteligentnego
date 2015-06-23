using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace kontroler_budynku
{
    abstract class ConnectManager
    {
      // protected static List<string> ConnectedClients;
       protected Int16 port;

       public ConnectManager(){}
       public ConnectManager(Int16 port){}

       public abstract void Start(Int16 port);
       protected abstract void Listen();
       /*public static void ViewClients()
       {
           ConnectedClients.ForEach(System.Console.WriteLine);
       } */
       
    }



    class TCPConnect : ConnectManager
    {
        private TcpListener tcpListener;
        private Thread listenThread;
        private Centrala Boss;

        public TCPConnect(Int16 port, ref Centrala ctr) 
        {
            this.Boss = ctr;
            this.port = port;
            this.tcpListener = new TcpListener(IPAddress.Any, port);
        }

        public override void Start(Int16 port)
        {
            this.listenThread = new Thread(new ThreadStart(Listen));
            this.listenThread.Start();
            System.Console.WriteLine("TcpManager is running");
        }

        protected override void Listen()
        {
            this.tcpListener.Start();
            while (true)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient(); 
                Thread registerThread = new Thread(new ParameterizedThreadStart(this.Received));               
                registerThread.Start(client);
            }
        }

        private void Received(object client)
        {
            TcpClient tcpclient = (TcpClient)client;
            NetworkStream clientStream = tcpclient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;
            string data;
            bool introduce = true;
            string[] words;

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
                    System.Console.WriteLine("utracono urzadzenie");
                    break;
                }

                try
                {
                    ASCIIEncoding encoder = new ASCIIEncoding();
                   // System.Console.Write(encoder.GetString(message, 0, bytesRead));
                    data=encoder.GetString(message, 0, bytesRead);
                    words=data.Split(' ');
                    if (introduce)
                    {
                        introduce = false;
                    }
                    else
                    {
                        Boss.ConnectDevice(words, tcpclient);
                        break;
                    }
                }
                catch
                { }
            }         
        }   
    }
}

