using UnityEngine;

public class AboutManager : MonoBehaviour
{
    public void GoToMenu()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        ChangeSceneManager.Instance.GoToMainMenu();
    }
}
