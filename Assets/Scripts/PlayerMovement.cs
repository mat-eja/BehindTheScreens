
    
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator walk_up;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walk_up = GetComponent<Animator>();
    }

    void Update()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void Move(InputAction.CallbackContext context)
    {
        walk_up.SetBool("isWalking", true);

        if (context.canceled)
        {

        walk_up.SetBool("isWalking", moveInput != Vector2.zero);
        walk_up.SetFloat("InputX", moveInput.x);
        walk_up.SetFloat("InputY", moveInput.y);
    }

        moveInput = context.ReadValue<Vector2>();
        walk_up.SetFloat("InputX", moveInput.x);
        walk_up.SetFloat("InputY", moveInput.y);
    }
}
