using System;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public float health;
    public float[] position; // x, y, z
    public float intelligence;
    public float concentration;
    public float hunger;
    public float bathroom;
    public string currentSceneName;
    public float timerCount; // Para el bebé
    public float energy; // Para el niño
    public float happiness; // Para el adolescente

    public PlayerData()
    {
        position = new float[3];
    }
}
