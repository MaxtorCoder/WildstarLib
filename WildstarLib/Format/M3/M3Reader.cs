using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WildstarLib.Extensions;

namespace WildstarLib.Format.M3
{
    public class M3Reader
    {
        static M3Header header;
        public static void ProcessM3(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                ReadHeader(reader);
            }
        }

        private static void ReadHeader(BinaryReader br)
        {
            header = new M3Header();

            header.magic            = br.ReadEnum<Magic>();
            header.version          = br.ReadUInt32();
            br.ReadBytes(376);
            header.Bones            = br.ReadM3Array();
            br.ReadBytes(32);
            header.BoneTable        = br.ReadM3Array();
            header.Textures         = br.ReadM3Array();
            br.ReadBytes(16);
            header.Unk1E0           = br.ReadM3Array();
            header.Materials        = br.ReadM3Array();
            header.Unk200           = br.ReadM3Array();
            header.Unk210           = br.ReadM3Array();
            br.ReadBytes(48);
            header.Geometry         = br.ReadM3Array();
            br.ReadBytes(976);

            Console.WriteLine($"Magic: {header.magic} Version: {header.version} Length: {br.BaseStream.Length}");
            Console.WriteLine($"\nBones Size: {header.Bones.Size}\nBones Offset: {header.Bones.Offset}");
            Console.WriteLine($"\nBoneTable Size: {header.BoneTable.Size}\nBoneTable Offset: {header.BoneTable.Offset}");
            Console.WriteLine($"\nTextures Size: {header.Textures.Size}\nTextures Offset: {header.Textures.Offset}");
            Console.WriteLine($"\nUnk1E0 Size: {header.Unk1E0.Size}\nUnk1E0 Offset: {header.Unk1E0.Offset}");
            Console.WriteLine($"\nMaterials Size: {header.Materials.Size}\nMaterials Offset: {header.Materials.Offset}");
            Console.WriteLine($"\nUnk200 Size: {header.Unk200.Size}\nUnk200 Offset: {header.Unk200.Offset}");
            Console.WriteLine($"\nUnk210 Size: {header.Unk210.Size}\nUnk210 Offset: {header.Unk210.Offset}");
            Console.WriteLine($"\nGeometry Size: {header.Geometry.Size}\nGeometry Offset: {header.Geometry.Offset}\n");

            if (header.Textures.Offset != 0)
            {
                var texHeaderSize = 32;
                var texOfs = 1584 + header.Textures.Offset;
                Texture[] _Texture = new Texture[header.Textures.Size];

                for (ulong i = 0; i < header.Textures.Size; i++)
                {
                    var Pos = (long)texOfs + (long)i * texHeaderSize;

                    br.BaseStream.Position      = Pos + 2;
                    _Texture[i].Type            = br.ReadByte();
                    br.BaseStream.Position      = Pos + 16;
                    _Texture[i].pathLength      = br.ReadUInt32();
                    br.BaseStream.Position      = Pos + 24;
                    _Texture[i].offsetTexture   = br.ReadUInt64();

                    Console.WriteLine($"Type: {_Texture[i].Type} Length: {_Texture[i].pathLength} Offset: {_Texture[i].offsetTexture}");

                    br.BaseStream.Position  = (int)texOfs + (int)_Texture[i].offsetTexture + texHeaderSize * (int)header.Textures.Size;
                    byte[] path             = br.ReadBytes((int)_Texture[i].pathLength * 2);
                    byte[] GetUTF8          = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, path);
                    _Texture[i].filePath    = Encoding.Default.GetString(GetUTF8);

                    Console.WriteLine($"Filename: {_Texture[i].filePath}\n");
                }
            }
        }
    }
}
