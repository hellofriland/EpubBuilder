namespace EpubBuilder {
    class Makefile {
        /// <summary>
        /// 通过makefilePath读取makefile文件，并且将值读入程序中
        /// </summary>
        /// <param name="makefilePath"></param>
        public static void ParseMakefile(string makefilePath) {
            if (!IOFile.IsFileExist(makefilePath)) {
                Console.WriteLine($"{makefilePath} does not have a makefile");
                Environment.Exit(0);
            }

            List<string> makefile = IOFile.InputTextLine(makefilePath);

            for (int i = 0; i < makefile.Count; i++) {
                // 若当前行为空，或者以符号「"」开头，则忽略该行
                if (makefile[i] == "" || makefile[i][0] == '"') {
                    continue;
                }

                // 获取第一个「:」之前的内容
                string head = "";
                string tail = "";
                for (int n = 0; n < makefile[i].Length; n++) {
                    if(makefile[i][n] != ':') {
                        head = head + makefile[i][n];
                    }
                    else {
                        tail = makefile[i].Substring(n+1).Trim();
                        break;
                    }
                }

                // 判断当前的head是哪个属性的值，并且将值进行分配
                head = head.Trim().ToLower();
                switch (head) {
                    case "coverpath":
                        if (!Check(tail, CheckType.FilePath)) {
                            Console.Write($"Cover file is not found: {tail}");
                            Environment.Exit(0);
                        }
                        Data.CoverPath = tail;
                        break;
                    case "mdpath":
                        if (!Check(tail, CheckType.FilePath)) {
                            Console.Write($"Markdown file is not found: {tail}");
                            Environment.Exit(0);
                        }
                        Data.MdPath = tail;
                        break;
                    case "epubbuildpath":
                        if (!Check(tail,CheckType.DirPath)) {
                            Console.Write($"EpubBuildPath directory is not found: {tail}");
                            Environment.Exit(0);
                        }
                        Data.EpubBuildPath = tail;
                        break;
                    
                    case "title":
                        if(tail == "") {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("The title cannot be empty, Please check the lines about title in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Environment.Exit(0);
                        }
                        Data.Title = tail;
                        break;
                    
                    case "creator":             {Data.Creator           = tail; break;}
                    case "description":         {Data.Description       = tail; break;}
                    case "type":                {Data.Type              = tail; break;}
                    case "date":                {Data.Date              = tail; break;}
                    case "language":            {Data.Language          = tail; break;}

                    case "p.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.P_FontSize =tail;
                        break;
                    
                    case "h1.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.H1_FontSize =tail;
                        break;
                    
                    case "h2.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.H2_FontSize =tail;
                        break;
                    
                    case "h3.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.H3_FontSize =tail;
                        break;
                    
                    case "h4.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.H4_FontSize =tail;
                        break;
                    
                    case "h5.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.H5_FontSize =tail;
                        break;
                    
                    case "h6.fontsize":
                        if(!Check(tail,CheckType.FontSize)) {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{tail} is a incorrent font size format, Please check the lines about font size in your makefile");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"Example of correct font size format: 17px / 1em");
                            Environment.Exit(0);
                        }
                        Data.H6_FontSize =tail;
                        break;

                    default: {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"line {i+1} of makefile.txt cannot be parsed");
                        Console.WriteLine("Content: " + makefile[i] + "\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                }
            }

            // 因为文件夹的创建需要Title，因此title必须不为空
            if (Data.Title == "") {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The title cannot be empty, Please check the lines about title in your makefile");
                Console.ForegroundColor = ConsoleColor.White;
                Environment.Exit(0);
            }
        }

        private static bool Check(string value,CheckType type) {
            bool isPass = false;
            
            switch (type) {
                case CheckType.FilePath: {
                    isPass = IOFile.IsFileExist(value);
                    break;
                }
                case CheckType.DirPath: {
                    isPass = IOFile.IsDirExist(value);
                    break;
                }
                case CheckType.FontSize: {
                    isPass = CheckFontSize(value);
                    break;
                }
            }

            return isPass;
        }

        private static bool CheckFontSize(string size) {
            bool isFontSize = false;

            // 若size为空，则绝对为false，直接返回
            if (size == "") {
                isFontSize = false;
                return false;
            }
            
            if(IsDigit(size[0])) {
                for (int i = 0; i < size.Length; i++) {
                    if (!IsDigit(size[i])) {
                        string tail = size.Substring(i);
                        if (tail == "em" || tail == "px") {
                            isFontSize = true;
                        }
                    }
                }
            }

            return isFontSize;
        }

        private static bool IsDigit(char ch) {
            bool isDigit = false;
            if (ch == '.') {
                return true;
            }

            int temp;
            if (int.TryParse(ch.ToString(),out temp)) {
                if ( temp >= 0 && temp <= 9) {
                    isDigit = true;
                }
            }

            return isDigit;
        }
    }
}