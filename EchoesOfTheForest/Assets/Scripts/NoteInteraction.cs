using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteInteraction : MonoBehaviour
{
    public TextMeshProUGUI interactionText;   // Texto del canvas del jugador
    public GameObject noteCanvas;             // Canvas que muestra la nota
    public AudioSource noteSound;             // Sonido al abrir/cerrar la nota
    public string noteContent = "Texto de la nota"; // El contenido de la nota
    public float interactionDistance = 3f;    // Distancia de interacción

    private Transform player;                 // Referencia al jugador
    private bool isNoteOpen = false;          // Estado de la nota
    private bool isPlayerInRange = false;     // Controla si el jugador está en rango

    void Start()
    {
        // Buscar al jugador por la etiqueta "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Asegurarse de que el texto de interacción esté desactivado al inicio
        interactionText.gameObject.SetActive(false);

        // Asegurarse de que el canvas de la nota esté desactivado al inicio
        noteCanvas.SetActive(false);
    }

    void Update()
    {
        // Calcular la distancia entre el jugador y la nota
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Verificar si el jugador está en rango
        if (distanceToPlayer <= interactionDistance)
        {
            if (!isPlayerInRange)
            {
                OnPlayerEnterRange();
            }

            // Verificar si el jugador presiona la tecla "E" para abrir/cerrar la nota
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isNoteOpen)
                {
                    OpenNote();
                }
                else
                {
                    CloseNote();
                }
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                OnPlayerExitRange();
            }
        }
    }

    private void OnPlayerEnterRange()
    {
        isPlayerInRange = true;
        interactionText.text = "Pulsa E para leer la nota";
        interactionText.gameObject.SetActive(true);
    }

    private void OnPlayerExitRange()
    {
        isPlayerInRange = false;
        interactionText.gameObject.SetActive(false);

        // Si el jugador sale del rango y la nota está abierta, cerrarla automáticamente
        if (isNoteOpen)
        {
            CloseNote();
        }
    }

    void OpenNote()
    {
        isNoteOpen = true;

        // Activar el canvas de la nota
        noteCanvas.SetActive(true);
        Debug.Log("El canvas de la nota ha sido activado.");

        // Actualizar el texto de la nota
        TextMeshProUGUI noteText = noteCanvas.GetComponentInChildren<TextMeshProUGUI>();
        if (noteText != null)
        {
            noteText.text = noteContent;
        }
        else
        {
            Debug.LogError("No se encontró un componente TextMeshProUGUI en los hijos de noteCanvas.");
        }

        // Reproducir el sonido al abrir la nota
        if (noteSound != null)
        {
            noteSound.Play();
        }

        // Desactivar el texto de interacción cuando la nota esté abierta
        interactionText.gameObject.SetActive(false);
    }

    void CloseNote()
    {
        isNoteOpen = false;

        // Ocultar el canvas de la nota
        noteCanvas.SetActive(false);
        Debug.Log("El canvas de la nota ha sido desactivado.");

        // Reproducir el sonido al cerrar la nota
        if (noteSound != null)
        {
            noteSound.Play();
        }

        // Mostrar nuevamente el texto de interacción si el jugador sigue en rango
        if (isPlayerInRange)
        {
            interactionText.gameObject.SetActive(true);
        }
    }
}