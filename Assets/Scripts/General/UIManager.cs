using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Scene Names")]
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string selectorScene = "KahootSelector";
    [SerializeField] private string optionsScene = "Options";
    [SerializeField] private string aboutScene = "About";

    private void Awake()
    {
        // Comprobar si ya existe un UIManager
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Mantener este objeto al cambiar de escena
        DontDestroyOnLoad(gameObject);
    }

    public void GoToMainMenu()
    {
        LoadScene(mainMenuScene);
    }

    public void GoToKahootSelector()
    {
        LoadScene(selectorScene);
    }

    public void GoToOptions()
    {
        LoadScene(optionsScene);
    }

    public void GoToAbout()
    {
        LoadScene(aboutScene);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
