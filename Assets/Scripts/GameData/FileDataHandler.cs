using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath = string.Empty;
    private string dataFileName = string.Empty;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        GameData gameData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;

                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                gameData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.Log($"Error occured when loading the file!! {fullPath} \n {e}");
            }
        }

        return gameData;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        try
        {
            if (!File.Exists(fullPath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            }

            string dataToStore = JsonUtility.ToJson(gameData);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log($"Error occured when saving the file!! {fullPath} \n {e}");
        }
    }
}
