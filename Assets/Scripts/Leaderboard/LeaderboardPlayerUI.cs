using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardPlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text positionText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Image playerImage;

    public void SetPlayer(LeaderboardPlayer player, int position)
    {
        positionText.text = position.ToString();
        usernameText.text = player.username;
        scoreText.text = player.score.ToString();
    }
}
