namespace EpubBuilder {
    class Data {
        private static string coverPath = "";
        private static string mdPath = "";
        private static string epubBuildPath = "";
        private static string title = "";
        private static string creator = "";
        private static string descributor = "";
        private static string type = "";
        private static string date = "";
        private static string language = "";

        private static string fontFamily = "";
        private static string p_fontSize = "";
        private static string h1_fontSize = "";
        private static string h2_fontSize = "";
        private static string h3_fontSize = "";
        private static string h4_fontSize = "";
        private static string h5_fontSize = "";
        private static string h6_fontSize = "";

        private static int navPointLevel = 0;

        public static string CoverPath {
            get { return coverPath; }
            set { coverPath = value; }
        }
        public static string MdPath {
            get { return mdPath; }
            set { mdPath = value; }
        }
        public static string EpubBuildPath {
            get { return epubBuildPath;}
            set {
                if (value[value.Length-1] != '/') {
                    epubBuildPath = value + "/";
                }
                else {
                    epubBuildPath = value;
                }
            }
        }
        public static string Title {
            get { return title; }
            set { title = value; }
        }
        public static string Creator {
            get { return creator; }
            set { creator = value; }
        }
        public static string Description {
            get { return descributor; }
            set { descributor = value; }
        }
        public static string Type {
            get { return type; }
            set { type = value; }
        }
        public static string Date {
            get { return date; }
            set { date = value; }
        }
        public static string Language {
            get { return language; }
            set { language = value; }
        }

        public static string EpubPath {
            get { return $"{EpubBuildPath}{Title}/"; }
        }
        public static string OEBPSPath {
            get { return $"{EpubBuildPath}{Title}/OEBPS/"; }
        }
        public static string ImagePath {
            get { return $"{EpubBuildPath}{Title}/OEBPS/Images/"; }
        }
        public static string TextPath {
            get { return $"{EpubBuildPath}{Title}/OEBPS/Text/"; }
        }
        public static string StylesPath {
            get { return $"{EpubBuildPath}{Title}/OEBPS/Styles/"; }
        }

        public static string FontFamily {
            get {
                if (fontFamily == "") {
                    return "Source Han Sans SC";
                }
                
                return fontFamily;
            }

            set { fontFamily = value; }
        }

        public static string P_FontSize {
            get {
                if (p_fontSize == "") {
                    return "1em";
                }

                return p_fontSize;
            }
            set { p_fontSize = value; }
        }

        public static string H1_FontSize {
            get {
                if (h1_fontSize == "") {
                    return "1.4em";
                }

                return h1_fontSize;
            }
            set { h1_fontSize = value; }
        }

        public static string H2_FontSize {
            get {
                if (h2_fontSize == "") {
                    return "1.28em";
                }

                return h2_fontSize;
            }
            set { h2_fontSize = value; }
        }

        public static string H3_FontSize {
            get {
                if (h3_fontSize == "") {
                    return "1.1em";
                }

                return h3_fontSize;
            }
            set { h3_fontSize = value; }
        }

        public static string H4_FontSize {
            get {
                if (h4_fontSize == "") {
                    return "1em";
                }

                return h4_fontSize;
            }
            set { h4_fontSize = value; }
        }

        public static string H5_FontSize {
            get {
                if (h5_fontSize == "") {
                    return "1em";
                }

                return h5_fontSize;
            }
            set { h5_fontSize = value; }
        }

        public static string H6_FontSize {
            get {
                if (h6_fontSize == "") {
                    return "1em";
                }

                return h6_fontSize;
            }
            set { h6_fontSize = value; }
        }

        public static int NavPointLevel {
            get { return navPointLevel;}
            set { navPointLevel = value; }
        }


        public static void PrintData() {
            Console.WriteLine($"CoverPath:\t{coverPath}");
            Console.WriteLine($"MarkdownPath:\t{mdPath}");
            Console.WriteLine($"EpubBuildPath:\t{epubBuildPath}");
            Console.WriteLine($"\nTitle:\t\t{title}");
            Console.WriteLine($"Creator:\t{creator}");
            Console.WriteLine($"Type:\t\t{type}");
            Console.WriteLine($"Date:\t\t{date}");
            Console.WriteLine($"Language:\t{language}");
            Console.WriteLine($"\nFontFamily:\t{FontFamily}");
            Console.WriteLine($"P.FontSize:\t{P_FontSize}");
        }
    }
}