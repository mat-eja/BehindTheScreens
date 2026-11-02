using System.Collections;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text speakerText;
    [SerializeField] TMP_Text bodyText;
    [SerializeField] float charsPerSecond = 40f;

    DialogueAsset current;
    int index;
    Coroutine typing;
    bool isTyping;

    public bool IsOpen => panel.activeSelf;

    public void StartDialogue(DialogueAsset asset)
    {
        current = asset;
        index = 0;
        panel.SetActive(true);
        ShowLine();
    }

    public void Advance()
    {
        if (isTyping)
        {
            StopCoroutine(typing);
            bodyText.maxVisibleCharacters = int.MaxValue;
            isTyping = false;
            return;
        }

        index++;
        if (index >= current.lines.Length) EndDialogue();
        else ShowLine();
    }

    void ShowLine()
    {
        var line = current.lines[index];
        speakerText.text = line.speaker;
        bodyText.text = line.text;
        typing = StartCoroutine(Typewriter());
    }

    IEnumerator Typewriter()
    {
        isTyping = true;
        bodyText.maxVisibleCharacters = 0;
        bodyText.ForceMeshUpdate();
        int total = bodyText.textInfo.characterCount;

        for (int i = 0; i <= total; i++)
        {
            bodyText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(1/ charsPerSecond);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        panel.SetActive(false);
        current = null;
    }

    void Update()
    {
        if (!IsOpen) return;
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) Advance();
    }
}
 