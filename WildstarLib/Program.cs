using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildstarLib.Format.M3;

namespace WildstarLib
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataPath = @"D:\Wildstar\Wildstar Studio\Art\Mount\Chua\PRP_Mount_Chua_000.m3";

            M3Main.Process(dataPath);

            Console.ReadLine();
        }
    }
}
