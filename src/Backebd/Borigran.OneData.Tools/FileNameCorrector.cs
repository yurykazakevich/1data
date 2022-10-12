namespace Borigran.OneData.Tools
{
    [Obsolete]
    public class FileNameCorrector
    {
        public static void FixFileNames()
        {
            const string itemName = "Stella";

            const string size = "120x60x5";
            const int type = 22;


            string rootPath = "D:\\Projects\\Borigran\\Git\\src\\Backebd\\Borigran.OneData.WebApi\\StaticResources\\CItemImages";

            string itemDirectoryPath = Path.Combine(rootPath, itemName);

            foreach (string fileName in Directory.GetFiles(itemDirectoryPath))
            {
                string extension = Path.GetExtension(fileName);
                string oldFileName = Path.GetFileNameWithoutExtension(fileName);

                if (!oldFileName.EndsWith("_10") &&
                    !oldFileName.EndsWith("_20") &&
                    !oldFileName.EndsWith("_21") &&
                    !oldFileName.EndsWith("_22") &&
                    !oldFileName.EndsWith("_23") &&
                    !oldFileName.EndsWith("_30") &&
                    !oldFileName.EndsWith("_31") &&
                    !oldFileName.EndsWith("_32") &&
                    !oldFileName.EndsWith("_33"))
                {
                    string newfileName = $"{oldFileName.Replace(" ",string.Empty).Replace("_позиция2",string.Empty).Replace("№", " ")}_{size}_{type}{extension}";
                    File.Move(fileName,
                        Path.Combine(itemDirectoryPath, newfileName));

                    Console.WriteLine($"Rename {Path.GetFileName(fileName)} to {newfileName}");
                }

                /*if(oldFileName.EndsWith("80x40x45_21"))
                {
                    string newfileName = $"{oldFileName.Replace("80x40x45", "80x40x5")}{extension}";
                    File.Move(fileName,
                        Path.Combine(itemDirectoryPath, newfileName));

                    Console.WriteLine($"Rename {Path.GetFileName(fileName)} to {newfileName}");
                }*/
  
            }
        }
    }
}
