using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class TimerMinigame : Minigame
{
    [Header("UI")]
    [Tooltip("the panel the player sees. starts hidden.")]
    [SerializeField] GameObject panel;

    [Tooltip("Optional. allows users to hold the button down with the mouse.")]
    [SerializeField] HoldButton windButton;
    
    [Tooltip("a slider that shows the charge")]
    [SerializeField] Slider chargeBar;
    
    [Header("Controls")]
    [Tooltip("hold this key down while the panel is open")]
    [SerializeField] KeyCode windKey = KeyCode.F;
    
    [Header("Timing")]
    [Tooltip("Seconds for a full box to run completely down.")]
    [SerializeField] float secondsToDrain = 20f;
    [Tooltip("Seconds of winding to go from empty to full.")]
    [SerializeField] float secondsToFill = 4f;
    [Range(0f, 1f)]
    [SerializeField] float startCharge = 1f;
    [Tooltip("Extra drain speed gained per minute. 0.5 = 50% faster after each minute. 0 = constant.")]
    [SerializeField] float speedUpPerMinute = 0f;
    
    [Header("Warning")]
    [Range(0f, 1f)]
    [SerializeField] float lowThreshold = 0.25f;
 
    [Header("Events")]
    [Tooltip("Fires when the charge drops to the warning level")]
    public UnityEvent onLow;
    [Tooltip("Fires once when the box runs out .")]
    public UnityEvent onEmpty;

    public float Charge { get;private set; }
    public bool IsEmpty {get; private set;}

    float elapsed;
    bool lowFired;
    bool panelOpen;

    void Awake()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }

    void Start()
    {
        ResetBox();
    }

    void Update()
    {
        if (IsEmpty) return;

        elapsed += Time.deltaTime;
        bool keyHeld = Input.GetKey(windKey);
        bool buttonHeld = windButton != null && windButton.IsHeld;

        if (panelOpen && (keyHeld || buttonHeld))
        {
            Charge += Time.deltaTime /  secondsToFill;
        }
        else
        {
            float drainPerSecond = (1f/ secondsToDrain) * (1f + speedUpPerMinute * elapsed / 60f);
            Charge -= drainPerSecond * Time.deltaTime;
        }
        
        Charge = Mathf.Clamp01(Charge);

        if (!lowFired && Charge <= lowThreshold)
        {
            lowFired = true;
            onLow?.Invoke();
        } else if (lowFired && Charge > lowThreshold) {
            lowFired = false; // rearm
        }

        if (chargeBar != null) chargeBar.value = Charge;

        if (Charge <= 0f)
        {
            IsEmpty = true;
            onEmpty?.Invoke();
        }
    }


    public void ResetBox()
    {
        Charge = startCharge;
        elapsed = 0f;
        IsEmpty = false;
        lowFired = false;
        if (chargeBar != null) chargeBar.value = Charge;
    }

    public override void Open()
    {
        panelOpen = true;
        if (panel != null) panel.SetActive(true);
    }

    public override void Close()
    {
        panelOpen = false;
        if(panelOpen != null)panel.SetActive(false);
        onClosed?.Invoke();
    }
    
    protected override void ResetGame() { }
}
