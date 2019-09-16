using System;
using System.IO;

namespace Ctyar.Pingct
{
    internal class StorageManager
    {
        public string? Read(string fileName)
        {
            var filePath = GetFilePath(fileName);

            if (!File.Exists(filePath))
            {
                return default;
            }

            return File.ReadAllText(filePath);
        }

        public void Write(string fileName, string content)
        {
            File.WriteAllText(GetFilePath(fileName), content);
        }

        private string GetFilePath(string fileName)
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create), "pingct");

            Directory.CreateDirectory(directory);

            return Path.Combine(directory, fileName);
        }
    }
}