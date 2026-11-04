using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WireTask : MonoBehaviour
{
    [SerializeField] Minigame minigame;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] float cooldown = 15f;

    [Tooltip("Fires once when task is complete e.g. to raise completion bar.")]
    public UnityEvent onTaskDone;

    bool playerInRange;
    bool done;
    private bool isMine;

    void Start()
    {
        minigame.onCompleted.AddListener(onMinigameCompleted);
        minigame.onClosed.AddListener(onMinigameClosed);
    }

    void OnDestroy()
    {
        if (minigame != null)
        {
            minigame.onCompleted.RemoveListener(onMinigameCompleted);
            minigame.onClosed.RemoveListener(onMinigameClosed);
        }
    }

    void Update()
    {
        if (!done && playerInRange && Input.GetKeyDown(interactKey))
        {
            isMine = true;
            minigame.Open();
        }
    }

    void onMinigameCompleted()
    {
        if (!isMine) return; 

        done = true;
        onTaskDone?.Invoke();
        StartCoroutine(ResetAfterDelay());
    }

    void onMinigameClosed()
    {
        isMine = false;
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(cooldown);
        done = false;
    }
    
    void OnTriggerEnter2D(Collider2D other) {if(other.CompareTag("Player")) playerInRange = true;}
    void OnTriggerExit2D(Collider2D other) {if(other.CompareTag("Player"))playerInRange = false;}
} 

