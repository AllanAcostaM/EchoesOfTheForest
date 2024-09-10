using System.Collections;
using UnityEngine;
using TMPro;

public class SubtitleManager : MonoBehaviour
{
    public AudioSource audioSource; // El AudioSource que contiene el audio
    public TextMeshProUGUI subtitleText; // El componente TextMeshPro para los subtítulos
    public Subtitle[] subtitles; // Array de subtítulos

    private int currentSubtitleIndex = 0;

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            StartCoroutine(ShowSubtitles());
        }
    }

    IEnumerator ShowSubtitles()
    {
        while (currentSubtitleIndex < subtitles.Length)
        {
            Subtitle subtitle = subtitles[currentSubtitleIndex];

            // Espera hasta que el tiempo de la pista de audio sea igual o mayor al tiempo de inicio del subtítulo
            yield return new WaitUntil(() => audioSource.time >= subtitle.startTime);

            // Mostrar subtítulo
            subtitleText.text = subtitle.text;

            // Esperar la duración del subtítulo
            yield return new WaitForSeconds(subtitle.duration);

            // Borrar subtítulo
            subtitleText.text = "";

            // Pasar al siguiente subtítulo
            currentSubtitleIndex++;
        }
    }
}

[System.Serializable]
public class Subtitle
{
    public float startTime; // Tiempo en segundos donde el subtítulo debe aparecer
    public float duration;  // Duración del subtítulo en pantalla
    public string text;     // Texto del subtítulo
}