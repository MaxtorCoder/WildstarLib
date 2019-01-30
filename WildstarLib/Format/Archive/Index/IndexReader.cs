using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace WildstarLib.Format.Archive.Index
{
    public class IndexReader
    {
        // 100% code from 
        // https://github.com/CucFlavius/WSEdit/blob/master/Assets/FileFormatParsers/Archive/IndexFile.cs

        private static IndexHeader header;
        private static List<PackBlockHeader> packBlockHeaders = new List<PackBlockHeader>();
        private static int AIDXBlockNumber = -1;
        private static AIDXEntry aidx;

        public static void ProcessIndex(string filename)
        {
            using (FileStream stream = File.OpenRead(filename))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                ReadHeader(reader);
                ReadGlobalBlockInfo(reader);
                ReadAIDXBlock(reader);
                FolderBlock folderBlock = ReadBlock((int)aidx.rootBlock, "AIDX", reader);
            }
        }


        private static void ReadHeader(BinaryReader br)
        {
            header = new IndexHeader();
            header.signature        = br.ReadUInt32(); // == 'PACK'
            header.version          = br.ReadUInt32();
            br.ReadBytes(512);      // skip empty
            header.indexFileSize    = br.ReadUInt64();
            br.ReadBytes(8);        // skip empty
            header.ofsBlockTable    = br.ReadUInt64();
            header.numBlocks        = br.ReadUInt32();
            br.ReadBytes(28);       // skip unknown
        }

        private static void ReadGlobalBlockInfo(BinaryReader br)
        {
            packBlockHeaders = new List<PackBlockHeader>();
            br.BaseStream.Seek((long)header.ofsBlockTable, SeekOrigin.Begin);
            for (int b = 0; b < header.numBlocks; b++)
            {
                PackBlockHeader pbh = new PackBlockHeader();
                pbh.blockOffset = br.ReadUInt64();
                pbh.blockSize = br.ReadUInt64();
                // find AIDX block by size //
                if (pbh.blockSize == 16)
                {
                    AIDXBlockNumber = b;
                }
                packBlockHeaders.Add(pbh);
            }
        }

        private static void ReadAIDXBlock(BinaryReader br)
        {
            if (AIDXBlockNumber != -1)
            {
                br.BaseStream.Seek((long)packBlockHeaders[AIDXBlockNumber].blockOffset, SeekOrigin.Begin);
                aidx                = new AIDXEntry();
                aidx.magic          = br.ReadUInt32();
                aidx.version        = br.ReadUInt32();
                aidx.clientBuild    = br.ReadUInt32();
                aidx.rootBlock      = br.ReadUInt32();

                Console.WriteLine($"Client Version: {aidx.clientBuild}");
            }
            else
            {
                Console.WriteLine("Missing AIDX block");
            }
        }

        private static FolderBlock ReadBlock(int blockNumber, string currentDir, BinaryReader br)
        {
            br.BaseStream.Seek((long)packBlockHeaders[blockNumber].blockOffset, SeekOrigin.Begin);
            FolderBlock folderBlock         = new FolderBlock();
            folderBlock.numSubdirectories   = br.ReadUInt32();
            folderBlock.numFiles            = br.ReadUInt32();

            if (folderBlock.numSubdirectories > 0)
            {
                DirectoryEntry[] directoryEntries = new DirectoryEntry[folderBlock.numSubdirectories];
                for (int i = 0; i < folderBlock.numSubdirectories; i++)
                {
                    directoryEntries[i] = ReadDirectoryEntry(br);
                }
                folderBlock.subDirectories = directoryEntries;
            }

            if (folderBlock.numFiles > 0)
            {
                FileEntry[] fileEntries = new FileEntry[folderBlock.numFiles];
                for (int i = 0; i < folderBlock.numFiles; i++)
                {
                    fileEntries[i] = ReadFileEntry(br);
                }
                folderBlock.files = fileEntries;
            }

            long remainingSize = (long)packBlockHeaders[blockNumber].blockSize - (br.BaseStream.Position - (long)packBlockHeaders[blockNumber].blockOffset);
            char[] nameslist = new char[remainingSize];
            for (int i = 0; i < remainingSize; i++)
            {
                nameslist[i] = br.ReadChar();
            }

            folderBlock.names = nameslist;
            if (folderBlock.subDirectories != null)
            {
                foreach (DirectoryEntry directoryEntry in folderBlock.subDirectories)
                {
                    string word = "";
                    int increment = 0;
                    for (int t = 0; t < 200; t++)
                    {
                        char c = folderBlock.names[directoryEntry.nameOffset + increment];
                        increment++;
                        if (c != '\0')
                            word += c;
                        else
                            break;
                    }
                    FolderBlock fB = ReadBlock((int)directoryEntry.nextBlock, currentDir + "\\" + word, br);
                }
            }

            if (folderBlock.files != null)
            {
                foreach (FileEntry fileEntry in folderBlock.files)
                {
                    string word = "";
                    int increment = 0;
                    for (int t = 0; t < 200; t++)
                    {
                        char c = folderBlock.names[fileEntry.nameOffset + increment];
                        increment++;
                        if (c != '\0')
                            word += c;
                        else
                            break;
                    }
                }
            }
            return folderBlock;
        }

        private static DirectoryEntry ReadDirectoryEntry(BinaryReader br)
        {
            DirectoryEntry directoryEntry   = new DirectoryEntry();
            directoryEntry.nameOffset       = br.ReadUInt32();
            directoryEntry.nextBlock        = br.ReadUInt32();
            return directoryEntry;
        }

        private static FileEntry ReadFileEntry(BinaryReader br)
        {
            FileEntry fileEntry         = new FileEntry();
            fileEntry.nameOffset        = br.ReadUInt32();
            fileEntry.flags             = br.ReadUInt32();
            fileEntry.writeTime         = br.ReadUInt64();
            fileEntry.uncompressedSize  = br.ReadUInt64();
            fileEntry.compressedSize    = br.ReadUInt64();
            fileEntry.hash              = br.ReadBytes(20);
            fileEntry.unk2              = br.ReadUInt32();
            return fileEntry;
        }
    }
}
