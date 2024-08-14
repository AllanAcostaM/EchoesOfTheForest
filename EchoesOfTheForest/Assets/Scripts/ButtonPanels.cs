using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonPanels : MonoBehaviour
{

    public GameObject panel; // El panel que quieres desactivar

    public void DeactivatePanel()
    {
        // Encuentra todos los botones dentro del panel
        Button[] buttons = panel.GetComponentsInChildren<Button>();

        // Para cada botón, simula el evento de salida
        foreach (Button button in buttons)
        {
            // Obtén el script que controla el hover del botón
            ButtonHoverEffect buttonHover = button.GetComponent<ButtonHoverEffect>();

            if (buttonHover != null)
            {
                // Simula el OnPointerExit manualmente
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                buttonHover.OnPointerExit(pointerData);
            }
        }

        // Ahora desactiva el panel
        panel.SetActive(false);
    }

    public void ActivatePanel()
    {
        // Activa el panel cuando lo necesites
        panel.SetActive(true);
    }



}
