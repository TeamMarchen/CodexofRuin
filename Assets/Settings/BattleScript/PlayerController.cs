using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamage
{
    void TakeDamage(float damage);
}
public class PlayerController : MonoBehaviour, IDamage
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Key Settings")]
    public PlayerKeySettings keySettings = new PlayerKeySettings();

    private Vector2 moveInput;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        moveInput = Vector2.zero;
        if (Input.GetKey(keySettings.upKey))
            moveInput.y += 1;
        if (Input.GetKey(keySettings.downKey))
            moveInput.y -= 1;
        if (Input.GetKey(keySettings.leftKey))
            moveInput.x -= 1;
        if (Input.GetKey(keySettings.rightKey))
            moveInput.x += 1;

        moveInput = moveInput.normalized;
    }

    private void MovePlayer()
    {
        Vector2 movement = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public void TakeDamage(float damage)
    {
        
    }
}
