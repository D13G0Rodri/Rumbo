using System;
using UnityEngine;
using System.Collections.Generic;

// --- NUEVA ESTRUCTURA PARA GUARDAR VECTORES ---
[Serializable]
public struct SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 vec)
    {
        x = vec.x;
        y = vec.y;
        z = vec.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class PlayerData
{
    // Datos Comunes
    public float health;
    public float[] position;
    public float hunger;
    public float bathroom;
    public string currentSceneName;
    public float timerCount;

    // --- DATOS NUEVOS (CORREGIDOS) ---
    [Tooltip("Guarda las posiciones de los objetos empujables.")]
    public SerializableDictionary<string, SerializableVector3> pushableObjectPositions;

    [Tooltip("Indica si el jugador ha completado el logro de ordenar los objetos.")]
    public bool hasOrderingAchievement;

    [Tooltip("Indica si el panel de presentación ya se ha mostrado.")]
    public bool presentationPanelShown;

    // Datos del Niño
    public float karma;
    public float intelligence;
    public float happiness;
    public float concentration;
    public float energy;

    public PlayerData()
    {
        pushableObjectPositions = new SerializableDictionary<string, SerializableVector3>();
    }
}

// Clase wrapper para que JsonUtility pueda serializar diccionarios
[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<TKey> keys = new List<TKey>();

    [SerializeField]
    private List<TValue> values = new List<TValue>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();

        if (keys.Count != values.Count)
            throw new System.Exception($"there are {keys.Count} keys and {values.Count} values after deserialization. Make sure that both key and value types are serializable.");

        for (int i = 0; i < keys.Count; i++)
            this.Add(keys[i], values[i]);
    }
}