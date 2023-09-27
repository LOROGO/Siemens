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











    }
}