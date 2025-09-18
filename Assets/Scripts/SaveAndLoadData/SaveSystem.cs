using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string GetSavePath()
    {
        return Application.persistentDataPath + "/playerData.json";
    }

    public static bool HasSave()
    {
        return File.Exists(GetSavePath());
    }

    public static void DeleteSave()
    {
        string path = GetSavePath();
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Archivo de guardado eliminado: " + path);
            }
            else
            {
                Debug.Log("No había archivo de guardado para eliminar.");
            }
        }
        catch (IOException ex)
        {
            Debug.LogError("Error eliminando guardado: " + ex.Message);
        }
    }

    // Guarda los datos del jugador en un archivo JSON
    public static void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data, true); // "true" para formato legible
        string path = GetSavePath();
        File.WriteAllText(path, jsonData);
        Debug.Log("Datos guardados en: " + path);
        Debug.Log($"Resumen guardado -> Vida={data.health}, Tiempo={data.timerCount}, EscenaDestino={data.currentSceneName}, Pos=({data.position[0]}, {data.position[1]}, {data.position[2]})");
    }

    // Carga los datos del jugador desde el archivo JSON
    public static PlayerData LoadPlayerData()
    {
        string path = GetSavePath();
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("Datos cargados desde: " + path);
            if (data != null && data.position != null && data.position.Length >= 3)
            {
                Debug.Log($"Resumen cargado -> Vida={data.health}, Tiempo={data.timerCount}, EscenaDestino={data.currentSceneName}, Pos=({data.position[0]}, {data.position[1]}, {data.position[2]})");
            }
            else
            {
                Debug.LogWarning("El archivo de guardado existe pero los datos son inválidos o incompletos.");
            }
            return data;
        }
        Debug.LogWarning("No se encontró archivo de guardado.");
        return null;
    }
}