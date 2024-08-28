using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSlot : MonoBehaviour
{
    public string noteID;
    public NotesInventory notesInventory;
    public GameObject noteCanvas;             // Canvas para mostrar la nota
    public AudioSource noteSound;             // Sonido al abrir/cerrar la nota

    private bool isNoteOpen = false;

    public void OnNoteSlotClicked()
    {
        if (isNoteOpen)
        {
            CloseNote();
        }
        else
        {
            OpenNote();
        }
    }

    void OpenNote()
    {
        isNoteOpen = true;
        noteCanvas.SetActive(true);
        noteSound.Play();

        // Aquí puedes mostrar el contenido de la nota en el canvas
    }

    void CloseNote()
    {
        isNoteOpen = false;
        noteCanvas.SetActive(false);
        noteSound.Play();
    }
}