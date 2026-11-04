using UnityEngine;

public class InfoPoint : MonoBehaviour
{
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] DialogueAsset dialogue;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    
    [Tooltip("Optional. Shown while the player is in range")]
    [SerializeField] GameObject promptIcon;
    [Tooltip("Optional. Shown when this has been read")]
    [SerializeField] GameObject doneMarker;
    
    bool playerInRange;
    bool completed;
    
    void Start()
    {
        if(promptIcon) promptIcon.SetActive(false);
        if(doneMarker) doneMarker.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange || dialogueManager.IsOpen) return;
        
        if(Input.GetKeyDown(interactKey))
            dialogueManager.StartDialogue(dialogue, OnDialogueFinished);
    }

    void OnDialogueFinished()
    {
        if (completed) return;
        completed = true;
        
        if (doneMarker) doneMarker.SetActive(true);
        if (InfoPointTracker.Instance != null)
            InfoPointTracker.Instance.PointCompleted();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;
        playerInRange = true;
        if(promptIcon) promptIcon.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if(promptIcon) promptIcon.SetActive(false);
    }
    
}
