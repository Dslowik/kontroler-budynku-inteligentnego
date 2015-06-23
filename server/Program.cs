using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace kontroler_budynku
{
    class Program
    {
        static void Main(string[] args)
        {
            Centrala centrala = new Centrala();
            centrala.Init(ref centrala);
            centrala.run();
            while (true)
            {
 
            }
        }
    }
}
