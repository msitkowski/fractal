using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace MandelbrotFractal
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFractalService" in both code and config file together.
    [ServiceContract]
    public interface IFractalService
    {
        [OperationContract]
        byte[] GetFractal(string filename);

        // TODO: Add your service operations here
        [OperationContract]
        string GenerateFractal(int width, int height, int threadsNumber);
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class MandelbrotFractal
    {
        int width = 0;
        int height = 0;

        public MandelbrotFractal(int w, int h)
        {
            width = w;
            height = h;
        }

        public string generate(int threadsNumber)
        {
            if (threadsNumber > 8)
            {
                threadsNumber = 8;
            }
            Color[] colors = new Color[8] { Color.Red, Color.Purple, Color.Black, Color.Blue, Color.Yellow, Color.Gray, Color.Green, Color.LightCyan};
            /* world ( double) coordinate = parameter plane*/
            const double CxMin = -2.5;
            const double CxMax = 1.5;
            const double CyMin = -2.0;
            const double CyMax = 2.0;
            /* */
            double PixelWidth = (CxMax - CxMin) / width;
            double PixelHeight = (CyMax - CyMin) / height;
            /* color component ( R or G or B) is coded from 0 to 255 */
            /* it is 24 bit color RGB file */
            Bitmap bmp = new Bitmap(width, height);
            Color[,] data = new Color[height, width];
            //byte[, ,] d = new byte[iXmax, iYmax, 3];
            /*  */
            int Iteration;
            const int IterationMax = 100;
            /* bail-out value , radius of circle ;  */
            const double EscapeRadius = 2;
            List<int> thread_ids = new List<int>();
            ParallelOptions p = new ParallelOptions();
            p.MaxDegreeOfParallelism = threadsNumber;
            Parallel.For(0, height, p, iY =>
            {
                int tid = Thread.CurrentThread.ManagedThreadId;
                if(!thread_ids.Contains(tid))
                {
                    thread_ids.Add(tid);
                }
                double Cy = CyMin + iY * PixelHeight;
                if (Math.Abs(Cy) < PixelHeight / 2)
                {
                    Cy = 0.0; /* Main antenna */
                }
                for (int iX = 0; iX < width; ++iX)
                {
                    double Cx = CxMin + iX * PixelWidth;
                    /* initial value of orbit = critical point Z= 0 */
                    double Zx, Zy, Zx2, Zy2;
                    Zx = Zy = Zx2 = Zy2 = 0.0;

                    for (Iteration = 0; Iteration < IterationMax && ((Zx2 + Zy2) < Math.Pow(EscapeRadius, 2)); ++Iteration)
                    {
                        Zy = 2 * Zx * Zy + Cy;
                        Zx = Zx2 - Zy2 + Cx;
                        Zx2 = Math.Pow(Zx, 2);
                        Zy2 = Math.Pow(Zy, 2);
                    }
                    /* compute  pixel color (24 bit = 3 bytes) */
                    if (Iteration == IterationMax)
                    { /*  interior of Mandelbrot set = black */
                        //bmp.SetPixel(iX, iY, Color.Blue);
                        data[iY, iX] = colors[thread_ids.IndexOf(tid)];
                    }
                    else
                    { /* exterior of Mandelbrot set = white */
                        //bmp.SetPixel(iX, iY, Color.White);
                        data[iY, iX] = Color.White;
                    }
                }
            });
            Random r = new Random();
            int id = r.Next();
            string fileName = id.ToString() + "_mandelbrot_" + width + "x" + height + ".bmp";
            string path = "D:\\STUDIA\\programy\\MandelbrotFractal\\MandelbrotFractal\\App_Data\\";

            while (File.Exists(path + fileName))
            {
                id = r.Next();
                fileName = id.ToString() + "_mandelbrot_" + width + "x" + height + ".bmp";
            }
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    bmp.SetPixel(j, i, data[i, j]);
                }
            }
            bmp.Save(path + fileName);
            return fileName;
        }
    }
}