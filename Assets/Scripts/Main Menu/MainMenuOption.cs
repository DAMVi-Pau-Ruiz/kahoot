using UnityEngine;

[System.Serializable]
public class MainMenuOption
{
    [Header("Option Information")]
    public string title;

    [TextArea]
    public string description;

    [Header("Visuals")]
    public Sprite icon;

    [Header("Action")]
    public AppAction action;
}