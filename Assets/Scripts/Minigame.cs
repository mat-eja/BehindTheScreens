using UnityEngine;
using UnityEngine.Events;


//Minigame base class
public abstract class Minigame: MonoBehaviour
{
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
        onCompleted?.Invoke();
        Close();
    }
protected abstract void ResetGame();
}
