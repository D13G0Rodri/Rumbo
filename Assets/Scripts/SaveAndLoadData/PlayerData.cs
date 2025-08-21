using System;

[Serializable]
public class PlayerData
{
    // Datos Comunes
    public float health;
    public float[] position;
    public float intelligence;
    public float concentration;
    public float hunger;
    public float bathroom;
    public string currentSceneName;
    public float timerCount;

    // --- CAMBIO: Añadir campos para cada etapa ---
    // Si una etapa no usa un campo, simplemente se quedará con su valor por defecto (0).
    
    // Datos del Niño
    public float energy;

    // Datos del Adolescente
    public float happiness;
}