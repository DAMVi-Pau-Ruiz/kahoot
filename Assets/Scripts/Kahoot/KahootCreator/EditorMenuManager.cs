using UnityEngine;

public class EditorMenuManager : MonoBehaviour
{
    public void CreateKahoot()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        AppManager.Instance.SetEditorMode(KahootEditorMode.CREATE);
        ChangeSceneManager.Instance.GoToKahootEditor();
    }

    public void EditKahoot()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        AppManager.Instance.SetEditorMode(KahootEditorMode.EDIT);
        ChangeSceneManager.Instance.GoToKahootSelector();
    }

    public void GoToMenu()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
