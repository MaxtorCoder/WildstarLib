using System;
using WildstarLib.Format.Archive.Index;
using WildstarLib.Format.M3;

namespace WildstarLib
{
    class Program
    {
        static void Main(string[] args)
        {
            //string dataPath = @"D:\Wildstar\Wildstar Studio\Art\Mount\Chua\PRP_Mount_Chua_000.m3";
            string dataPath = @"C:\Program Files (x86)\NCSOFT\WildStar\Patch\ClientData.index";

            MainReader.Process(dataPath);

            Console.ReadLine();
        }
    }
}
