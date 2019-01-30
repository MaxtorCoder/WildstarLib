using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildstarLib.Format.M3;

namespace WildstarLib.Extensions
{
    public static class Extensions
    {
        public static M3Array ReadM3Array(this BinaryReader br)
        {
            M3Array array = new M3Array()
            {
                Size    = br.ReadUInt64(),
                Offset  = br.ReadUInt64()
            };

            return array;
        }

        public static T ReadEnum<T>(this BinaryReader br) where T : Enum
        {
            return (T)Enum.ToObject(typeof(T), br.ReadUInt32());
        }
    }
}
