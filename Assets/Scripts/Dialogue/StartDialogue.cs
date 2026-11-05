using UnityEngine;

public class Level1StartDialogue : MonoBehaviour
{
  [SerializeField] DialogueManager dialogueManager;
  [SerializeField] DialogueAsset dialogueAsset;

  void Start()
  {
    dialogueManager.StartDialogue(dialogueAsset);
  }
}
