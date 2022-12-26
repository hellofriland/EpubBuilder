using System.IO.Compression;
namespace EpubBuilder {
    class IOFile {

        /// <summary>
        /// 从指定文件中导入所有行
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> InputTextLine(string path) {
            List<string> list;
            if (!File.Exists(path)) {
                Console.WriteLine($"Non-existent file: {path}");
                list = new List<string>();
                list.Add("Non-existent file");
            }
            else {
                list = new List<string>(File.ReadAllLines(path));
            }

            return list;
        }

        /// <summary>
        /// 将所有行导出到指定文件
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        public static void OutputText(List<string> text, string path) {
            FileInfo fl = new FileInfo(path);
            FileStream fs = fl.Create();
            fs.Close();
            fl.Delete();

            using (StreamWriter sw = new StreamWriter(path)) {
                for (int i = 0; i < text.Count; i++) {
                    sw.WriteLine(text[i]);
                }
            }
        }

        public static void OutputText(string text, string path) {
            FileInfo fl = new FileInfo(path);
            FileStream fs = fl.Create();
            fs.Close();
            fl.Delete();

            using (StreamWriter sw = new StreamWriter(path)) {
                sw.WriteLine(text);
            }
        }

        /// <summary>
        /// 在指定路径下创建指定名称的文件夹
        /// </summary>
        /// <param name="directoryName"></param>
        /// <param name="path"></param>
        public static void CreateDirectory(string directoryName, string path) {
            if(path[path.Length-1] != '/') {
                path = path + "/";
            }
            path = path + directoryName;
            Directory.CreateDirectory(@path);
        }

        public static void DeleteDirectory(string directoryPath) {
            Directory.Delete(directoryPath,true);
        }

        public static void CopyFileTo(string filePath,string glodPath) {
            if(File.Exists(glodPath)) {
                File.Delete(glodPath);
            }
            File.Copy(filePath,glodPath);
        }

        public static void Zip(string dirPath, string glodPath) {
            if (File.Exists(glodPath)) {
                File.Delete(glodPath);
            }
            ZipFile.CreateFromDirectory(dirPath,glodPath);
        }

        public static bool IsFileExist(string path) {
            bool isExist = false;
            if(File.Exists(path)) {
                isExist = true;
            }

            return isExist;
        }

        public static bool IsDirExist(string path) {
            bool isExist = false;
            if (Directory.Exists(path)) {
                isExist = true;
            }

            return isExist;
        }
    }
}