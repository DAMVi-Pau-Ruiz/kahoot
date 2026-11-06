using TMPro;
using UnityEngine;

public class RankingEntryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text scoreText;

    public void SetData(
        int position,
        string username,
        int score
    )
    {
        positionText.text = position.ToString();
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}