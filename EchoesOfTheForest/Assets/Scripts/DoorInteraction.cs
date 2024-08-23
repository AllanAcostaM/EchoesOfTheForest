using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Asegúrate de que esta línea esté presente para usar TextMeshPro

public class DoorInteraction : MonoBehaviour
{
    public Animator door;                // El Animator de la puerta
    public TextMeshProUGUI openText;     // El texto que indica que se puede abrir la puerta
    public AudioSource doorSound;        // El sonido de la puerta

    public float interactionDistance = 3f;  // Distancia necesaria para interactuar con la puerta

    private Transform player;            // Referencia al jugador
    private bool isDoorOpen = false;     // Estado de la puerta

    void Start()
    {
        // Buscar al jugador por la etiqueta "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Asegurarse de que el texto de interacción esté desactivado al inicio
        openText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Calcular la distancia entre el jugador y la puerta
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Si el jugador está dentro del rango de interacción
        if (distanceToPlayer <= interactionDistance)
        {
            // Activar el texto de interacción
            openText.gameObject.SetActive(true);

            // Verificar si el jugador presiona la tecla "E" para interactuar
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleDoor();
            }
        }
        else
        {
            // Si el jugador está fuera del rango, desactivar el texto
            openText.gameObject.SetActive(false);
        }
    }

    // Método para alternar entre abrir y cerrar la puerta
    void ToggleDoor()
    {
        if (!isDoorOpen)
        {
            DoorOpens();
        }
        else
        {
            DoorCloses();
        }

        // Invertir el estado de la puerta
        isDoorOpen = !isDoorOpen;
    }

    void DoorOpens()
    {
        Debug.Log("Door Opens");
        door.SetBool("Open", true);
        door.SetBool("Closed", false);
        doorSound.Play();  // Reproducir sonido al abrir
    }

    void DoorCloses()
    {
        Debug.Log("Door Closes");
        door.SetBool("Open", false);
        door.SetBool("Closed", true);
        doorSound.Play();  // Reproducir sonido al cerrar
    }
}