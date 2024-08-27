using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinTrigger : MonoBehaviour
{
    public GameObject gameWinPanel; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            gameWinPanel.SetActive(true); 
            Time.timeScale = 0f; 
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("Menu"); 
    }
}