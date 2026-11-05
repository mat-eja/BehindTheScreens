using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class WireConnector : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public bool isBottomSide;

    [HideInInspector] public int colorId;
    [HideInInspector] public bool connected;

    WireMinigame game;
    Image image;

    public void Setup(WireMinigame owner, int id, Color color)
    {
        game = owner;
        colorId = id;
        connected = false;

        if (image == null) image = GetComponent<Image>();
        image.color = color;
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (isBottomSide) game.BeginWire(this, e);
    }

    public void OnDrag(PointerEventData e)
    {
        if (isBottomSide) game.DragWire(e);
    }

    public void OnPointerUp(PointerEventData e)
    {
        if (isBottomSide) game.EndWire(e);
    }
}