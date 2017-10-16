using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using Accord.Statistics;
using Accord.Statistics.Analysis;
using Accord.Statistics.Kernels;


namespace dataMining_ex2
{
    class Program
    {
        static void Main(string[] args)
        {
            MainManager mainManager = new MainManager();
            mainManager.Run();

            Console.WriteLine("press any key to continu..");
            Console.ReadLine();
       }

    }
}
