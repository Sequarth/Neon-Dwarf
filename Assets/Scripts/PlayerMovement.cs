using UnityEngine;

public class PlayerMovement
{
    public static Vector2 movementUpdate()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public static void movementFixedUpdate(Rigidbody2D rb, Vector2 v2, float moveSpeed)
    {
        rb.MovePosition(rb.position + v2.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
