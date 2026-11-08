using UnityEngine;

public enum GameModeType
{
    PLAY,
    EDIT,
    RANKING
}

[System.Serializable]
public class ModeData
{
    public string modeName;
    public string description;
    public Sprite icon;
    public Sprite backgorund;

    public string sceneName;

    public GameModeType modeType;
}