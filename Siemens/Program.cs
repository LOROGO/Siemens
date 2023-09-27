using System;
using System.IO;
using System.Text.Json;

namespace Siemens
{

    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string path = GetFolderPath();

                if (path == null)
                {
                    break;
                }

                List<string> list = GetUniqueFileTypes(path);







            }

        }

        static string GetFolderPath()
        {
            Console.WriteLine("Please provide a folder or a JSON with folder information:");
            string path = Console.ReadLine();
            if (Directory.Exists(path))
            {

                return path;
            }
            else
                return null;

        }

        static List<string> GetUniqueFileTypes(string path)
        {
            List<string> types = new List<string>();

            string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string type = Path.GetExtension(file);
                if (!string.IsNullOrEmpty(type) && !types.Contains(type))
                {
                    types.Add(type);

                }
            }

            return types;
        }









    }
}