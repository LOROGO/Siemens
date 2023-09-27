using System;
using System.IO;
using System.Text.Json;

namespace Siemens
{
    public class FileData
    {
        public string name { get; set; }

        public string type { get; set; }

        public List<FileData>? children { get; set; }

        public List<string> GetUniqueFileTypesO()
        {
            if (children == null)
                return null;

            var list = new List<string>();
            foreach (var file in children)
            {
                if (file.type != "directory")
                {
                    if (!list.Contains(file.type))
                    {
                        list.Add(file.type);
                    }
                }
                else
                {
                    var sublist = file.GetUniqueFileTypesO();
                    list = list.Union(sublist).ToList();

                }
            }

            return list;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string path = GetFolderPath();

                if (path == null || path == "exit")
                {
                    break;
                }


                if (Directory.Exists(path))
                {

                    List<string> list = GetUniqueFileTypes(path);
                    PrintTypes(list);
                    string jsonPath = SaveDialogue();

                    if (jsonPath != null)
                    {
                        FileData directoryData = ProcessDirectory(path);
                        var jsonString = JsonSerializer.Serialize(directoryData);
                        SaveToJSON(jsonPath, jsonString);

                    }



                }
                else if (Path.GetExtension(path) == ".json")
                {
                    string jsonString = File.ReadAllText(path);
                    FileData directoryData = JsonSerializer.Deserialize<FileData>(jsonString);

                    PrintTypes(directoryData.GetUniqueFileTypesO());

                    string jsonPath = SaveDialogue();
                    if (jsonPath != null)
                    {
                        SaveToJSON(jsonPath, jsonString);
                        

                    }


                }
                
                Console.WriteLine("\n");


            }

        }

        static string GetFolderPath()
        {
            Console.WriteLine("Please provide a folder or a JSON with folder information:");
            string path = Console.ReadLine();

            if (string.IsNullOrEmpty(path))
            {
                return null;

            }
            else return path;


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


        public static FileData ProcessDirectory(string targetDirectory)
        {
            FileData file = new FileData();
            file.children = new List<FileData>();
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

        public static FileData ProcessFile(string path)
        {
            FileData file = new FileData();

            file.type = Path.GetExtension(path);
            file.name = Path.GetFileName(path);
            return file;

        }

        public static void SaveToJSON(string jsonPath, string jsonString)
        {
            try
            {
                File.WriteAllText(jsonPath, jsonString);
                //Console.WriteLine("JSON data has been successfully saved.");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error: Unauthorized Access - {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Error: Directory Not Found - {ex.Message}");
            }
            catch (PathTooLongException ex)
            {
                Console.WriteLine($"Error: Path Too Long - {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: IO Exception - {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }



    }
}