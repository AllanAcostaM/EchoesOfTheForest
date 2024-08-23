using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Asegúrate de que esta línea esté presente para usar TextMeshPro
using NavKeypad; // Importar el namespace de Keypad

public class KeyPadDoor : MonoBehaviour
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
            // Activar el texto de interacción solo si la puerta no ha sido abierta aún
            if (!isDoorOpen)
            {
                openText.gameObject.SetActive(true);
            }
        }
        else
        {
            // Si el jugador está fuera del rango, desactivar el texto
            openText.gameObject.SetActive(false);
        }
    }

    // Método para abrir la puerta con el Keypad
    public void OpenDoorWithKeypad()
    {
        if (!isDoorOpen)
        {
            DoorOpens();
            isDoorOpen = true;  // Evitar que se abra repetidamente
        }
    }

    void DoorOpens()
    {
        Debug.Log("Door Opens");
        door.SetBool("Open", true);
        doorSound.Play();  // Reproducir sonido al abrir
    }

    void DoorCloses()
    {
        Debug.Log("Door Closes");
        door.SetBool("Open", false);
        doorSound.Play();  // Reproducir sonido al cerrar
    }
}