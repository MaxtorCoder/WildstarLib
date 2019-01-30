using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildstarLib.Format.M3
{
    public class M3Main
    {
        public static void Process(string dataPath)
        {
            try
            {
                Console.WriteLine($"Processing: {dataPath}");
                ParseM3(dataPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : Trying to parse M3 - {dataPath}");
                Console.WriteLine(ex);
            }
        }

        private static void ParseM3(string dataPath)
        {
            if (File.Exists(dataPath))
            {
                using (FileStream stream = File.OpenRead(dataPath))
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    //while (stream.Position < stream.Length)
                    //{
                        M3Reader.ReadHeader(reader);
                    //}
                }
            }
            else
            {
                Console.WriteLine($"Missing File : {dataPath}");
            }
        }
    }
}
