using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad máxima que queremos alcanzar
    public float maxSpeed = 5f;

    // Tiempo que tarda en alcanzar la velocidad máxima
    public float accelerationTime = 1f;

    // Tiempo que tarda en detenerse (llegar a velocidad 0)
    public float decelerationTime = 0.5f;

    // Variables para el control del movimiento
    private Vector3 velocity = Vector3.zero;
    private float currentSpeed = 0f;

    void Update()
    {
        // Obtener la dirección del movimiento desde las teclas WASD
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;

        // Calcular la velocidad deseada basada en la dirección y la velocidad máxima
        float targetSpeed = inputDirection.magnitude * maxSpeed;

        // Calcular la velocidad actual usando la función SmoothDamp para suavizar el cambio de velocidad
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref velocity.x,
                                        (inputDirection == Vector3.zero) ? decelerationTime : accelerationTime);

        // Mover el GameObject basado en la velocidad actual y el tiempo de deltaTime
        transform.Translate(inputDirection * currentSpeed * Time.deltaTime);
    }
}
