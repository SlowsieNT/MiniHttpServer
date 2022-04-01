using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace minihttp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            new Server(args.Length > 0 ? args[0] : "");
            Application.Run();
        }
    }
}
