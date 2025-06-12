using UnityEngine;

public class PushableObject : MonoBehaviour
{
    public float pushPower = 5f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E)) // Если игрок зажимает "E"
            {
                float direction = collision.transform.position.x > transform.position.x ? 1 : -1;
                rb.velocity = new Vector2(direction * pushPower, rb.velocity.y);
            }
            else
            {
                // Если игрок не зажимает "E", замедляем камень
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Если игрок отходит, останавливаем камень
        if (collision.gameObject.CompareTag("Player"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}
