using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class Cube_Control : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;

    // Player health
    public int lives = 5;

    // Player score
    public int score = 0;
    public TextMeshProUGUI scoreTextObject;
    public TextMeshProUGUI healthTextObject;

    void Update()
    {
        // Right
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.Translate(
                Vector3.right * speed * Time.deltaTime,
                Space.World
            );
        }

        // Left
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.Translate(
                Vector3.left * speed * Time.deltaTime,
                Space.World
            );
        }

        // Forward
        if (Keyboard.current.upArrowKey.isPressed)
        {
            transform.Translate(
                Vector3.forward * speed * Time.deltaTime,
                Space.World
            );
        }

        // Backward
        if (Keyboard.current.downArrowKey.isPressed)
        {
            transform.Translate(
                Vector3.back * speed * Time.deltaTime,
                Space.World
            );
        }

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GetComponent<Rigidbody>().AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );
        }
    }

    // Enemy collision
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            lives--;

            Debug.Log("Player hit enemy! Lives remaining: " + lives);

            if (lives <= 0)
            {
                Debug.Log("Game Over!");
                Destroy(gameObject);
            }
        }
    }

    // Coin collection
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            score++;

            Debug.Log("Coin collected! Score: " + score);

            Destroy(other.gameObject);
        }
    }
}