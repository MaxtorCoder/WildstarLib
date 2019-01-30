using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildstarLib.Format.Archive.Index
{
    public struct IndexHeader
    {
        public UInt32 signature;
        public UInt32 version;
        public UInt64 indexFileSize;
        public UInt64 ofsBlockTable;
        public UInt32 numBlocks;
    }

    public struct PackBlockHeader
    {
        public UInt64 blockOffset;
        public UInt64 blockSize;
    };

    struct AIDXEntry
    {
        public UInt32 magic; // 'AIDX'
        public UInt32 version;
        public UInt32 clientBuild;
        public UInt32 rootBlock;
    }

    public struct FolderBlock
    {
        public UInt32 numSubdirectories;
        public UInt32 numFiles;
        public DirectoryEntry[] subDirectories;
        public FileEntry[] files;
        public char[] names;
    }

    public struct DirectoryEntry
    {
        public UInt32 nameOffset;
        public UInt32 nextBlock;
    }

    public struct FileEntry
    {
        public UInt32 nameOffset;
        public UInt32 flags;        // 3: zlib compressed, 5: lzma compressed
        public UInt64 writeTime;    // uint64 // FILETIME
        public UInt64 uncompressedSize;
        public UInt64 compressedSize;
        public byte[] hash;
        public UInt32 unk2;
    }
}
