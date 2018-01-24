using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            FractalServiceReference.FractalServiceClient client = new FractalServiceReference.FractalServiceClient();
            string fname = client.GenerateFractal(35, 35, 4);
            Console.WriteLine(fname);
             i = client.GetFractal(fname);
        }
    }
}
