using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Para manejar el botón
using TMPro; // Si estás usando TextMeshPro para el botón
using System.Collections; 

public class EndSceneManager : MonoBehaviour
{
    public AudioSource audioSource; // El AudioSource que reproduce el audio
    public Button nextSceneButton; // El botón que se muestra al final del audio
    public string nextSceneName; // El nombre de la siguiente escena

    void Start()
    {
        // Asegúrate de que el botón esté inicialmente desactivado
        nextSceneButton.gameObject.SetActive(false);

        // Comienza a escuchar si el audio ha terminado
        StartCoroutine(CheckIfAudioFinished());
    }

    IEnumerator CheckIfAudioFinished()
    {
        // Esperar hasta que el audio termine
        yield return new WaitUntil(() => !audioSource.isPlaying);

        // Activar el botón cuando el audio termine
        nextSceneButton.gameObject.SetActive(true);

        // Agregar un listener al botón para cargar la siguiente escena cuando se haga clic
        nextSceneButton.onClick.AddListener(LoadNextScene);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}