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
        // Calcular la nueva rotaci�n
        float rotationAmount = speedRotation * Time.deltaTime * rotationDirection;
        currentRotation += rotationAmount;

        // Cambiar la direcci�n si alcanzamos el �ngulo m�ximo
        if (Mathf.Abs(currentRotation) >= maxRotationAngle)
        {
            rotationDirection *= -1;
            currentRotation = Mathf.Sign(currentRotation) * maxRotationAngle;
        }

        // Aplicar la rotaci�n al objeto
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
