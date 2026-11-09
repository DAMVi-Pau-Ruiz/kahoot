using UnityEngine;

public class EditorMenuManager : MonoBehaviour
{
    public void CreateKahoot()
    {
        AppManager.Instance.SetEditorMode(KahootEditorMode.CREATE);
        ChangeSceneManager.Instance.GoToKahootEditor();
    }

    public void EditKahoot()
    {
        AppManager.Instance.SetEditorMode(KahootEditorMode.EDIT);
        ChangeSceneManager.Instance.GoToKahootSelector();
    }

    public void GoToMenu()
    {
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
