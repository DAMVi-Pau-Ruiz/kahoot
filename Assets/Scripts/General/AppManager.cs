using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }
    public AppAction CurrentAction { get; private set; }

    public KahootData CurrentKahoot { get; private set; }
    public string CurrentKahootJson { get; private set; }

    public string CurrentKahootPath { get; private set; }

    public KahootEditorMode EditorMode { get; private set; }

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

    public void SetAction (AppAction action)
    {
        CurrentAction = action;

        Debug.Log("Actual action: " + CurrentAction);
    }

    public void SetEditorMode(KahootEditorMode mode)
    {
        EditorMode = mode;
    }

    public void SetCurrentKahoot(KahootData kahoot, string json, string path)
    {
        CurrentKahoot = kahoot;
        CurrentKahootJson = json;
        CurrentKahootPath = path;
    }
}
