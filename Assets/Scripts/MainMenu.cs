using UnityEngine.SceneManagement;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    [SerializeField] string firstLevelName = "Level1";

    public void playGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(firstLevelName);
    }
}
