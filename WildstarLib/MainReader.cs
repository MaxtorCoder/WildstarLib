using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildstarLib.Format.Archive.Index;
using WildstarLib.Format.M3;

namespace WildstarLib
{
    public class MainReader
    {
        public static void Process(string dataPath)
        {
            try
            {
                if (File.Exists(dataPath))
                {
                    Console.WriteLine($"Processing: {dataPath}\n");

                    if (dataPath.EndsWith("m3"))
                        M3Reader.ProcessM3(dataPath);
                    else if (dataPath.EndsWith("index"))
                        IndexReader.ProcessIndex(dataPath);
                }
                else
                {
                    Console.WriteLine($"Cannot find file: {dataPath}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : Trying to parse file - {dataPath}");
                Console.WriteLine(ex);
            }
        }
    }
}
