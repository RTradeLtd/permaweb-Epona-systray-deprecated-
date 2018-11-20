namespace WindowsIPFSController
{
    class IPFSElement
    {
        public string Name;
        public string Path;
        public string Hash;
        public bool Active;
        public FileType FileType;
        public bool IsHardLink;
        public bool IsLinkOnly;

        public IPFSElement()
        {

        }

        public IPFSElement(string name, string path, string hash, bool active, FileType filetype, bool isHardLink, bool isLinkOnly)
        {
            Name = name;
            Path = path;
            Hash = hash;
            Active = active;
            FileType = filetype;
            IsHardLink = isHardLink;
            IsLinkOnly = isLinkOnly;
        }

    }
}

public enum FileType
{
    FILE,
    FOLDER
}
