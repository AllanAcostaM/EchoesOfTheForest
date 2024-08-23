using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneThrow : MonoBehaviour
{
    public Transform handPosition;  // Lugar donde se mostrará la piedra
    public GameObject stonePrefab;  // Prefab de la piedra
    public float throwForce = 20f;
    public Transform aimIndicator;  // Indicador visual para el apuntado

    private bool isAiming = false;

    void Update()
    {
        StoneInventory inventory = GetComponent<StoneInventory>();

        // Apuntar con clic derecho si la piedra está equipada
        if (inventory.isEquipped && Input.GetMouseButton(1))
        {
            StartAiming();
        }

        // Soltar la piedra con clic izquierdo
        if (isAiming && Input.GetMouseButtonDown(0))
        {
            ThrowStone();
        }

        // Detener apuntado al soltar clic derecho
        if (Input.GetMouseButtonUp(1))
        {
            StopAiming();
        }
    }

    void StartAiming()
    {
        isAiming = true;

        // Activar indicador de apuntado
        aimIndicator.gameObject.SetActive(true);

        // Calcular la trayectoria de lanzamiento
        UpdateAimIndicator();
    }

    void StopAiming()
    {
        isAiming = false;

        // Desactivar el indicador de apuntado
        aimIndicator.gameObject.SetActive(false);
    }

    void ThrowStone()
    {
        StoneInventory inventory = GetComponent<StoneInventory>();

        if (inventory.currentStoneCount > 0)
        {
            // Instanciar y lanzar la piedra
            GameObject stone = Instantiate(stonePrefab, handPosition.position, Quaternion.identity);
            Rigidbody rb = stone.GetComponent<Rigidbody>();
            rb.AddForce(CalculateThrowDirection() * throwForce, ForceMode.VelocityChange);

            // Restar una piedra del inventario
            inventory.RemoveStone();

            // Resetear estado
            StopAiming();
        }
        else
        {
            Debug.Log("No stones left!");
        }
    }

    Vector3 CalculateThrowDirection()
    {
        // Calcular la dirección del lanzamiento
        Camera cam = Camera.main;
        return cam.transform.forward + cam.transform.up * 0.3f;  // Ajuste para generar el arco
    }

    void UpdateAimIndicator()
    {
        // Actualizar la posición del indicador de apuntado
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            aimIndicator.position = hit.point;
        }
    }
}