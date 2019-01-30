using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildstarLib.Format.M3
{
    public class M3Header
    {
        public Magic magic;
        public uint version;
        public M3Array Bones;
        public M3Array BoneTable;
        public M3Array Textures;
        public M3Array Geometry;
    }

    public class M3Array
    {
        public ulong Size;
        public ulong Offset;
    }

    public enum Magic
    {
        LDOM = 1297040460
    }

    public struct Texture
    {
        public UInt16 Type;
        public UInt32 lengthTexture;
        public UInt64 offsetTexture;
        public string filePath;
    }
}
