using UnityEngine;
using UnityEngine.Events;


//Minigame base class
public abstract class Minigame: MonoBehaviour
{
    
    [Header("Progress")]
    [Tooltip("How much of the level progress bar this minigame fills when completed (0 to 1).")]
    [SerializeField] float progressReward = 0.25f;
    
    
    public UnityEvent onCompleted;
    public UnityEvent onClosed;

    public virtual void Open()
    {
        gameObject.SetActive(true);
        ResetGame();
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
        onClosed?.Invoke();
    }

    protected void Complete()
    {
        if(LevelManager.Instance != null) 
            LevelManager.Instance.AddProgress(progressReward);
        
        onCompleted?.Invoke();
        Close();
    }
protected abstract void ResetGame();
}
