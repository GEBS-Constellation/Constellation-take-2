using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Maximum speed of the player")]
    public float top_speed;
    [Tooltip("The acceleration of the player, until top speed is reached")]
    public float acceleration;

    [Header("Jumping")]
    [Tooltip("The upwards velocity of the player at the start of a jump (jump height will depend on gravity scale)")]
    public float jump_vel;
    [Tooltip("The time after falling over an edge where jumping is still possible")]
    public float coyote_time;

    private Rigidbody2D rb;
    private float coyote_timer;
    private bool jump_exec;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        // Take input
        Vector2 input = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Horizontal movement
        rb.velocity = new Vector2(
            Mathf.MoveTowards(rb.velocity.x, input.x * top_speed, acceleration),
            rb.velocity.y
        );

        // Jump (if grounded)
        if (Input.GetButton("Jump")) {
            if (coyote_timer > 0 && !jump_exec) {
                jump_exec = true;
                rb.velocity = new Vector2(
                    rb.velocity.x,
                    jump_vel
                );
            }
        }

        // Count coyote time
        if (Physics2D.BoxCast(
                transform.position,
                Vector2.one * transform.localScale.x,
                0,
                Vector2.down,
                ((transform.localScale.y-transform.localScale.x)/2) + 0.1f
            ).collider != null) {
            coyote_timer = coyote_time;
            jump_exec = false;
        } else {
            coyote_timer = Mathf.MoveTowards(coyote_timer, 0, Time.fixedDeltaTime);
            if (coyote_timer == 0) {
                jump_exec = false;
            }
        }
    }
}
