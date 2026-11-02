using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance {get; private set;}
    [Header("Progress")] 
    [SerializeField] Slider progressBar;
    [SerializeField] float progress;
    
    [Header("Win")]
    [SerializeField] string nextSceneName;
    [SerializeField] private GameObject winPanel;
    
    public float DifficultyMultiplier { get; private set; } = 1;
    public bool IsWon { get; private set; }
    public bool IsEasyMode { get; private set; }

    void Awake()
    {
        Instance = this;
        if(winPanel) winPanel.SetActive(false);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (IsWon) return;
    }


    public void AddProgress(float amount)
    {
        progress = Mathf.Clamp01(progress + amount);
        progressBar.value = progress;

        if (progress >= 1f && !IsWon) WinLevel();
    }

    public void WinLevel()
    {
        IsWon = true;
        if (winPanel) winPanel.SetActive(true);
    }
    
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void SetEasyMode(bool easy)
    {
        IsEasyMode = easy;
        DifficultyMultiplier = easy ? 0.5f : 1; 
    }
}
