using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // Guarda los datos del jugador en un archivo JSON
    public static void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data, true); // "true" para formato legible
        string path = Application.persistentDataPath + "/playerData.json";
        File.WriteAllText(path, jsonData);
        Debug.Log("Datos guardados en: " + path);
    }

    // Carga los datos del jugador desde el archivo JSON
    public static PlayerData LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("Datos cargados desde: " + path);
            return data;
        }
        Debug.LogWarning("No se encontró archivo de guardado.");
        return null;
    }
}