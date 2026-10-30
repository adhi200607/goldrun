using UnityEngine;
using UnityEngine.InputSystem;

public class Sphere_Control : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float rotatespeed = 300f;

    void Update()
    {
        // Right
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        // Left
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Forward
        if (Keyboard.current.upArrowKey.isPressed)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        // Backward
        if (Keyboard.current.downArrowKey.isPressed)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GetComponent<Rigidbody>().AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );
        }
        if (Keyboard.current.rKey.isPressed)
        {
            transform.Rotate(Vector3.up * rotatespeed * Time.deltaTime);
        }
    }
}