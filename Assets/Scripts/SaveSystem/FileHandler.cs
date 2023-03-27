using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public class FileHandler
    {
        public string filePath;
        public string fileName;

        public FileHandler(string path, string name)
        {
            filePath = path;
            fileName = name;
        }
        
        public SaveData Load()
        {
            string path = Path.Combine(filePath, fileName);
            SaveData loadData = null;
            if (File.Exists(path))
            {
                try
                {
                    string dataLoad = "";
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataLoad = reader.ReadToEnd();
                        }
                    }

                    loadData = JsonUtility.FromJson<SaveData>(dataLoad);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return loadData;
        }

        public void Save(SaveData data)
        {
            string path = Path.Combine(filePath, fileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
                string serializeData = JsonUtility.ToJson(data, true);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(serializeData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeleteFile()
        {
            string path = Path.Combine(filePath, fileName);
            if(File.Exists(path))
                File.Delete(path);
        }
        
        public bool CheckForNewGame()
        {
            string path = Path.Combine(filePath, fileName);
            if (!File.Exists(path))
                return true;
            return false;
        }
    }
}