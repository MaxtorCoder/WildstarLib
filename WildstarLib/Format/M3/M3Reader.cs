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
        public static void ReadHeader(BinaryReader br)
        {
            header = new M3Header();

            header.magic            = br.ReadEnum<Magic>();
            header.version          = br.ReadUInt32();
            br.ReadBytes(376);
            header.Bones            = br.ReadM3Array();
            br.ReadBytes(32);
            header.BoneTable        = br.ReadM3Array();
            header.Textures         = br.ReadM3Array();
            br.ReadBytes(128);
            header.Geometry         = br.ReadM3Array();
            br.ReadBytes(976);

            Console.WriteLine($"Magic: {header.magic} Version: {header.version} Length: {br.BaseStream.Length}");
            Console.WriteLine($"\nBones Size: {header.Bones.Size}\nBones Offset: {header.Bones.Offset}");
            Console.WriteLine($"\nBoneTable Size: {header.BoneTable.Size}\nBoneTable Offset: {header.BoneTable.Offset}");
            Console.WriteLine($"\nTextures Size: {header.Textures.Size}\nTextures Offset: {header.Textures.Offset}");
            Console.WriteLine($"\nGeometry Size: {header.Geometry.Size}\nGeometry Offset: {header.Geometry.Offset}");

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
                    _Texture[i].lengthTexture   = br.ReadUInt32();
                    br.BaseStream.Position      = Pos + 24;
                    _Texture[i].offsetTexture   = br.ReadUInt64();

                    Console.WriteLine($"Type: {_Texture[i].Type} Length: {_Texture[i].lengthTexture} Offset: {_Texture[i].offsetTexture}");

                    br.BaseStream.Position  = (int)texOfs + (int)_Texture[i].offsetTexture + texHeaderSize * (int)header.Textures.Size;
                    byte[] path             = br.ReadBytes((int)_Texture[i].lengthTexture * 2);
                    byte[] GetUTF8          = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, path);
                    _Texture[i].filePath    = Encoding.Default.GetString(GetUTF8);

                    Console.WriteLine($"Filename: {_Texture[i].filePath}");
                }
            }
        }
    }
}
