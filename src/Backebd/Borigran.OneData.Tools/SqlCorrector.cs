using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Borigran.OneData.Tools
{
    public class SqlCorrector
    {
        public const string srcPath = "D:\\Projects\\Borigran\\Git\\src\\Backebd\\Borigran.OneData.WebApi\\Sql\\ConstructorItems.sql";
        public const string dstPath = "D:\\Projects\\Borigran\\Git\\src\\Backebd\\Borigran.OneData.WebApi\\Sql\\ConstructorItems_corrected.sql";

        public const int fromStrNumber = 50;
        public const int toStrNumber = 191;

        public static void CorrectData()
        {
            using(var reader = File.OpenText(srcPath))
            {
                using(var writer = new StreamWriter(dstPath))
                {
                    WriteNoChange(1, fromStrNumber, reader, writer);

                    var tail = DoCorrection(reader, writer);

                    WriteNoChange(toStrNumber + 1, null, reader, writer);

                    if(tail != null)
                    {
                        writer.WriteLine();
                        writer.Write(tail);
                    }

                    writer.Flush();
                }
            }
        }

        private static void WriteNoChange(int firstLineNumber, int? lastLineNumber, 
            StreamReader reader, StreamWriter writer)
        {
            int i = firstLineNumber;
            string str = reader.ReadLine();

            while(str != null)
            {
                writer.WriteLine(str);
                i++;
                if (lastLineNumber.HasValue && i == lastLineNumber)
                {
                    break;
                }
                str = reader.ReadLine();
            }
        }

        private static StringBuilder DoCorrection(StreamReader reader, StreamWriter writer)
        {
            var tail = new StringBuilder();

            for(int i = 1; i <= toStrNumber - fromStrNumber + 1; i++)
            {
                string line = reader.ReadLine();
                string tailLine = CorrectLine(ref line, i);

                if(!string.IsNullOrEmpty(tailLine))
                {
                    tail.AppendLine(tailLine);
                }
                writer.WriteLine(line);
            }

            return tail;
        }

        private static string CorrectLine(ref string line, int id)
        {
            var random = new Random(id);

            int startBodyIndex = line.IndexOf('(') + 1;
            int endBodyIndex = line.IndexOf(')');
            if(startBodyIndex == 0 || endBodyIndex< 0)
            {
                return null;
            }

            string prefix = line.Substring(0, startBodyIndex);
            string tail = line.Substring(endBodyIndex);
            string bodyStr = line.Substring(startBodyIndex, endBodyIndex - startBodyIndex);

            var body = new List<string>(bodyStr.Split(','));
            if (body.Count < 7)
            {
                return null;
            }

            if (body[0].StartsWith("'Форма№"))
            {
                body[0] = body[0].Replace("Форма№", "Форма №");
            }    

            if (body[1] == "NULL")
            {
                body[1] = body[0];
            }

            var tailBody = new string[] { body[1], "0", id.ToString() };
            var newLine = $"{prefix}{string.Join(',', tailBody)}{tail}";

            body[2] = random.Next(100, 1000).ToString();
            body[8] = random.Next(20, 200).ToString();

            body.RemoveAt(1);
            body.Insert(0, id.ToString());

            line = $"{prefix}{string.Join(',', body)}{tail}";

            return newLine;
        }
    }
}
