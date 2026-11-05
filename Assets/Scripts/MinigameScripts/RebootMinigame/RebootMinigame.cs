using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebootMinigame : Minigame
{
    [Header("UI")]
    [SerializeField] Button[] switches;
    [SerializeField] TMP_Text instructionText;
    
    [Header("Colours")]
    [SerializeField] Color offColor = Color.gray;
    [SerializeField] Color onColor = Color.green;
    [SerializeField] Color errorColor = Color.red;
    
    int nextIndex;
    bool locked;
    char[] labels;
    int[] order;


    void Awake()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            int index = i;
            switches[i].onClick.AddListener (() => OnSwitchPressed(index));
        }
    }

    void onEnable()
    {
        ResetGame();
    }

    protected override void ResetGame()
    {
        StopAllCoroutines();
        locked = false;
        nextIndex = 0;

        int n = switches.Length;
        labels = new char[n];
        for(int i = 0; i < n; i++) labels[i] = (char) ('A' + i);

        order = Shuffled(n);

        string text = "";
        for (int i = 0; i < n; i++)
        {
            switches[i].GetComponentInChildren<TMP_Text>().text =  labels[i].ToString();
            SetColor(i, offColor);
            text +=  labels[order[i]] + (i< n - 1? ">" : "");
        }

        if (instructionText != null) instructionText.text = text;
    }

    void OnSwitchPressed(int index)
    {
        if (locked) return;

        if (index == order[nextIndex])
        {
            SetColor(index, onColor);
            nextIndex++;
            if (nextIndex >= switches.Length)
                StartCoroutine(WinRoutine());
        }
        else
        {
            StartCoroutine(FailRoutine());
        }
    }

    IEnumerator FailRoutine()
    {
        locked = true;
        for(int i = 0; i < switches.Length; i++) SetColor(i, errorColor);
        yield return new WaitForSecondsRealtime(0.5f);
        
        for (int i=0; i < switches.Length; i++) SetColor(i, offColor);
        nextIndex = 0;
        locked = false;
    }

    IEnumerator WinRoutine()
    {
        locked = true;
        yield return new WaitForSecondsRealtime(0.5f);
        Complete();
    }

    void SetColor(int i, Color c)
    {
        ((Image)switches[i].targetGraphic).color = c;
    }

    static int[] Shuffled(int n)
    {
        int[] a = new int[n];
        for (int i = 0; i < n; i++) a[i] = i;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (a[i], a[j]) = (a[j], a[i]); 
        }

        return a;
    }
}
