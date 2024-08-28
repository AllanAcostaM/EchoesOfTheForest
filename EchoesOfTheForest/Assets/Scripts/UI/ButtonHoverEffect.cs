using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText; // Si usas TextMeshPro, cámbialo a TMP_Text
    public Image underlineImage; // La línea que quieres mostrar
    public Color hoverTextColor = Color.yellow; // El color que quieres cuando el mouse está sobre el botón
    private Color originalTextColor;

    void Start()
    {
        // Guardar el color original del texto
        originalTextColor = buttonText.color;
        // Asegúrate de que la línea esté oculta inicialmente
        underlineImage.gameObject.SetActive(false);
    }

    void OnEnable()
    {
        // Restaurar el color original del texto
        buttonText.color = Color.white;
        // Ocultar la línea
        underlineImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cambiar el color del texto
        buttonText.color = hoverTextColor;
        // Mostrar la línea
        underlineImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar el color original del texto
        buttonText.color = originalTextColor;
        // Ocultar la línea
        underlineImage.gameObject.SetActive(false);
    }
}
