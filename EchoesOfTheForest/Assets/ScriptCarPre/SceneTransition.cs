using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public Image fadeImage; // Asigna la imagen de fade desde el Inspector
    public float fadeDuration = 1f; // Duración del fade

    void Start()
    {
        // Comienza con el fade in (aparece la escena)
        StartCoroutine(FadeIn());
    }

    public void TransitionToScene(string sceneName)
    {
        // Llama al fade out antes de cambiar de escena
        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(0f); // Asegúrate de que esté completamente transparente al final
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            SetImageAlpha(alpha);
            yield return null;
        }
        SetImageAlpha(1f); // Asegúrate de que esté completamente opaca antes de cambiar de escena

        // Cargar la nueva escena
        SceneManager.LoadScene(sceneName);

        // Cuando la escena cargue, automáticamente el script en la nueva escena debería empezar con el FadeIn.
    }

    private void SetImageAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}