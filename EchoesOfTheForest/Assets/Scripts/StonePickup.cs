using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StonePickup : MonoBehaviour
{
    public float pickupRange = 2f;

    void Update()
    {
        // Detectar si el jugador está cerca y presiona la tecla F
        if (Vector3.Distance(transform.position, PlayerController.Instance.transform.position) <= pickupRange && Input.GetKeyDown(KeyCode.F))
        {
            Pickup();
        }
    }

    void Pickup()
    {
        // Agregar piedra al inventario
        StoneInventory inventory = PlayerController.Instance.GetComponent<StoneInventory>();
        inventory.AddStone();

        // Destruir la piedra del suelo
        Destroy(gameObject);
    }
}