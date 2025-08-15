using UnityEngine;

public class PlayerControllerLight : PlayerControllerBase
{
    // Sobrescribe solo lo necesario para tu escena "light"
    protected override void Update()
    {
        base.Update(); // Mantiene el movimiento y salto de la clase base
        // Aquí puedes añadir lógica específica para esta escena
    }

    // Ejemplo: Sobrescribir el método de daño si necesitas algo diferente
    public override void ReceiveDamage(float damage)
    {
        base.ReceiveDamage(damage); // Usa la lógica base
        // Añade lógica adicional si es necesario
    }
}
