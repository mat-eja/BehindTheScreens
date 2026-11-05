using UnityEngine;

[CreateAssetMenu( menuName = "Dialogue/Dialogue Asset")]
public class DialogueAsset : ScriptableObject
{
    [System.Serializable]
    public class Line
    {
        public string speaker;
        [TextArea(2, 5)] public string text;
    }
    
    public Line[] lines;
}
