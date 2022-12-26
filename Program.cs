namespace EpubBuilder {
    class Program {
        public static void Main(string[] args) {
            Console.Write("Makefile Path:");
            string path = Console.ReadLine();
            if (path == "") {
                Console.WriteLine("Please enter Path!!!");
                Environment.Exit(0);
            }
            else {
                Makefile.ParseMakefile(path);
            }
        }
    }
}