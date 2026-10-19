using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class WireTask : MonoBehaviour
{
    [SerializeField] Minigame minigame;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] private float cooldown = 15f;

    [Tooltip("Fires once when task is complete e.g. to raise completion bar.")]
    public UnityEvent onTaskDone;

    bool playerInRange;
    bool done;

    void Start()
    {
        minigame.onCompleted.AddListener(onCompleted);
        
    }

    void onDestroy()
    {
        if (minigame != null)
        {
            minigame.onCompleted.RemoveListener(onCompleted);
        }
    }

    void Update()
    {
        if (!done && playerInRange && Input.GetKeyDown(interactKey))
        {
            minigame.Open();
        }
    }

    void onCompleted()
    {
        done = true;
        onTaskDone?.Invoke();
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(cooldown);
        done = false;
    }
    
    void OnTriggerEnter2D(Collider2D other) {if(other.CompareTag("Player")) playerInRange = true;}
    void OnTriggerExit2D(Collider2D other) {if(other.CompareTag("Player"))playerInRange = false;}
    
} 

