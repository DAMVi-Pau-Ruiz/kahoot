using TMPro;
using UnityEngine;

public class UsernameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField usernameInput;

    public void Continue()
    {
        string username = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            Debug.Log("No hay username escrito");
            return;
        }

        PlayerSession.StartSession(username);

        ChangeSceneManager.Instance.GoToKahootSelector();
    }
}
