using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    public GameObject popup;
    public string animationName = "WarningText"; // El nombre de tu animación
    private Animation popupAnimation;

    void Start()
    {
        // Ocultamos el popup al inicio
        popup.SetActive(false);

        // Obtenemos el componente Animation del popup
        popupAnimation = popup.GetComponent<Animation>();

        // Programamos el popup para que aparezca después de 10 segundos
        Invoke("ShowPopup", 8f);
    }

    void ShowPopup()
    {
        // Activamos el popup y reproducimos la animación de aparición
        popup.SetActive(true);
        popupAnimation.Play(animationName);

        // Programamos que se oculte después de 7 segundos
        Invoke("HidePopup", 7f);
    }

        void HidePopup()
    {
        // Reproducimos la misma animación pero en reversa
        StartCoroutine(PlayAnimationReverse(animationName));
    }

        IEnumerator PlayAnimationReverse(string animName)
    {
        // Obtenemos la animación
        AnimationState animState = popupAnimation[animName];

        // Empezamos desde el final de la animación
        animState.time = animState.length;
        animState.speed = -1;  // Velocidad negativa para reproducir en reversa

        // Reproducimos la animación
        popupAnimation.Play(animName);

        // Esperamos a que la animación en reversa termine
        yield return new WaitForSeconds(animState.length);

        // Desactivamos el popup
        popup.SetActive(false);

    }
}

