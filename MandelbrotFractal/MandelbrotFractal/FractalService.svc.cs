using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Drawing;
using System.IO;

namespace MandelbrotFractal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IFractalService
    {
        public byte[] GetFractal(string filename)
        {
            string path = "D:\\STUDIA\\programy\\MandelbrotFractal\\MandelbrotFractal\\App_Data\\";
            return (File.Exists(path + filename)) ? File.ReadAllBytes(path + filename) : null;
        }

        public string GenerateFractal(int width, int height, int threadsNumber)
        {
            MandelbrotFractal fractal = new MandelbrotFractal(width, height);
            return fractal.generate(threadsNumber);
        }
    }
}
