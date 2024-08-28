using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotesInventory : MonoBehaviour
{
    public GameObject noteInventoryCanvas; // Canvas del inventario de notas
    public List<GameObject> noteSlots; // Lista de botones en el inventario de notas
    public List<GameObject> noteCanvases; // Lista de canvases de las notas originales

    private bool isInventoryOpen = false;
    private GameObject currentOpenNote = null; // Referencia a la nota actualmente abierta desde el inventario

    void Start()
    {
        // Asegurarse de que el inventario esté desactivado al inicio
        if (noteInventoryCanvas != null)
        {
            noteInventoryCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("El Canvas del inventario no está asignado.");
        }

        // Desactivar la casilla de Interactable por defecto para todos los botones
        foreach (GameObject slot in noteSlots)
        {
            Button button = slot.GetComponent<Button>();
            if (button != null)
            {
                button.interactable = false;
            }
            else
            {
                Debug.LogError("El objeto " + slot.name + " no tiene un componente Button.");
            }
        }
    }

    void Update()
    {
        // Abrir/cerrar el inventario con la tecla TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }

        // Cerrar la nota abierta desde el inventario con las teclas E, Esc o Tab
        if (currentOpenNote != null)
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                CloseNoteFromInventory();
            }
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        if (noteInventoryCanvas != null)
        {
            noteInventoryCanvas.SetActive(isInventoryOpen);

            if (isInventoryOpen)
            {
                // Mostrar el cursor y desbloquearlo
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                // Ocultar el cursor y bloquearlo cuando se cierra el inventario
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                // Cerrar la nota si el inventario se cierra
                if (currentOpenNote != null)
                {
                    CloseNoteFromInventory();
                }
            }
        }
        else
        {
            Debug.LogError("El Canvas del inventario no está asignado.");
        }
    }

    public void UnlockNoteSlot(int noteIndex)
    {
        // Validar que el índice esté dentro del rango
        if (noteIndex >= 0 && noteIndex < noteSlots.Count)
        {
            Debug.Log("Desbloqueando nota con índice: " + noteIndex);

            // Desbloquear el botón correspondiente a la nota
            Transform lockTransform = noteSlots[noteIndex].transform.Find("Lock");
            Transform unlockTransform = noteSlots[noteIndex].transform.Find("Unlock");

            if (lockTransform != null && unlockTransform != null)
            {
                lockTransform.gameObject.SetActive(false);
                unlockTransform.gameObject.SetActive(true);

                // Activar la casilla de Interactable en el botón
                Button button = noteSlots[noteIndex].GetComponent<Button>();
                if (button != null)
                {
                    button.interactable = true;
                }
                else
                {
                    Debug.LogError("El objeto " + noteSlots[noteIndex].name + " no tiene un componente Button.");
                }
            }
            else
            {
                Debug.LogError("No se encontraron los objetos 'Lock' o 'Unlock' en el índice: " + noteIndex);
            }
        }
        else
        {
            Debug.LogWarning("Índice de nota fuera de rango: " + noteIndex);
        }
    }

    public void ShowNoteFromInventory(int noteIndex)
    {
        // Validar que el índice esté dentro del rango
        if (noteIndex >= 0 && noteIndex < noteCanvases.Count)
        {
            // Desactivar todos los canvases de notas para asegurarse de que solo uno esté activo a la vez
            foreach (GameObject canvas in noteCanvases)
            {
                canvas.SetActive(false);
            }

            // Activar el canvas de la nota correspondiente
            noteCanvases[noteIndex].SetActive(true);
            currentOpenNote = noteCanvases[noteIndex];
        }
        else
        {
            Debug.LogWarning("Índice de nota fuera de rango: " + noteIndex);
        }
    }

    void CloseNoteFromInventory()
    {
        if (currentOpenNote != null)
        {
            currentOpenNote.SetActive(false);
            currentOpenNote = null;
        }
    }
}