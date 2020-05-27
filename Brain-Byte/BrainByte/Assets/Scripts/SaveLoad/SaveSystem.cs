using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
    
public class SaveSystem
{
    public static void SavePlayer (MultiplayerSettings settings)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.bby";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(settings);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static PlayerData LoadPayer ()
    {
        string path = Application.persistentDataPath + "/player.bby";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;

            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteData ()
    {
        string path = Application.persistentDataPath + "/player.bby";

        if (File.Exists(path))
        {
            File.Delete(path);

            if (!File.Exists(path))
                Debug.Log("File was deleted");
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }
}
