using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu;

    [Header("Instruction Menu")]
    [SerializeField] private GameObject instructionsMenu;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private Button backToMenuButton;

    private void Start()
    {
        // Show Main Menu
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }

        // Hide Instruction Menu
        if (instructionsMenu != null)
        {
            instructionsMenu.SetActive(false);
        }

        // Connect buttons
        if (startButton != null)
        {
            startButton.onClick.AddListener(PlayGame);
        }

        if (instructionButton != null)
        {
            instructionButton.onClick.AddListener(OpenInstructions);
        }

        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(BackToMainMenu);
        }

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // ==========================================
    // START / PLAY BUTTON
    // ==========================================

    public void PlayGame()
    {
        Debug.Log("Starting Game...");

        SceneManager.LoadScene(0);
    }

    // ==========================================
    // INSTRUCTION BUTTON
    // ==========================================

    public void OpenInstructions()
    {
        Debug.Log("Opening Instructions...");

        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }

        if (instructionsMenu != null)
        {
            instructionsMenu.SetActive(true);
        }
    }

    // ==========================================
    // BACK TO MENU BUTTON
    // ==========================================

    public void BackToMainMenu()
    {
        Debug.Log("Returning to Main Menu...");

        if (instructionsMenu != null)
        {
            instructionsMenu.SetActive(false);
        }

        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
        }
    }

    // ==========================================
    // QUIT GAME
    // ==========================================

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}