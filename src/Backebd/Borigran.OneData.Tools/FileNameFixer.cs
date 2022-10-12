namespace Borigran.OneData.Tools
{
    public class FileNameFixer
    {
        public const string RootPath = "D:\\Projects\\Borigran\\Git\\src\\Backebd\\Borigran.OneData.WebApi\\StaticResources\\CItemImages";
        public const string TargetFileSearchPattern = "*.png";
        public const int NewImageWidth = 800;
        public static void FixImageNames()
        {
            string[] subDirs = Directory.GetDirectories(RootPath);
            foreach (var dir in subDirs)
            {
                ProcessFolder(dir);
            }
        }

        private static void ProcessFolder(string folderPath)
        {
            ProcessFiles(folderPath);

            string[] subDirs = Directory.GetDirectories(folderPath);
            foreach (var dir in subDirs)
            {
                ProcessFolder(dir);
            }
        }

        private static void ProcessFiles(string folderPath)
        {
            Console.WriteLine($"Process folder: {folderPath}");
            string[] targetFiles = Directory.GetFiles(folderPath, TargetFileSearchPattern);
            foreach(var file in targetFiles)
            {
                ProcessImageFile(file);
            }

            Console.WriteLine();
        }

        private static void ProcessImageFile(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            string ext = Path.GetExtension(filePath);

            string fileName = Path.GetFileNameWithoutExtension(filePath);
            fileName = RemoveTail(fileName, "_new");
            fileName = RemoveTail(fileName, " (1)");
            fileName = RemoveTail(fileName, " (2)");

            if(fileName.StartsWith("Форма№"))
            {
                fileName = fileName.Replace("Форма№", "Форма №");
            }
            if (fileName.StartsWith("Форма № "))
            {
                fileName = fileName.Replace("Форма № ", "Форма №");
            }
            if (fileName.StartsWith("Форма 1"))
            {
                fileName = fileName.Replace("Форма 1", "Форма №1");
            }
            if (fileName.StartsWith("Форма 2"))
            {
                fileName = fileName.Replace("Форма 2", "Форма №2");
            }

            string newFilePath = Path.Combine(dir, $"{fileName}{ext}");

            if (filePath != newFilePath)
            {
                File.Move(filePath, newFilePath, true);

                Console.WriteLine($"Copy {Path.GetFileName(filePath)} to {Path.GetFileName(newFilePath)}");
            }
        }

        private static string RemoveTail(string fileName, string tile)
        {
            if (fileName.EndsWith(tile))
            {
                return fileName.Substring(0, fileName.Length - tile.Length);
            }

            return fileName;
        }
    }
}
