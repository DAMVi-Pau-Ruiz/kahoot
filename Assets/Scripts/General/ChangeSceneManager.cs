using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneManager : MonoBehaviour
{
    public static ChangeSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToUsername()
    {
        SceneManager.LoadScene("Username");
    }

    public void GoToKahootSelector()
    {
        SceneManager.LoadScene("KahootSelector");
    }

    public void GoToKahootGame()
    {
        SceneManager.LoadScene("KahootPlay");
    }

    public void GoToKahootValidator()
    {
        SceneManager.LoadScene("KahootValidation");
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void GoToEditorMenu()
    {
        SceneManager.LoadScene("KahootEditorMenu");
    }

    public void GoToKahootEditor()
    {
        SceneManager.LoadScene("KahootEditor");
    }
}
