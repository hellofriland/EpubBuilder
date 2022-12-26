namespace EpubBuilder {
    class Epub {
        public static List<string> Manifest = new List<string>();
        public static List<string> Spine = new List<string>();
        /// <summary>
        /// 通过makefile文件进行Epub书籍的创建
        /// </summary>
        /// <param name="makefilePath"></param>
        public void MakeEpub(string makefilePath) {
            Makefile.ParseMakefile(makefilePath);
            CreateBasicEnvoriment();

            Parser parser = new Parser();
            List<Page> pages = parser.ParseArticle(IOFile.InputTextLine(Data.MdPath));

            for (int i = 0; i < pages.Count; i++) {
                IOFile.OutputText(pages[i].Content,$"{Data.TextPath}Chapter{i}.xhtml");

                Manifest.Add($"\t<item id=\"Chapter{i}.xhtml\" href=\"Text/Chapter{i}.xhtml\" media-type=\"application/xhtml+xml\"/>");
                Spine.Add($"\t<itemref idref=\"Chapter{i}.xhtml\"/>");
            }

            CreateNCX(pages);
            CreateOPF();
            CreateCss();
            IOFile.Zip(Data.EpubPath,$"{Data.EpubBuildPath}{Data.Title}.epub");
            IOFile.DeleteDirectory(Data.EpubPath);
        }

        private void CreateBasicEnvoriment() {
            // 创建存储epub文件的文件夹
            IOFile.CreateDirectory(Data.Title,Data.EpubBuildPath);
            // 创建epub/mimetype文件
            IOFile.OutputText("application/epub+zip",Data.EpubBuildPath+Data.Title+"/"+"mimetype");
            // 创建epub/META-INF文件夹
            IOFile.CreateDirectory("META-INF",Data.EpubBuildPath+Data.Title);
            // 创建epub/META-INF/container.xml文件
            IOFile.OutputText(
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<container version=\"1.0\" xmlns=\"urn:oasis:names:tc:opendocument:xmlns:container\">\n" +
                "\t<rootfiles>\n" +
                "\t\t<rootfile full-path=\"OEBPS/content.opf\" media-type=\"application/oebps-package+xml\"/>\n" +
                "\t</rootfiles>\n" +
                "</container>",
                Data.EpubBuildPath+Data.Title + "/" + "META-INF/" + "container.xml"
            );
            // 创建epub/OEBPS文件夹
            IOFile.CreateDirectory("OEBPS",Data.EpubPath);
            // 创建epub/OEBPS/Text文件夹
            IOFile.CreateDirectory("Text",Data.OEBPSPath);
            IOFile.CreateDirectory("Images",Data.OEBPSPath);
            // 创建epub/OEBPS/Style文件夹
            IOFile.CreateDirectory("Styles",Data.OEBPSPath);
            // 将封面图移动到Images文件夹中
            if (Data.CoverPath != "") {
                IOFile.CopyFileTo(Data.CoverPath,$"{Data.ImagePath}cover.jpg");
            }
        }

        private void CreateOPF() {
            List<string> opf = new List<string>();
            opf.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            opf.Add("<package version=\"2.0\" unique-identifier=\"BookId\" xmlns=\"http://www.idpf.org/2007/opf\">\n");

            opf.Add("<metadata xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:opf=\"http://www.idpf.org/2007/opf\">");
            if (Data.Title!= "")        { opf.Add($"\t<dc:title>{Data.Title}</dc:title>"); }
            if (Data.Creator!= "")      { opf.Add($"\t<dc:creator>{Data.Creator}</dc:creator>"); }
            if (Data.Description!= "")  { opf.Add($"\t<dc:description>{Data.Description}</dc:description>"); }
            if (Data.Type!= "")         { opf.Add($"\t<dc:type>{Data.Type}</dc:type>"); }
            if (Data.Date!= "")         { opf.Add($"\t<dc:date>{Data.Date}</dc:date>"); }
            if (Data.Language!= "")     { opf.Add($"\t<dc:language>{Data.Language}</dc:language>"); }
            if (Data.CoverPath != "")   { opf.Add($"\t<meta name=\"cover\" content=\"cover.jpg\"/>"); }
            opf.Add("</metadata>\n");

            opf.Add("<manifest>");
            opf.Add("\t<item id=\"ncx\" href=\"toc.ncx\" media-type=\"application/x-dtbncx+xml\"/>");
            opf.Add("\t<item id=\"stylesheet.css\" href=\"Styles/stylesheet.css\" media-type=\"text/css\"/>");
            if (File.Exists($"{Data.ImagePath}cover.jpg")) {
                opf.Add("\t<item id=\"cover.jpg\" href=\"Images/cover.jpg\" media-type=\"image/jpg\"/>");
            }
            for (int i = 0; i < Manifest.Count; i++) {
                opf.Add(Manifest[i]);
            }
            opf.Add("</manifest>\n");

            opf.Add("<spine toc=\"ncx\">");
            for (int i = 0; i < Spine.Count; i++) {
                opf.Add(Spine[i]);
            }
            opf.Add("</spine>\n");
            opf.Add("</package>");

            IOFile.OutputText(opf,$"{Data.OEBPSPath}content.opf");
        }

        private void CreateNCX(List<Page> pages) {
            List<string> ncx = new List<string>();
            ncx.Add("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            ncx.Add(
                "<!DOCTYPE ncx PUBLIC \"-//NISO//DTD ncx 2005-1//EN\"\n" +
                "\"http://www.daisy.org/z3986/2005/ncx-2005-1.dtd\"><ncx version=\"2005-1\" xmlns=\"http://www.daisy.org/z3986/2005/ncx/\">"
            );

            ncx.Add(
                "<head>\n" +
                "\t<meta content=\"urn:uuid:bf0b8110-3ab9-47a8-b09b-b1cdf6564e08\" name=\"dtb:uid\"/>\n" +
                "\t<meta content=\"1\" name=\"dtb:depth\"/>\n" +
                "\t<meta content=\"0\" name=\"dtb:totalPageCount\"/>\n" +
                "\t<meta content=\"0\" name=\"dtb:maxPageNumber\"/>\n" +
                "</head>\n"
            );

            ncx.Add($"<docTitle><text>{Data.Title}</text></docTitle>\n");

            ncx.Add("<navMap>");
            
            for (int i = 0; i < pages.Count; i++) {
                for (int n = 0; n < pages[i].Ncx.Count; n++) {
                    ncx.Add(pages[i].Ncx[n]);
                }
            }

            ncx.Add("</navMap>");

            ncx.Add("</ncx>");

            IOFile.OutputText(ncx,$"{Data.OEBPSPath}toc.ncx");
        }


        private void CreateCss() {
            List<string> css = new List<string>();
            // h1
            css.Add(
                "h1 {\n" +
                $"\tfont-family: \"{Data.FontFamily}\";\n" +
                $"\tfont-size:{Data.H1_FontSize};\n" +
                $"\ttext-align: center;\n" +
                "\tcolor: #0468bb;\n" +
                "\tfont-weight: normal;\n" +
                "\tmargin-top: 30%;\n" +
                "\tmargin-bottom: 2em;\n" + 
                "\tline-height:1.4;\n" +
                "}\n"
            );

            // h2
            css.Add(
                "h2 {\n" +
                $"\tfont-family: \"{Data.FontFamily}\";\n" +
                $"\tfont-size:{Data.H2_FontSize};\n" +
                $"\ttext-align:center;\n" +
                "\tcolor: #0468bb;\n" + 
                "\tfont-weight: normal;\n" +
                "\tmargin-top: 2em;\n" +
                "\tmargin-bottom: 1.5em;\n" +
                "}\n"
            );

            // h3
            css.Add(
                "h3 {\n" +
                $"\tfont-family: \"{Data.FontFamily}\";\n" +
                $"\tfont-size:{Data.H3_FontSize};\n" +
                $"\ttext-align:center;\n" +
                "\tcolor: #0468bb;\n" + 
                "\tfont-weight: normal;\n" +
                "\tmargin-top: 2em;\n" +
                "\tmargin-bottom: 1.2em;\n" +
                "}\n"
            );

            // h4
            css.Add(
                "h4 {\n" +
                $"\tfont-family: \"{Data.FontFamily}\";\n" +
                $"\tfont-size:{Data.H4_FontSize};\n" +
                $"\ttext-align:center;\n" +
                "\tcolor: #0468bb;\n" + 
                "\tfont-weight: normal;\n" +
                "\tmargin-top: 1.2em;\n" +
                "\tmargin-bottom: 1em;\n" +
                "}\n"
            );

            // h5
            css.Add(
                "h5 {\n" +
                $"\tfont-family: \"{Data.FontFamily}\";\n" +
                $"\tfont-size:{Data.H5_FontSize};\n" +
                $"\ttext-align:center;\n" +
                "\tcolor: #0468bb;\n" + 
                "\tfont-weight: normal;\n" +
                "\tmargin-top: 1em;\n" +
                "\tmargin-bottom: 1em;\n" +
                "}\n"
            );

            // h6
            css.Add(
                "h6 {\n" +
                $"\tfont-family: \"{Data.FontFamily}\";\n" +
                $"\tfont-size:{Data.H6_FontSize};\n" +
                $"\ttext-align:center;\n" +
                "\tcolor: #0468bb;\n" + 
                "\tfont-weight: normal;\n" +
                "\tmargin-top: 1em;\n" +
                "\tmargin-bottom: 1em;\n" +
                "}\n"
            );

            // p.bodytext
            css.Add (
                "p.bodytext {\n" +
                "\tline-height:1.84;\n" +
                $"\tfont-size:{Data.P_FontSize};\n" +
                $"\tfont-family:\"{Data.FontFamily}\";\n" +
                "\tmargin: 0.7em;\n" +
                "\ttext-align:justify;\n" +
                "\ttext-indent: 2em;\n" +
                "}\n"
            );

            IOFile.OutputText(css,$"{Data.StylesPath}stylesheet.css");
        }
    }
}