using TMPro;
using UnityEngine;

public class UsernameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField usernameInput;

    public void Continue()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        string username = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("No hay username escrito");
            return;
        }

        PlayerSession.StartSession(username);

        ChangeSceneManager.Instance.GoToKahootSelector();
    }

    public void GoToMenu()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
