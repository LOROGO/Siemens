using System;
using System.IO;
using System.Text.Json;

namespace Siemens
{
    public class File_data
    {
        public string name { get; set; }

        public string type { get; set; }

        public List<File_data> children { get; set; }

    }
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
                PrintTypes(list);

                string json_path = SaveDialogue();
                if (!String.IsNullOrEmpty(json_path))
                {
                    File_data directory_data = ProcessDirectory(path);
                    var jsonString = JsonSerializer.Serialize(directory_data);
                    File.WriteAllText(json_path, jsonString);


                }




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

        static void PrintTypes(List<string> types)
        {
            string result = string.Join(", ", types);
            Console.WriteLine($"Extensions found in folder: {result}");

        }

        static string SaveDialogue()
        {
            Console.WriteLine("Save to JSON?");
            string answer = Console.ReadLine();

            if (!string.IsNullOrEmpty(answer))
            {
                if (answer.StartsWith("y") || answer.StartsWith("Y"))
                {
                    Console.WriteLine("Please provide the JSON file location:");
                    string path = Console.ReadLine();
                   
                    return path;
                }

            }
            return null;
        }


        public static File_data ProcessDirectory(string targetDirectory)
        {
            File_data file = new File_data();
            file.children = new List<File_data>();
            file.type = "directory";
            file.name = Path.GetFileName(targetDirectory);

            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                file.children.Add(ProcessFile(fileName));

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                file.children.Add(ProcessDirectory(subdirectory));

            return file;
        }

        public static File_data ProcessFile(string path)
        {
            File_data file = new File_data();

            file.type = Path.GetExtension(path);
            file.name = Path.GetFileName(path);
            return file;

        }



    }
}