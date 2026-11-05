using TMPro;
using UnityEngine;

public class InfoPointTracker : MonoBehaviour
{
    public static InfoPointTracker Instance { get; private set; }
    
    [Tooltip("Optional. Shows info points count")]
    [SerializeField] TMP_Text counterText;
    
    int total;
    int completed;

    void Awake()
    {
        Instance = this;
    }
    
    
    void Start()
    {
        total = FindObjectsByType<InfoPoint>(FindObjectsSortMode.None).Length;
        UpdateText();
    }

    public void PointCompleted()
    {
        completed++;
        UpdateText();

        if (completed >= total && LevelManager.Instance != null)
            LevelManager.Instance.CompleteLevel();
    }
    
    void UpdateText()
    {
        if (counterText != null)
            counterText.text = "information found:" + completed + " / " + total;
    }
}
