using System.Text.RegularExpressions;

namespace EpubBuilder {
    class Parser {
        private int chapterNum = 0;
        private List<string> footnoteBackList = new List<string>();
        public List<Page> ParseArticle(List<string> article) {
            List<Page> pages = new List<Page>();
            // 将文章拆分成各个章节
            List<List<string>> chapterList = SplitChapter(article);

            // 将所有章节转换为Page存入List列表中，然后返回
            for (int i = 0; i < chapterList.Count; i++) {
                pages.Add(ParseChapter(chapterList[i]));
            }

            return pages;
        }

        private List<List<string>> SplitChapter(List<string> article) {
            int pos = 0;
            List<List<string>> chapterList = new List<List<string>>();
            
            for (int i = 0; i < article.Count; i++) {
                if (GetHeadlineLevel(article[i]) == 1) {
                    chapterList.Add(new List<string>());
                    pos++;
                }
                
                if (chapterList.Count == 0) {
                    chapterList.Add(new List<string>());
                }

                chapterList[pos -1].Add(article[i]);
                
            }

            return chapterList;
        }

        private Page ParseChapter(List<string> chapter) {
            Page page = new Page(GetHeadlineText(chapter[0]), new List<string>(), ParseNCX(chapter));
            page.Content.Add(
                "<?xml version='1.0' encoding='utf-8'?>\n" +
                "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n" +
                "<head>\n" +
                "\t<title>" + Data.Title + "</title>\n" +
                "\t<link href=\"../Styles/stylesheet.css\" rel=\"stylesheet\" type=\"text/css\"/>\n" +
                "</head>\n"
            );
            page.Content.Add("<body>");

            for (int i = 0; i < chapter.Count; i++ ) {
                page.Content.Add("\t" + ParseLine(chapter[i]));
            }

            page.Content.Add("</body>\n" +
            "</html>"
            );

            return page;
        }

        private List<string> ParseNCX(List<string> chapter) {
            List<string> chapterNcx = new List<string>();
            chapterNcx.Add($"<navPoint id=\"navPoint-{Data.NavPointLevel+1}\" playOrder=\"{Data.NavPointLevel+1}\">");
            chapterNcx.Add("\t<navLabel><text>" + GetHeadlineText(chapter[0]) + "</text></navLabel>");
            chapterNcx.Add($"\t<content src=\"Text/Chapter{chapterNum}.xhtml\"/>");
            chapterNcx.Add("</navPoint>\n");

            chapterNum++;
            Data.NavPointLevel++;

            return chapterNcx;
        }

        private string ParseLine(string text) {
            text = ScanParagrapy(text);
            text = ScanHeadline(text);
            text = ScanBold(text);
            if (IsFootnoteBack(text)) {
                ScanFootnote_Back(text);
                return "";
            }
            text = ScanFootnote_Body(text);

            return text;
        }

        private bool IsBR(string text) {
            bool isBR = false;
            if (text == "<br/>") {
                isBR = true;
            }

            return isBR;
        }

        private string ScanParagrapy(string text) {
            if (!IsHeadline(text) && !IsFootnoteBack(text) && !IsBR(text)) {
                text = "<p class=\"bodytext\">" + text + "</p>";
            }

            return text;
        }

        private bool IsFootnoteBack(string text) {
            bool isFootnoteBack = false;

            string pattern = @"\[\^[0-9][0-9]*\-[0-9][0-9]*\]:";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(text)) {
                isFootnoteBack = true;
            }

            return isFootnoteBack;
        }

        public int GetHeadlineLevel(string text) {
            int level = 0;
            for (int i = 0; i < text.Length; i++) {
                if(text[i] == '#') {
                    level++;
                }
                else {
                    break;
                }
            }
            if (level > 6) {
                level = 0;
            }

            return level;
        }

        public bool IsHeadline(string text) {
            bool isHeadline = false;
            int temp = GetHeadlineLevel(text);
            if (temp != 0) {
                isHeadline = true;
            }

            return isHeadline;
        }

        /// <summary>
        /// 获取标题文字内容
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetHeadlineText(string text) {
            int headlevel = GetHeadlineLevel(text);
            if (headlevel != 0) {
                text = text.Substring(GetHeadlineLevel(text) + 1);
            }
            else {
                throw new Exception($"\"{text}\" is not a headline");
            }

            return text;
        }

        public string ScanHeadline(string text) {
            if (IsHeadline(text)) {
                int level = GetHeadlineLevel(text);
                text = "<h" + level + ">" + text.Substring(level + 1) + "</h" + level + ">";
            }

            return text;
        }

        public int GetMarkNum(string mark, string text) {
            int markNum = 0;
            while (text.IndexOf(mark) != -1) {
                int index = text.IndexOf(mark);
                text = text.Substring(0,index) + text.Substring(index + mark.Length);
                markNum++;
            }

            return markNum;
        }

        private string ScanBold(string text) {
            int boldNum = GetMarkNum("**", text);
            if (boldNum % 2 == 1) {
                boldNum  = boldNum -1;
            }

            while (boldNum != 0) {
                int boldMarkPos = text.IndexOf("**");
                text = text.Substring(0,boldMarkPos) + text.Substring(boldMarkPos+ "**".Length);

                if (boldNum %2 == 0) {
                    text = text.Substring(0,boldMarkPos) + "<strong>" + text.Substring(boldMarkPos);
                }
                else {
                    text = text.Substring(0,boldMarkPos) + "</strong>" + text.Substring(boldMarkPos);
                }
                boldNum--;
            }

            return text;
        }

        private string ScanFootnote_Body(string text) {
            string pattern = @"\[\^[0-9][0-9]*\-[0-9][0-9]*\]";
            Regex regex = new Regex(pattern);
            while (regex.IsMatch(text)) {
                string v = regex.Match(text).ToString();
                string temp = "";
                for (int i=1; i<v.Length;i++) {
                    if (IsDigit(v[i]) || v[i] == '-') {
                        temp  = temp + v[i];
                    }
                }
                string p2 = $@"\[\^{temp}\]";
                Regex r2 = new Regex(p2);
                text = r2.Replace(text,$"<a href=\"#fn{temp}\" class=\"footnote-ref\" id=\"fnref{temp}\" epub:type=\"noteref\">{temp}</a>");
            }

            return text;
        }

        private void ScanFootnote_Back(string text) {
            string pattern = @"\[\^[0-9][0-9]*\-[0-9][0-9]*\]:";
            Regex regex = new Regex(pattern);
            while (regex.IsMatch(text)) {
                string v = regex.Match(text).ToString();
                string temp = "";
                for (int i=1; i<v.Length;i++) {
                    if (IsDigit(v[i]) || v[i] == '-') {
                        temp  = temp + v[i];
                    }
                }
                string p2 = $@"\[\^{temp}\]:";
                Regex r2 = new Regex(p2);
                text = r2.Replace(text,"");
                text = $"<li id=\"fn{temp}\" epub:type=\"footnote\"><p>{text}<a href=\"#fnref{temp}\" class=\"footnote-back\" role=\"doc-backlink\">↩︎</a></p></li>";
                footnoteBackList.Add(text);
            }
        }

        public static bool IsDigit(char ch) {
            bool isDigit = false;
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