
using UnityEngine;

public class ErrorsMenuManager : MonoBehaviour
{
    private void Start()
    {
        AppManager.Instance.SetAction(AppAction.ERRORS);
    }

    public void ViewAllErrors()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);

        AppManager.Instance.SetAction(AppAction.ERRORS);

        ChangeSceneManager.Instance.GoToErrors();
    }

    public void ViewKahootErrors()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);

        AppManager.Instance.SetAction(AppAction.ERRORLOG);

        ChangeSceneManager.Instance.GoToKahootSelector();
    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);

        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
