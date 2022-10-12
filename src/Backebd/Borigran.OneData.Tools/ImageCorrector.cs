using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Borigran.OneData.Tools
{
    public class ImageCorrector
    {
        public const string RootPath = "D:\\Projects\\Borigran\\Git\\src\\Backebd\\Borigran.OneData.WebApi\\StaticResources\\CItemImages";
        public const string TargetFileSearchPattern = "*.png";
        public const int NewImageWidth = 800;
        public static void ResizeImages()
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
                ProcessImage(file);
            }

            string[] jpegs = Directory.GetFiles(folderPath, "*.jpg");
            foreach (var file in jpegs)
            {
                File.Delete(file);
            }

            Console.WriteLine();
        }

        private static void ProcessImage(string filePath)
        {
            Console.WriteLine($"Resize {Path.GetFileName(filePath)}");

            var image = Image.FromFile(filePath);
            var oldSize = image.Size;

            var k = (double)oldSize.Width / NewImageWidth;
            var newHeight = (int)(oldSize.Height / k);

            using (var newImage = ResizeImage(image, NewImageWidth, newHeight))
            {
                image.Dispose();

                var newFileNamePng = Path.Combine(Path.GetDirectoryName(filePath),
                    $"{Path.GetFileNameWithoutExtension(filePath)}_new.png");
                newImage.Save(newFileNamePng, ImageFormat.Png);

            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
