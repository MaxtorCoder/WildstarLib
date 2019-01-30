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
        public M3Array Unk1E0;
        public M3Array Materials;
        public M3Array Unk200;
        public M3Array Unk210;
        public M3Array Geometry;
    }

    public class M3Array
    {
        public UInt64 Size;
        public UInt64 Offset;
    }

    public enum Magic
    {
        LDOM = 1297040460
    }

    public struct Texture
    {
        public UInt16 Type;
        public UInt32 pathLength;
        public UInt64 offsetTexture;
        public string filePath;
    }
}
