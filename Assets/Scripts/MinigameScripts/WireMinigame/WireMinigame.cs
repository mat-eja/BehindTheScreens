using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireMinigame : Minigame
{
    [Header("Connectors (same count on both sides)")]
    [SerializeField] WireConnector[] bottomConnectors;
    [SerializeField] WireConnector[] topConnectors;
 
    [Header("Wires")]
    [Tooltip("Empty RectTransform that sits ABOVE the connectors in the hierarchy order.")]
    [SerializeField] RectTransform wireContainer;
    [Tooltip("A plain UI Image used as the wire. Can be a prefab.")]
    [SerializeField] Image wirePrefab;
    [SerializeField] float wireThickness = 16f;
 
    [Header("Colors (need at least one per connector pair)")]
    [SerializeField] Color[] colors =
    {
        Color.red,
        Color.blue,
        Color.yellow,
        new Color(1f, 0.4f, 0.8f)
    };

    readonly List<GameObject> placedWires = new List<GameObject>();
    readonly List<RaycastResult> raycastResults = new List<RaycastResult>();

    WireConnector dragStart;
    RectTransform dragWire;
    int connectedCount;
    bool finished;
 
    void OnEnable() 
	{
		ResetGame();
	

	}
    protected override void ResetGame()
    {
        StopAllCoroutines();
        ClearWires();
 
        finished = false;
        connectedCount = 0;
        dragStart = null;
        dragWire = null;
 
        int n = bottomConnectors.Length;
        if (topConnectors.Length != n || colors.Length < n)
        {
            Debug.LogError("WiresMinigame: bottom/top connector counts must match and you need at least that many colors.", this);
            return;
        }
 
        int[] bottomOrder = Shuffled(n);
        int[] topOrder = Shuffled(n);
 
        for (int i = 0; i < n; i++)
        {
            bottomConnectors[i].Setup(this, bottomOrder[i], colors[bottomOrder[i]]);
            topConnectors[i].Setup(this, topOrder[i], colors[topOrder[i]]);
        }
    }
 
    //Called by WireConnector
 
    public void BeginWire(WireConnector start, PointerEventData e)
    {
        if (finished || start.connected) return;
 
        dragStart = start;
        dragWire = CreateWire(start.colorId);
        PlaceWire(dragWire, LocalPos(start.transform), LocalPoint(e));
    }
 
    public void DragWire(PointerEventData e)
    {
        if (dragWire == null) return;
        PlaceWire(dragWire, LocalPos(dragStart.transform), LocalPoint(e));
    }
 
    public void EndWire(PointerEventData e)
    {
        if (dragWire == null) return;
 
        WireConnector target = FindConnectorUnder(e);
        bool match = target != null
                     && !target.isBottomSide
                     && !target.connected
                     && target.colorId == dragStart.colorId;
 
        if (match)
        {
            // Snap the wire to the center of the target and lock both ends.
            PlaceWire(dragWire, LocalPos(dragStart.transform), LocalPos(target.transform));
            dragStart.connected = true;
            target.connected = true;
            connectedCount++;
 
            if (connectedCount >= bottomConnectors.Length)
                StartCoroutine(FinishRoutine());
        }
        else
        {
            placedWires.Remove(dragWire.gameObject);
            Destroy(dragWire.gameObject);
        }
 
        dragWire = null;
        dragStart = null;
    }
 
    //Helpers 
 
    IEnumerator FinishRoutine()
    {
        finished = true;
        yield return new WaitForSecondsRealtime(0.5f); // let the player see the last wire
        Complete();
    }
 
    RectTransform CreateWire(int colorId)
    {
        Image img = Instantiate(wirePrefab, wireContainer);
        img.color = colors[colorId];
        img.raycastTarget = false; // so it never blocks the drop raycast
 
        RectTransform rt = img.rectTransform;
        rt.anchorMin = rt.anchorMax = wireContainer.pivot; // anchoredPosition == local pos from pivot
        rt.pivot = new Vector2(0f, 0.5f);                  // rotate/stretch from the start point
        rt.localScale = Vector3.one;
 
        placedWires.Add(img.gameObject);
        return rt;
    }
 
    void PlaceWire(RectTransform wire, Vector2 from, Vector2 to)
    {
        Vector2 d = to - from;
        wire.anchoredPosition = from;
        wire.sizeDelta = new Vector2(d.magnitude, wireThickness);
        wire.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(d.y, d.x) * Mathf.Rad2Deg);
    }
 
    Vector2 LocalPos(Transform t)
    {
        return wireContainer.InverseTransformPoint(t.position);
    }
 
    Vector2 LocalPoint(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            wireContainer, e.position, e.pressEventCamera, out Vector2 p);
        return p;
    }
 
    WireConnector FindConnectorUnder(PointerEventData e)
    {
        raycastResults.Clear();
        EventSystem.current.RaycastAll(e, raycastResults);
 
        foreach (RaycastResult r in raycastResults)
        {
            WireConnector c = r.gameObject.GetComponentInParent<WireConnector>();
            if (c != null) return c;
        }
        return null;
    }
 
    void ClearWires()
    {
        foreach (GameObject w in placedWires)
            if (w != null) Destroy(w);
        placedWires.Clear();
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
