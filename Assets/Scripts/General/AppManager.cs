using UnityEngine;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance { get; private set; }
    public AppAction CurrentAction { get; private set; }

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
}
