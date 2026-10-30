using UnityEngine;

public class CoinCollect42 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddCoin();

            Destroy(gameObject);
        }
    }
}