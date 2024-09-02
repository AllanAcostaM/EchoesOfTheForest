using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RotationHands : MonoBehaviour
{
    public float speedRotation = 1.0f;
    public float maxRotationAngle = 20.0f;

    private float currentRotation = 0f;
    private int rotationDirection = 1;

    void Update()
    {
        // Calcular la nueva rotación
        float rotationAmount = speedRotation * Time.deltaTime * rotationDirection;
        currentRotation += rotationAmount;

        // Cambiar la dirección si alcanzamos el ángulo máximo
        if (Mathf.Abs(currentRotation) >= maxRotationAngle)
        {
            rotationDirection *= -1;
            currentRotation = Mathf.Sign(currentRotation) * maxRotationAngle;
        }

        // Aplicar la rotación al objeto
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
