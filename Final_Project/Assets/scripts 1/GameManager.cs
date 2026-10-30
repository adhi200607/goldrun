using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    [SerializeField] private PlayerControl player;

    [Header("Score")]
    [SerializeField] private int targetScore = 15;

    [Header("Gameplay HUD")]
    [SerializeField] private GameObject gamePlayMenu;

    [Header("Start Game")]
    [SerializeField] private GameObject gameStartMenu;

    [Header("Game Over")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject gameOverImage;
    [SerializeField] private GameObject playAgainButton;

    [Header("Game Winner")]
    [SerializeField] private GameObject gameWinMenu;
    [SerializeField] private GameObject gameWinImage;
    [SerializeField] private GameObject winRestartButton;

    private bool gameStarted = false;
    private bool gameEnded = false;

    // ==========================================
    // AWAKE
    // ==========================================

    private void Awake()
    {
        Instance = this;
    }

    // ==========================================
    // START
    // ==========================================

    private void Start()
    {
        gameStarted = false;
        gameEnded = false;

        // START MENU
        if (gameStartMenu != null)
        {
            gameStartMenu.SetActive(true);
        }

        // GAMEPLAY HUD
        if (gamePlayMenu != null)
        {
            gamePlayMenu.SetActive(false);
        }

        // GAME OVER
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(false);
        }

        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }

        if (playAgainButton != null)
        {
            playAgainButton.SetActive(false);
        }

        // WINNER
        if (gameWinMenu != null)
        {
            gameWinMenu.SetActive(false);
        }

        if (gameWinImage != null)
        {
            gameWinImage.SetActive(false);
        }

        if (winRestartButton != null)
        {
            winRestartButton.SetActive(false);
        }

        // Disable player until Start Game
        if (player != null)
        {
            player.enabled = false;
        }

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Debug.Log("GAME READY - PRESS START GAME");
    }

    // ==========================================
    // START GAME BUTTON
    // ==========================================

    public void StartGame()
    {
        if (gameStarted || gameEnded)
        {
            return;
        }

        Debug.Log("START GAME BUTTON CLICKED");

        gameStarted = true;

        // Hide start menu
        if (gameStartMenu != null)
        {
            gameStartMenu.SetActive(false);
        }

        // Show gameplay HUD
        if (gamePlayMenu != null)
        {
            gamePlayMenu.SetActive(true);
        }

        // Enable player
        if (player != null)
        {
            player.enabled = true;
        }

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Debug.Log("GAME STARTED");
    }

    // ==========================================
    // UPDATE
    // ==========================================

    private void Update()
    {
        if (!gameStarted || gameEnded)
        {
            return;
        }

        CheckPlayerHealth();
        CheckScore();
    }

    // ==========================================
    // CHECK PLAYER HEALTH
    // ==========================================

    private void CheckPlayerHealth()
    {
        if (player == null)
        {
            return;
        }

        if (player.CurrentHealth <= 0f)
        {
            GameOver();
        }
    }

    // ==========================================
    // CHECK SCORE
    // ==========================================

    private void CheckScore()
    {
        if (ScoreManager.Instance == null)
        {
            return;
        }

        if (ScoreManager.Instance.CurrentScore >= targetScore)
        {
            GameWinner();
        }
    }

    // ==========================================
    // GAME OVER
    // ==========================================

    public void GameOver()
    {
        if (gameEnded)
        {
            return;
        }

        Debug.Log("💀 GAME OVER");

        gameEnded = true;
        gameStarted = false;

        // Disable player
        if (player != null)
        {
            player.enabled = false;
        }

        // Hide gameplay HUD
        if (gamePlayMenu != null)
        {
            gamePlayMenu.SetActive(false);
        }

        // Hide start menu
        if (gameStartMenu != null)
        {
            gameStartMenu.SetActive(false);
        }

        // Show Game Over
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }

        if (gameOverImage != null)
        {
            gameOverImage.SetActive(true);
        }

        if (playAgainButton != null)
        {
            playAgainButton.SetActive(true);
        }

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ==========================================
    // GAME WINNER
    // ==========================================

    public void GameWinner()
    {
        if (gameEnded)
        {
            return;
        }

        Debug.Log("🏆 GAME WINNER!");

        gameEnded = true;
        gameStarted = false;

        // Disable player
        if (player != null)
        {
            player.enabled = false;
        }

        // Hide gameplay HUD
        if (gamePlayMenu != null)
        {
            gamePlayMenu.SetActive(false);
        }

        // Hide start menu
        if (gameStartMenu != null)
        {
            gameStartMenu.SetActive(false);
        }

        // Show Winner
        if (gameWinMenu != null)
        {
            gameWinMenu.SetActive(true);
        }

        if (gameWinImage != null)
        {
            gameWinImage.SetActive(true);
        }

        if (winRestartButton != null)
        {
            winRestartButton.SetActive(true);
        }

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ==========================================
    // PLAY AGAIN
    // ==========================================

    public void PlayAgain()
    {
        Debug.Log("PLAY AGAIN");

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}