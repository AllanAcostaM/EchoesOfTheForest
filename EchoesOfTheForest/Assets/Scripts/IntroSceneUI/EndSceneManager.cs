using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndSceneManager : MonoBehaviour
{
    public AudioSource audioSource; // El AudioSource que reproduce el audio
    public Button nextSceneButton; // El botón que se muestra al final del audio
    public string nextSceneName; // El nombre de la siguiente escena
    public Image fadeImage; // Imagen para el efecto de fade
    public float fadeDuration = 1f; // Duración del fade
    private Image buttonImage; // Imagen del botón
    private TextMeshProUGUI buttonText; // Texto del botón si usas TextMeshPro

    void Start()
    {
        // Asegúrate de que el botón esté inicialmente desactivado
        nextSceneButton.gameObject.SetActive(false);

        // Obtener los componentes del botón
        buttonImage = nextSceneButton.GetComponent<Image>();
        buttonText = nextSceneButton.GetComponentInChildren<TextMeshProUGUI>();

        // Comienza a escuchar si el audio ha terminado
        StartCoroutine(CheckIfAudioFinished());

        // Iniciar la escena con el fade in
        StartCoroutine(FadeIn());
    }

    IEnumerator CheckIfAudioFinished()
    {
        // Esperar hasta que el audio termine
        yield return new WaitUntil(() => !audioSource.isPlaying);

        // Activar el botón cuando el audio termine
        nextSceneButton.gameObject.SetActive(true);

        // Agregar un listener al botón para hacer fade out y cambiar de escena
        nextSceneButton.onClick.AddListener(() => StartCoroutine(FadeOutButtonAndScene()));
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetImageAlpha(fadeImage, alpha);
            yield return null;
        }
        SetImageAlpha(fadeImage, 0f); // Asegurarse de que el fade esté completamente transparente
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            SetImageAlpha(fadeImage, alpha);
            yield return null;
        }
        SetImageAlpha(fadeImage, 1f); // Asegúrate de que el fade esté completamente opaco

        // Cargar la nueva escena
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator FadeOutButtonAndScene()
    {
        // Fade out del botón
        float timer = 0f;
        while (timer <= fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            // Actualizar el alpha del botón y del texto
            SetImageAlpha(buttonImage, alpha);
            SetTextAlpha(buttonText, alpha);

            yield return null;
        }

        // Asegúrate de que el botón esté completamente invisible
        SetImageAlpha(buttonImage, 0f);
        SetTextAlpha(buttonText, 0f);

        // Después de hacer fade al botón, hacer fade de la escena
        StartCoroutine(FadeOutAndLoadScene(nextSceneName));
    }

    // Método para ajustar el alpha de una imagen
    private void SetImageAlpha(Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    // Método para ajustar el alpha del texto
    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}