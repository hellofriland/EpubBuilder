namespace EpubBuilder {
    struct Page {
        public string Name;
        public List<string> Content;

        public List<string> Ncx;

        public Page(string name, List<string> content, List<string> ncx) {
            Name = name;
            Content = content;
            Ncx = ncx;
        }
    }

    enum CheckType {
        DirPath,
        FilePath,
        FontSize
    }
}