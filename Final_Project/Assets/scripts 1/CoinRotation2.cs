using UnityEngine;

public class CoinRotation2 : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 400f;

    private void Update()
    {
        transform.Rotate(
            Vector3.up *
            rotateSpeed *
            Time.deltaTime
        );
    }
}