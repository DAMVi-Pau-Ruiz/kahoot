using UnityEngine;

public class UIButtonActions : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        UIManager.Instance.GoToMainMenu();
    }

    public void OpenKahootSelector()
    {
        UIManager.Instance.GoToKahootSelector();
    }

    public void OpenOptions()
    {
        UIManager.Instance.GoToOptions();
    }

    public void OpenAbout()
    {
        UIManager.Instance.GoToAbout();
    }

    public void ExitGame()
    {
        UIManager.Instance.QuitGame();
    }
}