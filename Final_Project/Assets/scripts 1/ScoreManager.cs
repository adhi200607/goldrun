using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private TMP_Text scoreText;

    private int score = 0;

    public int CurrentScore => score;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        score = 0;
        UpdateScore();
    }


    public void AddCoin()
    {
        score++;

        UpdateScore();

        Debug.Log(
            "Coins = " + score
        );
    }


    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text =
                " " + score;
        }
    }
}